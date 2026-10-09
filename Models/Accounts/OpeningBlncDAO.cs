// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\OpeningBlncDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class OpeningBlncDAO
    {
        public int? CompanyId { get; set; }

        public int? FinancialYearId { get; set; }

        public int? ChartOfAccountId { get; set; }
        public string Currency { get; set; }

        public decimal? Amount { get; set; }

        public int? CurrencyId { get; set; }

        public string DRorCR { get; set; }

        public bool? IsAutoProcess { get; set; }

        public int OpeningBalanceId { get; set; }

        public string Remarks { get; set; }

        public int? Quantity { get; set; }

    }



    public class OpeningBlncListDAO
    {
        public string CompanyName { get; set; }

        public string FinancialYearDesc { get; set; }

        public string AccountName { get; set; }
        public string Currency { get; set; }

        public string Amount { get; set; }

        public string CurrencyName { get; set; }

        public string DRorCR { get; set; }

        public bool? IsAutoProcess { get; set; }

        public int OpeningBalanceId { get; set; }

        public string Remarks { get; set; }

        public string Quantity { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }

    }
}