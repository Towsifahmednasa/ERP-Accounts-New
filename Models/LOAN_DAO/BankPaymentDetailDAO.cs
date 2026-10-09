// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\BankPaymentDetailDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
namespace New_Accounts.Models.LOAN_DAO
{
    public class BankPaymentDetailDAO
    {
        public int BankChargeDetailId { get; set; }
        public int BankChargeId { get; set; }
        public int? CommercialInvoiceId { get; set; }
        public decimal? AmountBDT { get; set; }
        public decimal? AmountFC { get; set; }
    }


}