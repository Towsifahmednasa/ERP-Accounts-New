// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Budget\BudgetDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Budget
{
    public class BudgetDAO
    {
        public int BudgetMasterId { get; set; }

        public string CompanyName { get; set; }

        public string FinancialYearDesc { get; set; }
        public string TotalAmount { get; set; }

        public string COAIds { get; set; }

        public string EntryBy { get; set; }

        public string EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public string UpdateDate { get; set; }
    }

    public class  BudgetDetailDAO
    {
        public int BudgetDetailId { get; set; }

        public int? BudgetMasterId { get; set; }

        public DateTime? Period { get; set; }

        public decimal? Amount { get; set; }

    }

    public class  BudgetMasterDAO
    {
        public int BudgetMasterId { get; set; }

        public int? CompanyId { get; set; }

        public int? FinancialYearId { get; set; }
        public decimal? TotalAmount { get; set; }

        public int? COAId { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public int? GLHeadId { get; set; }
        public int? ControlLId { get; set; }
        public int? SubsidiryLId { get; set; }
        public int? SubSubControlLedgerId { get; set; }


        public List<BudgetDetailDAO> DetailsInfo { get; set; }
    }



    public class BudgetMonthAmountDAO
    {

        
        public string MYId { get; set; }
        public string Period { get; set; }
        public string Amount { get; set; }
    }
}