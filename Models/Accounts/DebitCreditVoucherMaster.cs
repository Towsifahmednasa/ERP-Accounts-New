// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\DebitCreditVoucherMaster.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class DebitCreditVoucherMaster
    {
        public int VoucherMasterId { get; set; }

        public int? TransferType { get; set; }

        public string VoucherCode { get; set; }

        public int? CompanyInfoId { get; set; }

        public int? FinancialYearId { get; set; }

        public DateTime? PostingDate { get; set; }

        public string VoucherNo { get; set; }

        public int? VoucherCategoryID { get; set; }

        public int? CurrencyId { get; set; }

        public decimal? CurrencyCovRate { get; set; }

        public string Narration { get; set; }

        public bool? IsCostCenter { get; set; }

        public bool? IsBuyerCostCenter { get; set; }

        public bool? IsOtherCostCenter { get; set; }

        public bool? ReceiveFromIsBuyer { get; set; }

        public int? ByerId { get; set; }

        public bool? PaytoAndReceiveFromIsOther { get; set; }

        public string Other { get; set; }

        public string OtherAdress { get; set; }

        public bool? PaytoIsSupplier { get; set; }

        public int? SupplierId { get; set; }

        public bool? IsInvoice { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalAmountBDT { get; set; }

        public string Remarks { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string VoucherType { get; set; }

        public string BankCheckNo { get; set; }

        public DateTime? BankCheckDate { get; set; }

        public string CashRefNo { get; set; }

        public DateTime? CashRefDate { get; set; }

        public string JournalType { get; set; }

        public string Comments { get; set; }

        public bool? IsExportInvoice { get; set; }

        public bool? IsImportInvoice { get; set; }

        public bool? IsReconciliation { get; set; }

        public DateTime? ReconciliationDate { get; set; }

        public string ReconciliationComments { get; set; }

        public bool? isManualVCRNo { get; set; }

        public int? ProformaId { get; set; }

        public int? DcFreezeId { get; set; }

        public int? DestroyId { get; set; }

        public string ActionStatus { get; set; }

        public int? SalesReturnId { get; set; }

        public int? CustPayDetailId { get; set; }

        public int? WHStockInMasterId { get; set; }

        public int? PurchaseId { get; set; }

        public int? SampleInvoiceId { get; set; }

        public int? WholeSaleId { get; set; }

        public int? BankAccountId { get; set; }

        public string ChequeNo { get; set; }

        public DateTime? ChequeDate { get; set; }
        public string ChequeDateStr { get; set; }

        public int? CashAccountId { get; set; }
        public int? FromCoaId { get; set; }
        public int? ToCoaId { get; set; }

        public string RefNo { get; set; }

        public DateTime? RefDate { get; set; }

        public string RefDateStr { get; set; }
        public string PostingDateStr { get; set; }


        public int? ToCompanyInfoId { get; set; }

        public int? ToFinancialYearId { get; set; }


   

        public List<DebitCreditVoucherDetail>  DetailsInfo { get; set; }
    }


    public class DebitCreditVoucherDetail
    {

        public int VoucherDetailId { get; set; }

        public int? VoucherMasterId { get; set; }

        public int? AccountId { get; set; }

        public string DebitorCredit { get; set; }

        public decimal? DebitAmount { get; set; }

        public decimal? CreditAmount { get; set; }

        public decimal? DebitAmountBDT { get; set; }

        public decimal? CreditAmountBDT { get; set; }

        public int? VoucherCategory { get; set; }

        public int? CostCenterId { get; set; }
        public string CostCenter  { get; set; }
        public string AccountInfo { get; set; }

        public int? RefInfoInd { get; set; }
        public string RefInfo { get; set; }

        public int? SubsidoryLedgerId { get; set; }
        public string SubsidoryLedger  { get; set; }

        public string Remarks { get; set; }


        public decimal? Amount { get; set; }

    }



   


    public class ReconcileBankAccount 
    {

        

        public int? VoucherMasterId { get; set; }
 

        public DateTime? ReconcileDate { get; set; } 

    }
}