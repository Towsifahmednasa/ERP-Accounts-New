# Golden-master regression: the same requests are sent to the OLD site (legacy MVC 5 under IIS Express) and the
# NEW site (ASP.NET Core 10), both against the same database, and the observations are compared.
#
#   OLD:  iisexpress /path:<copy of D:\Projects\Unicorn\Accounts\CSTL_Pro_to_Pay> /port:30408 /clr:v4.0
#   NEW:  dotnet run --urls http://localhost:5260       (from D:\Projects\Unicorn\New_Accounts)
#
# Scope: read-only. Page GETs (status code + redirect target) and every JSON lookup/list endpoint (normalised JSON
# body). Nothing is written to the database. The credentials are passed in, never read from the database.
#
#   powershell -File tests\golden-master\run.ps1 -User demo -Password '***' [-OldUrl http://localhost:30408] [-NewUrl http://localhost:5260]
param(
    [Parameter(Mandatory = $true)][string]$User,
    [Parameter(Mandatory = $true)][string]$Password,
    [string]$OldUrl = 'http://localhost:30408',
    [string]$NewUrl = 'http://localhost:5260',
    [string]$Report = (Join-Path $PSScriptRoot 'last-run.json'),
    [int]$CompanyId = 1
)
$ErrorActionPreference = 'Stop'
[Net.ServicePointManager]::SecurityProtocol = 'Tls12'

function New-Site([string]$name, [string]$base) {
    [pscustomobject]@{ Name = $name; Base = $base; S = (New-Object Microsoft.PowerShell.Commands.WebRequestSession); Token = '' }
}
function Login($site) {
    $login = Invoke-WebRequest -Uri ($site.Base + '/Home/Index') -WebSession $site.S -UseBasicParsing
    $m = [regex]::Match($login.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"')
    $body = @{ username = $User; password = $Password }
    if ($m.Success) { $body['__RequestVerificationToken'] = $m.Groups[1].Value; $site.Token = $m.Groups[1].Value }
    $r = Invoke-WebRequest -Uri ($site.Base + '/Home/LoginAction') -Method Post -Body $body -WebSession $site.S -UseBasicParsing -MaximumRedirection 5
    $ok = $r.BaseResponse.ResponseUri.AbsolutePath -like '*Dashboard*'
    if (-not $ok) { throw "$($site.Name): login did not land on the dashboard (landed on $($r.BaseResponse.ResponseUri.AbsolutePath))" }
    # the NEW layout exposes the token in a meta tag; AJAX calls need it only for POST, GETs are compared here
    $meta = [regex]::Match($r.Content, 'name="RequestVerificationToken" content="([^"]+)"')
    if ($meta.Success) { $site.Token = $meta.Groups[1].Value }
}
function Get-Observation($site, [string]$path, [switch]$Json) {
    $headers = @{}
    if ($Json) { $headers['X-Requested-With'] = 'XMLHttpRequest'; $headers['Accept'] = 'application/json, text/javascript, */*' }
    try {
        $r = Invoke-WebRequest -Uri ($site.Base + $path) -WebSession $site.S -UseBasicParsing -Headers $headers -MaximumRedirection 0 -ErrorAction Stop
        $status = [int]$r.StatusCode; $content = $r.Content; $loc = ''
    }
    catch [System.Net.WebException] {
        $resp = $_.Exception.Response
        if (-not $resp) { return @{ status = -1; error = $_.Exception.Message } }
        $status = [int]$resp.StatusCode; $loc = [string]$resp.Headers['Location']
        $content = (New-Object IO.StreamReader($resp.GetResponseStream())).ReadToEnd()
    }
    $o = @{ status = $status; location = ($loc -replace '^https?://[^/]+', '') }
    if ($Json -and $status -eq 200) {
        try {
            $j = $content | ConvertFrom-Json
            # double-encoded DataTable results (JsonConvert.SerializeObject(dt) then Json(string)) come back as a string
            if ($j -is [string]) { $j = $j | ConvertFrom-Json }
            $o.json = Normalize $j
        }
        catch { $o.body = $content.Substring(0, [Math]::Min(300, $content.Length)) }
    }
    elseif (-not $Json -and $status -eq 200) {
        $o.title = [regex]::Match($content, '<title>([^<]*)</title>').Groups[1].Value.Trim()
        $o.h5 = @([regex]::Matches($content, '<h5[^>]*cstlfs[^>]*>\s*<span[^>]*>\s*([^<]*)') | ForEach-Object { $_.Groups[1].Value.Trim() })
        $o.ids = @([regex]::Matches($content, '<(?:input|select|table|button)\b[^>]*\bid="([^"]+)"') | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique)
    }
    return $o
}
# Microsoft date format "\/Date(1234)\/" and float formatting differ between JavaScriptSerializer and Json.NET only in
# representation; normalise to a comparable shape.
function Normalize($v) {
    if ($null -eq $v) { return $null }
    if ($v -is [string]) { if ($v -match '^/Date\((-?\d+)') { return [long]$Matches[1] } return $v }
    if ($v -is [System.Array]) { return @($v | ForEach-Object { Normalize $_ }) }
    if ($v -is [System.Management.Automation.PSCustomObject]) {
        $h = [ordered]@{}
        foreach ($p in ($v.PSObject.Properties | Sort-Object Name)) { $h[$p.Name] = Normalize $p.Value }
        return $h
    }
    if ($v -is [double] -or $v -is [decimal] -or $v -is [single]) { return [math]::Round([double]$v, 4) }
    return $v
}

$old = New-Site 'OLD' $OldUrl; $new = New-Site 'NEW' $NewUrl
Login $old; Login $new
Write-Host "logged in to both sites as $User"

# ---- scenarios: page GETs + JSON endpoints (read-only) ------------------------------------------------------
$pages = @(
    '/Dashboard/Index',
    '/FinancialYear/FinancialyearList', '/FinancialYear/FinancialyearCreate', '/FinancialYear/FinancialyearLock',
    '/BanckAccount/Index', '/BanckAccount/Create', '/CashAccount/Index', '/CashAccount/Create', '/CurrencyConversion/CCCreate',
    '/Ref_Sub_Type/ReferenceTypeList', '/Ref_Sub_Type/ReferenceType', '/Ref_Sub_Type/SubsidairyList', '/Ref_Sub_Type/SubsidairyType',
    '/Refference/ReferenceInfo', '/Refference/SubsidiaryInfo', '/CostCentre/CostcentreList', '/CostCentre/CostcentreInformation',
    '/LedgerHead/LedgerHeadList', '/ControlLedger/ControlLedgerList', '/ControlLedger/ControlLedgerCreate',
    '/SubControl/SubControlList', '/SubControl/SubControlCeate', '/SubSubControlLedger/SSCLList', '/SubSubControlLedger/SSCLCreate',
    '/ChartOfAccount/ChartOfAccountList', '/ChartOfAccount/ChartOfAccountCreate', '/Subsubsidairy/Index', '/Subsubsidairy/SubsubsidairyCreate',
    '/OpeningBlnc/OpningBlncList', '/OpeningBlnc/OpningBlncEntry',
    '/BankPayments/Index', '/BankPayments/Create', '/CreditVoucher/CVList', '/CreditVoucher/CVCreate',
    '/Journal/JRList', '/Journal/JRCreate', '/Contra/ContraList', '/Contra/ContraEntry', '/ReconcileBankAccount/RBAEntry', '/DebitVoucher/Create',
    '/AllApproval/AllVoucherApproval', '/AllApproval/LoanApproval', '/AllApproval/BankChargeApproval',
    '/Loan/L_List', '/Loan/L_Entry', '/LCRelatedBankPayment/BankPayment_List', '/LCRelatedBankPayment/BankPaymentIndex',
    '/Budget/BudgetList', '/Budget/BudgetEntry',
    '/BalanceSheet/BSCreate', '/IncomeStatement/ISCreate',
    '/AllAccountReport/ReportList', '/AllAccountReport/LedgerReport', '/AllAccountReport/LedgerReportCostCenter', '/AllAccountReport/LedgerReportSubsidiary',
    '/AllAccountReport/CashBookReport', '/AllAccountReport/BankBookReport', '/AllAccountReport/COAReport', '/AllAccountReport/BankReconciliationStatementReport',
    '/AllAccountReport/TrialBalanceReport', '/AllAccountReport/CashFlowReport', '/AllAccountReport/BalanceSheetDynamicReport',
    '/AllAccountReport/IncomeStatementDynamicReport', '/AllAccountReport/IncomeStatementReport', '/AllAccountReport/VoucherValidationReport',
    '/Test/Index', '/Test/Create', '/Home/About', '/Home/Contact'
)
$json = @(
    '/Utility/LoadCompanyInfo', "/Utility/LoadCompanyUnitByCompanyId?companyId=$CompanyId", "/Utility/GetFinansialYearComId?companyId=$CompanyId",
    "/Utility/GetFinansialYearNotLockedComId?companyId=$CompanyId", "/Utility/GetFinansialYearSingle?companyId=$CompanyId",
    "/Utility/GetCashAccountbyCom?companyId=$CompanyId", "/Utility/GetBankAccountComId?companyId=$CompanyId", "/Utility/GetBankCashAccountComId?companyId=$CompanyId",
    "/Utility/GetAllBankCashAccountComId?companyId=$CompanyId", "/Utility/GetCashAccountComId?companyId=$CompanyId", "/Utility/GetCOAComId?companyId=$CompanyId",
    "/Utility/GetAllCOAComId?companyId=$CompanyId", "/Utility/GetCOAALLComId?companyId=$CompanyId", "/Utility/GetCOAALLComIdAuto?companyId=$CompanyId&term=&take=50",
    '/Utility/LoadCountry', '/Utility/GetBankName', '/Utility/GetCurrency', '/Utility/GetAccountType', '/Utility/GetLadgerHead', '/Utility/GetRefferenceType',
    '/Utility/GetSubsidiaryType', '/Utility/GetReferanceType', '/Utility/LoadSubsidaryType', '/Utility/LoadReferenceType', '/Utility/LoadSupplierList',
    '/Utility/LoadSupplierType', '/Utility/LoadLoanCategoryList', '/Utility/LoadCurrencyList', '/Utility/LoadInterestTypeList', '/Utility/LoadPaymentTypeList',
    '/Utility/GetBankNameforLoan', '/Utility/LoadInterestMethodList', '/Utility/getLoadProfitCenterList',
    '/LedgerHead/Get_LedgerHead_List', '/ControlLedger/Get_ControlLedger_List', '/SubControl/Get_SubControll_List', '/SubSubControlLedger/Get_SSCL_List',
    "/FinancialYear/Get_Financialyear?param=", '/BanckAccount/BankAccountList', '/CashAccount/CashAccountList', '/CurrencyConversion/Load_CurrencyConversionList',
    '/Ref_Sub_Type/Get_ReferenceType_List', '/Ref_Sub_Type/Get_SubsidairyType_List', '/CostCentre/Get_CostCenter_List',
    "/ChartOfAccount/Get_ChartofAccount_List?param=", "/OpeningBlnc/Get_OpeningBalance_List?CompanyId=$CompanyId&FinancialYearId=0",
    "/BankPayments/BankAccountList?param= and mas.CompanyInfoId='$CompanyId'", "/CreditVoucher/BankAccountList?param= and mas.CompanyInfoId='$CompanyId'",
    "/Journal/BankAccountList?param= and mas.CompanyInfoId='$CompanyId'", "/Contra/GetContraList?param= and mas.CompanyInfoId='$CompanyId'",
    "/Loan/Get_LoanInfoList?param=", "/LCRelatedBankPayment/Get_BankChargeList?param=", "/Budget/GetBudgetList?param=",
    "/AllApproval/BankAccountList?param= and mas.CompanyInfoId='$CompanyId'", "/AllApproval/LoadLoanList?param="
)
$steps = [ordered]@{}; $same = 0; $diff = 0
foreach ($p in $pages) {
    $o = Get-Observation $old $p; $n = Get-Observation $new $p
    $k = "page $p"
    $eq = ($o.status -eq $n.status) -and (($o.ids -join ',') -eq ($n.ids -join ','))
    $steps[$k] = @{ old = $o; new = $n; same = $eq }
    if ($eq) { $same++ } else { $diff++; Write-Host "DIFF $k : old=$($o.status) new=$($n.status) idsOld=$(@($o.ids).Count) idsNew=$(@($n.ids).Count)" }
}
foreach ($p in $json) {
    $o = Get-Observation $old $p -Json; $n = Get-Observation $new $p -Json
    $k = "json $p"
    $eq = ($o.status -eq $n.status) -and ((ConvertTo-Json $o.json -Depth 12 -Compress) -eq (ConvertTo-Json $n.json -Depth 12 -Compress))
    $steps[$k] = @{ old = $o; new = $n; same = $eq }
    if ($eq) { $same++ } else { $diff++; Write-Host "DIFF $k : old=$($o.status) new=$($n.status)" }
}
@{ run = (Get-Date).ToString('s'); user = $User; same = $same; diff = $diff; steps = $steps } | ConvertTo-Json -Depth 14 | Set-Content $Report -Encoding UTF8
Write-Host "identical: $same  different: $diff  -> $Report"
