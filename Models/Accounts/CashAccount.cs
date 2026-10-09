// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\CashAccount.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class CashAccount
    {
        public int CashAccountId { get; set; }
        public string AccountName { get; set; }

        public int? CompanyId { get; set; }
        
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public int SubsidiryLId { get; set; }
        public int SubSubControlLedgerId { get; set; }
        public int Currency { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        public int? ControlLId { get; set; }

        public int? GLHeadId { get; set; }

    }
}