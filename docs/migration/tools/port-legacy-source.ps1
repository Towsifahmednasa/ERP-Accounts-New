# Mechanical port of the legacy ASP.NET MVC 5 project (D:\Projects\Unicorn\Accounts, READ-ONLY) into New_Accounts.
# Only framework-level edits are made (usings, namespace, JsonRequestBehavior, base class, config-backed
# connection string) plus the handful of documented hand patches at the end. Business code, SQL calls and
# view scripts are copied verbatim. Re-runnable: it overwrites the ported files, never the hand-written ones.
$ErrorActionPreference = 'Stop'
$old = 'D:\Projects\Unicorn\Accounts\CSTL_Pro_to_Pay'
$new = 'D:\Projects\Unicorn\New_Accounts'
$utf8 = New-Object System.Text.UTF8Encoding($false)

function Write-File([string]$dst, [string]$text) {
    New-Item -ItemType Directory -Force (Split-Path $dst) | Out-Null
    [IO.File]::WriteAllText($dst, $text, $utf8)
}

function Port-Cs([string]$rel) {
    $src = Join-Path $old $rel
    $t = [IO.File]::ReadAllText($src)
    $t = $t -creplace 'using System\.Data\.SqlClient;', 'using Microsoft.Data.SqlClient;'
    $t = $t -creplace 'using System\.Web\.Mvc;', 'using Microsoft.AspNetCore.Mvc;'
    $t = $t -creplace '(?m)^\s*using System\.Web(\.[\w\.]+)?;\r?\n', ''
    $t = $t -creplace 'CSTL_Pro_to_Pay', 'New_Accounts'
    $t = $t -creplace '\bFormCollection\b', 'Microsoft.AspNetCore.Http.IFormCollection'   # MVC 5 scaffold parameter type (TestController)
    $t = $t -creplace '\s*,\s*JsonRequestBehavior\.AllowGet', ''
    $t = $t -creplace '(class\s+\w+Controller\s*:\s*)Controller\b', '$1LegacyController'
    $t = $t -creplace 'Request\.QueryString\[', 'Request.Query['
    # DataAccessManager / DashboardDAL built the connection string from hard-coded credentials (finding S9).
    $t = $t -creplace '(?s)@"data source=" \+ SqlUserAccess\.DataSource \+ ";Initial Catalog=" \+ database.*?SqlUserAccess\.PassWord \+ ";";', 'SqlUserAccess.ConnectionString(database);'
    $t = $t -creplace '(?s)@"data source=" \+ SqlUserAccess\.DataSource \+ ";Initial Catalog=" \+ DataBase\.ControlPanel.*?SqlUserAccess\.PassWord \+ ";";', 'SqlUserAccess.ConnectionString(DataBase.ControlPanel);'
    # the compat shim replaces System.Web for Session[...] (LegacyController) and HttpContext.Current (DAL audit user)
    if ($t -cmatch 'LegacyController|HttpContext\.Current') {
        $t = $t -creplace '(?m)^(using System;\r?\n)', "`$1using New_Accounts.Compat;`r`n"
    }
    $header = "// Ported unchanged from Accounts\CSTL_Pro_to_Pay\$rel (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).`r`n"
    Write-File (Join-Path $new $rel) ($header + $t)
}

# ---- 1. compile items from the legacy csproj (the authority: stray duplicates are not compiled there either) ----
$csproj = [IO.File]::ReadAllText("$old\CSTL_Pro_to_Pay.csproj")
$items = [regex]::Matches($csproj, 'Compile Include="([^"]+)"') | ForEach-Object { $_.Groups[1].Value }
$skip = '^(App_Start\\|Global\.asax|Properties\\AssemblyInfo|CrystalViewer\\|Report\\DataSets\\|DataManager\\SqlUserAccess\.cs)'
$ported = 0
foreach ($rel in $items) {
    if ($rel -cmatch $skip) { continue }
    Port-Cs $rel
    $ported++
}

# ---- 2. views (verbatim; layout/menu/login/dashboard are replaced by the Fiori shell) ----
$replacedViews = @('Shared\_Layout.cshtml', 'Shared\_Partial_Menu_Leyout.cshtml', 'Home\Index.cshtml', 'Dashboard\Index.cshtml', 'Dashboard\UserDashboard.cshtml', '_ViewStart.cshtml', 'Web.config')
$script:views = 0
Get-ChildItem "$old\Views" -Recurse -File | ForEach-Object {
    $rel = $_.FullName.Substring("$old\Views\".Length)
    if ($replacedViews -contains $rel) { return }
    if ($_.Extension -ne '.cshtml') { return }   # stray .cs/.config under Views are not compiled in the legacy either
    $t = [IO.File]::ReadAllText($_.FullName)
    $t = $t -creplace 'CSTL_Pro_to_Pay', 'New_Accounts'
    $t = $t -creplace '"\.\./VerticalAsset/', '"/VerticalAsset/'
    $t = $t -creplace '"\.\./AssetsStatic/', '"/AssetsStatic/'
    $t = $t -creplace '"\.\./(\w+)/(\w+)"', '"/$1/$2"'          # relative page links (../AllAccountReport/LedgerReport)
    $t = $t -creplace '\.\./CrystalViewer/', '/CrystalViewer/'
    $header = "@* Ported from Accounts\CSTL_Pro_to_Pay\Views\$rel - page markup and inline scripts unchanged; presentation comes from the Fiori shell/stylesheet. *@`r`n"
    Write-File (Join-Path "$new\Views" $rel) ($header + $t)
    $script:views++
}

# ---- 3. static assets the views depend on (jQuery 2.2.3, Bootstrap 5, DataTables, select2, SweetAlert, toastr, pickers, icons) ----
foreach ($dir in 'VerticalAsset', 'AssetsStatic') {
    robocopy "$old\$dir" "$new\wwwroot\$dir" /E /NFL /NDL /NJH /NJS /NP | Out-Null
}
Copy-Item "$old\favicon.ico" "$new\wwwroot\favicon.ico" -Force

# ---- 4. hand patches (each one is documented in docs/migration/legacy-defects.md / docs/security/legacy-security-findings.md) ----
function Patch([string]$rel, [scriptblock]$edit) {
    $p = Join-Path $new $rel
    $t = [IO.File]::ReadAllText($p)
    $t2 = & $edit $t
    if ($t2 -eq $t) { throw "patch did not apply: $rel" }
    [IO.File]::WriteAllText($p, $t2, $utf8)
}

Patch 'Controllers\HomeController.cs' {
    param($t)
    $t = $t -creplace 'using Microsoft\.AspNetCore\.Mvc;', "using Microsoft.AspNetCore.Mvc;`r`nusing Microsoft.AspNetCore.Authorization;"
    # login/logout/error are the only anonymous actions (global SessionRequiredFilter)
    foreach ($sig in 'public ActionResult Index\(\)', 'public ActionResult Login\(\)', 'public ActionResult LoginAction\(', 'public ActionResult LockScreen\(', 'public ActionResult Logout\(\)') {
        $t = $t -creplace "(\r?\n)(\s*)($sig)", "`$1`$2[AllowAnonymous]`$1`$2`$3"
    }
    # S2: CheckLogin was a routable public action (credential oracle); the view only ever called it from commented-out code.
    $t = $t -creplace '(\r?\n)(\s*)(public bool CheckLogin\()', "`$1`$2[NonAction]`$1`$2`$3"
    # GAP-PLT-001: Login.cshtml never existed (HTTP 500 on session loss); render the real login view.
    $t = $t -creplace 'return View\("Login"\);', 'return View("Index"); // legacy: View("Login") - the view never existed (GAP-PLT-001)'
    # GAP-PLT-002: failure message was put in ViewData and lost by the redirect.
    $t = $t -creplace 'ViewData\["LoginMsg"\] = "Invalid Credentials";', 'TempData["LoginMsg"] = "Invalid Credentials"; // legacy used ViewData, lost on redirect (GAP-PLT-002)'
    # System.Web HttpCookie -> ASP.NET Core cookie (same never-read cookie, same values)
    $t = $t -creplace '(?s)HttpCookie cookie = Request\.Cookies\["LoginUserIdCookie"\];.*?Response\.Cookies\.Add\(cookie\);\r?\n\s*\}', ('string loginName = dt.Rows[0]["LoginUserName"].ToString();' + "`r`n" +
        '                    Response.Cookies.Append("LoginUserIdCookie", "EmpId=" + loginName + "&EmpName=" + loginName, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true }); // legacy multi-value cookie, never read')
    # error page for UseExceptionHandler (legacy: HandleErrorAttribute -> Views/Shared/Error.cshtml)
    $t = $t -creplace '(\r?\n\s*\}\s*\}\s*)$', ("`r`n`r`n        [AllowAnonymous]`r`n        public ActionResult Error()`r`n        {`r`n            return View();`r`n        }" + '$1')
    return $t
}

Patch 'Controllers\DashboardController.cs' {
    param($t)
    $t = $t -creplace '\(List<MenuStepOne>\)Session\["MenuList"\]', 'Session.Get<List<MenuStepOne>>("MenuList")'
    $t = $t -creplace '"~/Views/Shared/_Partial_Menu_Leyout\.cshtml"', '"_SideNav"'
    return $t
}

Write-Host "ported $ported C# files, $script:views views, assets; hand patches applied"
