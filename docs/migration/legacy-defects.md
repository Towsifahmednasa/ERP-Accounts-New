# Legacy defects (preserved or documented, never silently fixed)

The legacy reverse-engineering pass registered 491 gaps (`Accounts/specs/SPEC-GAPS.md`); this file lists the ones
that matter for the migration and records the decision taken for each. Decision vocabulary: **preserved** (same
behaviour in the new app), **fixed-framework** (fixed only because the .NET Core host required or made it
unavoidable; documented), **environment** (local DB), **dead** (not reachable).

| # | Defect | Where (legacy) | Classification | Decision |
|---|---|---|---|---|
| D01 | `Home/Login` returns `View("Login")` but `Login.cshtml` never existed → HTTP 500 on session loss from the dashboard (GAP-PLT-001) | `HomeController.cs:35`, `DashboardController.cs:22,55` | defect | **fixed-framework**: `Login()` renders the real login view (`Index`). Navigation only; no business logic |
| D02 | Failed login never shows a message: `ViewData["LoginMsg"]` set and then redirected (GAP-PLT-002) | `HomeController.cs:64-70` | defect (UI) | **fixed-framework**: `TempData` + strip on the login page; same text "Invalid Credentials" |
| D03 | Main dashboard is static demo markup with fabricated P&L/cash-flow figures (GAP-PLT-004) | `Views/Dashboard/Index.cshtml` | not business logic | replaced by the Fiori launchpad built from the user's menu (the brief requires the wireframe UI; no calculation existed) |
| D04 | `Home/CheckLogin` routable credential oracle (GAP-PLT-003) | `HomeController.cs:79` | security defect | **fixed-framework**: `[NonAction]` (never called by the UI) |
| D05 | `@UpdateDate` SP parameter is bound to `UpdateBy` in the voucher DALs (GAP-VCH) | `BankPaymentsDAL.cs:108`, Credit/Journal/Contra DALs | defect inside the SP contract | **preserved** (SP parameters must not change; the SP decides what it does with the value) |
| D06 | `DataAccessManager.isException/returnValue` are never reset; `Save/Update/DeleteData` return true whenever no `SqlException` (success ≠ rows affected) | `DataManager/DataAccessManager.cs` | behaviour | **preserved** verbatim (DAL instances are per controller instance, as before) |
| D07 | 46 of 54 `UtilityDAL` lookups never close the connection/transaction (rely on GC) | `DAL/UtilityDAL.cs` | resource leak | **preserved** (Microsoft.Data.SqlClient pooling + GC behave as System.Data.SqlClient did); candidate for a later clean-up |
| D08 | List actions take a browser-built SQL WHERE fragment (`param`) (S7) | 16 actions | design | **preserved**; now requires login |
| D09 | DR=CR balancing, required fields, approved-voucher edit lock are client-side only (S14, GAP-VCH-200) | voucher views | business validation | **preserved** (server-side validation would be new business logic; the SP bodies are available locally for review) |
| D10 | Saves reachable by GET (no `[HttpPost]` on most actions) | 39 controllers | design | **preserved**; POSTs now carry anti-forgery tokens |
| D11 | `TrialBalanceController` reads session keys that are never set (`COAFromDate`, `COAToDate`, `TBCompanyName`, `TBFinacialYear`) and assigns null to DataRow columns inside try/catch | `TrialBalanceController.cs:398-401, 549-552, 840-843, 978-981, 1114-1117` | defect | **preserved** (same null reads through `LegacySession`) |
| D12 | Stray duplicate controllers not compiled (`Controllers/CostCentreController.cs`, `Views/OpeningBlnc/OpeningBlncController.cs`) and a stale nested project copy (GAP-SYS-001) | repo | dead | **not ported** (the csproj compile list is the authority) |
| D13 | `Dashboard/ApprovalDashBoard`, `VoucherApprovalDashBoard` have no views and call missing SPs | `DashboardController.cs:84-99` | dead | **preserved** as actions (unreachable from the UI) |
| D14 | `Home/LockScreen` has no view; `Home/About`, `Home/Contact` are MVC template leftovers | `HomeController.cs` | dead | **preserved** (About/Contact render inside the Fiori shell) |
| D15 | Menu rows of module 13 point to controllers that do not exist (`AssetConversion`, `AssetDisposal`) and to `DashBoard/Index` placeholders (Loan Schedule/Payment/IOU/Cheque Print) | `CSTL_ERPDB_Unicorn` menu data | data | **preserved**: shown under the "More" group exactly as the DB returns them; they 404 / go to Home as in legacy |
| D16 | Budget / Loan / Fixed-asset menu groups are returned without their parent rows for user 1, so the legacy tree drops them | `sp_Select_Menu_Step_WithUserAccessERPAcc` | data/SP | **preserved** (same tree-building code `DashboardDAL.GetMenuStepOne`) |
| D17 | Session-expiry replies to AJAX were the login page HTML (JSON parse error in the browser) | all AJAX actions | UX defect | **fixed-framework**: 401 JSON + redirect to login (the legacy redirect for page requests is kept) |
| D18 | `JsonRequestBehavior.AllowGet` everywhere; JSON hijack surface on old browsers (S4/S6) | controllers | security | n/a on ASP.NET Core; endpoints now require login |
| D19 | 32 stored procedures missing in the local databases (see `blocked-pages.md`) | DB | environment | **environment**; pages behave as in legacy against this DB |
| D20 | `sp_Select_Menu_Step_WithUserAccessERPAcc` returns `ModuleId` 1014 for module 13 calls; `MenuOperation` defaults ModuleId to 1014 when null | `DashboardDAL.cs` | oddity | **preserved** |
| D21 | `UpdateBy`/`EntryBy` audit values are inconsistently the user name or the user id depending on the DAL | several DALs | audit | **preserved** (same session key per call site) |
| D22 | `Views/Loan/L_Entry.cshtml:412` carries a stray `asp-for` attribute (no effect in MVC 5) | view | harmless | **preserved**: MVC tag helpers are not enabled (only `~/` URL resolution), so the attribute is still inert |
| D23 | Legacy layout initialised select2 twice and applied a global keypress filter on number inputs | `_Layout.cshtml` | behaviour | **preserved** verbatim in the Fiori layout (same order relative to page scripts) |
| D24 | `CommandTimeout = 9,600,000 s` on every read | `DataAccessManager.cs` | config | **preserved** |
| D25 | Voucher print hand-off through session (`reportvalue`/`reportparam`): last click wins across tabs (GAP-VCH-024) | `AccountingTaskReportController` | design | **preserved** (and Crystal printing itself is blocked, see `blocked-pages.md`) |

## Compiler-visible legacy issues kept as-is (documented NoWarn in the csproj)

`CS0168`/`CS0219` unused locals, `CS0414` assigned-never-used field (`DashboardDAL.sqlTransaction`), `CA2200`
`throw ex;` (stack trace lost) in every DAL `catch`, `CS0108` member hiding, `CS8981` lower-case type names
(`MenuOperation` etc. are fine; one DTO), `SYSLIB0051` legacy serialization attributes on DTOs. These are legacy code
patterns, not migration defects; suppressing them keeps the build at 0 warnings without touching business code.
