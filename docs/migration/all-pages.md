# All legacy pages (complete inventory)

Generated 2026-10-07 by `docs/migration/tools/gen-migration-docs.py` from the legacy reverse-engineering inventory (100 pages/screens, `Accounts/docs/reverse-engineering/page-inventory.md`), the per-page specs (`Accounts/specs/**/pages/`), the stored-procedure catalog and the ported source. Nothing was skipped: dead/stub pages are listed with their legacy status. Status values are maintained in `status.json` and merged by the generator.

Batches: **A** Application foundation (17 pages); **B** Master setup (21 pages); **C** Chart of accounts & opening balance (16 pages); **D** Vouchers & bank reconciliation (13 pages); **E** Approvals, loan/LC, budget (10 pages); **F** Financial statements (8 pages); **G** Reports, print & utilities (12 pages); **H** Sandbox/test scaffold (3 pages)

## Summary table

| Page ID | Batch | Module | Page | Type | Route | Controller/Action | Fiori nav group (screen) | SPs | Migration status | Functional test | UI QA |
|---|---|---|---|---|---|---|---|---|---|---|---|
| PAGE-001 | A | PLT | Login (this is the REAL login screen) | list | `/ , /Home/Index, /Home/Logout (renders same view), form POST /Home/LoginAction` | Home/Index | More | 1 | DONE | PASS (login page, CSRF, failed-login message, redirect) | PASS (1366x768, 1024, 500) |
| PAGE-002 | A | PLT | Home/Login (`View("Login")`) | dead | `/Home/Login` | Home/Login | More | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-003 | A | PLT | Home/LockScreen (`View("LockScreen")`) | dead | `/Home/LockScreen?EmpId=&EmpName=` | Home/LockScreen | More | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-004 | A | PLT | About (MVC template leftover) | list | `/Home/About` | Home/About | More | 0 | MIGRATED | Pending | Pending |
| PAGE-005 | A | PLT | Contact (MVC template leftover) | list | `/Home/Contact` | Home/Contact | More | 0 | MIGRATED | Pending | Pending |
| PAGE-006 | A | PLT | Main Dashboard (`Dashboard/Index` when IsMainDashboard=true) | list | `/Dashboard/Index` | Dashboard/Index | More | 2 | BLOCKED-LOCAL (missing SP: sp_GetApprovalMenuListByUser, sp_GetVoucherApprovalDashboard) | Pending | Pending |
| PAGE-007 | A | PLT | User Dashboard (`Dashboard/Index` when IsMainDashboard=false; also `/Dashboard/UserDashboard`) | list | `/Dashboard/Index, /Dashboard/UserDashboard` | Dashboard/Index | More | 0 | MIGRATED | Pending | Pending |
| PAGE-008 | A | PLT | Dashboard/_FilterMasterInfo (shared report filter partial) | partial | `/Dashboard/LoadFilterInPartialView` | Dashboard/LoadFilterInPartialView | More | 3 | MIGRATED | Pending | Pending |
| PAGE-009 | A | PLT | Shared/_Layout (page shell) | partial | `(applies to every view via Views/_ViewStart.cshtml, except views with Layout = null)` |  | - | 0 | MIGRATED | Pending | Pending |
| PAGE-010 | A | PLT | Shared/_Partial_Menu_Leyout (DB-driven 3-level menu) | partial | `rendered by Dashboard/MenuGeneration` | Dashboard/MenuGeneration | More | 1 | MIGRATED | Pending | Pending |
| PAGE-011 | A | PLT | Shared/Error | list | `HandleErrorAttribute target` |  | - | 0 | MIGRATED | Pending | Pending |
| PAGE-012 | A | PLT | Dashboard/ApprovalDashBoard | dead | `/Dashboard/ApprovalDashBoard` | Dashboard/ApprovalDashBoard | More | 1 | N/A (dead in legacy) | Pending | Pending |
| PAGE-013 | A | PLT | Dashboard/VoucherApprovalDashBoard | dead | `/Dashboard/VoucherApprovalDashBoard` | Dashboard/VoucherApprovalDashBoard | More | 1 | N/A (dead in legacy) | Pending | Pending |
| PAGE-014 | A | PLT | Utility shared AJAX endpoint provider (no UI) | list | `/Utility/{action} (62 actions)` | Utility/62 | More | 51 | BLOCKED-LOCAL (missing SP: sp_Get_UserInfoString) | Pending | Pending |
| PAGE-015 | A | PLT | Shared JS library `AssetsStatic/JS/custom-functions.js` (no UI) | list | `loaded by _Layout.cshtml:74` |  | - | 0 | MIGRATED | Pending | Pending |
| PAGE-016 | A | PLT | Logout (action; renders login view) | list | `/Home/Logout` | Home/Logout | More | 0 | MIGRATED | Pending | Pending |
| PAGE-017 | A | PLT | Home/CheckLogin (public bool method = routable GET action, no view) | list | `/Home/CheckLogin?userName=&pass=` | Home/CheckLogin | More | 1 | MIGRATED | Pending | Pending |
| PAGE-020 | B | 02-master-setup | Financial Year List | list | `/FinancialYear/FinancialyearList` | FinancialYear/FinancialyearList | Chart of Accounts (80) | 2 | MIGRATED | Pending | Pending |
| PAGE-021 | B | 02-master-setup | Financial Year Create/Edit | entry | `/FinancialYear/FinancialyearCreate/{id}` | FinancialYear/FinancialyearCreate | Chart of Accounts (80) | 4 | MIGRATED | Pending | Pending |
| PAGE-022 | B | 02-master-setup | Financial Year Lock | list | `/FinancialYear/FinancialyearLock/{id}` | FinancialYear/FinancialyearLock | Chart of Accounts (80) | 5 | MIGRATED | Pending | Pending |
| PAGE-023 | B | 02-master-setup | Bank Account List | list | `/BanckAccount/Index` | BanckAccount/Index | Bank & Cash Reconciliation (83) | 3 | MIGRATED | Pending | Pending |
| PAGE-024 | B | 02-master-setup | Bank Account Create/Edit | entry | `/BanckAccount/Create/{id}` | BanckAccount/Create | Bank & Cash Reconciliation (83) | 17 | MIGRATED | Pending | Pending |
| PAGE-025 | B | 02-master-setup | Cash Account List | list | `/CashAccount/Index` | CashAccount/Index | Bank & Cash Reconciliation (83) | 4 | MIGRATED | Pending | Pending |
| PAGE-026 | B | 02-master-setup | Cash Account Create/Edit | entry | `/CashAccount/Create/{id}` | CashAccount/Create | Bank & Cash Reconciliation (83) | 14 | MIGRATED | Pending | Pending |
| PAGE-027 | B | 02-master-setup | Currency Conversion Rate | entry | `/CurrencyConversion/CCCreate` | CurrencyConversion/CCCreate | Chart of Accounts (80) | 4 | MIGRATED | Pending | Pending |
| PAGE-040 | B | MST | Reference Type Entry | list | `/Ref_Sub_Type/ReferenceType[?id=]` | Ref_Sub_Type/ReferenceType | Chart of Accounts (80) | 8 | BLOCKED-LOCAL (missing SP: sp_GetReferenceTypeByGlId) | Pending | Pending |
| PAGE-041 | B | MST | Reference Type List | list | `/Ref_Sub_Type/ReferenceTypeList` | Ref_Sub_Type/ReferenceTypeList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-042 | B | MST | Subsidiary Type Entry | list | `/Ref_Sub_Type/SubsidairyType[?id=]` | Ref_Sub_Type/SubsidairyType | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-043 | B | MST | Subsidiary Type List | list | `/Ref_Sub_Type/SubsidairyList` | Ref_Sub_Type/SubsidairyList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-044 | B | MST | Reference Info | list | `/Refference/ReferenceInfo` | Refference/ReferenceInfo | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-045 | B | MST | Reference Info checklist partial | partial | `/Refference/LoadReferenceInfoInPartialView?ReferenceType=` | Refference/LoadReferenceInfoInPartialView | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-046 | B | MST | Subsidiary Info | list | `/Refference/SubsidiaryInfo` | Refference/SubsidiaryInfo | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-047 | B | MST | Subsidiary Info checklist partial | partial | `/Refference/LoadSubsidiaryInfoInPartialView?SubsidiaryType=` | Refference/LoadSubsidiaryInfoInPartialView | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-048 | B | MST | Cost Centre Entry | list | `/CostCentre/CostcentreInformation[?id=]` | CostCentre/CostcentreInformation | Cost Centre & Budget Control (85) | 0 | MIGRATED | Pending | Pending |
| PAGE-049 | B | MST | Cost Centre List | dead | `/CostCentre/CostcentreList` | CostCentre/CostcentreList | Cost Centre & Budget Control (85) | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-050 | B | MST | Reference Type (Refference stub) | list | `/Refference/ReferenceType` | Refference/ReferenceType | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-051 | B | MST | Subsidiary Type (Refference stub) | list | `/Refference/SubsidiaryType` | Refference/SubsidiaryType | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-052 | B | MST | Cost Center Info (Refference stub) | list | `/Refference/CostCenterInfo` | Refference/CostCenterInfo | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-060 | C | COA | Ledger Head List | list | `/LedgerHead/LedgerHeadList` | LedgerHead/LedgerHeadList | Chart of Accounts (80) | 2 | MIGRATED | Pending | Pending |
| PAGE-061 | C | COA | Control Ledger List | list | `/ControlLedger/ControlLedgerList` | ControlLedger/ControlLedgerList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-062 | C | COA | Control Ledger Entry | entry | `/ControlLedger/ControlLedgerCreate?id=` | ControlLedger/ControlLedgerCreate | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-063 | C | COA | Sub-Control Ledger List | list | `/SubControl/SubControlList` | SubControl/SubControlList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-064 | C | COA | Sub-Control Ledger Entry | list | `/SubControl/SubControlCeate?id=` | SubControl/SubControlCeate | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-065 | C | COA | Sub Sub Control Ledger List | list | `/SubSubControlLedger/SSCLList` | SubSubControlLedger/SSCLList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-066 | C | COA | Sub Sub Control Ledger Entry | entry | `/SubSubControlLedger/SSCLCreate?id=` | SubSubControlLedger/SSCLCreate | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-067 | C | COA | Subsidiary Ledger List | list | `/Subsubsidairy/Index` | Subsubsidairy/Index | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-068 | C | COA | Subsidiary Ledger Entry | entry | `/Subsubsidairy/SubsubsidairyCreate?id=` | Subsubsidairy/SubsubsidairyCreate | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-080 | C | COA | Chart Of Account Entry (create/edit GL) | entry | `/ChartOfAccount/ChartOfAccountCreate?id={AccountId}` | ChartOfAccount/ChartOfAccountCreate | Chart of Accounts (80) | 19 | MIGRATED | Pending | Pending |
| PAGE-081 | C | COA | Chart Of Account List | list | `/ChartOfAccount/ChartOfAccountList` | ChartOfAccount/ChartOfAccountList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-082 | C | COA | Reference-type partial (_RefPartial) | partial | `/ChartOfAccount/LoadReferanceInPartialView?glid=` | ChartOfAccount/LoadReferanceInPartialView | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-083 | C | COA | Subsidiary-type partial (_SubSidaryPartial) | partial | `/ChartOfAccount/LoadSubSidaryInPartialView?glid=` | ChartOfAccount/LoadSubSidaryInPartialView | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-084 | C | COA | Cost-centre partial (_CostCenterPartial) | partial | `/ChartOfAccount/LoadCostCenterInPartialView?glid=` | ChartOfAccount/LoadCostCenterInPartialView | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-085 | C | COA | Opening Balance Entry | entry | `/OpeningBlnc/OpningBlncEntry?id={OpeningBalanceId}` | OpeningBlnc/OpningBlncEntry | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-086 | C | COA | Opening Balance List | list | `/OpeningBlnc/OpningBlncList` | OpeningBlnc/OpningBlncList | Chart of Accounts (80) | 0 | MIGRATED | Pending | Pending |
| PAGE-100 | D | 04-vouchers | Debit Voucher List | list | `/BankPayments/Index` | BankPayments/Index | Voucher Entry (81) | 10 | MIGRATED | Pending | Pending |
| PAGE-101 | D | 04-vouchers | Debit Voucher Entry (bank/cash payment) | entry | `/BankPayments/Create[?MasterId=]` | BankPayments/Create | Voucher Entry (81) | 19 | MIGRATED | Pending | Pending |
| PAGE-102 | D | 04-vouchers | Voucher detail row partial | dead | `/BankPayments/AddRowForCOA?tr=` | BankPayments/AddRowForCOA | Voucher Entry (81) | 4 | N/A (dead in legacy) | Pending | Pending |
| PAGE-103 | D | 04-vouchers | Debit/Payment Voucher (legacy eflag ed/ap) | entry | `/DebitVoucher/Create?VoucherId=&eflag=` | DebitVoucher/Create | Voucher Entry (81) | 14 | BLOCKED-LOCAL (missing SP: sp_GetBankBranchByGlId, sp_GetCostCenterByGlId, sp_GetGLInfoById, sp_GetRefDetailsByRefTypeId) | Pending | Pending |
| PAGE-104 | D | 04-vouchers | Credit Voucher List | list | `/CreditVoucher/CVList` | CreditVoucher/CVList | Voucher Entry (81) | 7 | MIGRATED | Pending | Pending |
| PAGE-105 | D | 04-vouchers | Credit Voucher Entry | entry | `/CreditVoucher/CVCreate[?MasterId=]` | CreditVoucher/CVCreate | Voucher Entry (81) | 9 | MIGRATED | Pending | Pending |
| PAGE-120 | D | VCH | Journal List | list | `/Journal/JRList` | Journal/JRList | Voucher Entry (81) | 7 | MIGRATED | Pending | Pending |
| PAGE-121 | D | VCH | Journal Entry (create/edit) | entry | `/Journal/JRCreate[?MasterId=]` | Journal/JRCreate | Voucher Entry (81) | 14 | MIGRATED | Pending | Pending |
| PAGE-122 | D | VCH | Journal GL row partial | partial | `/Journal/AddJRRowForCOA?tr=` | Journal/AddJRRowForCOA | Voucher Entry (81) | 1 | MIGRATED | Pending | Pending |
| PAGE-123 | D | VCH | Contra List | list | `/Contra/ContraList` | Contra/ContraList | Voucher Entry (81) | 5 | MIGRATED | Pending | Pending |
| PAGE-124 | D | VCH | Contra Entry (4 transfer types) | dead | `/Contra/ContraEntry[?MasterId=]` | Contra/ContraEntry | Voucher Entry (81) | 14 | N/A (dead in legacy) | Pending | Pending |
| PAGE-125 | D | VCH | Bank Reconciliation Entry | entry | `/ReconcileBankAccount/RBAEntry[?id=]` | ReconcileBankAccount/RBAEntry | Bank & Cash Reconciliation (83) | 3 | MIGRATED | Pending | Pending |
| PAGE-126 | D | VCH | Reconcile grid partial | partial | `/ReconcileBankAccount/ReconcileBankAccountList?param=` | ReconcileBankAccount/ReconcileBankAccountList | Bank & Cash Reconciliation (83) | 1 | MIGRATED | Pending | Pending |
| PAGE-140 | E | Approvals | All Voucher Approval | list | `/AllApproval/AllVoucherApproval` | AllApproval/AllVoucherApproval | Voucher Entry (81) | 4 | MIGRATED | Pending | Pending |
| PAGE-141 | E | Approvals | Loan Approval | list | `/AllApproval/LoanApproval` | AllApproval/LoanApproval | Voucher Entry (81) | 2 | MIGRATED | Pending | Pending |
| PAGE-142 | E | Approvals | Bank Charge Approval | dead | `/AllApproval/BankChargeApproval` | AllApproval/BankChargeApproval | Voucher Entry (81) | 2 | N/A (dead in legacy) | Pending | Pending |
| PAGE-150 | E | Budget | Budget List | dead | `/Budget/BudgetList` | Budget/BudgetList | Cost Centre & Budget Control (85) | 4 | N/A (dead in legacy) | Pending | Pending |
| PAGE-151 | E | Budget | Budget Entry | entry | `/Budget/BudgetEntry?id=` | Budget/BudgetEntry | Cost Centre & Budget Control (85) | 12 | MIGRATED | Pending | Pending |
| PAGE-152 | E | Budget | Budget Month Amount grid (partial) | partial | `/Budget/BudgetMonthAmountDetails` | Budget/BudgetMonthAmountDetails | Cost Centre & Budget Control (85) | 2 | MIGRATED | Pending | Pending |
| PAGE-160 | E | 06-loan-lc | Loan Info Entry | entry | `/Loan/L_Entry[?id=N]` | Loan/L_Entry | Receivables & Payables Ageing (82) | 17 | MIGRATED | Pending | Pending |
| PAGE-161 | E | 06-loan-lc | Loan List | list | `/Loan/L_List` | Loan/L_List | Receivables & Payables Ageing (82) | 1 | MIGRATED | Pending | Pending |
| PAGE-162 | E | 06-loan-lc | Bank Charge Entry (LC related bank payment) | list | `/LCRelatedBankPayment/BankPaymentIndex[?id=N]` | LCRelatedBankPayment/BankPaymentIndex | Receivables & Payables Ageing (82) | 13 | BLOCKED-LOCAL (missing SP: SP_UpdateBankPaymentMaster) | Pending | Pending |
| PAGE-163 | E | 06-loan-lc | Bank Charge List | list | `/LCRelatedBankPayment/BankPayment_List` | LCRelatedBankPayment/BankPayment_List | Receivables & Payables Ageing (82) | 1 | MIGRATED | Pending | Pending |
| PAGE-200 | F | FST | Balance Sheet Setup | report | `/BalanceSheet/BSCreate` | BalanceSheet/`BSCreate` | Trial Balance & Financials (84) | 7 | BLOCKED-LOCAL (missing SP: sp_D_BalanceSheetDetailByMasterId, sp_GET_BalanceSheetByCompanyId, sp_GET_BalanceSheetDetailByCompanyId, sp_I_BalanceSheetDetail) | Pending | Pending |
| PAGE-201 | F | FST | Income Statement Setup | report | `/IncomeStatement/ISCreate` | IncomeStatement/`ISCreate` | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-202 | F | FST | Balance Sheet Dynamic Report | report | `/BalanceSheetDynamicReport/Index` | BalanceSheetDynamicReport/`Index` | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-203 | F | FST | Income Statement Dynamic Report | report | `/IncomeStatementDynamicReport/Index` | IncomeStatementDynamicReport/`Index` | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-204 | F | FST | Income Statement Report (legacy SP-based) | dead | `/IncomeStatement/IncomeStatementReport` | IncomeStatement/`IncomeStatementReport` | Trial Balance & Financials (84) | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-205 | F | FST | Cash Flow Report | report | `/AllAccountReport/CashFlowReport` | AllAccountReport/`CashFlowReport` | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-206 | F | FST | Trial Balance Report | report | `/AllAccountReport/TrialBalanceReport` | AllAccountReport/`TrialBalanceReport` | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-207 | F | FST | Balance Sheet Report (legacy, view missing) | dead | `/BalanceSheet/BalanceSheetReport` | BalanceSheet/`BalanceSheetReport` | Trial Balance & Financials (84) | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-220 | G | RPT | Accounts Operation Report (ReportList) | report | `/AllAccountReport/ReportList` | AllAccountReport/ReportList | Trial Balance & Financials (84) | 0 | MIGRATED | Pending | Pending |
| PAGE-221 | G | RPT | Ledger Report | report | `/AllAccountReport/LedgerReport` | AllAccountReport/LedgerReport | Trial Balance & Financials (84) | 21 | MIGRATED | Pending | Pending |
| PAGE-222 | G | RPT | Subsidiary Wise Ledger Report | report | `/AllAccountReport/LedgerReportSubsidiary` | AllAccountReport/LedgerReportSubsidiary | Trial Balance & Financials (84) | 7 | MIGRATED | Pending | Pending |
| PAGE-223 | G | RPT | Cost Center Wise Ledger Report | report | `/AllAccountReport/LedgerReportCostCenter` | AllAccountReport/LedgerReportCostCenter | Trial Balance & Financials (84) | 6 | MIGRATED | Pending | Pending |
| PAGE-224 | G | RPT | Chart of Account Report | report | `/AllAccountReport/COAReport` | AllAccountReport/COAReport | Trial Balance & Financials (84) | 2 | MIGRATED | Pending | Pending |
| PAGE-225 | G | RPT | Cash Book Report | report | `/AllAccountReport/CashBookReport` | AllAccountReport/CashBookReport | Trial Balance & Financials (84) | 7 | MIGRATED | Pending | Pending |
| PAGE-226 | G | RPT | Bank Book Report | report | `/AllAccountReport/BankBookReport` | AllAccountReport/BankBookReport | Trial Balance & Financials (84) | 6 | MIGRATED | Pending | Pending |
| PAGE-227 | G | RPT | Bank Reconciliation Statement Report | report | `/AllAccountReport/BankReconciliationStatementReport` | AllAccountReport/BankReconciliationStatementReport | Trial Balance & Financials (84) | 4 | MIGRATED | Pending | Pending |
| PAGE-228 | G | RPT | Voucher Validation / Accounts Integrity Report | report | `/VoucherValidation/Index` | VoucherValidation/Index | Trial Balance & Financials (84) | 3 | MIGRATED | Pending | Pending |
| PAGE-229 | G | RPT | AccountingTaskReport controller (hand-off endpoints; no view; Views/AccountingTaskReport is an empty folder) | dead | `/AccountingTaskReport/<rpt*>` | AccountingTaskReport/rpt* | Trial Balance & Financials (84) | 0 | N/A (dead in legacy) | Pending | Pending |
| PAGE-260 | G | RPT | Crystal Report Viewer | dead | `/CrystalViewer/CrystalViewer.aspx[?query]` | AccountingTaskReport/`Page_Init` | Trial Balance & Financials (84) | 7 | BLOCKED (Crystal Reports has no .NET 10 runtime) | Blocked | - |
| PAGE-261 | G | RPT | Payment Voucher HTML print | report | `/CrystalViewer/RptJournal.aspx` | /`Page_Init` | - | 5 | BLOCKED (Crystal Reports has no .NET 10 runtime) | Blocked | - |
| PAGE-290 | H | Sandbox | Test List ("Voucher List") | dead | `/Test/Index` | Test/Index | More | 4 | N/A (dead in legacy) | Pending | Pending |
| PAGE-291 | H | Sandbox | Test Create ("Voucher Info") | entry | `/Test/Create` | Test/Create | More | 7 | MIGRATED | Pending | Pending |
| PAGE-292 | H | Sandbox | Test Add Country Row (partial) | partial | `/Test/AddRowForCountry` | Test/AddRowForCountry | More | 0 | MIGRATED | Pending | Pending |

## Per-page detail

Columns the brief asks for (tables, SPs, functions, views, APIs, session, auth, CRUD, reports, export, files, transactions, rules, defects) are listed per page. "Tables" are the objects the stored procedure references in the LOCAL database (`sys.sql_expression_dependencies`; W = the SP updates the object). Business rules per page: see the linked legacy spec, section 19.

### PAGE-001 — Login (this is the REAL login screen)

| Attribute | Value |
|---|---|
| Source path | `Views/Home/Index.cshtml` (Layout=null) |
| Page type / purpose | list — Login (this is the REAL login screen) |
| Legacy status | active (with defects) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | `sp_GET_UserInfo_Acc` |
| Tables / views / functions (local DB) | tblUser |
| APIs / AJAX | API-G1-001, 005, 008 |
| Session variables (spec) | LoginUserId, MenuList, UserDashboard, loginuserempid, loginuserid, loginusertype, username |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-001, GAP-PLT-002, GAP-PLT-003, GAP-PLT-022 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-001-login.md` |
| Migration status / build / functional / UI QA | DONE / PASS / PASS (login page, CSRF, failed-login message, redirect) / PASS (1366x768, 1024, 500) |
| Notes | Fiori login screen |

### PAGE-002 — Home/Login (`View("Login")`)

| Attribute | Value |
|---|---|
| Source path | MISSING — no `Login.cshtml` anywhere in `Views/` |
| Page type / purpose | dead — Home/Login (`View("Login")`) |
| Legacy status | dead/broken (HTTP 500 view-not-found). It is the redirect target of `Dashboard/Index` and `Dashboard/MenuGeneration` on session loss |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | loginuserempid |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-001 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-002-home-login-missing-view.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-003 — Home/LockScreen (`View("LockScreen")`)

| Attribute | Value |
|---|---|
| Source path | MISSING — no `LockScreen.cshtml` |
| Page type / purpose | dead — Home/LockScreen (`View("LockScreen")`) |
| Legacy status | dead (no view, no caller) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-007 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-003-home-lockscreen-missing-view.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-004 — About (MVC template leftover)

| Attribute | Value |
|---|---|
| Source path | `Views/Home/About.cshtml` |
| Page type / purpose | list — About (MVC template leftover) |
| Legacy status | active-but-unlinked (template boilerplate) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | MenuList, loginusertype, username |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-006 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-004-home-about.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-005 — Contact (MVC template leftover)

| Attribute | Value |
|---|---|
| Source path | `Views/Home/Contact.cshtml` |
| Page type / purpose | list — Contact (MVC template leftover) |
| Legacy status | active-but-unlinked (template boilerplate, placeholder address/e-mail) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-006 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-005-home-contact.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-006 — Main Dashboard (`Dashboard/Index` when IsMainDashboard=true)

| Attribute | Value |
|---|---|
| Source path | `Views/Dashboard/Index.cshtml` (model `IEnumerable<ApprovalMenuList>`, always empty) |
| Page type / purpose | list — Main Dashboard (`Dashboard/Index` when IsMainDashboard=true) |
| Legacy status | active — STATIC DEMO DATA |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | `sp_GetApprovalMenuListByUser` **(missing locally)**, `sp_GetVoucherApprovalDashboard` **(missing locally)** |
| Tables / views / functions (local DB) | UNKNOWN (SP missing locally) |
| APIs / AJAX | none (no data calls; ALL figures hard-coded) |
| Session variables (spec) | UserDashboard, loginuserempid, username |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-001, GAP-PLT-004, GAP-PLT-017 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-006-dashboard-index.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: sp_GetApprovalMenuListByUser, sp_GetVoucherApprovalDashboard) / PASS / Pending / Pending |
| Notes | — |

### PAGE-007 — User Dashboard (`Dashboard/Index` when IsMainDashboard=false; also `/Dashboard/UserDashboard`)

| Attribute | Value |
|---|---|
| Source path | `Views/Dashboard/UserDashboard.cshtml` |
| Page type / purpose | list — User Dashboard (`Dashboard/Index` when IsMainDashboard=false; also `/Dashboard/UserDashboard`) |
| Legacy status | active — EMPTY SHELL (title + empty row) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-001, GAP-PLT-005 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-007-dashboard-userdashboard.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-008 — Dashboard/_FilterMasterInfo (shared report filter partial)

| Attribute | Value |
|---|---|
| Source path | `Views/Dashboard/_FilterMasterInfo.cshtml` |
| Page type / purpose | partial — Dashboard/_FilterMasterInfo (shared report filter partial) |
| Legacy status | active |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_ERPVoucherIntegrationByType`, `sp_GET_FinancialYearforOpeningBalanceDropDown` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblFinancialYear |
| APIs / AJAX | API-G1-015, 018 (+ host pages call 016) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-023, GAP-PLT-024 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-008-dashboard-filtermasterinfo-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-009 — Shared/_Layout (page shell)

| Attribute | Value |
|---|---|
| Source path | `Views/Shared/_Layout.cshtml` |
| Page type / purpose | partial — Shared/_Layout (page shell) |
| Legacy status | active |
| Batch / Fiori group | A — Application foundation / - |
| Controller / DAL | (child action Dashboard/MenuGeneration) / — |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G1-011 (child action) |
| Session variables (spec) | MenuList, loginuserempid, loginusertype, username |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-006, GAP-PLT-007, GAP-PLT-015, GAP-PLT-018, GAP-PLT-019 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-009-shared-layout.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-010 — Shared/_Partial_Menu_Leyout (DB-driven 3-level menu)

| Attribute | Value |
|---|---|
| Source path | `Views/Shared/_Partial_Menu_Leyout.cshtml` (model `IEnumerable<MenuStepOne>`) |
| Page type / purpose | partial — Shared/_Partial_Menu_Leyout (DB-driven 3-level menu) |
| Legacy status | active |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | `sp_CheckMenuAccess` **(missing locally)** |
| Tables / views / functions (local DB) | UNKNOWN (SP missing locally) |
| APIs / AJAX | – |
| Session variables (spec) | MenuList, loginuserempid |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-008, GAP-PLT-014, GAP-PLT-016 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-010-shared-menu-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-011 — Shared/Error

| Attribute | Value |
|---|---|
| Source path | `Views/Shared/Error.cshtml` |
| Page type / purpose | list — Shared/Error |
| Legacy status | active (generic, no model, no detail) |
| Batch / Fiori group | A — Application foundation / - |
| Controller / DAL | (global filter) / — |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | username |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-006, GAP-PLT-019 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-011-shared-error.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-012 — Dashboard/ApprovalDashBoard

| Attribute | Value |
|---|---|
| Source path | MISSING — `Views/Dashboard/ApprovalDashBoard.cshtml` does not exist |
| Page type / purpose | dead — Dashboard/ApprovalDashBoard |
| Legacy status | dead/broken (view-not-found; CheckAccess block commented out) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | `sp_GetApprovalMenuListByUser` **(missing locally)** |
| Tables / views / functions (local DB) | UNKNOWN (SP missing locally) |
| APIs / AJAX | – |
| Session variables (spec) | username |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-017 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-012-dashboard-approvaldashboard-missing-view.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-013 — Dashboard/VoucherApprovalDashBoard

| Attribute | Value |
|---|---|
| Source path | MISSING — `Views/Dashboard/VoucherApprovalDashBoard.cshtml` does not exist |
| Page type / purpose | dead — Dashboard/VoucherApprovalDashBoard |
| Legacy status | dead/broken (view-not-found) |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | DashboardController / DashboardDAL |
| Stored procedures | `sp_GetVoucherApprovalDashboard` **(missing locally)** |
| Tables / views / functions (local DB) | UNKNOWN (SP missing locally) |
| APIs / AJAX | – |
| Session variables (spec) | username |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadFilterInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-017 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-013-dashboard-voucherapprovaldashboard-missing-view.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-014 — Utility shared AJAX endpoint provider (no UI)

| Attribute | Value |
|---|---|
| Source path | none (`Views/Utility/` is an empty folder) |
| Page type / purpose | list — Utility shared AJAX endpoint provider (no UI) |
| Legacy status | active |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | UtilityController / ChartOfAccountDal, UtilityDAL |
| Stored procedures | `sp_AllSupplierType`, `sp_CheckMenuAccess` **(missing locally)**, `sp_DDL_AccountType`, `sp_DDL_BankBranchDDL`, `sp_DDL_Company`, `sp_DDL_CompanyUnit`, `sp_GET_ALLBankCashAccountComId`, `sp_GET_ALLCOAComId`, `sp_GET_AllBankAccountUnderCompanyID`, `sp_GET_AllBankInformationDropDown`, `sp_GET_AllCashAccountUnderCompanyID`, `sp_GET_AllCitybyCountryId`, `sp_GET_AllCountryDropDown`, `sp_GET_AllCurrencyDropDown`, `sp_GET_AllReferanceType_Ddl`, `sp_GET_AllSubsidiaryType_Ddl`, `sp_GET_BankAccountComId`, `sp_GET_BankCashAccountComId`, `sp_GET_CINoByLCNo`, `sp_GET_COAALLComId`, `sp_GET_COAComId`, `sp_GET_COASubSubControlLedgerId`, `sp_GET_CashAccountComId`, `sp_GET_ChartOfAccountDropDownById`, `sp_GET_ControllLadgerDropDown`, `sp_GET_CostCenterByCOAId`, `sp_GET_CurrencyDDL`, `sp_GET_ERPVoucherIntegrationByType`, `sp_GET_FinancialYearSingle`, `sp_GET_FinancialYearforNotLock`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_GET_InterestMethodDDL`, `sp_GET_InterestTypeDDL`, `sp_GET_LadgerHeadDropDown`, `sp_GET_LcBankPayType`, `sp_GET_LoanCategoryDDL`, `sp_GET_LoanInfobyLCNoID`, `sp_GET_ProfitCenterDDL`, `sp_GET_RefByCOAId`, `sp_GET_ReferenceByTypeDDL`, `sp_GET_ReferenceByTypeDDLCompanyWise`, `sp_GET_ReferenceTypeDDL`, `sp_GET_SubSubControlLedgerDropDown`, `sp_GET_SubsidiaryByTypeDDL`, `sp_GET_SubsidiaryDropDown`, `sp_GET_SubsidiaryTypeDDL`, `sp_GET_SubsidoryLedgerByCOAId`, `sp_GET_SupplierDDL`, `sp_GET_SupplierbyType`, `sp_Get_FinancialYearDateRange`, `sp_Get_UserInfoString` **(missing locally)** |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblSupplierType, tblAccountType, tblBankBranchInformation, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCompanyUnit, tblBankAccount, tblBankAccountUnitdtl, tblChartOfAccounts, tblCOACompanyPermission, tblBankInformation, tblCashAccount, tblCity, tblCountry, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblCOARef, tblReferenceType, tblCOASubsidiary, tblSubsidiaryType, CSTL_ERPDB_Unicorn.Table_CInvoice_Master, tblSubSubControlLedger, tblMultipleCompanySameCashAccount, tblControlLedger, tblCOACostCenter, tblCostCenter, tblFinancialYear, tblInterestMethod, tblInterestType, tblLedgerHead, tblLcBankPayType, tblLoanCategory, tblLoanInfo, tblProfitCenter, tblReferenceInfo, CSTL_ERPDB_UnicorntblSupplier, CSTL_ERPDB_Unicorn.Table_LC_LCOpenMaster, CSTL_ERPDB_Unicorn.Table_LC_PreparationMaster, SalesRollDB_ZAS.tblInvoice, CSTL_ERPDB_Unicorn.tblPurchaseOrderLocalMaster, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblSupplier, tblSubsidiaryLedger |
| APIs / AJAX | API-G1-015..076 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | GetFinansialYearNotLockedComId |
| Read / lookup actions | GetAccountNObyBankCom, GetAccountType, GetAllBankCashAccountComId, GetAllCOAComId, GetBankAccountComId, GetBankCashAccountComId, GetBankName, GetBankNameforLoan, GetCOAALLComId, GetCOAALLComIdAuto, GetCOAALLComIdByAccountId, GetCOAComId, GetCOASubSubControlLedgerId, GetCashAccountComId, GetCashAccountbyCom, GetCostCenterByCOAId, GetCostCenterByCOAIdAuto, GetCostCenterByCOAIdById, GetCurrency, GetDaterangebyFiscal, GetFinansialYearComId, GetFinansialYearNotLockedComId, GetFinansialYearSingle, GetLadgerHead, GetProfitCenterListSearch, GetRefNoByCOAId, GetRefNoByCOAIdAuto, GetRefNoByCOAIdById, GetReferanceType, GetReferenceByReferenceTypeComWiseId, GetReferenceByReferenceTypeComWiseIdSearch, GetReferenceByReferenceTypeId, GetRefferenceType, GetSubsidiaryType, GetSubsidoryLedgerByCOAId, GetSubsidoryLedgerByCOAIdAuto, GetSubsidoryLedgerByCOAIdById, GetSubsidoryLedgerBySupplierTypeId, LoadBankWiseBranch, LoadCINoByLCNo, LoadChartofAccountId, LoadCitybyCountry, LoadCompanyInfo, LoadCompanyUnitByCompanyId, LoadControlLedgerByLedgerHeadId, LoadCountry, LoadCurrencyList, LoadERPVoucherIntegration, LoadInterestMethodList, LoadInterestTypeList, LoadLoanCategoryList, LoadPaymentTypeList, LoadReferenceType, LoadSubSubControlLedgerId, LoadSubsidaryByControlLedgerId, LoadSubsidaryType, LoadSupplierList, LoadSupplierType, LoadloanAccountList, getLoadProfitCenterList |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-009, GAP-PLT-010, GAP-PLT-011, GAP-PLT-012, GAP-PLT-013, GAP-PLT-023 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-014-utility-ajax-endpoints.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: sp_Get_UserInfoString) / PASS / Pending / Pending |
| Notes | — |

### PAGE-015 — Shared JS library `AssetsStatic/JS/custom-functions.js` (no UI)

| Attribute | Value |
|---|---|
| Source path | `AssetsStatic/JS/custom-functions.js` |
| Page type / purpose | list — Shared JS library `AssetsStatic/JS/custom-functions.js` (no UI) |
| Legacy status | active (24 of 33 functions have no active caller) |
| Batch / Fiori group | A — Application foundation / - |
| Controller / DAL | – / — |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | calls Utility (3 live endpoints) + 19 non-existent endpoints |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-011 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-015-custom-functions-js.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-016 — Logout (action; renders login view)

| Attribute | Value |
|---|---|
| Source path | `Views/Home/Index.cshtml` via `View("Index")` |
| Page type / purpose | list — Logout (action; renders login view) |
| Legacy status | active |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | – |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-007 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-016-logout.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-017 — Home/CheckLogin (public bool method = routable GET action, no view)

| Attribute | Value |
|---|---|
| Source path | none (returns text `True`/`False`) |
| Page type / purpose | list — Home/CheckLogin (public bool method = routable GET action, no view) |
| Legacy status | active, unintended endpoint |
| Batch / Fiori group | A — Application foundation / More |
| Controller / DAL | HomeController / DashboardDAL, HomeDAL, UtilityDAL |
| Stored procedures | `sp_GET_UserInfo_Acc` |
| Tables / views / functions (local DB) | tblUser |
| APIs / AJAX | – |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter), anonymous (login) |
| CRUD / state-changing actions | LockScreen |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-PLT-003 |
| Legacy spec | `specs/01-platform-auth-dashboard/pages/PAGE-017-home-checklogin-action.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-020 — Financial Year List

| Attribute | Value |
|---|---|
| Source path | CSTL_Pro_to_Pay/Views/FinancialYear/FinancialyearList.cshtml |
| Page type / purpose | list — Financial Year List |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | FinancialYearController / FinalCialYearDal |
| Stored procedures | `sp_DDL_Company`, `sp_GET_FinancialYear` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo |
| APIs / AJAX | API-G2A-003; Utility/LoadCompanyInfo |
| Session variables (spec) | LoginUserId, MenuList |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | FinancialyearLock, Lock_Financialyear, Save_Financialyear |
| Read / lookup actions | FinancialyearList, Get_Financialyear, Get_Financialyear_ById |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-001, GAP-MST-002, GAP-MST-008, GAP-MST-012, GAP-MST-014 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-020-financial-year-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-021 — Financial Year Create/Edit

| Attribute | Value |
|---|---|
| Source path | .../Views/FinancialYear/FinancialyearCreate.cshtml |
| Page type / purpose | entry — Financial Year Create/Edit |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | FinancialYearController / FinalCialYearDal |
| Stored procedures | `sp_DDL_Company`, `sp_GET_FinancialYearById`, `sp_I_FinancialYear_New`, `sp_UD_FinancialYear_New` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblFinancialYear (W) |
| APIs / AJAX | API-G2A-004, API-G2A-005; Utility/LoadCompanyInfo |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | FinancialyearLock, Lock_Financialyear, Save_Financialyear |
| Read / lookup actions | FinancialyearList, Get_Financialyear, Get_Financialyear_ById |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-003, GAP-MST-006, GAP-MST-007, GAP-MST-011, GAP-MST-015 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-021-financial-year-create-edit.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-022 — Financial Year Lock

| Attribute | Value |
|---|---|
| Source path | .../Views/FinancialYear/FinancialyearLock.cshtml |
| Page type / purpose | list — Financial Year Lock |
| Legacy status | partial (no inbound link in code; copy-paste residue) |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | FinancialYearController / FinalCialYearDal |
| Stored procedures | `sp_DDL_Company`, `sp_GET_FinancialYearforNotLock`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Get_FinancialYearDateRange`, `sp_Lock_FinancialYear_New` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblFinancialYear (W) |
| APIs / AJAX | API-G2A-007; Utility/GetFinansialYearNotLockedComId, LoadCompanyInfo; OpeningBlnc/Get_OpningBlnc_ById (only id>0, residue) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | FinancialyearLock, Lock_Financialyear, Save_Financialyear |
| Read / lookup actions | FinancialyearList, Get_Financialyear, Get_Financialyear_ById |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-003, GAP-MST-004, GAP-MST-005, GAP-MST-009, GAP-MST-016, GAP-MST-017 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-022-financial-year-lock.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-023 — Bank Account List

| Attribute | Value |
|---|---|
| Source path | .../Views/BanckAccount/Index.cshtml |
| Page type / purpose | list — Bank Account List |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Bank & Cash Reconciliation (83) |
| Controller / DAL | BanckAccountController / BanckAccountDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AllBankAccount`, `sp_I_BankAccount_Unit` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblBankAccount, tblBankAccountUnitdtl (W), tblBankInformation, tblControlLedger, tblLedgerHead, tblMultipleCompanySameBankAccount, tblSubsidiaryLedger, tblSubSubControlLedger, tblChartOfAccounts, tblCOACompanyPermission (W) |
| APIs / AJAX | API-G2A-012; Utility/LoadCompanyInfo |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBankAccount |
| Read / lookup actions | BankAccountList, Get_BankAcoount_Edit |
| Reports / export / print / files | Excel/CSV export, PDF export, Crystal print (blocked), DataTables grid, SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-002, GAP-MST-008, GAP-MST-020, GAP-MST-029, GAP-MST-030 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-023-bank-account-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-024 — Bank Account Create/Edit

| Attribute | Value |
|---|---|
| Source path | .../Views/BanckAccount/Create.cshtml |
| Page type / purpose | entry — Bank Account Create/Edit |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Bank & Cash Reconciliation (83) |
| Controller / DAL | BanckAccountController / BanckAccountDAL |
| Stored procedures | `sp_DDL_AccountType`, `sp_DDL_Company`, `sp_DDL_CompanyUnit`, `sp_GET_AllBankInformationDropDown`, `sp_GET_AllCurrencyDropDown`, `sp_GET_BankAccountById`, `sp_GET_BankAccountComId`, `sp_GET_ControllLadgerDropDown`, `sp_GET_LadgerHeadDropDown`, `sp_GET_SubSubControlLedgerDropDown`, `sp_GET_SubsidiaryDropDown`, `sp_I_BankAccount_New`, `sp_I_BankAccount_Unit`, `sp_I_ChartOfAccount_New`, `sp_UD_BankAccount_New`, `sp_UD_ChartOfAccount_New`, `sp_check_BankCashUp` |
| Tables / views / functions (local DB) | tblAccountType, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCompanyUnit, tblBankInformation, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblBankAccount (W), tblControlLedger, tblLedgerHead, tblMultipleCompanySameBankAccount, tblSubsidiaryLedger, tblSubSubControlLedger, tblBankAccountUnitdtl (W), tblChartOfAccounts (W), tblCOACompanyPermission (W), tblSubsubsidiaryLadger, tblCashAccount |
| APIs / AJAX | API-G2A-010, API-G2A-011; Utility/GetBankName, GetCurrency, GetAccountType, GetLadgerHead, LoadSubsidaryByControlLedgerId, LoadSubSubControlLedgerId, LoadCompanyInfo; SubControl/Get_ControlLedger_By_Ledger_DDL |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBankAccount |
| Read / lookup actions | BankAccountList, Get_BankAcoount_Edit |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-021, GAP-MST-022, GAP-MST-023, GAP-MST-024, GAP-MST-025, GAP-MST-026, GAP-MST-027, GAP-MST-028, GAP-MST-031, GAP-MST-032, GAP-MST-033, GAP-MST-034 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-024-bank-account-create-edit.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-025 — Cash Account List

| Attribute | Value |
|---|---|
| Source path | .../Views/CashAccount/Index.cshtml |
| Page type / purpose | list — Cash Account List |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Bank & Cash Reconciliation (83) |
| Controller / DAL | CashAccountController / CashAccountDal |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AllCashAccount`, `sp_GET_AllCashAccountUnderCompanyID`, `sp_I_MultipleCompanySameCashAccount_New` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCashAccount, tblControlLedger, tblLedgerHead, tblMultipleCompanySameCashAccount (W), tblSubsidiaryLedger, tblSubSubControlLedger, tblChartOfAccounts, tblCOACompanyPermission (W) |
| APIs / AJAX | API-G2A-017; Utility/LoadCompanyInfo |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveCashAccount |
| Read / lookup actions | CashAccountList, Get_CashAcoount_Edit |
| Reports / export / print / files | Excel/CSV export, PDF export, Crystal print (blocked), DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-002, GAP-MST-008, GAP-MST-029, GAP-MST-030, GAP-MST-035 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-025-cash-account-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-026 — Cash Account Create/Edit

| Attribute | Value |
|---|---|
| Source path | .../Views/CashAccount/Create.cshtml |
| Page type / purpose | entry — Cash Account Create/Edit |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Bank & Cash Reconciliation (83) |
| Controller / DAL | CashAccountController / CashAccountDal |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AllCashAccountUnderCompanyID`, `sp_GET_AllCurrencyDropDown`, `sp_GET_CashAccountById_new`, `sp_GET_ControllLadgerDropDown`, `sp_GET_LadgerHeadDropDown`, `sp_GET_SubSubControlLedgerDropDown`, `sp_GET_SubsidiaryDropDown`, `sp_I_CashAccount_New`, `sp_I_ChartOfAccount_New`, `sp_I_MultipleCompanySameCashAccount_New`, `sp_UD_CashAccount_New`, `sp_UD_ChartOfAccount_New`, `sp_check_BankCashUp` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCashAccount (W), tblChartOfAccounts (W), tblCOACompanyPermission (W), CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblControlLedger, tblLedgerHead, tblMultipleCompanySameCashAccount (W), tblSubsidiaryLedger, tblSubSubControlLedger, tblSubsubsidiaryLadger, tblBankAccount |
| APIs / AJAX | API-G2A-015, API-G2A-016; Utility/GetCurrency, GetLadgerHead, LoadSubsidaryByControlLedgerId, LoadSubSubControlLedgerId, LoadCompanyInfo, GetBankName*, GetAccountType* (*targets missing); SubControl/Get_ControlLedger_By_Ledger_DDL |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveCashAccount |
| Read / lookup actions | CashAccountList, Get_CashAcoount_Edit |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-023, GAP-MST-024, GAP-MST-025, GAP-MST-028, GAP-MST-034, GAP-MST-040, GAP-MST-041, GAP-MST-042, GAP-MST-043 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-026-cash-account-create-edit.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-027 — Currency Conversion Rate

| Attribute | Value |
|---|---|
| Source path | .../Views/CurrencyConversion/CCCreate.cshtml (layout via _ViewStart) |
| Page type / purpose | entry — Currency Conversion Rate |
| Legacy status | partial (no inbound link in code) |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | CurrencyConversionController / BankPaymentsDAL |
| Stored procedures | `sp_GET_AllCurrencyConversionByDate`, `sp_GET_AllCurrencyDropDown`, `sp_del_CurrencyConversion`, `sp_iNS_CurrencyConversion` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCurrency, tblCurrencyConversionRate (W), tblCurrency, tblCurrencyConversionRateLog (W) |
| APIs / AJAX | API-G2A-019, API-G2A-020 |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveCurrencyConversion |
| Read / lookup actions | Load_CurrencyConversionList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-060, GAP-MST-061, GAP-MST-062, GAP-MST-063, GAP-MST-064, GAP-MST-065, GAP-MST-066, GAP-MST-067 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-027-currency-conversion-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-040 — Reference Type Entry

| Attribute | Value |
|---|---|
| Source path | `Views/Ref_Sub_Type/ReferenceType.cshtml` |
| Page type / purpose | list — Reference Type Entry |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | Ref_Sub_TypeController / ReferenceTypeDal, SubsidaryTypeDal |
| Stored procedures | `sp_GET_AllReferanceType_Ddl`, `sp_GET_RefByCOAId`, `sp_GetReferenceTypeByGlId` **(missing locally)**, `sp_Get_ReferenceType_ById`, `sp_I_COARefDetail`, `sp_I_ReferenceType`, `sp_UD_ReferenceType`, `sp_check_ReferenceType` |
| Tables / views / functions (local DB) | tblCOARef (W), tblReferenceType (W), tblReferenceInfo |
| APIs / AJAX | API-G2B-004, 005 |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_ReferenceType, Save_SubsidairyType |
| Read / lookup actions | GetSubsidairyType_ById, Get_ReferenceType, Get_SubsidairyType, ReferenceTypeList, SubsidairyList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-MST-201, GAP-MST-202, GAP-MST-203, GAP-MST-204, GAP-MST-205, GAP-MST-221, GAP-MST-224 |
| Legacy spec | `specs/02-master-setup/pages/PAGE-040-reference-type-entry.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: sp_GetReferenceTypeByGlId) / PASS / Pending / Pending |
| Notes | — |

### PAGE-041 — Reference Type List

| Attribute | Value |
|---|---|
| Source path | `Views/Ref_Sub_Type/ReferenceTypeList.cshtml` |
| Page type / purpose | list — Reference Type List |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | Ref_Sub_TypeController / ReferenceTypeDal, SubsidaryTypeDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-003 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_ReferenceType, Save_SubsidairyType |
| Read / lookup actions | GetSubsidairyType_ById, Get_ReferenceType, Get_SubsidairyType, ReferenceTypeList, SubsidairyList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-041-reference-type-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-042 — Subsidiary Type Entry

| Attribute | Value |
|---|---|
| Source path | `Views/Ref_Sub_Type/SubsidairyType.cshtml` |
| Page type / purpose | list — Subsidiary Type Entry |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | Ref_Sub_TypeController / ReferenceTypeDal, SubsidaryTypeDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-009, 010 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_ReferenceType, Save_SubsidairyType |
| Read / lookup actions | GetSubsidairyType_ById, Get_ReferenceType, Get_SubsidairyType, ReferenceTypeList, SubsidairyList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-042-subsidiary-type-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-043 — Subsidiary Type List

| Attribute | Value |
|---|---|
| Source path | `Views/Ref_Sub_Type/SubsidairyList.cshtml` |
| Page type / purpose | list — Subsidiary Type List |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | Ref_Sub_TypeController / ReferenceTypeDal, SubsidaryTypeDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-008 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_ReferenceType, Save_SubsidairyType |
| Read / lookup actions | GetSubsidairyType_ById, Get_ReferenceType, Get_SubsidairyType, ReferenceTypeList, SubsidairyList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-043-subsidiary-type-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-044 — Reference Info

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/ReferenceInfo.cshtml` |
| Page type / purpose | list — Reference Info |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-032, 016, 018, 034 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-044-reference-info.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-045 — Reference Info checklist partial

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/_RefCheckInfo.cshtml` |
| Page type / purpose | partial — Reference Info checklist partial |
| Legacy status | active (partial) |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-016 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-045-reference-info-checklist-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-046 — Subsidiary Info

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/SubsidiaryInfo.cshtml` |
| Page type / purpose | list — Subsidiary Info |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-033, 015, 017, 035 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-046-subsidiary-info.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-047 — Subsidiary Info checklist partial

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/_SubsidiaryInfo.cshtml` |
| Page type / purpose | partial — Subsidiary Info checklist partial |
| Legacy status | active (partial) |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-015 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-047-subsidiary-info-checklist-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-048 — Cost Centre Entry

| Attribute | Value |
|---|---|
| Source path | `Views/CostCentre/CostcentreInformation.cshtml` |
| Page type / purpose | list — Cost Centre Entry |
| Legacy status | active |
| Batch / Fiori group | B — Master setup / Cost Centre & Budget Control (85) |
| Controller / DAL | CostCentreController (Account_Master) / CostcentreDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-026, 024, 025 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Del_CostCenter_Info_By_Id, Save_CostCenter, Save_CostCenter_Rana |
| Read / lookup actions | CostcentreList, GetCostCentre_ById_Rana, Get_CostCenter_ById, Get_CostCentre, Get_CostCentre_R, Load_Company_Info, Load_Company_Info_ById |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-048-cost-centre-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-049 — Cost Centre List

| Attribute | Value |
|---|---|
| Source path | `Views/CostCentre/CostcentreList.cshtml` |
| Page type / purpose | dead — Cost Centre List |
| Legacy status | active (delete UI dead) |
| Batch / Fiori group | B — Master setup / Cost Centre & Budget Control (85) |
| Controller / DAL | CostCentreController (Account_Master) / CostcentreDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G2B-023, (028 dead UI) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Del_CostCenter_Info_By_Id, Save_CostCenter, Save_CostCenter_Rana |
| Read / lookup actions | CostcentreList, GetCostCentre_ById_Rana, Get_CostCenter_ById, Get_CostCentre, Get_CostCentre_R, Load_Company_Info, Load_Company_Info_ById |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid, SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-049-cost-centre-list.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-050 — Reference Type (Refference stub)

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/ReferenceType.cshtml` |
| Page type / purpose | list — Reference Type (Refference stub) |
| Legacy status | stale/stub (no links, no script) |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-050-reference-type-stub.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-051 — Subsidiary Type (Refference stub)

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/SubsidiaryType.cshtml` |
| Page type / purpose | list — Subsidiary Type (Refference stub) |
| Legacy status | stale/stub |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-051-subsidiary-type-stub.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-052 — Cost Center Info (Refference stub)

| Attribute | Value |
|---|---|
| Source path | `Views/Refference/CostCenterInfo.cshtml` |
| Page type / purpose | list — Cost Center Info (Refference stub) |
| Legacy status | stale/stub |
| Batch / Fiori group | B — Master setup / Chart of Accounts (80) |
| Controller / DAL | RefferenceController / Ref_sub_InfoDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | LoadRefTableInfo, LoadReferenceInfoInPartialView, LoadSubsidiaryInfoInPartialView, LoadSubsidiaryTableInfo |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-052-cost-center-info-stub.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-060 — Ledger Head List

| Attribute | Value |
|---|---|
| Source path | Views/LedgerHead/LedgerHeadList.cshtml |
| Page type / purpose | list — Ledger Head List |
| Legacy status | active (read-only, no menu link in repo) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | LedgerHeadController / — |
| Stored procedures | `sp_GET_LadgerHead`, `sp_GET_LadgerHeadDropDown` |
| Tables / views / functions (local DB) | tblLedgerHead |
| APIs / AJAX | API-G3A-002 |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | Get_LedgerHead_List, LedgerHeadList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-COA-001, GAP-COA-002, GAP-COA-003, GAP-COA-004, GAP-COA-005 |
| Legacy spec | `specs/03-chart-of-accounts/pages/PAGE-060-ledger-head-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-061 — Control Ledger List

| Attribute | Value |
|---|---|
| Source path | Views/ControlLedger/ControlLedgerList.cshtml |
| Page type / purpose | list — Control Ledger List |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ControlLedgerController / ControlLedgerDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-006 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_EmployeeInformation |
| Read / lookup actions | ControlLedgerList, Get_ControlLed_ById, Get_ControlLedger_List, Get_Ledger_DDL |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-061-control-ledger-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-062 — Control Ledger Entry

| Attribute | Value |
|---|---|
| Source path | Views/ControlLedger/ControlLedgerCreate.cshtml |
| Page type / purpose | entry — Control Ledger Entry |
| Legacy status | active (defects) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ControlLedgerController / ControlLedgerDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-005,007,008 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_EmployeeInformation |
| Read / lookup actions | ControlLedgerList, Get_ControlLed_ById, Get_ControlLedger_List, Get_Ledger_DDL |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-062-control-ledger-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-063 — Sub-Control Ledger List

| Attribute | Value |
|---|---|
| Source path | Views/SubControl/SubControlList.cshtml |
| Page type / purpose | list — Sub-Control Ledger List |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubControlController / SubcontrolLedger |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-012 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveSubcontrolInformation |
| Read / lookup actions | Get_ControlLedger_By_Ledger_DDL, Get_Ledger_DDL, Get_SubControlLed_ById, Get_SubControll_List, SubControlList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-063-sub-control-ledger-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-064 — Sub-Control Ledger Entry

| Attribute | Value |
|---|---|
| Source path | Views/SubControl/SubControlCeate.cshtml |
| Page type / purpose | list — Sub-Control Ledger Entry |
| Legacy status | active (defects) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubControlController / SubcontrolLedger |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-005(ControlLedger),013,014,015 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveSubcontrolInformation |
| Read / lookup actions | Get_ControlLedger_By_Ledger_DDL, Get_Ledger_DDL, Get_SubControlLed_ById, Get_SubControll_List, SubControlList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-064-sub-control-ledger-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-065 — Sub Sub Control Ledger List

| Attribute | Value |
|---|---|
| Source path | Views/SubSubControlLedger/SSCLList.cshtml |
| Page type / purpose | list — Sub Sub Control Ledger List |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubSubControlLedgerController / SubSubControlLedgerDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-019 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_SSCL |
| Read / lookup actions | Get_SSCLById, Get_SSCLList, SSCLList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-065-sub-sub-control-ledger-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-066 — Sub Sub Control Ledger Entry

| Attribute | Value |
|---|---|
| Source path | Views/SubSubControlLedger/SSCLCreate.cshtml |
| Page type / purpose | entry — Sub Sub Control Ledger Entry |
| Legacy status | active (defects) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubSubControlLedgerController / SubSubControlLedgerDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-013,018,020 + Utility.GetLadgerHead, Utility.LoadSubsidaryByControlLedgerId |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_SSCL |
| Read / lookup actions | Get_SSCLById, Get_SSCLList, SSCLList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-066-sub-sub-control-ledger-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-067 — Subsidiary Ledger List

| Attribute | Value |
|---|---|
| Source path | Views/Subsubsidairy/Index.cshtml |
| Page type / purpose | list — Subsidiary Ledger List |
| Legacy status | partial (row edit link goes to wrong page) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubsubsidairyController / SubsubsidairyDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-024 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_SubsidiaryLedger |
| Read / lookup actions | Get_SubsidiaryLedgerList |
| Reports / export / print / files | DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-067-subsidiary-ledger-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-068 — Subsidiary Ledger Entry

| Attribute | Value |
|---|---|
| Source path | Views/Subsubsidairy/SubsubsidairyCreate.cshtml |
| Page type / purpose | entry — Subsidiary Ledger Entry |
| Legacy status | partial (create works; edit broken) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | SubsubsidairyController / SubsubsidairyDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3A-005(ControlLedger),013,023, ChartOfAccount.Get_Sub_ControlLed_ById, Utility.LoadChartofAccountId, SubControl.Get_SubControlLed_ById (edit load) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_SubsidiaryLedger |
| Read / lookup actions | Get_SubsidiaryLedgerList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/PAGE-068-subsidiary-ledger-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-080 — Chart Of Account Entry (create/edit GL)

| Attribute | Value |
|---|---|
| Source path | `CSTL_Pro_to_Pay/Views/ChartOfAccount/ChartOfAccountCreate.cshtml` |
| Page type / purpose | entry — Chart Of Account Entry (create/edit GL) |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ChartOfAccountController / ChartOfAccountDal |
| Stored procedures | `sp_CheckAccountCodeExists`, `sp_DDL_Company`, `sp_DDL_CompanyUnit`, `sp_GET_ALLCOAComId`, `sp_GET_AccountCodebySubSubControlLedgerId`, `sp_GET_AllReferanceType_Ddl`, `sp_GET_AllSubsidiaryType_Ddl`, `sp_GET_ChartofAccount_ById`, `sp_GET_ControllLadgerDropDown`, `sp_GET_CostCenter_Ddl`, `sp_GET_LadgerHeadDropDown`, `sp_GET_SubSubControlLedgerDropDown`, `sp_GET_SubsidiaryDropDown`, `sp_I_COACostCenterDetail`, `sp_I_COARefDetail`, `sp_I_COASubsidiaryDetail`, `sp_I_ChartOfAccount`, `sp_I_MultipleCOACompanyPermission`, `sp_UP_ChartOfAccount_new` |
| Tables / views / functions (local DB) | tblChartOfAccounts (W), CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCompanyUnit, tblCOACompanyPermission (W), tblCOARef (W), tblReferenceType, tblCOASubsidiary (W), tblSubsidiaryType, tblControlLedger, tblLedgerHead, tblSubsidiaryLedger, tblSubSubControlLedger, tblCOACostCenter (W), tblCostCenter |
| APIs / AJAX | API-G3B-009,010,012,014,007,004,005,006 + Utility.GetLadgerHead/LoadControlLedgerByLedgerHeadId/LoadSubsidaryByControlLedgerId/LoadSubSubControlLedgerId |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveRefInfoDetails, SaveSubInfoDetails, Save_ChartOfAccount |
| Read / lookup actions | ChartOfAccountList, GetAccountCodebySubSubControlLedgerId, GetChartofAccount_ByCompanyId, Get_ChartofAccount_ById, Get_ChartofAccount_List, Get_CompanyUnit_DDL, Get_Company_DDL, Get_Sub_ControlLed_ById, LoadCostCenterInPartialView, LoadReferanceInPartialView, LoadSubSidaryInPartialView |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-COA-200, GAP-COA-203, GAP-COA-204, GAP-COA-205, GAP-COA-206, GAP-COA-207, GAP-COA-208, GAP-COA-209, GAP-COA-210, GAP-COA-211, GAP-COA-212, GAP-COA-214, GAP-COA-216 |
| Legacy spec | `specs/03-chart-of-accounts/pages/PAGE-080-chart-of-account-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-081 — Chart Of Account List

| Attribute | Value |
|---|---|
| Source path | `Views/ChartOfAccount/ChartOfAccountList.cshtml` |
| Page type / purpose | list — Chart Of Account List |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ChartOfAccountController / ChartOfAccountDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-008 + Utility ledger cascades + Utility.LoadCompanyInfo |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveRefInfoDetails, SaveSubInfoDetails, Save_ChartOfAccount |
| Read / lookup actions | ChartOfAccountList, GetAccountCodebySubSubControlLedgerId, GetChartofAccount_ByCompanyId, Get_ChartofAccount_ById, Get_ChartofAccount_List, Get_CompanyUnit_DDL, Get_Company_DDL, Get_Sub_ControlLed_ById, LoadCostCenterInPartialView, LoadReferanceInPartialView, LoadSubSidaryInPartialView |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-081-chart-of-account-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-082 — Reference-type partial (_RefPartial)

| Attribute | Value |
|---|---|
| Source path | `Views/ChartOfAccount/_RefPartial.cshtml` |
| Page type / purpose | partial — Reference-type partial (_RefPartial) |
| Legacy status | active (partial) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ChartOfAccountController / ChartOfAccountDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-004 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveRefInfoDetails, SaveSubInfoDetails, Save_ChartOfAccount |
| Read / lookup actions | ChartOfAccountList, GetAccountCodebySubSubControlLedgerId, GetChartofAccount_ByCompanyId, Get_ChartofAccount_ById, Get_ChartofAccount_List, Get_CompanyUnit_DDL, Get_Company_DDL, Get_Sub_ControlLed_ById, LoadCostCenterInPartialView, LoadReferanceInPartialView, LoadSubSidaryInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-082-ref-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-083 — Subsidiary-type partial (_SubSidaryPartial)

| Attribute | Value |
|---|---|
| Source path | `Views/ChartOfAccount/_SubSidaryPartial.cshtml` |
| Page type / purpose | partial — Subsidiary-type partial (_SubSidaryPartial) |
| Legacy status | active (partial) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ChartOfAccountController / ChartOfAccountDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-005 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveRefInfoDetails, SaveSubInfoDetails, Save_ChartOfAccount |
| Read / lookup actions | ChartOfAccountList, GetAccountCodebySubSubControlLedgerId, GetChartofAccount_ByCompanyId, Get_ChartofAccount_ById, Get_ChartofAccount_List, Get_CompanyUnit_DDL, Get_Company_DDL, Get_Sub_ControlLed_ById, LoadCostCenterInPartialView, LoadReferanceInPartialView, LoadSubSidaryInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-083-subsidiary-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-084 — Cost-centre partial (_CostCenterPartial)

| Attribute | Value |
|---|---|
| Source path | `Views/ChartOfAccount/_CostCenterPartial.cshtml` |
| Page type / purpose | partial — Cost-centre partial (_CostCenterPartial) |
| Legacy status | active (partial) |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | ChartOfAccountController / ChartOfAccountDal |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-006 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveRefInfoDetails, SaveSubInfoDetails, Save_ChartOfAccount |
| Read / lookup actions | ChartOfAccountList, GetAccountCodebySubSubControlLedgerId, GetChartofAccount_ByCompanyId, Get_ChartofAccount_ById, Get_ChartofAccount_List, Get_CompanyUnit_DDL, Get_Company_DDL, Get_Sub_ControlLed_ById, LoadCostCenterInPartialView, LoadReferanceInPartialView, LoadSubSidaryInPartialView |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-084-cost-center-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-085 — Opening Balance Entry

| Attribute | Value |
|---|---|
| Source path | `Views/OpeningBlnc/OpningBlncEntry.cshtml` |
| Page type / purpose | entry — Opening Balance Entry |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | OpeningBlncController (Controllers/Accounts) / OpeningBlncDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-019,021 + Utility.LoadCompanyInfo/GetCurrency/GetFinansialYearComId/GetAllCOAComId |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_OpningBlnc |
| Read / lookup actions | Get_OpningBlncList, Get_OpningBlnc_ById, OpningBlncList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-085-opening-balance-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-086 — Opening Balance List

| Attribute | Value |
|---|---|
| Source path | `Views/OpeningBlnc/OpningBlncList.cshtml` |
| Page type / purpose | list — Opening Balance List |
| Legacy status | active |
| Batch / Fiori group | C — Chart of accounts & opening balance / Chart of Accounts (80) |
| Controller / DAL | OpeningBlncController / OpeningBlncDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G3B-020 + Utility.GetFinansialYearComId |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Save_OpningBlnc |
| Read / lookup actions | Get_OpningBlncList, Get_OpningBlnc_ById, OpningBlncList |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-086-opening-balance-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-100 — Debit Voucher List

| Attribute | Value |
|---|---|
| Source path | Views/BankPayments/Index.cshtml |
| Page type / purpose | list — Debit Voucher List |
| Legacy status | active |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | Controllers/Accounts/BankPaymentsController.cs / BankPaymentsDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AllReportHeaderReport`, `sp_GET_DebitVoucherList`, `sp_GET_ERPVoucherIntegrationByType`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblReportHeading, tblRptImage, tblFinancialYear, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster (W), tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice, fnSplit |
| APIs / AJAX | API-G4-005, Utility/LoadERPVoucherIntegration, Dashboard/LoadFilterInPartialView, AccountingTaskReport/rptDebitVoucher |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBankPayment |
| Read / lookup actions | BankAccountList, GetBankCashInfoMasterId, GetBankInfoMasterId, GetBankPaymentMasterId, GetCashInfoIndoById |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-001, GAP-VCH-005, GAP-VCH-012, GAP-VCH-019, GAP-VCH-024, GAP-VCH-026, GAP-VCH-031 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-100-bankpayments-index-debit-voucher-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-101 — Debit Voucher Entry (bank/cash payment)

| Attribute | Value |
|---|---|
| Source path | Views/BankPayments/Create.cshtml |
| Page type / purpose | entry — Debit Voucher Entry (bank/cash payment) |
| Legacy status | active |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | BankPaymentsController / BankPaymentsDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AccountClosingBalance_TrialBalanceWise`, `sp_GET_AllCurrencyDropDown`, `sp_GET_BankAccountComId`, `sp_GET_BankInfoByAccountId`, `sp_GET_BankPaymentDetailById`, `sp_GET_BankPaymentMasterById`, `sp_GET_COAALLComId`, `sp_GET_CashAccountComId`, `sp_GET_CashInfoByAccountId`, `sp_GET_CostCenterByCOAId`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_GET_RefByCOAId`, `sp_GET_SubsidoryLedgerByCOAId`, `sp_Get_FinancialYearDateRange`, `sp_I_VoucherDetail_new`, `sp_I_VoucherMaster`, `sp_UD_VoucherMaster_New`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblChartOfAccounts, tblControlLedger, tblDebitCreditVoucherDetail (W), tblDebitCreditVoucherMaster (W), tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblBankAccount, tblBankAccountUnitdtl, tblBankInformation, tblCOACompanyPermission, tblCashAccount, tblMultipleCompanySameCashAccount, tblCOACostCenter, tblCostCenter, tblFinancialYear, tblCOARef, tblReferenceInfo, tblReferenceType, tblCOASubsidiary, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, tblSubsidiaryInfo, tblSubsidiaryType, CSTL_ERPDB_Unicorn.tblSupplier, fnSplit |
| APIs / AJAX | API-G4-003/004/006/007/008/002, Utility/* (17 endpoints) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBankPayment |
| Read / lookup actions | BankAccountList, GetBankCashInfoMasterId, GetBankInfoMasterId, GetBankPaymentMasterId, GetCashInfoIndoById |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-001, GAP-VCH-002, GAP-VCH-003, GAP-VCH-004, GAP-VCH-005, GAP-VCH-006, GAP-VCH-011, GAP-VCH-013, GAP-VCH-014, GAP-VCH-015, GAP-VCH-016, GAP-VCH-017, GAP-VCH-018, GAP-VCH-020, GAP-VCH-026, GAP-VCH-027, GAP-VCH-028, GAP-VCH-029, GAP-VCH-030, GAP-VCH-035 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-101-bankpayments-create-debit-voucher-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-102 — Voucher detail row partial

| Attribute | Value |
|---|---|
| Source path | Views/BankPayments/_addRowForCOA.cshtml |
| Page type / purpose | dead — Voucher detail row partial |
| Legacy status | active (Credit twin action dead) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | BankPaymentsController (also dead twin in CreditVoucherController) / BankPaymentsDAL |
| Stored procedures | `sp_GET_COAALLComId`, `sp_GET_CostCenterByCOAId`, `sp_GET_RefByCOAId`, `sp_GET_SubsidoryLedgerByCOAId` |
| Tables / views / functions (local DB) | tblChartOfAccounts, tblCOACompanyPermission, tblSubSubControlLedger, tblCOACostCenter, tblCostCenter, tblCOARef, tblReferenceInfo, tblReferenceType, tblCOASubsidiary, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, tblSubsidiaryInfo, tblSubsidiaryType, CSTL_ERPDB_Unicorn.tblSupplier |
| APIs / AJAX | none |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBankPayment |
| Read / lookup actions | BankAccountList, GetBankCashInfoMasterId, GetBankInfoMasterId, GetBankPaymentMasterId, GetCashInfoIndoById |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-007, GAP-VCH-029, GAP-VCH-036, GAP-VCH-037 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-102-bankpayments-addrowforcoa-partial.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-103 — Debit/Payment Voucher (legacy eflag ed/ap)

| Attribute | Value |
|---|---|
| Source path | Views/DebitVoucher/Create.cshtml |
| Page type / purpose | entry — Debit/Payment Voucher (legacy eflag ed/ap) |
| Legacy status | partial/stale (non-functional: missing partial + actions) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | Controllers/Accounts/DebitVoucherController.cs / DebitVoucherDAL |
| Stored procedures | `sp_DDL_Company`, `sp_DDL_CompanyUnit`, `sp_GET_AllCurrencyDropDown`, `sp_GetBankBranchByGlId` **(missing locally)**, `sp_GetCostCenterByGlId` **(missing locally)**, `sp_GetGLInfoById` **(missing locally)**, `sp_GetRefDetailsByRefTypeId` **(missing locally)**, `sp_GetReferenceTypeByGlId` **(missing locally)**, `sp_Get_VoucherDetailsById` **(missing locally)**, `sp_Get_VoucherMasterById` **(missing locally)**, `sp_I_VoucherMaster`, `sp_Save_DebitVoucherDetails` **(missing locally)**, `sp_Save_DebitVoucherMaster` **(missing locally)**, `sp_Save_DebitVoucherTransaction` **(missing locally)** |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCompanyUnit, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblDebitCreditVoucherDetail (W), tblDebitCreditVoucherMaster (W) |
| APIs / AJAX | API-G4-010..019 (incl. referenced-but-missing actions) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveVoucher |
| Read / lookup actions | DebitVoucherListData, GetBankBranch, GetGLDetails, GetVoucherByMasterId, LoadCostCenterByGlId, LoadRefDetailsByRefTypeId, LoadReferenceTypeByGlId |
| Reports / export / print / files | SweetAlert confirm, date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-001, GAP-VCH-004, GAP-VCH-008, GAP-VCH-009, GAP-VCH-010, GAP-VCH-033, GAP-VCH-041 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-103-debitvoucher-create-legacy-payment-voucher.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: sp_GetBankBranchByGlId, sp_GetCostCenterByGlId, sp_GetGLInfoById, sp_GetRefDetailsByRefTypeId) / PASS / Pending / Pending |
| Notes | — |

### PAGE-104 — Credit Voucher List

| Attribute | Value |
|---|---|
| Source path | Views/CreditVoucher/CVList.cshtml |
| Page type / purpose | list — Credit Voucher List |
| Legacy status | active |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | Controllers/Accounts/CreditVoucherController.cs / BankPaymentsDAL, CreditVoucherDAL |
| Stored procedures | `sp_GET_DebitVoucherList`, `sp_GET_ERPVoucherIntegrationByType`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice` |
| Tables / views / functions (local DB) | tblFinancialYear, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCompanyInfo, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster, tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice |
| APIs / AJAX | API-G4-020, API-G4-024, Utility/LoadERPVoucherIntegration, rptDebitVoucher |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveCRVoucher |
| Read / lookup actions | BankAccountList, CVList, GetCreditMasterId |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-005, GAP-VCH-012, GAP-VCH-019, GAP-VCH-023, GAP-VCH-024, GAP-VCH-026, GAP-VCH-031 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-104-creditvoucher-cvlist-credit-voucher-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-105 — Credit Voucher Entry

| Attribute | Value |
|---|---|
| Source path | Views/CreditVoucher/CVCreate.cshtml |
| Page type / purpose | entry — Credit Voucher Entry |
| Legacy status | active |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | CreditVoucherController / BankPaymentsDAL, CreditVoucherDAL |
| Stored procedures | `sp_GET_AccountClosingBalance_TrialBalanceWise`, `sp_GET_BankInfoByAccountId`, `sp_GET_BankPaymentMasterById`, `sp_GET_CashInfoByAccountId`, `sp_GET_CreditVoucherDetailById`, `sp_I_VoucherDetail_new`, `sp_I_VoucherMaster`, `sp_UD_VoucherMaster_New`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | tblChartOfAccounts, tblControlLedger, tblDebitCreditVoucherDetail (W), tblDebitCreditVoucherMaster (W), tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, tblBankAccount, tblBankInformation, tblCashAccount, fnSplit |
| APIs / AJAX | API-G4-021, 023, 025, BankPayments/GetBankInfoMasterId, GetCashInfoIndoById, AddRowForCOA, Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveCRVoucher |
| Read / lookup actions | BankAccountList, CVList, GetCreditMasterId |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-002, GAP-VCH-003, GAP-VCH-004, GAP-VCH-005, GAP-VCH-006, GAP-VCH-007, GAP-VCH-011, GAP-VCH-013, GAP-VCH-014, GAP-VCH-023, GAP-VCH-026, GAP-VCH-029, GAP-VCH-032 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-105-creditvoucher-cvcreate-credit-voucher-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-120 — Journal List

| Attribute | Value |
|---|---|
| Source path | `Views/Journal/JRList.cshtml` |
| Page type / purpose | list — Journal List |
| Legacy status | active (grid empty until Search) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | JournalController / BankPaymentsDAL, JournalDAL |
| Stored procedures | `sp_GET_DebitVoucherList`, `sp_GET_ERPVoucherIntegrationByType`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice` |
| Tables / views / functions (local DB) | tblFinancialYear, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCompanyInfo, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster, tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice |
| APIs / AJAX | API-G5-001, 004; Dashboard/LoadFilterInPartialView; Utility/LoadERPVoucherIntegration; AccountingTaskReport/rptDebitVoucher |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveJournal |
| Read / lookup actions | BankAccountList, GetJournalMasterId, JRList |
| Reports / export / print / files | Excel/CSV export, PDF export, Crystal print (blocked), DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-220, GAP-VCH-221, GAP-VCH-222, GAP-VCH-223, GAP-VCH-224, GAP-VCH-225 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-120-journal-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-121 — Journal Entry (create/edit)

| Attribute | Value |
|---|---|
| Source path | `Views/Journal/JRCreate.cshtml` |
| Page type / purpose | entry — Journal Entry (create/edit) |
| Legacy status | active (hidden legacy bank/cash block) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | JournalController / BankPaymentsDAL, JournalDAL |
| Stored procedures | `sp_GET_AllCurrencyDropDown`, `sp_GET_BankAccountComId`, `sp_GET_BankPaymentMasterById`, `sp_GET_COAALLComId`, `sp_GET_CashAccountComId`, `sp_GET_CostCenterByCOAId`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_GET_JournalDetailById`, `sp_GET_RefByCOAId`, `sp_GET_SubsidoryLedgerByCOAId`, `sp_I_VoucherDetail_new`, `sp_I_VoucherMaster`, `sp_UD_VoucherMaster_New`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblBankAccount, tblBankAccountUnitdtl, tblChartOfAccounts, tblDebitCreditVoucherMaster (W), tblCOACompanyPermission, tblSubSubControlLedger, tblCashAccount, tblMultipleCompanySameCashAccount, tblCOACostCenter, tblCostCenter, tblFinancialYear, tblDebitCreditVoucherDetail (W), tblCOARef, tblReferenceInfo, tblReferenceType, tblCOASubsidiary, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, tblSubsidiaryInfo, tblSubsidiaryType, CSTL_ERPDB_Unicorn.tblSupplier, fnSplit |
| APIs / AJAX | API-G5-002, 003, 005, 006; Utility/*; BankPayments/GetBankInfoMasterId |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveJournal |
| Read / lookup actions | BankAccountList, GetJournalMasterId, JRList |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-200, GAP-VCH-201, GAP-VCH-202, GAP-VCH-203, GAP-VCH-204, GAP-VCH-205, GAP-VCH-206, GAP-VCH-207, GAP-VCH-208, GAP-VCH-209, GAP-VCH-210, GAP-VCH-211 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-121-journal-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-122 — Journal GL row partial

| Attribute | Value |
|---|---|
| Source path | `Views/Journal/_addJRRowForCOA.cshtml` |
| Page type / purpose | partial — Journal GL row partial |
| Legacy status | active partial |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | JournalController / BankPaymentsDAL, JournalDAL |
| Stored procedures | `sp_I_VoucherDetail_new` |
| Tables / views / functions (local DB) | tblDebitCreditVoucherDetail (W) |
| APIs / AJAX | API-G5-003 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveJournal |
| Read / lookup actions | BankAccountList, GetJournalMasterId, JRList |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-206, GAP-VCH-230, GAP-VCH-231 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-122-journal-gl-row-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-123 — Contra List

| Attribute | Value |
|---|---|
| Source path | `Views/Contra/ContraList.cshtml` |
| Page type / purpose | list — Contra List |
| Legacy status | active (grid empty until Search) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | ContraController / BankPaymentsDAL, ContraDAL |
| Stored procedures | `sp_GET_DebitVoucherList`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCompanyInfo, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster, tblFinancialYear, tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice |
| APIs / AJAX | API-G5-007, 011; Dashboard/LoadFilterInPartialView; rptDebitVoucher |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveContra |
| Read / lookup actions | ContraList, GetContraList, GetContraMasterId |
| Reports / export / print / files | Excel/CSV export, PDF export, Crystal print (blocked), DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-240, GAP-VCH-241, GAP-VCH-242, GAP-VCH-243 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-123-contra-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-124 — Contra Entry (4 transfer types)

| Attribute | Value |
|---|---|
| Source path | `Views/Contra/ContraEntry.cshtml` |
| Page type / purpose | dead — Contra Entry (4 transfer types) |
| Legacy status | active (with edit-mode defects, dead code) |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Voucher Entry (81) |
| Controller / DAL | ContraController / BankPaymentsDAL, ContraDAL |
| Stored procedures | `sp_GET_ALLBankCashAccountComId`, `sp_GET_AccountClosingBalance_TrialBalanceWise`, `sp_GET_AllCurrencyDropDown`, `sp_GET_BankAccountComId`, `sp_GET_BankInfoByAccountId`, `sp_GET_BankPaymentMasterByIdContra`, `sp_GET_CashAccountComId`, `sp_GET_CashInfoByAccountId`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Get_FinancialYearDateRange`, `sp_I_VoucherMaster_Contra`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_UD_VoucherMaster_New_Contra`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | tblBankAccount, tblBankAccountUnitdtl, tblChartOfAccounts, tblCOACompanyPermission, tblControlLedger, tblDebitCreditVoucherDetail (W), tblDebitCreditVoucherMaster (W), tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblBankInformation, tblCashAccount, tblMultipleCompanySameCashAccount, tblFinancialYear, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, fnSplit |
| APIs / AJAX | API-G5-008, 009, 010; Utility/*; BankPayments/GetBankInfoMasterId, GetCashInfoIndoById |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveContra |
| Read / lookup actions | ContraList, GetContraList, GetContraMasterId |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-244, GAP-VCH-245, GAP-VCH-246, GAP-VCH-247, GAP-VCH-248, GAP-VCH-249, GAP-VCH-250, GAP-VCH-251, GAP-VCH-252, GAP-VCH-253, GAP-VCH-254 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-124-contra-entry.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-125 — Bank Reconciliation Entry

| Attribute | Value |
|---|---|
| Source path | `Views/ReconcileBankAccount/RBAEntry.cshtml` |
| Page type / purpose | entry — Bank Reconciliation Entry |
| Legacy status | active |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Bank & Cash Reconciliation (83) |
| Controller / DAL | ReconcileBankAccountController / ReconcileBankAccountDAL |
| Stored procedures | `sp_GET_DebitVoucherList`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_UP_ReconcileBankAccount` |
| Tables / views / functions (local DB) | tblFinancialYear, tblDebitCreditVoucherMaster (W) |
| APIs / AJAX | API-G5-013, 016, 017; rptDebitVoucher |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveReconcileBank |
| Read / lookup actions | GetReconcileBankAccountListAll, GetReconcileBankAccountRptAll, RBAList, ReconcileBankAccountList |
| Reports / export / print / files | Crystal print (blocked), DataTables grid, SweetAlert confirm, date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-300, GAP-VCH-301, GAP-VCH-302, GAP-VCH-303, GAP-VCH-304, GAP-VCH-305, GAP-VCH-306, GAP-VCH-307, GAP-VCH-308, GAP-VCH-309, GAP-VCH-310 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-125-reconcile-bank-account-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-126 — Reconcile grid partial

| Attribute | Value |
|---|---|
| Source path | `Views/ReconcileBankAccount/_partialRBAccount.cshtml` |
| Page type / purpose | partial — Reconcile grid partial |
| Legacy status | active partial |
| Batch / Fiori group | D — Vouchers & bank reconciliation / Bank & Cash Reconciliation (83) |
| Controller / DAL | ReconcileBankAccountController / ReconcileBankAccountDAL |
| Stored procedures | `sp_GET_DebitVoucherList` |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G5-017 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveReconcileBank |
| Read / lookup actions | GetReconcileBankAccountListAll, GetReconcileBankAccountRptAll, RBAList, ReconcileBankAccountList |
| Reports / export / print / files | date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-VCH-320, GAP-VCH-321 |
| Legacy spec | `specs/04-vouchers/pages/PAGE-126-reconcile-bank-account-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-140 — All Voucher Approval

| Attribute | Value |
|---|---|
| Source path | Views/AllApproval/AllVoucherApproval.cshtml |
| Page type / purpose | list — All Voucher Approval |
| Legacy status | active (list not auto-loaded; menu link only in commented block, DB menu row UNKNOWN) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Voucher Entry (81) |
| Controller / DAL | AllApprovalController / AllApprovalDAL, BankPaymentsDAL, LoanDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_DebitVoucherList`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_UP_AllVoucherApp` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblFinancialYear, fnSplit, tblDebitCreditVoucherMaster (W) |
| APIs / AJAX | Dashboard.LoadFilterInPartialView; AllApproval.BankAccountList; AllApproval.Approve_AllVoucher; AccountingTaskReport.rptDebitVoucher; Utility.LoadCompanyInfo/GetFinansialYearComId |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Approve_AllBankCharge, Approve_AllLoan, Approve_AllVoucher |
| Read / lookup actions | BankAccountList, LoadLoanList |
| Reports / export / print / files | Crystal print (blocked), SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-APR-001, GAP-APR-002, GAP-APR-003, GAP-APR-004, GAP-APR-005, GAP-APR-006, GAP-APR-007, GAP-APR-008, GAP-APR-009, GAP-APR-010 |
| Legacy spec | `specs/05-approvals/pages/PAGE-140-all-voucher-approval.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-141 — Loan Approval

| Attribute | Value |
|---|---|
| Source path | Views/AllApproval/LoanApproval.cshtml |
| Page type / purpose | list — Loan Approval |
| Legacy status | active (no filters; boolean badge defect) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Voucher Entry (81) |
| Controller / DAL | AllApprovalController / AllApprovalDAL, BankPaymentsDAL, LoanDAL |
| Stored procedures | `sp_GET_LoanInfoList`, `sp_UP_AllLoanApp` |
| Tables / views / functions (local DB) | fnSplit, tblLoanInfo (W), CSTL_ACCDB.tblReferenceInfo (W) |
| APIs / AJAX | AllApproval.LoadLoanList; AllApproval.Approve_AllLoan |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Approve_AllBankCharge, Approve_AllLoan, Approve_AllVoucher |
| Read / lookup actions | BankAccountList, LoadLoanList |
| Reports / export / print / files | Crystal print (blocked), SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-APR-011, GAP-APR-012, GAP-APR-013, GAP-APR-014, GAP-APR-015, GAP-APR-016 |
| Legacy spec | `specs/05-approvals/pages/PAGE-141-loan-approval.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-142 — Bank Charge Approval

| Attribute | Value |
|---|---|
| Source path | Views/AllApproval/BankChargeApproval.cshtml |
| Page type / purpose | dead — Bank Charge Approval |
| Legacy status | partial/effectively dead (both load calls commented out; grid always empty) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Voucher Entry (81) |
| Controller / DAL | AllApprovalController / AllApprovalDAL, BankPaymentsDAL, LoanDAL |
| Stored procedures | `SP_GetLCRelatedBankPaymentMaster`, `sp_UP_AllBankChargeApp` |
| Tables / views / functions (local DB) | fnSplit, tblLCRelatedBankPaymentMaster (W) |
| APIs / AJAX | AllApproval.Approve_AllBankCharge (list call to LCRelatedBankPayment.Get_BankChargeList is never made) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Approve_AllBankCharge, Approve_AllLoan, Approve_AllVoucher |
| Read / lookup actions | BankAccountList, LoadLoanList |
| Reports / export / print / files | Crystal print (blocked), SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-APR-017, GAP-APR-018, GAP-APR-019, GAP-APR-020, GAP-APR-021 |
| Legacy spec | `specs/05-approvals/pages/PAGE-142-bank-charge-approval.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-150 — Budget List

| Attribute | Value |
|---|---|
| Source path | Views/Budget/BudgetList.cshtml |
| Page type / purpose | dead — Budget List |
| Legacy status | active (approve/view/print code dead) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Cost Centre & Budget Control (85) |
| Controller / DAL | BudgetController / BudgetInfoDAL |
| Stored procedures | `sp_GET_BudgetList_Bgt`, `sp_I_BudgetDetail_Bgt`, `sp_I_BudgetMaster_Bgt`, `sp_Up_BudgetMaster_Bgt` |
| Tables / views / functions (local DB) | tblBudgetMaster (W), tblChartOfAccounts, tblCompanyInfo, tblFinancialYear, tblBudgetDetail (W) |
| APIs / AJAX | Budget.GetBudgetList (dead: Approve_Budget, LoadBudgetViewDtlsList, BgtList, rptDebitVoucher) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBudgetInfo |
| Read / lookup actions | BudgetList, GetBudgetList, GetBudgetMasterId |
| Reports / export / print / files | Excel/CSV export, PDF export, Crystal print (blocked), DataTables grid, SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-BUD-001, GAP-BUD-002, GAP-BUD-003, GAP-BUD-004, GAP-BUD-005, GAP-BUD-006 |
| Legacy spec | `specs/07-budget/pages/PAGE-150-budget-list.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-151 — Budget Entry

| Attribute | Value |
|---|---|
| Source path | Views/Budget/BudgetEntry.cshtml |
| Page type / purpose | entry — Budget Entry |
| Legacy status | active |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Cost Centre & Budget Control (85) |
| Controller / DAL | BudgetController / BudgetInfoDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_BudgetMasterById`, `sp_GET_BudgetMonthAmountDetails`, `sp_GET_COASubSubControlLedgerId`, `sp_GET_ControllLadgerDropDown`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_GET_LadgerHeadDropDown`, `sp_GET_SubSubControlLedgerDropDown`, `sp_GET_SubsidiaryDropDown`, `sp_I_BudgetDetail_Bgt`, `sp_I_BudgetMaster_Bgt`, `sp_Up_BudgetMaster_Bgt` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblBudgetMaster (W), tblChartOfAccounts, tblControlLedger, tblLedgerHead, tblSubsidiaryLedger, tblSubSubControlLedger, tblBudgetDetail (W), tblFinancialYear |
| APIs / AJAX | Budget.BudgetMonthAmountDetails/SaveBudgetInfo/GetBudgetMasterId; Utility.GetLadgerHead/LoadControlLedgerByLedgerHeadId/LoadSubsidaryByControlLedgerId/LoadSubSubControlLedgerId/GetCOASubSubControlLedgerId/GetFinansialYearComId/LoadCompanyInfo |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBudgetInfo |
| Read / lookup actions | BudgetList, GetBudgetList, GetBudgetMasterId |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-BUD-007, GAP-BUD-008, GAP-BUD-009, GAP-BUD-010, GAP-BUD-011, GAP-BUD-012, GAP-BUD-013, GAP-BUD-014, GAP-BUD-015, GAP-BUD-016, GAP-BUD-017 |
| Legacy spec | `specs/07-budget/pages/PAGE-151-budget-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-152 — Budget Month Amount grid (partial)

| Attribute | Value |
|---|---|
| Source path | Views/Budget/_partialBudgetDtlsInfo.cshtml |
| Page type / purpose | partial — Budget Month Amount grid (partial) |
| Legacy status | active (partial) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Cost Centre & Budget Control (85) |
| Controller / DAL | BudgetController / BudgetInfoDAL |
| Stored procedures | `sp_GET_BudgetMonthAmountDetails`, `sp_I_BudgetDetail_Bgt` |
| Tables / views / functions (local DB) | tblBudgetDetail (W), tblFinancialYear |
| APIs / AJAX | - |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBudgetInfo |
| Read / lookup actions | BudgetList, GetBudgetList, GetBudgetMasterId |
| Reports / export / print / files | date picker |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-BUD-018, GAP-BUD-019, GAP-BUD-020 |
| Legacy spec | `specs/07-budget/pages/PAGE-152-budget-month-amount-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-160 — Loan Info Entry

| Attribute | Value |
|---|---|
| Source path | CSTL_Pro_to_Pay/Views/Loan/L_Entry.cshtml |
| Page type / purpose | entry — Loan Info Entry |
| Legacy status | active (LC modal broken) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Receivables & Payables Ageing (82) |
| Controller / DAL | LoanController / LoanDAL |
| Stored procedures | `sp_AllSupplierType`, `sp_DDL_BankBranchDDL`, `sp_GET_AllBankInformationDropDown`, `sp_GET_CINoByLCNo`, `sp_GET_CurrencyDDL`, `sp_GET_InterestMethodDDL`, `sp_GET_InterestTypeDDL`, `sp_GET_LoanCategoryDDL`, `sp_GET_LoanInfoList`, `sp_GET_LoanInfobyLCNoID`, `sp_GET_ProfitCenterDDL`, `sp_GET_SupplierbyType`, `sp_GetLoanInfoById`, `sp_InsertLoanInfo`, `sp_LC_ReferenceNoDDLForLoan`, `sp_UP_AllLoanApp`, `sp_UpdateLoanInfo` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblSupplierType, tblBankBranchInformation, tblBankInformation, CSTL_ERPDB_Unicorn.Table_CInvoice_Master, CSTL_ERPDB_Unicorn.tblCurrency, tblInterestMethod, tblInterestType, tblLoanCategory, tblLoanInfo (W), tblProfitCenter, CSTL_ERPDB_UnicorntblSupplier, Table_LC_LCOpenMaster, Table_LC_PreparationMaster, fnSplit, CSTL_ACCDB.tblReferenceInfo (W) |
| APIs / AJAX | Loan.Load_LC_Reference_Number, Loan.InsUpLoanInfo, Loan.Get_LoanById, Utility.* (11), LCReport.* (4, controller missing) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | Get_LoanById, Get_LoanInfoList, L_List, Load_LC_Reference_Number |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-LN-001, GAP-LN-002, GAP-LN-003, GAP-LN-005, GAP-LN-006, GAP-LN-009, GAP-LN-010, GAP-LN-011, GAP-LN-016, GAP-LN-018, GAP-LN-019, GAP-LN-020 |
| Legacy spec | `specs/06-loan-lc/pages/PAGE-160-loan-entry.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-161 — Loan List

| Attribute | Value |
|---|---|
| Source path | CSTL_Pro_to_Pay/Views/Loan/L_List.cshtml |
| Page type / purpose | list — Loan List |
| Legacy status | active |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Receivables & Payables Ageing (82) |
| Controller / DAL | LoanController / LoanDAL |
| Stored procedures | `sp_GET_LoanInfoList` |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | Loan.Get_LoanInfoList, Utility.LoadCompanyInfo |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | Get_LoanById, Get_LoanInfoList, L_List, Load_LC_Reference_Number |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-LN-004, GAP-LN-006, GAP-LN-007, GAP-LN-015, GAP-LN-017 |
| Legacy spec | `specs/06-loan-lc/pages/PAGE-161-loan-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-162 — Bank Charge Entry (LC related bank payment)

| Attribute | Value |
|---|---|
| Source path | CSTL_Pro_to_Pay/Views/LCRelatedBankPayment/BankPaymentIndex.cshtml |
| Page type / purpose | list — Bank Charge Entry (LC related bank payment) |
| Legacy status | active (create works; edit broken; LC modal broken) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Receivables & Payables Ageing (82) |
| Controller / DAL | LCRelatedBankPaymentController / LCRelatedBankPaymentDAL |
| Stored procedures | `SP_GetLCRelatedBankPaymentMaster`, `SP_InsertBankPaymentDetail`, `SP_InsertBankPaymentMaster`, `SP_UpdateBankPaymentMaster` **(missing locally)**, `sp_AllSupplierType`, `sp_GET_AllCurrencyDropDown`, `sp_GET_BankAccountComId`, `sp_GET_LcBankPayType`, `sp_GET_LcCInvoice_MasterLCId`, `sp_GET_LoanInfobyLCNoID`, `sp_GET_SupplierbyType`, `sp_LC_ReferenceNoDDLForLoan`, `sp_UP_AllBankChargeApp` |
| Tables / views / functions (local DB) | tblLCRelatedBankPaymentDetail (W), tblLCRelatedBankPaymentMaster (W), CSTL_ERPDB_Unicorn.tblSupplierType, CSTL_ERPDB_Unicorn.tblCurrency, tblCurrency, tblCurrencyConversionRate, tblBankAccount, tblBankAccountUnitdtl, tblChartOfAccounts, tblLcBankPayType, CSTL_ERPDB_Unicorn.Table_CInvoice_Master, tblLoanInfo, CSTL_ERPDB_UnicorntblSupplier, Table_LC_LCOpenMaster, Table_LC_PreparationMaster, fnSplit |
| APIs / AJAX | LCRelatedBankPayment.LoadCIInfo, .InsUpBankPaymentInfo, Loan.Load_LC_Reference_Number, Loan.Get_LoanById (edit, wrong), Utility.* (12), LCReport.* (missing) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | BankPayment_List, Get_BankChargeList, Get_LCRelatedBankPayment_Edit_Mode, LoadCIInfo |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-LN-003, GAP-LN-006, GAP-LN-008, GAP-LN-012, GAP-LN-013, GAP-LN-014, GAP-LN-021, GAP-LN-022, GAP-LN-023, GAP-LN-024 |
| Legacy spec | `specs/06-loan-lc/pages/PAGE-162-lc-bank-payment-entry.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: SP_UpdateBankPaymentMaster) / PASS / Pending / Pending |
| Notes | — |

### PAGE-163 — Bank Charge List

| Attribute | Value |
|---|---|
| Source path | CSTL_Pro_to_Pay/Views/LCRelatedBankPayment/BankPayment_List.cshtml |
| Page type / purpose | list — Bank Charge List |
| Legacy status | active (edit link wrong) |
| Batch / Fiori group | E — Approvals, loan/LC, budget / Receivables & Payables Ageing (82) |
| Controller / DAL | LCRelatedBankPaymentController / LCRelatedBankPaymentDAL |
| Stored procedures | `SP_GetLCRelatedBankPaymentMaster` |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | LCRelatedBankPayment.Get_BankChargeList, Utility.LoadCompanyInfo |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | BankPayment_List, Get_BankChargeList, Get_LCRelatedBankPayment_Edit_Mode, LoadCIInfo |
| Reports / export / print / files | Excel/CSV export, PDF export, DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-LN-003, GAP-LN-006, GAP-LN-007, GAP-LN-015, GAP-LN-017 |
| Legacy spec | `specs/06-loan-lc/pages/PAGE-163-lc-bank-payment-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-200 — Balance Sheet Setup

| Attribute | Value |
|---|---|
| Source path | `Views/BalanceSheet/BSCreate.cshtml` |
| Page type / purpose | report — Balance Sheet Setup |
| Legacy status | active |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Accounts/BalanceSheetController.cs / BalanceSheetDAL |
| Stored procedures | `sp_DDL_Company`, `sp_D_BalanceSheetDetailByMasterId` **(missing locally)**, `sp_GET_BalanceSheetByCompanyId` **(missing locally)**, `sp_GET_BalanceSheetDetailByCompanyId` **(missing locally)**, `sp_I_BalanceSheetDetail` **(missing locally)**, `sp_I_BalanceSheetMaster` **(missing locally)**, `sp_U_BalanceSheetMaster` **(missing locally)** |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo |
| APIs / AJAX | API-G8-004, -005, `/Utility/LoadCompanyInfo` |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBalanceSheet |
| Read / lookup actions | GetByCompany |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-FST-001, GAP-FST-002, GAP-FST-003, GAP-FST-004, GAP-FST-005 |
| Legacy spec | `specs/08-financial-statements/pages/PAGE-200-balance-sheet-setup.md` |
| Migration status / build / functional / UI QA | BLOCKED-LOCAL (missing SP: sp_D_BalanceSheetDetailByMasterId, sp_GET_BalanceSheetByCompanyId, sp_GET_BalanceSheetDetailByCompanyId, sp_I_BalanceSheetDetail) / PASS / Pending / Pending |
| Notes | — |

### PAGE-201 — Income Statement Setup

| Attribute | Value |
|---|---|
| Source path | `Views/IncomeStatement/ISCreate.cshtml` |
| Page type / purpose | report — Income Statement Setup |
| Legacy status | active |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Accounts/IncomeStatementController.cs / IncomeStatementDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G8-009, -010, `/Utility/LoadCompanyInfo` |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveIncomeStatement |
| Read / lookup actions | GetByCompany |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-201-income-statement-setup.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-202 — Balance Sheet Dynamic Report

| Attribute | Value |
|---|---|
| Source path | `Views/AllAccountReport/BalanceSheetDynamicReport.cshtml` |
| Page type / purpose | report — Balance Sheet Dynamic Report |
| Legacy status | active |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Reports/BalanceSheetDynamicReportController.cs / BalanceSheetDynamicReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G8-012, `/Utility/LoadCompanyInfo`, `GetFinansialYearComId`, `GetDaterangebyFiscal` |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetReport |
| Reports / export / print / files | Excel/CSV export, Print |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-202-balance-sheet-dynamic-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-203 — Income Statement Dynamic Report

| Attribute | Value |
|---|---|
| Source path | `Views/AllAccountReport/IncomeStatementDynamicReport.cshtml` |
| Page type / purpose | report — Income Statement Dynamic Report |
| Legacy status | active |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Reports/IncomeStatementDynamicReportController.cs / IncomeStatementDynamicReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G8-014, Utility as above |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetReport |
| Reports / export / print / files | Excel/CSV export, Print |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-203-income-statement-dynamic-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-204 — Income Statement Report (legacy SP-based)

| Attribute | Value |
|---|---|
| Source path | `Views/AllAccountReport/IncomeStatementReport.cshtml` |
| Page type / purpose | dead — Income Statement Report (legacy SP-based) |
| Legacy status | dead/partial (calls non-existent endpoint; no link) |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Accounts/IncomeStatementController.cs / IncomeStatementDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | `/AllAccountReport/GetIncomeStatementReport` (API-G8-021, MISSING) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveIncomeStatement |
| Read / lookup actions | GetByCompany |
| Reports / export / print / files | Excel/CSV export, Print |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-204-income-statement-report-legacy.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-205 — Cash Flow Report

| Attribute | Value |
|---|---|
| Source path | `Views/AllAccountReport/CashFlowReport.cshtml` |
| Page type / purpose | report — Cash Flow Report |
| Legacy status | active (tile hidden in ReportList) |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Reports/AllAccountReportController.cs (shared) / AccountsReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G8-016, Utility |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Excel/CSV export |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-205-cash-flow-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-206 — Trial Balance Report

| Attribute | Value |
|---|---|
| Source path | `Views/AllAccountReport/TrialBalanceReport.cshtml` |
| Page type / purpose | report — Trial Balance Report |
| Legacy status | active |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Reports/AllAccountReportController.cs (shared) / AccountsReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | API-G8-018, 12 Utility endpoints |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Excel/CSV export, Print |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-206-trial-balance-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-207 — Balance Sheet Report (legacy, view missing)

| Attribute | Value |
|---|---|
| Source path | MISSING `Views/AllAccountReport/BalanceSheetReport.cshtml` |
| Page type / purpose | dead — Balance Sheet Report (legacy, view missing) |
| Legacy status | dead (view file absent) |
| Batch / Fiori group | F — Financial statements / Trial Balance & Financials (84) |
| Controller / DAL | Controllers/Accounts/BalanceSheetController.cs / BalanceSheetDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none |
| Session variables (spec) | — |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | SaveBalanceSheet |
| Read / lookup actions | GetByCompany |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | — |
| Legacy spec | `specs/.../PAGE-207-balance-sheet-report-legacy-missing-view.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-220 — Accounts Operation Report (ReportList)

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/ReportList.cshtml |
| Page type / purpose | report — Accounts Operation Report (ReportList) |
| Legacy status | active |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | none (links only) |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-015, GAP-RPT-040 |
| Legacy spec | `specs/09-reports/pages/PAGE-220-report-list.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-221 — Ledger Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/LedgerReport.cshtml |
| Page type / purpose | report — Ledger Report |
| Legacy status | active |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | `gl_Rpt_Cr_Proc_Costcenter`, `gl_Rpt_Cr_Proc_GL`, `gl_Rpt_Cr_Proc_Subsidiary_Ledger`, `gl_Rpt_Cr_Proc_reference`, `sp_DDL_Company`, `sp_GET_COAAccountOpeningBalanceForNonTractionLedger`, `sp_GET_COAComId`, `sp_GET_CostCenterByCOAId`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_GET_ProfitCenterDDL`, `sp_GET_ReferenceByTypeDDLCompanyWise`, `sp_GET_ReferenceTypeDDL`, `sp_GET_SubsidiaryByTypeDDL`, `sp_GET_SubsidiaryTypeDDL`, `sp_GET_SubsidoryLedgerByCOAId`, `sp_Get_FinancialYearDateRange`, `sp_Rpt_ChartofAccountTransaction_LedgerOnly`, `sp_Rpt_ChartofAccountTransaction_MonthlySummery`, `sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance`, `sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance_reference`, `sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance_supplier` |
| Tables / views / functions (local DB) | tblChartOfAccounts, tblCOASubsidiary, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherDetail, tblDebitCreditVoucherMaster, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblControlLedger, tblFinancialYear, tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubsubsidiaryLadger, tblCOACompanyPermission, tblSubSubControlLedger, tblCOACostCenter, tblCostCenter, tblProfitCenter, CSTL_ERPDB_UnicorntblSupplier, CSTL_ERPDB_Unicorn.Table_LC_LCOpenMaster, CSTL_ERPDB_Unicorn.Table_LC_PreparationMaster, SalesRollDB_ZAS.tblInvoice, CSTL_ERPDB_Unicorn.tblPurchaseOrderLocalMaster, tblReferenceInfo, tblReferenceType, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, CSTL_ERPDB_Unicorn.tblSupplier, tblSubsidiaryType, tblCurrency |
| APIs / AJAX | API-G9-001, 002, 013, 019(hidden); Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Crystal print (blocked), DataTables grid |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-011, GAP-RPT-012, GAP-RPT-013, GAP-RPT-017, GAP-RPT-018, GAP-RPT-019, GAP-RPT-025, GAP-RPT-035, GAP-RPT-041 |
| Legacy spec | `specs/09-reports/pages/PAGE-221-ledger-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-222 — Subsidiary Wise Ledger Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/LedgerReportSubsidiary.cshtml |
| Page type / purpose | report — Subsidiary Wise Ledger Report |
| Legacy status | active (unlinked) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | `gl_Rpt_Cr_Proc_Subsidiary_Ledger`, `sp_GET_COAAccountOpeningBalance`, `sp_GET_COAAccountOpeningBalanceForNonTractionLedger`, `sp_GET_SubsidoryLedgerByCOAId`, `sp_Get_FinancialYearDateRange`, `sp_Rpt_ChartofAccountTransaction_LedgerOnly`, `sp_Rpt_ChartofAccountTransaction_MonthlySummery` |
| Tables / views / functions (local DB) | tblChartOfAccounts, tblCOASubsidiary, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherDetail, tblDebitCreditVoucherMaster, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblControlLedger, tblFinancialYear, tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, tblSubsubsidiaryLadger, SalesRollDB_ZAS.tblCustMaster, ZASHRIS.tblEmpGeneralInfo, tblSubsidiaryType, CSTL_ERPDB_Unicorn.tblSupplier, tblCompanyInfo, tblCurrency, tblCOACompanyPermission |
| APIs / AJAX | API-G9-020; Utility/* |
| Session variables (spec) | LoginUserId, reportparam, reportvalue |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-013, GAP-RPT-014, GAP-RPT-015, GAP-RPT-038 |
| Legacy spec | `specs/09-reports/pages/PAGE-222-ledger-report-subsidiary.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-223 — Cost Center Wise Ledger Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/LedgerReportCostCenter.cshtml |
| Page type / purpose | report — Cost Center Wise Ledger Report |
| Legacy status | active (unlinked) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | `gl_Rpt_Cr_Proc_Costcenter`, `sp_GET_COAAccountOpeningBalance`, `sp_GET_COAAccountOpeningBalanceForNonTractionLedger`, `sp_GET_CostCenterByCOAId`, `sp_Rpt_ChartofAccountTransaction_LedgerOnly`, `sp_Rpt_ChartofAccountTransaction_MonthlySummery` |
| Tables / views / functions (local DB) | tblChartOfAccounts, tblCOASubsidiary, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherDetail, tblDebitCreditVoucherMaster, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblControlLedger, tblFinancialYear, tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, tblSubsubsidiaryLadger, tblCOACostCenter, tblCostCenter, tblCompanyInfo, tblCurrency, tblCOACompanyPermission |
| APIs / AJAX | API-G9-021; Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-013, GAP-RPT-014, GAP-RPT-015, GAP-RPT-038 |
| Legacy spec | `specs/09-reports/pages/PAGE-223-ledger-report-cost-center.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-224 — Chart of Account Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/COAReport.cshtml |
| Page type / purpose | report — Chart of Account Report |
| Legacy status | active |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | `sp_DDL_Company`, `sp_Rpt_ChartofAccount` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblChartOfAccounts, tblCOACompanyPermission, tblControlLedger, tblLedgerHead, tblSubsidiaryLedger, tblSubSubControlLedger |
| APIs / AJAX | ChartOfAccount/GetChartofAccount_ByCompanyId; Utility/LoadCompanyInfo |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Excel/CSV export, Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-018, GAP-RPT-019, GAP-RPT-023, GAP-RPT-024, GAP-RPT-032 |
| Legacy spec | `specs/09-reports/pages/PAGE-224-coa-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-225 — Cash Book Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/CashBookReport.cshtml |
| Page type / purpose | report — Cash Book Report |
| Legacy status | active |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController (+CashBookController data) / AccountsReportDAL |
| Stored procedures | `sp_DDL_Company`, `sp_GET_AllCashAccountUnderCompanyID`, `sp_GET_COAAccountOpeningBalance`, `sp_GET_FinancialYearforOpeningBalanceDropDown`, `sp_Get_FinancialYearDateRange`, `sp_Rpt_ChartofAccountTransaction_CashBook`, `sp_Rpt_ChartofAccountTransaction_MonthlySummery` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCashAccount, tblChartOfAccounts, tblCOACompanyPermission, tblControlLedger, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherDetail, tblDebitCreditVoucherMaster, tblFinancialYear, tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, tblCostCenter, SalesRollDB_ZAS.tblCustMaster, tblReferenceInfo, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblSupplier |
| APIs / AJAX | API-G9-026/027, 013; Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Excel/CSV export, Print, Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-002, GAP-RPT-005, GAP-RPT-007, GAP-RPT-009, GAP-RPT-010, GAP-RPT-017, GAP-RPT-023, GAP-RPT-029, GAP-RPT-031, GAP-RPT-035 |
| Legacy spec | `specs/09-reports/pages/PAGE-225-cash-book-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-226 — Bank Book Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/BankBookReport.cshtml |
| Page type / purpose | report — Bank Book Report |
| Legacy status | active |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController (+BankBookController data) / AccountsReportDAL |
| Stored procedures | `sp_GET_AllBankAccountUnderCompanyID`, `sp_GET_AllBankInformationDropDown`, `sp_GET_COAAccountOpeningBalance`, `sp_Rpt_ChartofAccountTransaction_CashBook`, `sp_Rpt_ChartofAccountTransaction_MonthlySummery`, `sp_SP_BankBookBlank` |
| Tables / views / functions (local DB) | tblBankAccount, tblChartOfAccounts, tblCOACompanyPermission, tblBankInformation, CSTL_ERPDB_Unicorn.tblCompanyInfo, tblControlLedger, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherDetail, tblDebitCreditVoucherMaster, tblFinancialYear, tblLedgerHead, tblOpeningBalance, tblSubsidiaryLedger, tblSubSubControlLedger, tblCostCenter, SalesRollDB_ZAS.tblCustMaster, tblReferenceInfo, tblSubsidiaryInfo, CSTL_ERPDB_Unicorn.tblSupplier |
| APIs / AJAX | API-G9-024, 013; Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Excel/CSV export, Print, Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-002, GAP-RPT-005, GAP-RPT-007, GAP-RPT-008, GAP-RPT-010, GAP-RPT-017, GAP-RPT-023, GAP-RPT-029, GAP-RPT-030, GAP-RPT-031 |
| Legacy spec | `specs/09-reports/pages/PAGE-226-bank-book-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-227 — Bank Reconciliation Statement Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/BankReconciliationStatementReport.cshtml |
| Page type / purpose | report — Bank Reconciliation Statement Report |
| Legacy status | active (only hidden link) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AllAccountReportController / AccountsReportDAL |
| Stored procedures | `sp_GET_AllBankAccountUnderCompanyID`, `sp_GET_AllBankInformationDropDown`, `sp_GET_DebitVoucherList`, `sp_UP_ReconcileBankAccount` |
| Tables / views / functions (local DB) | tblBankAccount, tblChartOfAccounts, tblCOACompanyPermission, tblBankInformation, tblDebitCreditVoucherMaster (W) |
| APIs / AJAX | ReconcileBankAccount/GetReconcileBankAccountRptAll, API-G9-013; Utility/* |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | GetLedgerReport, GetLedgerReportOpeningBlnc, ReportList |
| Reports / export / print / files | Crystal print (blocked) |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-001, GAP-RPT-002, GAP-RPT-015, GAP-RPT-017, GAP-RPT-033 |
| Legacy spec | `specs/09-reports/pages/PAGE-227-bank-reconciliation-statement-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-228 — Voucher Validation / Accounts Integrity Report

| Attribute | Value |
|---|---|
| Source path | Views/AllAccountReport/VoucherValidationReport.cshtml (explicit path; no Views/VoucherValidation) |
| Page type / purpose | report — Voucher Validation / Accounts Integrity Report |
| Legacy status | active (new, unlinked) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | VoucherValidationController / VoucherValidationDAL |
| Stored procedures | `sp_AccIntegrity_AnomaliesCorrection`, `sp_DDD_AccountsIntigrityPages`, `sp_RPT_AccountsIntigrityReport` |
| Tables / views / functions (local DB) | SalesRollDB_ZAS.sp_Acc_MaterialConversionReceive, CSTL_ERPDB_Unicorn.sp_LC_CommercialInvoiceApproval, CSTL_ERPDB_Unicorn.sp_LC_LandededCostApproval, CSTL_ERPDB_Unicorn.sp_PTP_IOUAdavnecPaymentApproval, CSTL_ERPDB_Unicorn.sp_PTP_IOUAdvanceAdjustmentApproval, CSTL_ERPDB_Unicorn.sp_PTP_SupplierAdvancePaymentApproval, CSTL_ERPDB_Unicorn.sp_PTP_SupplierImportInvoiceApproval, CSTL_ERPDB_Unicorn.sp_PTP_UpdateInvoicePaymentApproval, CSTL_ERPDB_Unicorn.sp_PTP_UpdateMovementIssueApproval, CSTL_ERPDB_Unicorn.sp_PTP_UpdateMovementIssueReceiveApproval, CSTL_ERPDB_Unicorn.sp_PTP_UpdateVATTaxApproval, SalesRollDB_ZAS.sp_Sales_CustomerPaymentPosting, SalesRollDB_ZAS.sp_Sales_DeliverySalesPosting, SalesRollDB_ZAS.sp_Sales_DeliverySalesReturnPosting, SalesRollDB_ZAS.sp_Sales_ProductDestroyPosting, SalesRollDB_ZAS.sp_SampleInvoicePosting, CSTL_ERPDB_Unicorn.tbl_INV_MaterialReceiveLocalJournal, CSTL_ERPDB_Unicorn.tbl_MC_MaterialConversionWIPJournal, CSTL_ACCDB.tblDebitCreditVoucherDetail (W), CSTL_ACCDB.tblDebitCreditVoucherMaster (W), Table_Integration_AccountsConnectivity, CSTL_ERPDB_Unicorn.Table_CInvoice_Details, CSTL_ERPDB_Unicorn.Table_CInvoice_Master, CSTL_ERPDB_Unicorn.Table_LC_ForeignPurchaseReceiveMaster, CSTL_ERPDB_Unicorn.Table_LC_LandedCostItemDetails, CSTL_ERPDB_Unicorn.Table_LC_LandedCostMaster, CSTL_ERPDB_Unicorn.Table_LC_LCOpenMaster, CSTL_ERPDB_Unicorn.tbl_AccIntegrationTable, CSTL_ERPDB_Unicorn.tbl_INV_WeightedAvg, SalesRollDB_ZAS.tbl_INV_WeightedAvg, CSTL_ERPDB_Unicorn.tbl_IOUAdjustment, CSTL_ERPDB_Unicorn.tbl_MC_MaterialConversionMaster, CSTL_ERPDB_Unicorn.tbl_PTP_AdavnecPayToSupplierMaster, CSTL_ERPDB_Unicorn.tbl_PTP_AdvanePayToSupplierDetails, CSTL_ERPDB_Unicorn.tbl_PTP_InvoiceMaster, CSTL_ERPDB_Unicorn.tbl_PTP_InvoicePaymentMaster, CSTL_ERPDB_Unicorn.tbl_PTP_IOUAdavnecPayMaster, CSTL_ERPDB_Unicorn.tbl_PTP_VatTaxVouchers, SalesRollDB_ZAS.tblCustomerPay, SalesRollDB_ZAS.tblCustomerReturnMaster, SalesRollDB_ZAS.tblCustPayDetail, SalesRollDB_ZAS.tblDestroyProduct, SalesRollDB_ZAS.tblInvoice, SalesRollDB_ZAS.tblInvoiceDetail, CSTL_ERPDB_Unicorn.tblPurchaseOrderLocalReceive, SalesRollDB_ZAS.tblWHStockInMaster, CSTL_ERPDB_Unicorn.tblWHTransferIssueDetail, CSTL_ERPDB_Unicorn.tblWHTransferIssueMaster, CSTL_ERPDB_Unicorn.tblWHTransferItemRcvDetail, CSTL_ERPDB_Unicorn.tblWHTransferItemRcvMaster |
| APIs / AJAX | API-G9-029/030/031 |
| Session variables (spec) | LoginUserId |
| Authentication / authorization | legacy: Session["LoginUserId"] on page actions; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Reprocess |
| Read / lookup actions | GetData, GetPages |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-002, GAP-RPT-004, GAP-RPT-016, GAP-RPT-017, GAP-RPT-018, GAP-RPT-026, GAP-RPT-027, GAP-RPT-042, GAP-RPT-043 |
| Legacy spec | `specs/09-reports/pages/PAGE-228-voucher-validation-report.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-229 — AccountingTaskReport controller (hand-off endpoints; no view; Views/AccountingTaskReport is an empty folder)

| Attribute | Value |
|---|---|
| Source path | none |
| Page type / purpose | dead — AccountingTaskReport controller (hand-off endpoints; no view; Views/AccountingTaskReport is an empty folder) |
| Legacy status | partial (6/10 dead) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | AccountingTaskReportController / ParamReport, ReportDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | - |
| Session variables (spec) | reportparam, reportvalue |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-002, GAP-RPT-006, GAP-RPT-023, GAP-RPT-038 |
| Legacy spec | `specs/09-reports/pages/PAGE-229-accounting-task-report-launcher.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-260 — Crystal Report Viewer

| Attribute | Value |
|---|---|
| Source path | `CSTL_Pro_to_Pay/CrystalViewer/CrystalViewer.aspx` (+`.aspx.cs` 3,076 lines, `.designer.cs`) |
| Page type / purpose | dead — Crystal Report Viewer |
| Legacy status | ACTIVE (voucher print x13 launchers + 3 ledger launchers); most other branches DEAD (see catalog) |
| Batch / Fiori group | G — Reports, print & utilities / Trial Balance & Financials (84) |
| Controller / DAL | none (WebForms code-behind; staged by Controllers/Reports/AccountingTaskReportController.cs) / ParamReport, ReportDAL |
| Stored procedures | `sp_GET_AllReportHeaderReport`, `sp_GET_ClosingBalanceForTB`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice`, `sp_SP_BankBookBlank` |
| Tables / views / functions (local DB) | tblReportHeading, tblRptImage, tblInventoryParticulars, tblMonthlyInvenBalance, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCompanyInfo, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster, tblFinancialYear, tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice |
| APIs / AJAX | `GET /CrystalImageHandler.aspx`; SPs via `ReportDAL`/`AccountsReportDAL` |
| Session variables (spec) | reportparam, reportvalue, username |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-300, GAP-RPT-301, GAP-RPT-302, GAP-RPT-303, GAP-RPT-304, GAP-RPT-305, GAP-RPT-306, GAP-RPT-307, GAP-RPT-308, GAP-RPT-309, GAP-RPT-310, GAP-RPT-311, GAP-RPT-312, GAP-RPT-313, GAP-RPT-314, GAP-RPT-315 |
| Legacy spec | `specs/09-reports/pages/PAGE-260-crystalviewer-aspx.md` |
| Migration status / build / functional / UI QA | BLOCKED (Crystal Reports has no .NET 10 runtime) / - / Blocked / - |
| Notes | CrystalViewerController renders an explanatory page at the same URL |

### PAGE-261 — Payment Voucher HTML print

| Attribute | Value |
|---|---|
| Source path | `CSTL_Pro_to_Pay/CrystalViewer/RptJournal.aspx` (+`.aspx.cs` 3,168 lines, `.designer.cs`) |
| Page type / purpose | report — Payment Voucher HTML print |
| Legacy status | DORMANT/partial (only launcher commented out in `Views/Journal/JRList.cshtml:461-478`) |
| Batch / Fiori group | G — Reports, print & utilities / - |
| Controller / DAL | none / — |
| Stored procedures | `sp_GET_AllReportHeaderReport`, `sp_Rpt_DebitCreditVoucherDetail`, `sp_Rpt_DebitCreditVoucherMaster`, `sp_Rpt_DebitCreditVoucherMultiCostCenter`, `sp_Rpt_DebitCreditVoucherMultiInvoice` |
| Tables / views / functions (local DB) | tblReportHeading, tblRptImage, CSTL_ERPDB_Unicorn.Table_LC_ChargeHeads, CSTL_ERPDB_Unicorn.tbl_LCCharges_PaymentDetails, tblChartOfAccounts, tblCostCenter, SalesDisDB_Unicorn_DB.tblCustMaster, tblDebitCreditVoucherDetail, SalesDisDB_Unicorn_DB.tblEmpGeneralInfo, tblReferenceInfo, tblSubsidiaryInfo, tblSupplier, CSTL_ERPDB_Unicorn.tblCompanyInfo, CSTL_ERPDB_Unicorn.tblCurrency, tblDebitCreditVoucherMaster, tblFinancialYear, tblVoucherCategory, tblCostCenterCalculation, tblDebitCreditVoucherMultiInvoice |
| APIs / AJAX | SPs as PAGE-260 |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | — |
| Read / lookup actions | — |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-RPT-300, GAP-RPT-320, GAP-RPT-321, GAP-RPT-322, GAP-RPT-323, GAP-RPT-324, GAP-RPT-325, GAP-RPT-326 |
| Legacy spec | `specs/09-reports/pages/PAGE-261-rptjournal-aspx.md` |
| Migration status / build / functional / UI QA | BLOCKED (Crystal Reports has no .NET 10 runtime) / - / Blocked / - |
| Notes | dormant clone of PAGE-260 |

### PAGE-290 — Test List ("Voucher List")

| Attribute | Value |
|---|---|
| Source path | Views/Test/Index.cshtml |
| Page type / purpose | dead — Test List ("Voucher List") |
| Legacy status | sandbox/dead-ish (renders, no links to it) |
| Batch / Fiori group | H — Sandbox/test scaffold / More |
| Controller / DAL | TestController / TestDAL |
| Stored procedures | `sp_DDL_Company`, `sp_Get_TestList`, `sp_I_TestSave`, `sp_I_TestSaveDetail` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblTestMaster (W), tblTestDetail (W) |
| APIs / AJAX | Test.TsetListData; Report.VoucherPrintReports (missing controller) |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Delete, SaveTest |
| Read / lookup actions | TsetListData |
| Reports / export / print / files | Crystal print (blocked), SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-TST-001, GAP-TST-002, GAP-TST-003, GAP-TST-004, GAP-TST-005, GAP-TST-006 |
| Legacy spec | `specs/10-sandbox-test/pages/PAGE-290-test-list.md` |
| Migration status / build / functional / UI QA | N/A (dead in legacy) / - / Pending / Pending |
| Notes | — |

### PAGE-291 — Test Create ("Voucher Info")

| Attribute | Value |
|---|---|
| Source path | Views/Test/Create.cshtml |
| Page type / purpose | entry — Test Create ("Voucher Info") |
| Legacy status | sandbox; save broken (JS ReferenceError `detailsId`) |
| Batch / Fiori group | H — Sandbox/test scaffold / More |
| Controller / DAL | TestController / TestDAL |
| Stored procedures | `sp_DDL_Company`, `sp_DDL_CompanyUnit`, `sp_GET_AllCitybyCountryId`, `sp_GET_AllCountryDropDown`, `sp_Get_TestList`, `sp_I_TestSave`, `sp_I_TestSaveDetail` |
| Tables / views / functions (local DB) | CSTL_ERPDB_Unicorn.tblCompanyInfo, tblCompanyInfo, tblCompanyUnit, tblCity, tblCountry, tblTestMaster (W), tblTestDetail (W) |
| APIs / AJAX | Test.AddRowForCountry/SaveTest; Utility.LoadCompanyInfo/LoadBusinessUnit/LoadCountry/LoadCitybyCountry |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Delete, SaveTest |
| Read / lookup actions | TsetListData |
| Reports / export / print / files | SweetAlert confirm |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-TST-007, GAP-TST-008, GAP-TST-009, GAP-TST-010, GAP-TST-011, GAP-TST-012, GAP-TST-013, GAP-TST-014, GAP-TST-015 |
| Legacy spec | `specs/10-sandbox-test/pages/PAGE-291-test-create.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |

### PAGE-292 — Test Add Country Row (partial)

| Attribute | Value |
|---|---|
| Source path | Views/Test/_addNewCountryRow.cshtml |
| Page type / purpose | partial — Test Add Country Row (partial) |
| Legacy status | sandbox (partial) |
| Batch / Fiori group | H — Sandbox/test scaffold / More |
| Controller / DAL | TestController / TestDAL |
| Stored procedures | — |
| Tables / views / functions (local DB) | — |
| APIs / AJAX | - |
| Session variables (spec) | — |
| Authentication / authorization | legacy: no session check on this controller; new: global session guard (SessionRequiredFilter) |
| CRUD / state-changing actions | Delete, SaveTest |
| Read / lookup actions | TsetListData |
| Reports / export / print / files | — |
| Transactions | every DAL call runs inside DataAccessManager's connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |
| Known legacy defects (spec §30) | GAP-TST-016, GAP-TST-017 |
| Legacy spec | `specs/10-sandbox-test/pages/PAGE-292-test-add-country-row-partial.md` |
| Migration status / build / functional / UI QA | MIGRATED / PASS / Pending / Pending |
| Notes | — |
