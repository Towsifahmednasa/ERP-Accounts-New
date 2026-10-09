# Legacy security findings and what the migration did about them

Source: `Accounts/docs/security.md` (findings S1–S17, read from code, nothing executed) plus the migration pass.
Policy (brief §17): do not weaken security, do not silently change business behaviour. Every change below is
framework-level and documented; where a fix would change what users can do, it is **opt-in** or **left as-is and listed**.

| # | Legacy finding | Risk | Migration decision | Where |
|---|---|---|---|---|
| S1 | Hand-rolled session login; password sent raw to `sp_GET_UserInfo_Acc` (the SP compares `tblUser.Password` in clear); no lockout/throttling; no session-id regeneration | credential storage/transport | **Preserved** (database and SP must not change). Session cookie is HttpOnly, new session container per app. Recommendation: hash passwords in the SP/table in a later DB change | `HomeController.CheckLogin`, `HomeDAL` |
| S2 | `Home/CheckLogin(userName, pass)` was a public GET action returning true/false and creating a session (credential oracle) | brute force, credentials in logs | **Closed**: marked `[NonAction]`. The login view only referenced it from commented-out script, so no UI behaviour changes | `tools/port-legacy-source.ps1` patch |
| S3 | `AccessAuthoraization.CheckAccess` (menu-based authorization via `sp_CheckMenuAccess`) never called; menu hiding was the only gate | any logged-in user can open any page by URL | **Opt-in**: `MenuAccessFilter` enforces "controller must be in the user's DB menu, admin bypass" for HTML page requests when `Security:EnforceMenuAccess = true`. Default **false** because the legacy never enforced it and `sp_CheckMenuAccess` does not exist in the local DBs (the filter uses the session menu tree instead of the SP) | `Compat/MenuAccessFilter.cs`, `appsettings.json` |
| S4 | Session check only on some page actions; most JSON/partial/save actions and 9 whole controllers (incl. `UtilityController`, 62 endpoints) had none | unauthenticated data reads/writes | **Closed**: global `SessionRequiredFilter` — every action requires a logged-in session except `[AllowAnonymous]` login/logout/error. Pages redirect to the login page exactly like the legacy guard; AJAX gets `401 {isSuccess:false, sessionExpired:true}` and the layout redirects to login (legacy returned the login HTML to the AJAX caller, which then failed to parse) | `Compat/SessionRequiredFilter.cs` |
| S5 | No role model; approver identity never checked | any user can approve/lock | **Preserved** (business rule lives in the DB/SPs; adding maker-checker is a business change). Listed in `legacy-defects.md` | — |
| S6 | No anti-forgery anywhere; most state-changing actions accept GET | CSRF | **Closed for POST**: `AutoValidateAntiforgeryTokenAttribute` globally, token in the login form and in a `RequestVerificationToken` header on every jQuery AJAX call (`$.ajaxSetup` in the layout). Legacy GET-triggerable saves remain reachable by GET (adding `[HttpPost]` would change 750 call sites' behaviour); listed as residual risk | `Program.cs`, `_Layout.cshtml` |
| S7 | List actions accept a browser-built SQL WHERE fragment (`param`) passed to SPs (`sp_GET_DebitVoucherList`, `sp_GET_ChartOfAccountParamSearch`, FY list, loan/LC lists…) | SQL injection if the SP concatenates dynamic SQL | **Preserved** (SP contract; the SPs must not change). The fragment is now only reachable by authenticated users (S4). The SP bodies are in the local DB and should be reviewed for `EXEC(@param)` before production — see `database-dependencies.md` | — |
| S8 | XSS: grids/dropdowns built with string concatenation + `.html(dbValue)`; some reports render DB text unescaped | stored XSS from master-data names | **Preserved** (inline page scripts are verbatim; encoding every concatenation is a rewrite of 44k lines of view script). Razor output stays encoded. Listed as residual risk | views |
| S9 | SQL Server `sa`-class credentials hard-coded in `DataManager/SqlUserAccess.cs` (+ commented alternates in `DashboardDAL`) | credential exposure, no least privilege | **Closed**: connection string template in configuration (`ConnectionStrings:Template`, integrated security locally); `SqlUserAccess.cs` replaced; no credential in the new source tree | `appsettings.json`, `DataManager/SqlUserAccess.cs` |
| S10 | No HTTPS enforcement; cookies without Secure; logout by GET; `LoginUserIdCookie` without HttpOnly | transport/cookie hygiene | Session cookie HttpOnly + essential; `LoginUserIdCookie` now HttpOnly (still never read). HTTPS redirection/HSTS is a hosting decision (enable `UseHttpsRedirection` when a certificate is configured). Logout stays a GET link (legacy UI) | `Program.cs`, `HomeController` |
| S11 | `debug=true`, version header, legacy menu with route names in HTML comments on every page, `ex.Message` returned in places | information disclosure | Developer exception page only in Development; production uses `/Home/Error`; the commented static menu is gone (new `_SideNav`). `ex.Message` responses preserved (page behaviour) | `Program.cs` |
| S12 | Lookup endpoints return any company's data; Crystal viewer prints any voucher for any session user | data exposure by URL | Reduced by S4 (login required). Company-scope checks would be a business change → preserved and listed | — |
| S13 | `Reprocess`, `SaveCurrencyConversion`, `SaveRefInfoDetails`, BS/IS save etc. had no auth check | unauthenticated writes | **Closed** by S4 | — |
| S14 | DR=CR balance, required fields and the approved-voucher edit lock are client-side only | integrity bypass by direct POST | **Preserved** (server-side validation = business logic change; the SPs may enforce it — their bodies are now readable in the local DB). Listed in `legacy-defects.md` | — |
| S15 | CDN scripts without SRI (SheetJS, Font Awesome), old jQuery 2.2.3 / Bootstrap 5.0.2 / SweetAlert 1.x | supply chain, known CVEs | **Preserved** (page scripts depend on these exact libraries); SheetJS/Font Awesome still from CDN. Recommendation: vendor them with SRI before production | `_Layout.cshtml` |
| S16 | No file upload found | — | n/a (imageuploadify library no longer loaded) | — |
| S17 | `.cshtml` not requestable directly | — | n/a on ASP.NET Core (views are compiled) | — |

## New in the migrated application

- `LegacyJsonBodyMiddleware` flattens JSON bodies into form values (MVC 5 `JsonValueProviderFactory` semantics) — it never evaluates content, and the anti-forgery check runs after it on the flattened form/header.
- Session keys are case-insensitive as in System.Web (`LegacySession`), stored server-side in the distributed memory cache (single server, like the legacy InProc session).
- `MenuAccessFilter` (opt-in) uses the session menu tree, so enabling it cannot be bypassed by a stale `sp_CheckMenuAccess`.

## Residual risks to decide before production

1. Password storage in clear in `tblUser` (S1) — DB change, out of migration scope.
2. SQL-fragment parameters (S7) — review the SP bodies for dynamic SQL.
3. GET-triggerable saves (S6) and client-only accounting validation (S14).
4. XSS from unencoded grid building (S8).
5. Enable `Security:EnforceMenuAccess` after confirming the DB menu rights are complete for every role.
