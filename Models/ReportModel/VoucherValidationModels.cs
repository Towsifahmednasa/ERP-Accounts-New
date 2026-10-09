// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\ReportModel\VoucherValidationModels.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;

namespace New_Accounts.Models.ReportModel
{
    public class VoucherValidationFilter
    {
        public string PageName { get; set; }
        public string IntegrityStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class VoucherValidationRow
    {
        public string PageName { get; set; }
        public string TransRefNo { get; set; }
        public string TransDate { get; set; }
        public string VoucherType { get; set; }
        public string TransactionType { get; set; }
        public string VoucherCode { get; set; }
        public string PostingDate { get; set; }
        public string SourceRefNo { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal VoucherAmount { get; set; }
        public string VoucherLog { get; set; }
        public int CommonPK_ID { get; set; }
        public int TableID { get; set; }
        public int InvoiceId { get; set; }
        public int VoucherMasterId { get; set; }
        public int TranscationId { get; set; }
    }

    public class VoucherValidationViewModel
    {
        public VoucherValidationViewModel()
        {
            Filter = new VoucherValidationFilter
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today
            };
            Rows = new List<VoucherValidationRow>();
        }

        public VoucherValidationFilter Filter { get; set; }
        public IList<VoucherValidationRow> Rows { get; set; }
    }

    public class VoucherValidationPageOption
    {
        public string PageName { get; set; }
    }
}
