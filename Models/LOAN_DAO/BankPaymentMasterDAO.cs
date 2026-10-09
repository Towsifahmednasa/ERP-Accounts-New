// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\BankPaymentMasterDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;

namespace New_Accounts.Models.LOAN_DAO
{
    public class BankPaymentMasterDAO
    {
        public int BankChargeId { get; set; }
        public int LCOpenId { get; set; }
        public decimal Conversionrate { get; set; }
        public int LcBankPayTypeId { get; set; }
        public string ChargeBasisof { get; set; }
        public decimal TotalAmountBDT { get; set; }
        public decimal TotalAmountFC { get; set; }
        public string PaymentFrom { get; set; }
        public int LoanAccID { get; set; }
        public int COAID { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string Updateby { get; set; }
        public DateTime Updatedate { get; set; }



        public List<BankPaymentDetailDAO> DetailsInfo { get; set; }
    }

}