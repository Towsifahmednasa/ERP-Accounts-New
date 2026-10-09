# Visual QA against the SAP Fiori wireframe

Reference: `D:\Wireframe-ERP-DISTRIBUTION\SAP_Fiori_Style_Wireframe_1.html` — shell (screen 02), Login, and the six
Accounts & Finance screens 80–85. Pixel equality with the legacy application is not a goal (brief §29).
Screenshots are taken with headless Chrome (`chrome.exe --headless=new --window-size=W,H --screenshot=...`); note
that headless Chrome enforces a minimum window width of about 500 px, so "phone" captures are taken at 500 px.

## Checklist

| Check | Where it is implemented | Status |
|---|---|---|
| Sidebar: "ERP — Accounts & Finance" heading, groups Chart of Accounts 80 … Cost Centre & Budget Control 85 with numbers, user's DB menu items underneath, active item highlighted | `Views/Shared/_SideNav.cshtml`, `Compat/MenuMap.cs`, `fiori-bridge.css` (`.navhead`, `.grp .nu`) | built; needs a login to capture |
| Shell bar: logo, title, search, notifications, user avatar/popover with Sign out | `Views/Shared/_Layout.cshtml` | built; needs a login to capture |
| Breadcrumb Home › group (nn) › page | `_Layout.cshtml` `.crumbs` | built; needs a login to capture |
| Page header (title + toolbar) on every page | `fiori-bridge.css` `.page-breadcrumb` mapping | built |
| Buttons (emphasized / default / negative), 8 px radius, 32 px height | `fiori-bridge.css` `.btn*`, `.button*` | built |
| Fields: label above, 32 px `.ctl` look, required marker, select2/pickadate skinned | `fiori-bridge.css` forms section | built |
| Tables: header row, 1 px list borders, hover, tabular numerals, bold totals; DataTables toolbar/pager restyled | `fiori-bridge.css` tables + DataTables sections | built |
| Dialogs/toasts: SweetAlert → Fiori dialog, toastr → Fiori toast colours | `fiori-bridge.css` | built |
| Launchpad tiles from the user's menu | `Views/Dashboard/Index.cshtml` | built; needs a login to capture |
| Login (wireframe split layout) at 1366×768 | `docs/ui/screenshots/login-1366.png` | PASS |
| Login at tablet 1024×768 | `docs/ui/screenshots/login-1024.png` | PASS |
| Login at 500 px (narrow: left panel hidden, card full width) | `docs/ui/screenshots/login-500.png` | PASS |
| Responsive rail (<1024 icons only) and drawer (<600) | `fiori.css` PART 2 media queries | built; needs a login to capture |

## Pending (needs a test login)

Capture and review every page of batches B–G at 1366×768 and 1024×768 and record per-page results in
`docs/migration/status.json` (`uiqa` field), then regenerate `migration-progress.md`.
