# Blocked pages

Policy (brief §18/§33): a blocked page does not stop the migration. Every page below is ported and builds; what is
blocked is a runtime dependency. Two classes: **platform** (cannot be solved inside this application) and **local
environment** (object missing in the local databases — the legacy application fails identically against this
database, so this is not a migration defect; it needs the production DB objects or a DBA decision).

## Platform-blocked: Crystal Reports (22 `.rpt`, viewer `CrystalViewer.aspx` / `RptJournal.aspx`)

| Item | Detail |
|---|---|
| Pages | PAGE-262 `CrystalViewer.aspx` (WebForms, 3,076 lines code-behind), PAGE-263 `RptJournal.aspx` (dormant clone); the 10 `AccountingTaskReport/rpt*` hand-off actions; the "Print" buttons on Ledger / Cash Book / Bank Book / COA / Bank Reconciliation Statement / Approval pages (12 views call `window.open("/CrystalViewer/CrystalViewer.aspx")`) |
| Dependency | SAP Crystal Reports for .NET 13 runtime (`CrystalDecisions.*` assemblies) + System.Web WebForms page model |
| Exact error | the assemblies target .NET Framework only; there is no Crystal Reports SDK for .NET Core / .NET 10 (build error `CS0246 CrystalDecisions could not be found` when the files are compiled) |
| Evidence | `Accounts/CSTL_Pro_to_Pay/CrystalViewer/*.aspx.cs`, `packages.config` (`Monitor_CrystalDecisions.CrystalReports.Engine.13.0.2000`) |
| Cause | vendor platform limitation |
| What the new app does | `CrystalViewerController` answers the same URL with a Fiori page explaining the situation and showing the parameters the report page staged in session, so the user's request is not lost; the `rpt*` actions still stage `reportvalue`/`reportparam` exactly as before |
| Required action (user decision) | (a) keep the legacy site running only for Crystal printing (the `rpt*` hand-off could redirect to it), (b) re-implement the 8 UI-reachable reports as HTML/PDF views (new work, needs report layouts from the `.rpt` binaries), or (c) a reporting service. On-screen and Excel versions of ledger, cash/bank book, trial balance, COA and the dynamic statements are available in the migrated pages. |

## Local-environment-blocked: stored procedures missing from `CSTL_ACCDB_Unicorn` / `CSTL_ERPDB_Unicorn`

32 of the 226 called stored procedures do not exist locally (`database-dependencies.md`, section *Missing stored
procedures*). The pages that call them at load or on save:

| Page | Missing stored procedure(s) | Effect (same in legacy) | Required action |
|---|---|---|---|
| PAGE-006 Main Dashboard (`Dashboard/ApprovalDashBoard`, `VoucherApprovalDashBoard`) | `sp_GetApprovalMenuListByUser`, `sp_GetVoucherApprovalDashboard` | these two actions throw; the launchpad itself does not call them (legacy `Index` had the call commented out) | deploy the SPs if the approval dashboard is wanted; the views for these actions never existed in the legacy either (dead) |
| PAGE-014 Utility (`GetUserInfoStrig`) | `sp_Get_UserInfoString` | the DAL method exists; its only caller in `HomeController` is commented out | none (dead code) |
| PAGE-040 Reference Type Entry (`Ref_Sub_Type/ReferenceType`) | `sp_GetReferenceTypeByGlId` | the GL-wise lookup fails (SqlException → rollback → HTTP 500 in legacy) | deploy SP |
| PAGE-048 Cost Centre Entry — delete | `sp_Del_CostCenterInfoById`, `sp_GET_CompanyById` | delete button fails; entry/save work | deploy SP |
| PAGE-081 Chart of Account — subsidiary detail delete | `sp_DEL_SubsidiaryInfoDtlsNew` | removing a subsidiary row fails | deploy SP |
| PAGE-103 legacy `DebitVoucher/Create` pipeline | `sp_GetBankBranchByGlId`, `sp_GetCostCenterByGlId`, `sp_GetGLInfoById`, `sp_GetRefDetailsByRefTypeId`, `sp_Save_DebitVoucher*`, `sp_Get_VoucherMasterById`, `sp_Get_VoucherDetailsById` | the whole legacy pipeline is non-functional (also documented as dead in the legacy specs; the live debit voucher is `BankPayments`) | none — superseded page |
| PAGE-162/163 Bank Charge Entry / List (LC related bank payment) — update | `SP_UpdateBankPaymentMaster` | editing an existing bank charge fails; new entries work | deploy SP |
| PAGE-200 Balance Sheet Setup (`BalanceSheet/BSCreate`) | `sp_GET_BalanceSheetByCompanyId`, `sp_GET_BalanceSheetDetailByCompanyId`, `sp_I_BalanceSheetMaster`, `sp_I_BalanceSheetDetail`, `sp_U_BalanceSheetMaster`, `sp_D_BalanceSheetDetailByMasterId` | page cannot load or save its layout | deploy SPs |
| PAGE-201 Income Statement Setup (`IncomeStatement/ISCreate`) | `sp_GET_IncomeStatementByCompanyId`, `sp_GET_IncomeStatementDetailByCompanyId`, `sp_I_IncomeStatementDetail`, `sp_U_IncomeStatementMaster`, `sp_D_IncomeStatementDetailByMasterId` | same | deploy SPs |
| Trial balance "parameter-wise new" mode, Cash Flow all-company summary, monthly stock for income/balance | `sp_COAForTrailBalanceParameterwise_New`, `sp_GET_CashFlowSummaryBDTCurrencyAllCompany`, `sp_AllStockMonthly_ForIncomeBalance` | the affected report modes fail; other modes work | deploy SPs |
| Menu authorization (`AccessAuthoraization.CheckAccess`) | `sp_CheckMenuAccess` | never called in the legacy; the new opt-in `MenuAccessFilter` uses the session menu tree instead | none |

Evidence: `docs/migration/tools/data/spdeps.txt` (catalog query `gen-spdeps.sql`, run 2026-10-07 against `TOWSIF\MSSQLSERVER2019`).

## Blocked by missing test credentials (verification, not migration)

Functional testing, the OLD-vs-NEW golden-master run and the visual QA of authenticated pages need a login. The
only users are `demo`, `Anik`, `UHSL`, `balay` (`tblUser`); passwords are stored in clear and were **not read**
(credential handling requires the user's approval). See `final-migration-report.md` → *What remains*.
