# Final migration report — Accounts & Finance

Status date: 2026-10-07. **Provisional**: the code migration, build and documentation are complete; functional
testing of authenticated pages, the OLD-vs-NEW regression run and the visual QA of authenticated pages are pending
a test login (see *What remains*).

## Project

| | |
|---|---|
| Source | `D:\Projects\Unicorn\Accounts` (ASP.NET MVC 5.2.7 on .NET Framework 4.7.2, `CSTL_Pro_to_Pay`) — unchanged, SHA-256 verified (8,022 files) |
| Target | `D:\Projects\Unicorn\New_Accounts` |
| Technology | ASP.NET Core 10 · .NET 10 · MVC · Razor · Newtonsoft.Json · Microsoft.Data.SqlClient |
| Database | `CSTL_ACCDB_Unicorn` + `CSTL_ERPDB_Unicorn` on `TOWSIF\MSSQLSERVER2019`, unchanged |
| UI | SAP Fiori Horizon wireframe (`D:\Wireframe-ERP-DISTRIBUTION`, screens 80–85) |

## Migration statistics

| Metric | Count |
|---|---|
| Legacy pages/screens discovered (incl. partials, dead and stub pages) | 100 |
| Ported and building | 85 (all pages with a view) |
| Dead in legacy (no view / never reachable), listed not ported | 14 |
| Blocked by platform (Crystal viewer pages) | 2 (PAGE-260/261) + 12 "Print" buttons that open them |
| Blocked locally by missing DB objects (page still works for the rest of its functions) | 6 pages, 32 stored procedures |
| Controllers ported | 39 (+1 new `CrystalViewerController`) |
| DAL classes / model classes ported verbatim | 37 / 72 |
| Views ported verbatim / replaced by Fiori views | 84 / 5 (layout, menu partial, login, 2 dashboards) |
| Reports (on-screen/Excel, migrated) | Ledger ×3, Cash Book, Bank Book, COA, Bank Reconciliation Statement, Trial Balance (7 modes), Balance Sheet / Income Statement (dynamic + legacy), Cash Flow, Voucher Validation, Voucher print (HTML) |
| Crystal reports (`.rpt`) | 22 (8 UI-reachable) — blocked |
| Stored procedures referenced | 226 (194 present locally, 32 missing) |
| Database tables/views/functions referenced by those SPs | 114 (first time evidenced — the legacy repo had no SQL) |
| External integrations | SQL Server ×2 DBs, Crystal Reports 13 (blocked), CDN assets (SheetJS, Font Awesome), in-DB "ERP voucher integration" filter (`sp_GET_ERPVoucherIntegrationByType`, preserved) |

## Build

| | |
|---|---|
| `dotnet restore` | PASS |
| `dotnet build` | PASS |
| Warnings | 0 (legacy-code analyzers suppressed with documented reasons in the csproj) |
| Errors | 0 |

## Database

| | |
|---|---|
| Schema changed | NO |
| Stored procedures changed | NO |
| Tables changed | NO |
| Same SPs / parameters / transaction behaviour as legacy | YES — DAL and `DataAccessManager` are verbatim (connection-level transaction, commit on close, rollback on `SqlException`) |

## Functional testing

| | |
|---|---|
| Smoke tests without login (login page, auth redirect, 401 JSON for AJAX, CSRF rejection, failed-login message, static assets) | 6 passed |
| Authenticated page tests / golden-master OLD-vs-NEW run (`tests/golden-master/run.ps1`: 68 page GETs + 54 JSON endpoints) | **Blocked: no test credential** |
| Legacy site hosted for comparison | YES — IIS Express on port 30408 from a scratch copy, database access confirmed |

## UI

| | |
|---|---|
| SAP Fiori wireframe | APPLIED — shell, side navigation (groups 80–85 with the user's DB menu), breadcrumb, launchpad, login; every ported page restyled through `fiori-bridge.css` (one design system) |
| Visual QA | Login page verified at 1366×768 and phone width; authenticated pages pending login |
| Responsive | rail → icons < 1024 px, drawer < 600 px, tables scroll horizontally |

## Known legacy issues

See `legacy-defects.md` (D01–D25) and the 491-gap register in `Accounts/specs/SPEC-GAPS.md`. Highlights: client-only
accounting validation, SQL-fragment list filters, audit fields inconsistently user-name/user-id, `UpdateDate` bound to
`UpdateBy` in voucher saves, dashboards with no real data, menu rows pointing at non-existent controllers.

## Security findings

See `docs/security/legacy-security-findings.md`. Closed in the migration: unauthenticated endpoints (global session
guard), CSRF on POST, hard-coded SQL credentials, credential-oracle action, information disclosure. Preserved and
listed: clear-text passwords in `tblUser`, SQL-fragment parameters, client-only validation, unencoded grid building,
GET-triggerable saves. Opt-in: DB-menu page authorization (`Security:EnforceMenuAccess`).

## Final recommendation

Not yet ready for production: (1) run the golden-master and functional test passes with a real user (pending
credential), (2) decide the Crystal printing strategy (`blocked-pages.md`), (3) deploy the 32 missing stored
procedures to the test database or confirm they are obsolete, (4) decide on the residual security items before
exposure beyond the LAN. The code, database contract and documentation are complete; the remaining work is
verification and two product decisions.

## What remains

| Item | Owner |
|---|---|
| Provide a test login (or approve reading the `demo` password from `tblUser`, as done for Inventory batch 1) and approve test rows (`ZZTEST`-marked, deleted afterwards) for save tests | user |
| Run `tests/golden-master/run.ps1 -User … -Password …`, browser functional tests per batch, UI QA screenshots of authenticated pages; update `status.json` → regenerate docs | migration |
| Crystal printing decision | user |
| Missing SPs on the local DB | user / DBA |
