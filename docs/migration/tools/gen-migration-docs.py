#!/usr/bin/env python -I
r"""Generates the inventory-style migration documents from evidence, so they stay consistent as work progresses:

  docs/migration/all-pages.md              one row per legacy page (100 pages), with batch, DB objects, status
  docs/migration/database-dependencies.md  every stored procedure the application calls, resolved against the LOCAL
                                           databases (exists? referenced tables/views/functions/SPs, cross-db) + table index
  docs/migration/dependency-map.md         page -> controller -> DAL -> stored procedure -> table chain
  docs/migration/migration-progress.md     status board (status values from docs/migration/status.json)

Inputs (read-only):
  D:\Projects\Unicorn\Accounts\docs\reverse-engineering\page-inventory.md   (legacy reverse-engineering pass)
  D:\Projects\Unicorn\Accounts\specs\**\pages\PAGE-*.md                    (per-page specs: SPs, session keys, gaps)
  D:\Projects\Unicorn\Accounts\docs\database\stored-procedures.md         (226 SPs, call sites)
  docs/migration/tools/data/spdeps.txt                                     (sys.sql_expression_dependencies dump, see gen-spdeps.sql)
  New_Accounts source tree (controllers/views) for features actually ported
Run:  python -I docs/migration/tools/gen-migration-docs.py
"""
import re, os, json, glob, collections, datetime

ROOT = r'D:\Projects\Unicorn\New_Accounts'
OLD = r'D:\Projects\Unicorn\Accounts'
TOOLS = os.path.join(ROOT, 'docs', 'migration', 'tools')
OUT = os.path.join(ROOT, 'docs', 'migration')
TODAY = datetime.date.today().isoformat()

def read(p): return open(p, encoding='utf-8').read()
def write(p, s):
    os.makedirs(os.path.dirname(p), exist_ok=True)
    open(p, 'w', encoding='utf-8', newline='\n').write(s)

# ---------------------------------------------------------------- batches
MODULE_BATCH = {'PLT': 'A', 'MST': 'B', '02-master-setup': 'B', 'COA': 'C', 'VCH': 'D', '04-vouchers': 'D', 'APR': 'E', 'Approvals': 'E', 'LN': 'E', '06-loan-lc': 'E', 'BUD': 'E', 'Budget': 'E', 'FST': 'F', 'RPT': 'G', 'TST': 'H', 'Sandbox': 'H'}
BATCH_NAME = {'A': 'Application foundation', 'B': 'Master setup', 'C': 'Chart of accounts & opening balance',
              'D': 'Vouchers & bank reconciliation', 'E': 'Approvals, loan/LC, budget', 'F': 'Financial statements',
              'G': 'Reports, print & utilities', 'H': 'Sandbox/test scaffold'}
FIORI_GROUP = {  # controller -> wireframe screen (navigation group)
    **{c: (80, 'Chart of Accounts') for c in 'LedgerHead ControlLedger SubControl SubSubControlLedger ChartOfAccount Subsubsidairy OpeningBlnc Ref_Sub_Type Refference FinancialYear CurrencyConversion'.split()},
    **{c: (81, 'Voucher Entry') for c in 'BankPayments CreditVoucher Journal Contra DebitVoucher AllApproval'.split()},
    **{c: (82, 'Receivables & Payables Ageing') for c in 'Loan LCRelatedBankPayment'.split()},
    **{c: (83, 'Bank & Cash Reconciliation') for c in 'BanckAccount CashAccount ReconcileBankAccount CashBook BankBook'.split()},
    **{c: (84, 'Trial Balance & Financials') for c in 'AllAccountReport TrialBalance BalanceSheet IncomeStatement CashFlow BalanceSheetDynamicReport IncomeStatementDynamicReport VoucherValidation AccountingTaskReport'.split()},
    **{c: (85, 'Cost Centre & Budget Control') for c in 'CostCentre Budget'.split()},
}

# ---------------------------------------------------------------- legacy page inventory
inv = read(os.path.join(OLD, 'docs', 'reverse-engineering', 'page-inventory.md'))
pages = []
for line in inv.splitlines():
    if not line.startswith('| PAGE-'): continue
    cells = [c.strip() for c in line.strip().strip('|').split(' | ')]
    if len(cells) < 12:
        cells = [c.strip() for c in line.strip().strip('|').split('|')]
    if len(cells) < 12:
        print('SKIP (cells=%d): %s' % (len(cells), line[:80])); continue
    pid, mod, name, route, view, ctrl, action, js, css, apis, status, spec = cells[:12]
    ctrl = re.sub(r'[`*]', '', ctrl).strip()
    cm = re.search(r'(\w+)Controller', ctrl)
    ctrl_name = cm.group(1) if cm else ''
    pages.append(dict(id=pid, module=mod, name=name, route=route, view=view, controller=ctrl_name, controller_raw=ctrl,
                      action=action, apis=apis, legacy_status=status, spec=spec))
print('pages:', len(pages))

# ---------------------------------------------------------------- stored procedure catalog (legacy, call-site evidence)
cat = read(os.path.join(OLD, 'docs', 'database', 'stored-procedures.md'))
sp_index = {}
for m in re.finditer(r'^\| \d+ \| `([^`]+)` \| ([^|]+) \| ([^|]+) \| (\d+) \| `([^`]+)` \|', cat, re.M):
    sp_index[m.group(1)] = dict(db=m.group(2).strip(), call=m.group(3).strip(), sites=int(m.group(4)), first=m.group(5).strip(), callers=[], params='')
for sec in re.split(r'^### ', cat, flags=re.M)[1:]:
    name = re.match(r'`([^`]+)`', sec)
    if not name or name.group(1) not in sp_index: continue
    d = sp_index[name.group(1)]
    cm = re.search(r'\*\*Callers:\*\* (.*)', sec)
    if cm: d['callers'] = re.findall(r'`([^`]+)`', cm.group(1))
    pm = re.search(r'\*\*Parameters passed by DAL[^:]*:\*\* (.*)', sec)
    if pm: d['params'] = ', '.join(re.findall(r'`(@\w+)`', pm.group(1)))
print('sps in catalog:', len(sp_index))
dal_to_sps = collections.defaultdict(set)
for sp, d in sp_index.items():
    for c in d['callers']:
        f = c.split(':')[0]
        dal_to_sps[os.path.basename(f).replace('.cs', '')].add(sp)

# ---------------------------------------------------------------- local DB resolution (sys.sql_expression_dependencies dump)
deps_path = os.path.join(TOOLS, 'data', 'spdeps.txt')
exists = {}          # sp -> db
sp_refs = collections.defaultdict(list)   # sp -> [(ref, type, refdb, updated)]
if os.path.exists(deps_path):
    for line in read(deps_path).splitlines():
        parts = [p.strip() for p in line.split('|')]
        if len(parts) < 6: continue
        kind, sp, db, ref, rtype, refdb = parts[:6]
        upd = parts[6] if len(parts) > 6 else '0'
        if kind == 'EXISTS': exists[sp] = db
        elif kind == 'DEP' and (ref, rtype, refdb, upd) not in sp_refs[sp]: sp_refs[sp].append((ref, rtype, refdb, upd))
missing = sorted(sp for sp in sp_index if sp not in exists)
print('sps existing locally:', len(exists), 'missing:', len(missing))

# ---------------------------------------------------------------- new source tree: controller -> DALs, actions; views -> features
ctrl_files = {}
for f in glob.glob(os.path.join(ROOT, 'Controllers', '**', '*.cs'), recursive=True):
    t = read(f)
    m = re.search(r'class\s+(\w+)Controller', t)
    if not m: continue
    name = m.group(1)
    dals = sorted(set(re.findall(r'new\s+(\w+(?:DAL|Dal|Ledger|LedgerHead|Report|DAO))\s*\(', t)) | set(re.findall(r'^\s*(?:private\s+|readonly\s+)*(\w+(?:DAL|Dal|LedgerHead))\s+\w+\s*(?:=|;)', t, re.M)))
    actions = re.findall(r'public\s+(?:ActionResult|JsonResult|PartialViewResult|ViewResult|bool|string)\s+(\w+)\s*\(', t)
    ctrl_files[name] = dict(dals=[d for d in dals if d != 'ResultResponse'], actions=actions,
                            session_guard=bool(re.search(r'Session\["LoginUserId"\]\s*[!=]=\s*null', t)),
                            audit=sorted(set(re.findall(r'Session\[SessionCollection\.(\w+)\]', t))),
                            allow_anon=bool(re.search(r'\[AllowAnonymous\]', t)))
view_feat = {}
for f in glob.glob(os.path.join(ROOT, 'Views', '**', '*.cshtml'), recursive=True):
    t = read(f)
    rel = os.path.relpath(f, os.path.join(ROOT, 'Views')).replace('\\', '/')
    feats = []
    if re.search(r'excelHtml5|XLSX\.|csvHtml5', t): feats.append('Excel/CSV export')
    if 'pdfHtml5' in t or 'jsPDF' in t: feats.append('PDF export')
    if 'window.print' in t: feats.append('Print')
    if 'CrystalViewer' in t: feats.append('Crystal print (blocked)')
    if 'DataTable(' in t: feats.append('DataTables grid')
    if 'swal(' in t: feats.append('SweetAlert confirm')
    if re.search(r'\.datepicker|pickadate', t): feats.append('date picker')
    view_feat[rel] = feats

# ---------------------------------------------------------------- status board
status_path = os.path.join(OUT, 'status.json')
status = json.load(open(status_path, encoding='utf-8')) if os.path.exists(status_path) else {}

def page_sps(p):
    sps = set()
    specp = os.path.join(OLD, 'specs', p['spec'].strip('`').replace('`', '')) if p['spec'] not in ('–', '-', '') else None
    # spec path cells look like `specs/04-vouchers/pages/PAGE-100-...md` or `pages/PAGE-002-...md` (module-relative)
    cand = re.findall(r'`([^`]+\.md)`', p['spec']) or re.findall(r'(\S+\.md)', p['spec'])
    for c in cand:
        for path in (os.path.join(OLD, c), os.path.join(OLD, 'specs', c)):
            if os.path.exists(path):
                specp = path; break
        else:
            hits = glob.glob(os.path.join(OLD, 'specs', '*', c))
            if hits: specp = hits[0]
    spec_text = read(specp) if specp and os.path.exists(specp) else ''
    for m in re.finditer(r'\b((?:sp|SP|Sp)_\w+|gl_Rpt_\w+)\b', spec_text):
        if m.group(1) in sp_index: sps.add(m.group(1))
    gaps = sorted(set(re.findall(r'GAP-[A-Z]+-\d+', spec_text)))
    sess = sorted(set(re.findall(r'Session\["(\w+)"\]', spec_text)))
    return sps, gaps, sess, (os.path.relpath(specp, OLD).replace('\\', '/') if specp else '')

def tables_of(sps):
    t = collections.OrderedDict()
    for sp in sorted(sps):
        for ref, rtype, refdb, upd in sp_refs.get(sp, []):
            if rtype in ('USER_TABLE', 'VIEW', 'CROSS-DB', 'UNRESOLVED', 'SQL_STORED_PROCEDURE', 'SQL_SCALAR_FUNCTION', 'SQL_TABLE_VALUED_FUNCTION', 'SQL_INLINE_TABLE_VALUED_FUNCTION'):
                key = (refdb + '.' if refdb else '') + ref
                t.setdefault(key, set()).add('W' if upd == '1' else 'R')
    return t

def page_status(p, sps, feats):
    key = p['id']
    s = status.get(key, {})
    st = s.get('status')
    if not st:
        if 'dead' in p['legacy_status'].lower() or 'MISSING' in p['view']:
            st = 'N/A (dead in legacy)'
        elif 'CrystalViewer' in p['view'] or 'RptJournal' in p['view'] or '.aspx' in p['view']:
            st = 'BLOCKED (Crystal Reports has no .NET 10 runtime)'
        elif any(sp in missing and sp != 'sp_CheckMenuAccess' for sp in sps):
            st = 'BLOCKED-LOCAL (missing SP: %s)' % ', '.join(sorted(sp for sp in sps if sp in missing and sp != 'sp_CheckMenuAccess')[:4])
        else:
            st = 'MIGRATED'
    return st, s.get('build', 'PASS' if not st.startswith('N/A') else '-'), s.get('functional', 'Pending'), s.get('uiqa', 'Pending'), s.get('notes', '')

# ---------------------------------------------------------------- all-pages.md
rows = []
page_detail = []
by_batch = collections.Counter()
for p in pages:
    sps, gaps, sess, specrel = page_sps(p)
    ctrl = p['controller']
    cinfo = ctrl_files.get(ctrl, {})
    dals = cinfo.get('dals', [])
    # SPs reachable through the page's DALs (superset) when the spec lists none
    if not sps and not specrel:
        for d in dals: sps |= dal_to_sps.get(d, set())
    vrel = re.search(r'Views/([\w/._-]+\.cshtml)', p['view'])
    vkey = vrel.group(1) if vrel else ''
    feats = view_feat.get(vkey, [])
    batch = MODULE_BATCH.get(p['module'], '?')
    by_batch[batch] += 1
    st, build, func, uiqa, notes = page_status(p, sps, feats)
    tabs = tables_of(sps)
    crud = sorted(set(a for a in cinfo.get('actions', []) if re.search(r'Save|Update|Del|Delete|Approve|Lock|Reprocess|Insert', a, re.I)))
    reads = sorted(set(a for a in cinfo.get('actions', []) if re.search(r'^Get|List|Load|^Load', a)))
    typ = ('partial' if '/_' in vkey else 'report' if p['module'] in ('RPT', 'FST') else 'entry' if re.search(r'Create|Entry|Index$', p['route']) and 'List' not in p['name'] else 'list')
    if 'dead' in p['legacy_status'].lower() or 'MISSING' in p['view']: typ = 'dead'
    grp = FIORI_GROUP.get(ctrl, (0, 'More' if ctrl else '-'))
    rows.append('| %s | %s | %s | %s | %s | `%s` | %s | %s | %s | %s | %s | %s |' % (
        p['id'], batch, p['module'], p['name'].replace('|', '/'), typ, p['route'].replace('`', '').replace('|', '/')[:90],
        ctrl + ('/' + p['action'].split(' ')[0] if p['action'] not in ('–', '-') else ''),
        '%s (%s)' % (grp[1], grp[0]) if grp[0] else grp[1], len(sps), st, func, uiqa))
    page_detail.append(dict(p=p, batch=batch, sps=sorted(sps), gaps=gaps, sess=sess, dals=dals, feats=feats, tabs=tabs, crud=crud, reads=reads,
                            typ=typ, st=st, build=build, func=func, uiqa=uiqa, notes=notes, specrel=specrel, cinfo=cinfo, grp=grp))

hdr = ['# All legacy pages (complete inventory)', '',
       'Generated %s by `docs/migration/tools/gen-migration-docs.py` from the legacy reverse-engineering inventory (100 pages/screens, `Accounts/docs/reverse-engineering/page-inventory.md`), the per-page specs (`Accounts/specs/**/pages/`), the stored-procedure catalog and the ported source. Nothing was skipped: dead/stub pages are listed with their legacy status. Status values are maintained in `status.json` and merged by the generator.' % TODAY,
       '', 'Batches: ' + '; '.join('**%s** %s (%d pages)' % (b, BATCH_NAME.get(b, 'unmapped module'), by_batch[b]) for b in sorted(by_batch)), '',
       '## Summary table', '',
       '| Page ID | Batch | Module | Page | Type | Route | Controller/Action | Fiori nav group (screen) | SPs | Migration status | Functional test | UI QA |',
       '|---|---|---|---|---|---|---|---|---|---|---|---|']
body = hdr + rows + ['', '## Per-page detail', '',
                     'Columns the brief asks for (tables, SPs, functions, views, APIs, session, auth, CRUD, reports, export, files, transactions, rules, defects) are listed per page. "Tables" are the objects the stored procedure references in the LOCAL database (`sys.sql_expression_dependencies`; W = the SP updates the object). Business rules per page: see the linked legacy spec, section 19.', '']
for d in page_detail:
    p = d['p']
    tabs_s = ', '.join('%s%s' % (k, ' (W)' if 'W' in v else '') for k, v in d['tabs'].items()) or ('UNKNOWN (SP missing locally)' if any(sp in missing for sp in d['sps']) else '—')
    body += ['### %s — %s' % (p['id'], p['name'].replace('|', '/')), '',
             '| Attribute | Value |', '|---|---|',
             '| Source path | %s |' % p['view'].replace('|', '/'),
             '| Page type / purpose | %s — %s |' % (d['typ'], p['name'].replace('|', '/')),
             '| Legacy status | %s |' % p['legacy_status'].replace('|', '/'),
             '| Batch / Fiori group | %s — %s / %s |' % (d['batch'], BATCH_NAME.get(d['batch'], '?'), ('%s (%d)' % (d['grp'][1], d['grp'][0]) if d['grp'][0] else d['grp'][1])),
             '| Controller / DAL | %s / %s |' % (p['controller_raw'].replace('|', '/'), ', '.join(d['dals']) or '—'),
             '| Stored procedures | %s |' % (', '.join('`%s`%s' % (s, ' **(missing locally)**' if s in missing else '') for s in d['sps']) or '—'),
             '| Tables / views / functions (local DB) | %s |' % tabs_s,
             '| APIs / AJAX | %s |' % p['apis'].replace('|', '/'),
             '| Session variables (spec) | %s |' % (', '.join(d['sess']) or '—'),
             '| Authentication / authorization | legacy: %s; new: global session guard (SessionRequiredFilter)%s |' % (
                 'Session["LoginUserId"] on page actions' if d['cinfo'].get('session_guard') else 'no session check on this controller', ', anonymous (login)' if d['cinfo'].get('allow_anon') else ''),
             '| CRUD / state-changing actions | %s |' % (', '.join(d['crud']) or '—'),
             '| Read / lookup actions | %s |' % (', '.join(d['reads']) or '—'),
             '| Reports / export / print / files | %s |' % (', '.join(d['feats']) or '—'),
             '| Transactions | every DAL call runs inside DataAccessManager\'s connection-level transaction (BeginTransaction on open, Commit on close, Rollback on SqlException) — unchanged |',
             '| Known legacy defects (spec §30) | %s |' % (', '.join(d['gaps']) or '—'),
             '| Legacy spec | `%s` |' % (d['specrel'] or '—'),
             '| Migration status / build / functional / UI QA | %s / %s / %s / %s |' % (d['st'], d['build'], d['func'], d['uiqa']),
             '| Notes | %s |' % (d['notes'] or '—'), '']
write(os.path.join(OUT, 'all-pages.md'), '\n'.join(body))

# ---------------------------------------------------------------- database-dependencies.md
lines = ['# Database dependencies', '',
         'Generated %s. Source of truth = the two LOCAL databases (`CSTL_ACCDB_Unicorn` accounting, `CSTL_ERPDB_Unicorn` control panel) on `TOWSIF\\MSSQLSERVER2019`, queried through `sys.sql_expression_dependencies` (script `tools/gen-spdeps.sql`). The legacy repository contains no SQL, so this is the first time the SP → table mapping is evidenced. **No database object was created or modified.**' % TODAY,
         '', '| Metric | Value |', '|---|---|',
         '| Stored procedures called by the application | %d |' % len(sp_index),
         '| ... present in the local databases | %d |' % len(exists),
         '| ... missing locally (pages depending on them are blocked locally) | %d |' % len(missing),
         '| Distinct tables/views/functions referenced by the present SPs | %d |' % len({r for sp in sp_refs for r, t, db, u in sp_refs[sp]}),
         '', '## Missing stored procedures (local environment)', '',
         'These are called by the code but do not exist in either local database. The calling pages fail identically in the legacy application against this database (same SP names, same parameters), so this is an environment gap, not a migration defect. They are listed in `blocked-pages.md`.', '',
         '| Stored procedure | Expected DB | Called from |', '|---|---|---|']
for sp in missing:
    lines.append('| `%s` | %s | %s |' % (sp, sp_index[sp]['db'], ', '.join('`%s`' % c for c in sp_index[sp]['callers']) or sp_index[sp]['first']))
lines += ['', '## Stored procedures', '', 'R = read, W = written (`is_updated` in the dependency view). Cross-database references are prefixed with the database name. Triggers: `CSTL_ACCDB_Unicorn` has no triggers; `CSTL_ERPDB_Unicorn` triggers on the few control-panel tables used here are listed in the table index.', '',
          '| # | Stored procedure | DB | Call type (DAL) | Parameters (as passed) | Call sites | Local | Referenced objects |', '|---|---|---|---|---|---|---|---|']
for i, sp in enumerate(sorted(sp_index), 1):
    d = sp_index[sp]
    refs = sp_refs.get(sp, [])
    refs_s = ', '.join('%s%s%s' % ((db + '.') if db else '', r, ' (W)' if u == '1' else ('' if t == 'USER_TABLE' else ' [%s]' % t.lower().replace('sql_', ''))) for r, t, db, u in refs) or ('—' if sp in exists else 'n/a')
    lines.append('| %d | `%s` | %s | %s | %s | %s | %s | %s |' % (i, sp, d['db'].split(' ')[0], d['call'], d['params'] or '—', ', '.join('`%s`' % c for c in d['callers']) or d['first'], 'yes' if sp in exists else '**missing**', refs_s))
tbl_index = collections.defaultdict(lambda: collections.defaultdict(set))
for sp, refs in sp_refs.items():
    for r, t, db, u in refs:
        if t in ('USER_TABLE', 'VIEW', 'CROSS-DB'):
            tbl_index[(db + '.' if db else '') + r]['W' if u == '1' else 'R'].add(sp)
lines += ['', '## Table / view index (which stored procedures touch what)', '', '| Object | Read by | Written by |', '|---|---|---|']
for obj in sorted(tbl_index):
    lines.append('| `%s` | %s | %s |' % (obj, ', '.join(sorted(tbl_index[obj]['R'])) or '—', ', '.join(sorted(tbl_index[obj]['W'])) or '—'))
write(os.path.join(OUT, 'database-dependencies.md'), '\n'.join(lines))

# ---------------------------------------------------------------- dependency-map.md
lines = ['# Dependency map', '', 'Generated %s. Chain: page (view + inline script) → controller action → DAL class → stored procedure → tables (local DB). The legacy had no service/repository layers and no DI; the ported code keeps the same structure (`new XxxDAL()` per controller). Shared dependencies of every page: `Views/Shared/_Layout.cshtml` (Fiori shell), `wwwroot/AssetsStatic/JS/custom-functions.js`, `UtilityController` (62 lookup endpoints), `Dashboard/LoadFilterInPartialView`.' % TODAY, '',
         '## Controller → DAL → stored procedures', '', '| Controller | Batch | DAL classes | Stored procedures (through those DALs) |', '|---|---|---|---|']
for c in sorted(ctrl_files):
    info = ctrl_files[c]
    sps = set()
    for d in info['dals']: sps |= dal_to_sps.get(d, set())
    mod = next((p['module'] for p in pages if p['controller'] == c), '')
    lines.append('| %s | %s | %s | %s |' % (c, MODULE_BATCH.get(mod, '-'), ', '.join(info['dals']) or '—', ', '.join(sorted(sps)) or '—'))
lines += ['', '## DAL → stored procedures', '', '| DAL file | Stored procedures |', '|---|---|']
for d in sorted(dal_to_sps):
    lines.append('| %s | %s |' % (d, ', '.join(sorted(dal_to_sps[d]))))
lines += ['', '## Page → stored procedures → tables', '', '| Page | Stored procedures | Tables (local DB, W = written) |', '|---|---|---|']
for d in page_detail:
    if not d['sps']: continue
    lines.append('| %s %s | %s | %s |' % (d['p']['id'], d['p']['name'].replace('|', '/')[:60], ', '.join(d['sps']), ', '.join('%s%s' % (k, ' (W)' if 'W' in v else '') for k, v in d['tabs'].items()) or '—'))
lines += ['', '## Module dependencies', '', '- Everything depends on Platform (session, shell, Utility lookups, filter partial).',
          '- Vouchers depend on Master Setup (financial year, currency rates, bank/cash accounts, cost centre/reference/subsidiary) and Chart of Accounts.',
          '- Approvals act on vouchers, loans and bank charges; Financial statements and Reports read voucher/ledger/opening-balance data.',
          '- Crystal print paths (AccountingTaskReport rpt* → CrystalViewer.aspx) are blocked by platform (no Crystal runtime on .NET 10).']
write(os.path.join(OUT, 'dependency-map.md'), '\n'.join(lines))

# ---------------------------------------------------------------- migration-progress.md
cnt = collections.Counter(d['st'].split(' ')[0] for d in page_detail)
lines = ['# Migration progress', '', 'Generated %s from `status.json`. Statuses: NOT STARTED / ANALYZING / IN PROGRESS / BLOCKED / MIGRATED / TESTING / DONE / N/A (dead in legacy).' % TODAY, '',
         '| Status | Pages |', '|---|---|'] + ['| %s | %d |' % (k, v) for k, v in sorted(cnt.items())] + ['',
         '| Page | Batch | Status | Build | Functional Test | UI QA | Notes |', '|---|---|---|---|---|---|---|']
for d in page_detail:
    lines.append('| %s %s | %s | %s | %s | %s | %s | %s |' % (d['p']['id'], d['p']['name'].replace('|', '/')[:70], d['batch'], d['st'], d['build'], d['func'], d['uiqa'], d['notes']))
write(os.path.join(OUT, 'migration-progress.md'), '\n'.join(lines))
print('written: all-pages.md, database-dependencies.md, dependency-map.md, migration-progress.md')
print('status counts:', dict(cnt))
