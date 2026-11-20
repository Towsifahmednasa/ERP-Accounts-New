# New_Accounts — Accounts & Finance (ASP.NET Core 10 MVC)

Migration of the legacy ASP.NET MVC 5 accounting application `D:\Projects\Unicorn\Accounts` (read-only reference)
to ASP.NET Core 10 / .NET 10 MVC, keeping the existing SQL Server databases, stored procedures and business logic,
with the SAP Fiori Accounts & Finance interface from `D:\Wireframe-ERP-DISTRIBUTION`.

## Run

```
dotnet restore
dotnet build                      # 0 warnings / 0 errors
dotnet run --urls http://localhost:5260
```

Configuration: `appsettings.json` → `ConnectionStrings:Template` (SQL Server, `{db}` placeholder), `Db:ControlPanel`
(`CSTL_ERPDB_Unicorn`), `Db:CSTL_ERP_DB` (`CSTL_ACCDB_Unicorn`), `Security:EnforceMenuAccess` (opt-in server-side
menu authorization). No credentials live in source.

## Where to look

| Need | Document |
|---|---|
| How it is built, the compatibility layer, session contract | `docs/architecture/accounts-architecture.md` |
| Every legacy page with batch, SPs, tables, status | `docs/migration/all-pages.md` |
| Batches and order of verification | `docs/migration/migration-batches.md` |
| Status board | `docs/migration/migration-progress.md` (+ `status.json`) |
| Controller → DAL → SP → table chains | `docs/migration/dependency-map.md` |
| All 226 stored procedures resolved against the local DBs | `docs/migration/database-dependencies.md` |
| What is blocked and why | `docs/migration/blocked-pages.md` |
| Legacy defects preserved/documented | `docs/migration/legacy-defects.md` |
| Security findings and decisions | `docs/security/legacy-security-findings.md` |
| Final report | `docs/migration/final-migration-report.md` |

## Accounts Integrity Dashboard (new page, not ported from the legacy app)

`/AccountsIntegrity/Index` lists the voucher problems of Delivery Confirmation, Customer Payment, Invoice Adjustment,
Non-Transactional Sales Return and Delivery Sales Return (missing voucher, value mismatch, subsidiary ledger, reference),
with a Fix button per transaction (and "fix all" per problem). It reads the Sales DB (`Db:SalesDb`) and the accounts DB.

- Detectors: `Sql/AccountsIntegrity/Detect_*.sql` (one per page, `{ACC}` / `{ERP}` are replaced by `Db:CSTL_ERP_DB` / `Db:ControlPanel`).
- Fixes: `DAL/AccountsIntegrity/AccountsIntegrityFixSql.cs`. Each fix runs in its own transaction: re-check, fix, check again, commit.
  The "Simulate only" box runs the same steps and rolls back. Posting fixes call the same stored procedures as the pages.
- Every fix attempt is logged in `dbo.tblAccountsIntegrityFixLog` (Sales DB, created on first use).
- The page is not in the DB-driven side menu; open it by URL or add a `tbl_MainMenu_MVC` row (controller `AccountsIntegrity`, action `Index`).

## Tools

- `docs/migration/tools/port-legacy-source.ps1` — re-runnable mechanical port of the legacy code (controllers, DAL, models, views, assets) plus the documented hand patches.
- `docs/migration/tools/gen-migration-docs.py` — regenerates the inventory/dependency/progress documents from evidence (`gen-spdeps.sql` dumps the SP → object references from the local DBs).
- `docs/migration/tools/verify-legacy-unchanged.sh` — SHA-256 check that the legacy source is untouched.
- `tests/golden-master/run.ps1` — OLD (IIS Express) vs NEW read-only regression: page GETs + every JSON lookup/list endpoint.
