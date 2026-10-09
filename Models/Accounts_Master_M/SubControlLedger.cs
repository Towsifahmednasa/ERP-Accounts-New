// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\SubControlLedger.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class SubControlLedger
    {
        public int SubsidiryLId { get; set; }
        public string SubsidiryLCode { get; set; }
        public int ControlLId { get; set; }
        public string SubsidiryLName { get; set; }
        public bool InBalanceSheet { get; set; }
        public bool InIncomeStatement { get; set; }

        public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}