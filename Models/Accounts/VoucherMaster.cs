// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\VoucherMaster.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class VoucherMaster
    {
        public int VoucherMasterId { get; set; }

        public string VoucherType { get; set; }
        public int VoucherTypeId { get; set; }

        public string VoucherCode { get; set; }

        public DateTime VoucherDate { get; set; }

        public int CompanyId { get; set; }
        public int BUId { get; set; }

        public int FinancialYearId { get; set; }

        public int CurrencyId { get; set; }

        public decimal CurrencyConRate { get; set; }

        public string Narration { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalAmountBDT { get; set; }

        public int? PaymentTypeId { get; set; }

        public string ChequeNo { get; set; }

        public DateTime? ChequeDate { get; set; }

        public string RefNo { get; set; }

        public DateTime? RefDate { get; set; }

        public string Remarks { get; set; }

        public string EntryBy { get; set; }

        public DateTime EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public int? BankId { get; set; }
        public int? BranchId { get; set; }
        public string eflag { get; set; }
        public bool IsApprove { get; set; }
        public bool IsPage { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public  bool IsReject { get; set; }
        public string RejectBy { get; set; }
        public DateTime? RejectDate { get; set; }

        public List<VoucherDetails> VoucherDetails { get; set; }

    }
}