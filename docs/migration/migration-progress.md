# Migration progress

Generated 2026-10-07 from `status.json`. Statuses: NOT STARTED / ANALYZING / IN PROGRESS / BLOCKED / MIGRATED / TESTING / DONE / N/A (dead in legacy).

| Status | Pages |
|---|---|
| BLOCKED | 2 |
| BLOCKED-LOCAL | 6 |
| DONE | 1 |
| MIGRATED | 78 |
| N/A | 13 |

| Page | Batch | Status | Build | Functional Test | UI QA | Notes |
|---|---|---|---|---|---|---|
| PAGE-001 Login (this is the REAL login screen) | A | DONE | PASS | PASS (login page, CSRF, failed-login message, redirect) | PASS (1366x768, 1024, 500) | Fiori login screen |
| PAGE-002 Home/Login (`View("Login")`) | A | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-003 Home/LockScreen (`View("LockScreen")`) | A | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-004 About (MVC template leftover) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-005 Contact (MVC template leftover) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-006 Main Dashboard (`Dashboard/Index` when IsMainDashboard=true) | A | BLOCKED-LOCAL (missing SP: sp_GetApprovalMenuListByUser, sp_GetVoucherApprovalDashboard) | PASS | Pending | Pending |  |
| PAGE-007 User Dashboard (`Dashboard/Index` when IsMainDashboard=false; also `/D | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-008 Dashboard/_FilterMasterInfo (shared report filter partial) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-009 Shared/_Layout (page shell) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-010 Shared/_Partial_Menu_Leyout (DB-driven 3-level menu) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-011 Shared/Error | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-012 Dashboard/ApprovalDashBoard | A | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-013 Dashboard/VoucherApprovalDashBoard | A | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-014 Utility shared AJAX endpoint provider (no UI) | A | BLOCKED-LOCAL (missing SP: sp_Get_UserInfoString) | PASS | Pending | Pending |  |
| PAGE-015 Shared JS library `AssetsStatic/JS/custom-functions.js` (no UI) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-016 Logout (action; renders login view) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-017 Home/CheckLogin (public bool method = routable GET action, no view) | A | MIGRATED | PASS | Pending | Pending |  |
| PAGE-020 Financial Year List | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-021 Financial Year Create/Edit | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-022 Financial Year Lock | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-023 Bank Account List | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-024 Bank Account Create/Edit | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-025 Cash Account List | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-026 Cash Account Create/Edit | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-027 Currency Conversion Rate | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-040 Reference Type Entry | B | BLOCKED-LOCAL (missing SP: sp_GetReferenceTypeByGlId) | PASS | Pending | Pending |  |
| PAGE-041 Reference Type List | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-042 Subsidiary Type Entry | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-043 Subsidiary Type List | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-044 Reference Info | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-045 Reference Info checklist partial | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-046 Subsidiary Info | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-047 Subsidiary Info checklist partial | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-048 Cost Centre Entry | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-049 Cost Centre List | B | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-050 Reference Type (Refference stub) | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-051 Subsidiary Type (Refference stub) | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-052 Cost Center Info (Refference stub) | B | MIGRATED | PASS | Pending | Pending |  |
| PAGE-060 Ledger Head List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-061 Control Ledger List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-062 Control Ledger Entry | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-063 Sub-Control Ledger List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-064 Sub-Control Ledger Entry | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-065 Sub Sub Control Ledger List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-066 Sub Sub Control Ledger Entry | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-067 Subsidiary Ledger List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-068 Subsidiary Ledger Entry | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-080 Chart Of Account Entry (create/edit GL) | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-081 Chart Of Account List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-082 Reference-type partial (_RefPartial) | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-083 Subsidiary-type partial (_SubSidaryPartial) | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-084 Cost-centre partial (_CostCenterPartial) | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-085 Opening Balance Entry | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-086 Opening Balance List | C | MIGRATED | PASS | Pending | Pending |  |
| PAGE-100 Debit Voucher List | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-101 Debit Voucher Entry (bank/cash payment) | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-102 Voucher detail row partial | D | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-103 Debit/Payment Voucher (legacy eflag ed/ap) | D | BLOCKED-LOCAL (missing SP: sp_GetBankBranchByGlId, sp_GetCostCenterByGlId, sp_GetGLInfoById, sp_GetRefDetailsByRefTypeId) | PASS | Pending | Pending |  |
| PAGE-104 Credit Voucher List | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-105 Credit Voucher Entry | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-120 Journal List | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-121 Journal Entry (create/edit) | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-122 Journal GL row partial | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-123 Contra List | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-124 Contra Entry (4 transfer types) | D | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-125 Bank Reconciliation Entry | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-126 Reconcile grid partial | D | MIGRATED | PASS | Pending | Pending |  |
| PAGE-140 All Voucher Approval | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-141 Loan Approval | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-142 Bank Charge Approval | E | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-150 Budget List | E | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-151 Budget Entry | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-152 Budget Month Amount grid (partial) | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-160 Loan Info Entry | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-161 Loan List | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-162 Bank Charge Entry (LC related bank payment) | E | BLOCKED-LOCAL (missing SP: SP_UpdateBankPaymentMaster) | PASS | Pending | Pending |  |
| PAGE-163 Bank Charge List | E | MIGRATED | PASS | Pending | Pending |  |
| PAGE-200 Balance Sheet Setup | F | BLOCKED-LOCAL (missing SP: sp_D_BalanceSheetDetailByMasterId, sp_GET_BalanceSheetByCompanyId, sp_GET_BalanceSheetDetailByCompanyId, sp_I_BalanceSheetDetail) | PASS | Pending | Pending |  |
| PAGE-201 Income Statement Setup | F | MIGRATED | PASS | Pending | Pending |  |
| PAGE-202 Balance Sheet Dynamic Report | F | MIGRATED | PASS | Pending | Pending |  |
| PAGE-203 Income Statement Dynamic Report | F | MIGRATED | PASS | Pending | Pending |  |
| PAGE-204 Income Statement Report (legacy SP-based) | F | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-205 Cash Flow Report | F | MIGRATED | PASS | Pending | Pending |  |
| PAGE-206 Trial Balance Report | F | MIGRATED | PASS | Pending | Pending |  |
| PAGE-207 Balance Sheet Report (legacy, view missing) | F | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-220 Accounts Operation Report (ReportList) | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-221 Ledger Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-222 Subsidiary Wise Ledger Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-223 Cost Center Wise Ledger Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-224 Chart of Account Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-225 Cash Book Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-226 Bank Book Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-227 Bank Reconciliation Statement Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-228 Voucher Validation / Accounts Integrity Report | G | MIGRATED | PASS | Pending | Pending |  |
| PAGE-229 AccountingTaskReport controller (hand-off endpoints; no view; Views/Ac | G | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-260 Crystal Report Viewer | G | BLOCKED (Crystal Reports has no .NET 10 runtime) | - | Blocked | - | CrystalViewerController renders an explanatory page at the same URL |
| PAGE-261 Payment Voucher HTML print | G | BLOCKED (Crystal Reports has no .NET 10 runtime) | - | Blocked | - | dormant clone of PAGE-260 |
| PAGE-290 Test List ("Voucher List") | H | N/A (dead in legacy) | - | Pending | Pending |  |
| PAGE-291 Test Create ("Voucher Info") | H | MIGRATED | PASS | Pending | Pending |  |
| PAGE-292 Test Add Country Row (partial) | H | MIGRATED | PASS | Pending | Pending |  |