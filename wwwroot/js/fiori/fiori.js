// Fiori shell behaviour: popovers, drawer, confirmation dialogs, toasts, DataTables toolbar wiring.
// Functionality contract: confirmed buttons re-submit exactly as the plain button would have
// (same name/value in the post), so the server-side postback handling is untouched.
(function () {
    'use strict';

    // ---- shell popovers -------------------------------------------------------
    var pops = [['appsBtn', 'appsPop'], ['notifBtn', 'notifPop'], ['userBtn', 'userPop']];
    pops.forEach(function (pair) {
        var btn = document.getElementById(pair[0]), pop = document.getElementById(pair[1]);
        if (!btn || !pop) return;
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            var open = pop.hidden;
            pops.forEach(function (p) { var el = document.getElementById(p[1]); if (el) el.hidden = true; });
            pop.hidden = !open;
        });
    });
    document.addEventListener('click', function () {
        pops.forEach(function (p) { var el = document.getElementById(p[1]); if (el) el.hidden = true; });
        document.querySelectorAll('.pop.menu').forEach(function (el) { el.hidden = true; });
    });

    // ---- responsive drawer ----------------------------------------------------
    var burger = document.getElementById('burger');
    if (burger) burger.addEventListener('click', function (e) {
        e.stopPropagation();
        document.body.classList.toggle('drawer-open');
    });

    // ---- confirmation dialog (replaces confirm() and sweetAlertConfirm_Submit) -
    // Buttons carry data-confirm="delete|submit" and data-confirm-text with the legacy message text.
    var overlay = document.getElementById('dlgConfirm');
    function askConfirm(btn) {
        var kind = btn.getAttribute('data-confirm');
        document.getElementById('dlgTitle').textContent = kind === 'delete' ? 'Delete?' : 'Are You Sure ?';
        document.getElementById('dlgText').textContent = btn.getAttribute('data-confirm-text') ||
            (kind === 'delete' ? 'Are you sure you want to delete ?' : 'You are about to submit the data!');
        var ok = document.getElementById('dlgOk');
        ok.textContent = kind === 'delete' ? 'Delete' : 'Confirm';
        ok.className = kind === 'delete' ? 'btn neg' : 'btn emph';
        document.getElementById('dlgIcon').style.color = kind === 'delete' ? 'var(--sapNegativeColor)' : 'var(--sapBrandColor)';
        overlay.hidden = false;
        ok.focus();
        ok.onclick = function () {
            overlay.hidden = true;
            btn.dataset.confirmed = '1';
            btn.click();
        };
    }
    if (overlay) {
        document.getElementById('dlgCancel').onclick = function () { overlay.hidden = true; };
        overlay.addEventListener('click', function (e) { if (e.target === overlay) overlay.hidden = true; });
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') overlay.hidden = true; });
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-confirm]');
            if (!btn) return;
            if (btn.dataset.confirmed === '1') { delete btn.dataset.confirmed; return; }
            e.preventDefault();
            e.stopPropagation();
            askConfirm(btn);
        }, true);
    }

    // ---- toasts: transient success confirmation, ~4 s (wireframe UI-states screen) ----
    document.querySelectorAll('#toasts .toast').forEach(function (toast) {
        setTimeout(function () {
            toast.style.transition = 'opacity .25s ease';
            toast.style.opacity = '0';
            setTimeout(function () { toast.remove(); }, 260);
        }, 4000);
    });

    // ---- DataTables toolbar wiring -------------------------------------------
    // fioriDataTable(tableId, {export:'full'|'simple'|null, paging:true})
    // Search box, export popover and "Showing X of Y" line live in the Fiori table toolbar/footer;
    // the engine (search/sort/paging/export) is unchanged DataTables.
    window.fioriDataTable = function (tableId, opts) {
        opts = opts || {};
        var $table = $('#' + tableId);
        if (!$table.length || !$.fn.DataTable) return null;
        var table = $table.DataTable({
            paging: opts.paging !== false,
            pageLength: 10,
            lengthChange: false,
            info: false,
            dom: 'tp',
            autoWidth: false,
            drawCallback: function () {
                var info = this.api().page.info();
                var line = document.getElementById('dtinfo_' + tableId);
                if (line) line.textContent = info.recordsDisplay === 0 ? 'No entries'
                    : 'Showing ' + (info.start + 1) + '–' + info.end + ' of ' + info.recordsDisplay;
            }
        });
        var search = document.getElementById('dtsearch_' + tableId);
        if (search) search.addEventListener('input', function () { table.search(this.value).draw(); });
        var sortBtn = document.getElementById('dtsort_' + tableId);
        if (sortBtn) sortBtn.addEventListener('click', function () {
            var order = table.order();
            table.order([[order.length && order[0][0] === 1 && order[0][1] === 'asc' ? 1 : 1,
                order.length && order[0][0] === 1 && order[0][1] === 'asc' ? 'desc' : 'asc']]).draw();
        });
        if (opts.export && $.fn.dataTable.Buttons) {
            var kinds = opts.export === 'simple'
                ? ['copy', 'excel', 'pdf', 'print']
                : ['copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5'];
            new $.fn.dataTable.Buttons(table, { buttons: kinds });
            table.buttons().container().appendTo(document.body);
            var menu = document.getElementById('dtexport_' + tableId);
            var toggle = document.getElementById('dtexportbtn_' + tableId);
            if (menu && toggle) {
                toggle.addEventListener('click', function (e) { e.stopPropagation(); menu.hidden = !menu.hidden; });
                menu.querySelectorAll('[data-export]').forEach(function (item) {
                    item.addEventListener('click', function () {
                        menu.hidden = true;
                        table.button(parseInt(item.getAttribute('data-export'), 10)).trigger();
                    });
                });
            }
        }
        return table;
    };
})();
