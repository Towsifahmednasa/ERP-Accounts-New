// Presentation-only mapping of the legacy DB-driven menu (sp_Select_Menu_Step_WithUserAccessERPAcc, 3-level tree
// stored in Session["MenuList"]) onto the six SAP Fiori "ERP - Accounts & finance" navigation groups of the
// wireframe (screens 80-85). WHICH items a user sees still comes from the database unchanged; only the grouping,
// numbering and icons are new. Items whose controller is not known to this map go to the trailing "More" group.
using System;
using System.Collections.Generic;
using System.Linq;
using New_Accounts.Models;

namespace New_Accounts.Compat
{
    public class NavItem
    {
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Href => "/" + Controller + "/" + Action;
        public string Icon { get; set; } = "s-doc";
        public string LegacyGroup { get; set; }
    }

    public class NavGroup
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<NavItem> Items { get; } = new List<NavItem>();
    }

    public static class MenuMap
    {
        // wireframe screen number, title, icon, controllers that belong to it
        private static readonly (int No, string Title, string Icon, string[] Controllers)[] Groups =
        {
            (80, "Chart of Accounts", "s-grid", new[] { "LedgerHead", "ControlLedger", "SubControl", "SubSubControlLedger", "ChartOfAccount", "Subsubsidairy", "OpeningBlnc", "Ref_Sub_Type", "Refference", "FinancialYear", "CurrencyConversion" }),
            (81, "Voucher Entry", "s-doc", new[] { "BankPayments", "CreditVoucher", "Journal", "Contra", "DebitVoucher", "AllApproval" }),
            (82, "Receivables & Payables Ageing", "s-people", new[] { "Loan", "LCRelatedBankPayment" }),
            (83, "Bank & Cash Reconciliation", "s-shop", new[] { "BanckAccount", "CashAccount", "ReconcileBankAccount", "CashBook", "BankBook" }),
            (84, "Trial Balance & Financials", "s-chart", new[] { "AllAccountReport", "TrialBalance", "BalanceSheet", "IncomeStatement", "CashFlow", "BalanceSheetDynamicReport", "IncomeStatementDynamicReport", "VoucherValidation", "AccountingTaskReport" }),
            (85, "Cost Centre & Budget Control", "s-pin", new[] { "CostCentre", "Budget" }),
        };

        private static readonly (string Match, string Icon)[] IconMap =
        {
            ("approval", "s-ok"), ("report", "s-chart"), ("bank", "s-shop"), ("cash", "s-shop"), ("journal", "s-doc"),
            ("voucher", "s-doc"), ("contra", "s-sort"), ("reconcil", "s-ok"), ("budget", "s-pin"), ("cost", "s-pin"),
            ("loan", "s-people"), ("year", "s-clock"), ("currency", "s-cart"), ("ledger", "s-grid"), ("account", "s-grid"),
        };

        public static int GroupNumber(string controller)
        {
            foreach (var g in Groups)
                if (g.Controllers.Any(c => c.Equals(controller, StringComparison.OrdinalIgnoreCase))) return g.No;
            return 0;
        }

        public static string GroupTitle(string controller)
        {
            int n = GroupNumber(controller);
            return n == 0 ? "More" : Groups.First(g => g.No == n).Title;
        }

        /// <summary>Flatten the legacy 3-level tree to leaf links (folder nodes have an empty ControllerName).</summary>
        public static List<NavItem> Leaves(IEnumerable<MenuStepOne> tree)
        {
            var leaves = new List<NavItem>();
            foreach (var one in tree ?? Enumerable.Empty<MenuStepOne>())
            {
                if (!string.IsNullOrEmpty(one.ControllerName)) leaves.Add(Item(one.MenuName, one.ControllerName, one.ActionName, one.MenuName));
                foreach (var two in one.MenuStepTwoList ?? new List<MenuStepTwo>())
                {
                    if (!string.IsNullOrEmpty(two.ControllerName)) leaves.Add(Item(two.MenuName, two.ControllerName, two.ActionName, one.MenuName));
                    foreach (var three in two.MenuStepThreeList ?? new List<MenuStepThree>())
                        if (!string.IsNullOrEmpty(three.ControllerName)) leaves.Add(Item(three.MenuName, three.ControllerName, three.ActionName, two.MenuName));
                }
            }
            return leaves;
        }

        public static List<NavGroup> Build(IEnumerable<MenuStepOne> tree)
        {
            var leaves = Leaves(tree);
            var groups = Groups.Select(g => new NavGroup { Number = g.No, Title = g.Title, Icon = g.Icon }).ToList();
            var more = new NavGroup { Number = 0, Title = "More", Icon = "s-doc" };
            foreach (var leaf in leaves)
            {
                int n = GroupNumber(leaf.Controller);
                (n == 0 ? more : groups.First(g => g.Number == n)).Items.Add(leaf);
            }
            if (more.Items.Count > 0) groups.Add(more);
            return groups.Where(g => g.Items.Count > 0).ToList();
        }

        private static NavItem Item(string title, string controller, string action, string legacyGroup)
        {
            var item = new NavItem { Title = (title ?? "").Trim(), Controller = controller.Trim(), Action = (action ?? "Index").Trim(), LegacyGroup = legacyGroup };
            foreach (var (match, icon) in IconMap)
                if (item.Title.Contains(match, StringComparison.OrdinalIgnoreCase)) { item.Icon = icon; break; }
            return item;
        }
    }
}
