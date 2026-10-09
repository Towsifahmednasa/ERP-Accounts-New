// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\LoanInfoModelDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;

namespace New_Accounts.Models.LOAN_DAO
{
    public class LoanInfoModelDAO
    {
        public int LoanInfoID { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProfitCenterID { get; set; }
        public int? SupplierID { get; set; }
        public int? LCNoId { get; set; }
        public int? CINoId { get; set; }
        public DateTime? LCOpenDate { get; set; }
        public int? LoanIssuingBankId { get; set; }
        public int? LoanIssuingBranchId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ConversionRate { get; set; }
        public string LoanNo { get; set; }
        public int? LoanTenorDays { get; set; }
        public int? LoanCategoryID { get; set; }
        public DateTime? LoanCreateDate { get; set; }
        public DateTime? LoanEffectiveDate { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? GracePeriodDays { get; set; }
        public bool? AdvancedInterestPayment { get; set; }
        public int? InterestTypeId { get; set; }
        public int InterestMethodId { get; set; }
        public int? InstallmentNo { get; set; }
        public bool? Funded { get; set; }
        public bool? LoanAgreement { get; set; }
        public string ReimbursementBank { get; set; }
        public decimal? MarginAdjusted { get; set; }
        public decimal? InterestRate { get; set; }
        public string LoanPurpose { get; set; }
        public decimal? InterestAmount { get; set; }
        public decimal? TotalChargeAmount { get; set; }
        public decimal? TotalExpense { get; set; }
        public decimal? TotalPayableAmount { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ApproveBy { get; set; }
        public string ApproveStatus { get; set; }
        public DateTime? ApprovDate { get; set; }
    }

}