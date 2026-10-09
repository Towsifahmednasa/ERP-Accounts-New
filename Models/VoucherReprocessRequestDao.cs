// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\VoucherReprocessRequestDao.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class VoucherReprocessRequestDao
    {
        public int TableId { get; set; }
        public int CommonPkId { get; set; }
        public string TransctionErrorType { get; set; }
        public int VoucherMasterId { get; set; }
        public int CusPaymentInvoiceId { get; set; }
        public int TranscationId { get; set; }
    }
}