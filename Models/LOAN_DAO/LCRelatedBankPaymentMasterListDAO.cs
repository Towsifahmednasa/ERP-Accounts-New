// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\LCRelatedBankPaymentMasterListDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
namespace New_Accounts.Models.LOAN_DAO
{
    public class LCRelatedBankPaymentMasterListDAO
    {
        public string BankChargeId { get; set; }
        public string CompanyName { get; set; }
        public string LCNo { get; set; }
        public string LCDate { get; set; }
        public string CurrencyName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierTypeName { get; set; }
        public string TypeName { get; set; }
        public string LCOpenId { get; set; }
        public string Conversionrate { get; set; }
        public string LcBankPayTypeId { get; set; }
        public string ChargeBasisof { get; set; }
        public string TotalAmountBDT { get; set; }
        public string TotalAmountFC { get; set; }
        public string PaymentFrom { get; set; }
        public string LoanAccID { get; set; }
        public string COAID { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
        public string Updateby { get; set; }
        public string Updatedate { get; set; }
    }


}