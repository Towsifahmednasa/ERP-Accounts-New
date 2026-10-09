// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\DebitVoucherView.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class DebitVoucherView
    {

        public int VoucherMasterId { get; set; }

        public string VoucherCode { get; set; }
        public string VoucherNo { get; set; }
        public string PostingDate { get; set; }
        public string VoucherCategory { get; set; }
        public string CompanyName { get; set; }
        public string Narration { get; set; }
        public string TotalAmountBDT { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string VoucherType { get; set; }

        public string ReconcileDate { get; set; }
        public string ActionStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string BankCheckDate { get; set; }
        public string BankCheckNo { get; set; }
        public bool IsReconcile { get; set; }



    }
}