// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\TestMaster.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class TestMaster
    {
        public int TestMasterId { get; set; }

        public int? CompanyId { get; set; }

        public int? UnitId { get; set; }

        public DateTime? VoucherDate { get; set; }

        public decimal? CurrencyRate { get; set; }

        public string PaymentType { get; set; }

        public decimal? TotalAmount { get; set; }

        public string Entryby { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Narration { get; set; }
        public List<TestDetail> TestDetails { get; set; }
    }
}