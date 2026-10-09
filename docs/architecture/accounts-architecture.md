# New_Accounts architecture

ASP.NET Core 10 MVC port of the legacy ASP.NET MVC 5.2.7 "CSTL Pro-to-Pay" accounting application
(`D:\Projects\Unicorn\Accounts`, read-only reference). Rule of the migration: **functionality = legacy, UI = SAP Fiori
wireframe** (`D:\Wireframe-ERP-DISTRIBUTION\SAP_Fiori_Style_Wireframe_1.html`, screens 80–85), database unchanged.

## Layers (unchanged shape, new host)

```
Browser  Fiori shell (_Layout) + legacy page markup/inline jQuery (verbatim) + fiori-bridge.css
   │  $.ajax GET/POST (JSON or form) with RequestVerificationToken header
   ▼
ASP.NET Core MVC  conventional route {controller=Home}/{action=Index}/{id?}   (legacy RouteConfig had only this route)
   │  filters: AutoValidateAntiforgeryToken · SessionRequiredFilter · MenuAccessFilter (opt-in)
   │  middleware: LegacyJsonBodyMiddleware (JSON body → form values, MVC 5 binding semantics)
   ▼
Controllers/**   39 legacy controllers ported verbatim, base class LegacyController (exposes Session[...] indexer)
   ▼
DAL/**           37 legacy DAL classes verbatim (ADO.NET, List<SqlParameter>, SqlDataReader/DataTable → DTO)
   ▼
DataManager/DataAccessManager.cs   verbatim: open connection + BeginTransaction, every command = stored procedure,
                                   Commit on close / Rollback on SqlException, CommandTimeout 9,600,000 s on reads
   ▼
SQL Server   CSTL_ERPDB_Unicorn (control panel: users, menu) · CSTL_ACCDB_Unicorn (accounting) — 226 stored procedures
```

## Project layout

| Folder | Content | Origin |
|---|---|---|
| `Compat/` | `LegacySession` (case-insensitive keys, JSON for objects), `LegacyController`, `HttpContext.Current` shim for DAL audit user, `LegacyJsonBodyMiddleware`, `SessionRequiredFilter`, `MenuAccessFilter`, `MenuMap` (DB menu → Fiori groups) | new (framework compatibility only) |
| `Controllers/` | legacy controllers (+ `CrystalViewerController` replacing the WebForms viewer page) | ported by script |
| `DAL/`, `Models/`, `Utility/`, `DataManager/` | legacy data access, DTOs, `SessionCollection`, `AccessAuthoraization` | ported by script (`SqlUserAccess` rewritten: config instead of hard-coded login) |
| `Views/` | 84 legacy views verbatim (markup + inline scripts); new `Shared/_Layout`, `Shared/_SideNav`, `Shared/_FioriIcons`, `Home/Index` (login), `Dashboard/Index` + `UserDashboard` (launchpad), `Shared/AccessDenied`, `CrystalViewer/Index` | ported / new |
| `wwwroot/VerticalAsset`, `wwwroot/AssetsStatic` | legacy client libraries the page scripts need (jQuery 2.2.3, Bootstrap 5.0.2, DataTables, select2, SweetAlert 1.x, toastr, pickadate, jQuery UI, `custom-functions.js`) | copied |
| `wwwroot/css/fiori/fiori.css` | Fiori Horizon tokens + components (verbatim from the wireframe, shared with New_Inventory) | wireframe |
| `wwwroot/css/fiori/fiori-bridge.css` | maps the legacy class vocabulary (`.card`, `.page-breadcrumb`, `.btn-*`, `.form-control`, `.table`, select2, DataTables, SweetAlert, toastr, accordion, modal) onto the tokens — the single design system applied to every page | new |
| `wwwroot/js/fiori/fiori.js` | shell popovers, drawer, dialog/toast helpers | shared with New_Inventory |
| `docs/`, `tests/golden-master/` | migration documentation, regeneration tools, OLD-vs-NEW harness | new |

## WebForms/MVC 5 → ASP.NET Core mapping actually used

| Legacy | New | Note |
|---|---|---|
| `Session["key"]` (System.Web, case-insensitive) | `LegacyController.Session` → `LegacySession` over `ISession` | keys lower-cased; `MenuList`, `reportvalue`, `reportparam` stored as JSON |
| `HttpContext.Current.Session[...]` in 8 DAL files | `New_Accounts.Compat.HttpContext.Current.Session` (IHttpContextAccessor) | audit user unchanged |
| `Json(x, JsonRequestBehavior.AllowGet)` | `Json(x)` with Newtonsoft (PascalCase, `\/Date(ms)\/`) | page scripts read `result[i].GLCode`; one view parses the MS date format |
| JSON body → action parameters (JsonValueProviderFactory) | `LegacyJsonBodyMiddleware` flattens to `Request.Form` | nested `DetailsInfo[0].Prop` keys as before; culture en-US for `"18 August, 2026"` |
| `@Html.Action("MenuGeneration")` + metisMenu | `_SideNav` partial from `Session["MenuList"]` grouped by `MenuMap` | menu content per user unchanged (same SP) |
| `HandleErrorAttribute` → `Error.cshtml` | `UseExceptionHandler("/Home/Error")` (non-Development) | |
| `CrystalViewer.aspx` (WebForms + Crystal 13) | `CrystalViewerController` explanatory page | blocked: no Crystal runtime on .NET 10 |
| hard-coded SQL login | `ConnectionStrings:Template` | |
| in-proc session, 20 min | `AddSession` 20 min idle, memory cache | single-server like legacy |

## UI composition (one design system)

- **App shell**: shell bar (title "ERP — Accounts & Finance", apps/notification/user popovers), side navigation with the six wireframe groups numbered 80–85 and the user's DB-menu items underneath, breadcrumb (Home › group (nn) › page), content page. Responsive: rail collapses to icons < 1024px, drawer < 600px (wireframe screen 02).
- **Page header**: the legacy breadcrumb block (`.page-breadcrumb` + `h5.cstlfs` + right `.btn-group`) is restyled by the bridge into the Fiori object-page title + toolbar, so all 69 pages get the same header without editing each one.
- **Forms / tables / dialogs**: Bootstrap controls → `.ctl` look; `.table` → `table.sap` look with tabular numerals; DataTables chrome → toolbar styling; SweetAlert → Fiori dialog; toastr → Fiori toast colours; accordion/modal → Fiori panels.
- **Accounting fields**: numeric cells keep tabular numerals; totals rows bold; status badges mapped to positive/critical/negative tokens. Values and calculations are untouched (inline scripts verbatim).

## Session contract (unchanged)

| Key | Set by | Readers |
|---|---|---|
| `username`, `loginuserid`, `loginusertype`, `loginuserempid`, `moduleid`="13", `UserDashboard`, `MenuList`, `UserInfo` | `HomeController.CheckLogin` | guards, audit fields, layout, launchpad |
| `reportvalue`, `reportparam` | `AccountingTaskReportController.rpt*` | `CrystalViewer` page (shows them; printing blocked) |
| `COAFromDate`, `COAToDate`, `TBCompanyName`, `TBFinacialYear` | never set (legacy defect) | `TrialBalanceController` (null reads, as before) |

## Build / run

```
dotnet restore && dotnet build        # 0 warnings, 0 errors (NoWarn list documented in the csproj)
dotnet run --urls http://localhost:5260
```
