// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\VoucherDetails.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class VoucherDetails
    {
        public int VoucherDetailId { get; set; }

        public int VoucherMasterId { get; set; }

        public int CALayerSevenId { get; set; }

        public string DrCr { get; set; }

        public decimal? DrAmt { get; set; }

        public decimal? CrAmt { get; set; }

        public decimal? DrAmtBDT { get; set; }

        public decimal? CrAmtBDT { get; set; }
        public bool IsDelete { get; set; }
        public string eflag { get; set; }
        public string Remarks { get; set; }
        public VoucherTransection VoucherTransections { get; set; }

        public int VoucherTransectionId { get; set; }

        public decimal? Amt { get; set; }

        public decimal? AmtBDT { get; set; }

        public int? SalesOrderChallanMasterId { get; set; }

        public int? MRRMasterId { get; set; }

        public int? CostCenterId { get; set; }

        public int? RefTypeId { get; set; }

        public int? RefDetailsId { get; set; }
    }
}