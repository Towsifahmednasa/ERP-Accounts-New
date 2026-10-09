// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\CashAccountView.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class CashAccountView
    {
        public int CashAccountId { get; set; }
        public string CompanyName { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string GLName { get; set; }
        public string ControlLName { get; set; }
        public string SubsidiryLName { get; set; }
        public string SubSubControlLedgerName { get; set; }


    }
}