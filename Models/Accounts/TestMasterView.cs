// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\TestMasterView.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class TestMasterView
    {

        public int? TestId { get; set; }

        public string CompanyName { get; set; }
 

        public string VoucherDate { get; set; }

        public decimal? CurrencyRate { get; set; }

        public string PaymentType { get; set; }

        public decimal? TotalAmount { get; set; }

        public string EntryBy { get; set; }

        public string EntryDate { get; set; }
    }
}