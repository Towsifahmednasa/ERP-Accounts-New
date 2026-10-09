// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\LoanInfoEditDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
namespace New_Accounts.Models.LOAN_DAO
{
    public class LoanInfoEditDAO
    {
        public int LoanInfoID { get; set; }
        public string BusinessUnitId { get; set; }
        public string ProfitCenterID { get; set; }
        public string SupplierID { get; set; }
        public string LCNoId { get; set; }
        public string LCOpenDate { get; set; }
        public string LoanIssuingBankId { get; set; }
        public string LoanIssuingBranchId { get; set; }
        public string CurrencyId { get; set; }
        public string ConversionRate { get; set; }
        public string LoanNo { get; set; }
        public string LoanTenorDays { get; set; }
        public string LoanCategoryID { get; set; }
        public string LoanCreateDate { get; set; }
        public string LoanEffectiveDate { get; set; }
        public string LoanAmount { get; set; }
        public string GracePeriodDays { get; set; }
        public string AdvancedInterestPayment { get; set; }
        public string InterestTypeId { get; set; }
        public string InterestMethodId { get; set; }
        public string InstallmentNo { get; set; }
        public string Funded { get; set; }
        public string LoanAgreement { get; set; }
        public string ReimbursementBank { get; set; }
        public string MarginAdjusted { get; set; }
        public string InterestRate { get; set; }
        public string LoanPurpose { get; set; }
        public string InterestAmount { get; set; }
        public string TotalChargeAmount { get; set; }
        public string TotalExpense { get; set; }
        public string TotalPayableAmount { get; set; }
    }

}