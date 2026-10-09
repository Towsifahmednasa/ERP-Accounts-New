// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\FinancialYearDao.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class FinancialYearDao
    {

        public int FinancialYearId { get; set; }

        public string FinancialCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? CompanyId { get; set; }

        public string Status { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        public string FinancialYearDesc { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }
        public string EntryDateStr { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CompanyName { get; set; }

        public string _StartDate { get; set; }
        public string _EndDate { get; set; }
        public string _ActiveDate { get; set; }
        

    }



    public class FinancialYearDDL
    {

 

        
        public string StartDate { get; set; }

        public string EndDate { get; set; }


    }
    }