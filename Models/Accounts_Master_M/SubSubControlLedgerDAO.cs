// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\SubSubControlLedgerDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class SubSubControlLedgerDAO
    {
        public int SubSubControlLedgerId { get; set; }
        public string SubSubControlLedgerCode { get; set; }

        public string Description { get; set; }

        public string SubSubControlLedgerName { get; set; }

        public int? SubsidiaryLId { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool? IsActive { get; set; }

       

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }


    }


    public class SubSubControlLedgerView
    {
        public int SubSubControlLedgerId { get; set; }
        public string GLName { get; set; }
        public string ControlLName { get; set; }
        public string SubsidiryLName { get; set; }
        public string SubSubControlLedgerCode { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }

        public string SubSubControlLedgerName { get; set; }
     
         
    }
}