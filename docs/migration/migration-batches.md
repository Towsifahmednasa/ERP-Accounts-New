# Migration batches

Batches follow dependency order and business function, not the alphabet. The whole code base was ported in one
mechanical pass (`tools/port-legacy-source.ps1`: 153 C# files, 84 views, assets — verbatim business code), so every
batch builds; the batches drive **verification order** (functional test, DB regression, UI QA) and the order in
which blocked items are resolved. Page lists per batch: `all-pages.md` (column *Batch*).

| Batch | Scope | Legacy module(s) | Pages | Depends on | Fiori navigation group |
|---|---|---|---|---|---|
| **A — Application foundation** | login/session (`Home`), launchpad (`Dashboard`), Fiori shell (`_Layout`, `_SideNav`), DB-driven menu, 62 `Utility` lookup endpoints, filter partial, `custom-functions.js`, error page, CSRF/auth filters | PLT | 17 | — | shell |
| **B — Master setup** | Financial Year (list/create/lock), Bank Account, Cash Account, Currency Conversion, Reference Type/Info, Subsidiary Type/Info, Cost Centre | MST | 13 | A | 80 Chart of Accounts · 83 Bank & Cash · 85 Cost Centre |
| **C — Chart of accounts & opening balance** | Ledger Head, Control Ledger, Sub-Control, Sub-Sub-Control, Chart of Account (+ cost centre / reference / subsidiary partials), Subsidiary ledger, Opening Balance | COA | 16 | A, B | 80 Chart of Accounts |
| **D — Vouchers & bank reconciliation** | Debit voucher / Bank Payments (bank & cash), Credit voucher, Journal, Contra (4 transfer types), Bank Reconciliation, legacy `DebitVoucher/Create` pipeline | VCH | 7 | A, B, C | 81 Voucher Entry · 83 Bank & Cash |
| **E — Approvals, loan/LC, budget** | All-voucher approval, Loan approval, Bank-charge approval, Loan entry/list, LC-related bank payment, Budget entry/list | APR, LN, BUD | 27 | B, C, D | 81 Voucher Entry · 82 Receivables & Payables · 85 Budget |
| **F — Financial statements** | Balance Sheet / Income Statement layout definition, dynamic BS/IS, Trial Balance (7 modes), Cash Flow | FST | 8 | C, D | 84 Trial Balance & Financials |
| **G — Reports, print & utilities** | Report launcher, Ledger (GL / cost centre / subsidiary), Cash Book, Bank Book, COA report, Bank Reconciliation Statement, Voucher Validation + reprocess, voucher print (HTML), Crystal viewer hand-off (blocked) | RPT | 12 | C, D | 84 Trial Balance & Financials · 83 Bank & Cash |
| **H — Sandbox/test scaffold** | `TestController` demo master/detail (not a business feature) | TST | (in A/PLT count) | A | More |

## Execution order used

1. Discovery (legacy reverse-engineering docs + source + local DB catalog) — done 2026-10-07.
2. Foundation: project, compatibility layer (session shim, JSON-body binder, auth/CSRF filters), Fiori shell, login, launchpad.
3. Mechanical port of all batches B–H in one pass (controllers, DAL, models, views, assets); build to 0 errors / 0 warnings.
4. Verification batch by batch with the golden-master harness (`tests/golden-master/run.ps1`, OLD site on IIS Express vs NEW) and the browser (functional + UI QA against the wireframe).
5. Blocked items recorded in `blocked-pages.md`; legacy defects in `legacy-defects.md`.

## Wireframe mapping (brief §8 example categories → real system)

| Wireframe group (screen) | What the legacy system actually has |
|---|---|
| Chart of Accounts (80) | 5-level account tree + subsidiary ledger + opening balance + the master data that scopes vouchers (financial year, currency rates, reference/subsidiary types) |
| Voucher Entry (81) | Debit (bank/cash payment), Credit, Journal, Contra vouchers; all-voucher approval |
| Receivables & Payables Ageing (82) | **No AR/AP ageing module exists in the legacy application.** The closest functions — Loan entry/approval and LC-related bank-charge payments — are placed here. Ageing reports would be new business functionality (out of scope for a migration). |
| Bank & Cash Reconciliation (83) | Bank/cash account masters, manual bank reconciliation, Cash Book, Bank Book, Bank Reconciliation Statement |
| Trial Balance & Financials (84) | Trial Balance (7 modes), Balance Sheet / Income Statement definitions + dynamic statements, Cash Flow, report launcher, ledger/COA reports, voucher validation |
| Cost Centre & Budget Control (85) | Cost centre master, budget master + monthly detail (no budget-vs-actual control exists in the legacy) |
