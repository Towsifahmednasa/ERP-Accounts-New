// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\VoucherTransection.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class VoucherTransection
    {
        public int VoucherTransectionId { get; set; }

        public int VoucherDetailId { get; set; }

        public decimal? Amt { get; set; }

        public decimal? AmtBDT { get; set; }

        public int? SalesOrderChallanMasterId { get; set; }

        public int? MRRMasterId { get; set; }

        public int? CostCenterId { get; set; }

        public int? RefTypeId { get; set; }

        public int? RefDetailsId { get; set; }
        public string eflag { get; set; }

    }
}