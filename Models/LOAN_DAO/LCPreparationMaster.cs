// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\LCPreparationMaster.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.LOAN_DAO
{
    public class LCPreparationMaster
    {
        public Int64 LCPreparationId { get; set; }

        public string CentralBankReferenceNo { get; set; }

        public int? BusinessUnitId { get; set; }
        public int? LcTypeId { get; set; }
        public int? SupplierId { get; set; }

        public string LCDate { get; set; }

        public int? BeneficiaryId { get; set; }

        public string ReferenceNo { get; set; }

        public DateTime? LCInitiateDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public int? PayTermId { get; set; }

        public int? TransportModeId { get; set; }

        public string Tenor { get; set; }

        public decimal? TolerancePercentage { get; set; }

        public int? IncoTermId { get; set; }

        public string LCANo { get; set; }

        public DateTime? EstimateDeliveryDate { get; set; }

        public DateTime? LastShipmentDate { get; set; }

        public DateTime? LCExpiryDate { get; set; }

        public string IPNo { get; set; }

        public DateTime? IPDate { get; set; }

        public DateTime? IPExpiryDate { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public DateTime? EstimateDateofDepurture { get; set; }

        public int? LCIssuingBankId { get; set; }

        public int? BranchId { get; set; }

        public int? InsuranceCompanyId { get; set; }

        public string InsurCoverNoteCondition { get; set; }

        public string InternalFileNo { get; set; }

        public string CurrencyId { get; set; }

        public string ConversionRate { get; set; }

        public string InsurCoverNoteNo { get; set; }

        public DateTime? InsurCoverNoteDate { get; set; }

        public int? ReimbursementBankId { get; set; }

        public int? AdvisingBankId { get; set; }

        public int? NegotiatingBankId { get; set; }

        public DateTime? MaturityFrom { get; set; }

        public decimal? MarginPercentage { get; set; }

        public decimal? MarginAmount { get; set; }

        public decimal? LCValue { get; set; }
        public decimal? LCValueFc { get; set; }
        public decimal? TotalFreightChargeFc { get; set; }
        public decimal? TitalFreightCharge { get; set; }

        public decimal? TaxPercentage { get; set; }

        public decimal? VatPercentage { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? NetInsuranceAmount { get; set; }

        public decimal? InsuranceCoverAmount { get; set; }

        public decimal? InsurCompanyRefundPercentage { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? ModeofTransportId { get; set; }

        public int? PortofLoadingId { get; set; }

        public int? PortofDischarge { get; set; }

        public int? IncoTerm { get; set; }

        public int? CountryofOrigin { get; set; }


    }

}