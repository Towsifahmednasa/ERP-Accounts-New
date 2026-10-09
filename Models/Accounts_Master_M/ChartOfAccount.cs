// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\ChartOfAccount.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class ChartOfAccount
    {
        public string AccountCode { get; set; }

        public string Description { get; set; }

        public string AccountName { get; set; }

        public int? SubsidiaryLId { get; set; }

        public int? CompanyId { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool? IsActive { get; set; }

        public int? BankAccountId { get; set; }

        public int? CashAccountId { get; set; }

        public bool? InBalanceSheet { get; set; }

        public bool? InIncomeStatement { get; set; }

        public int? SupplierId { get; set; }

        public int? CustomerId { get; set; }

        public int AccountId { get; set; }

        public bool? IsEmployee { get; set; }

        public int? EmployeeId { get; set; }

        public int? ProductId { get; set; }

        public string CompanyUnitAll { get; set; }

        public bool? IsReference { get; set; }
        public bool? IsSubsidiary { get; set; }
        public bool? IsCostCenter { get; set; }

        public List<COARef> RefDetails { get; set; }
        public List<COASubsidiary> SubDetails { get; set; }
        public List<COACostCenter> CSDetails { get; set; }

    }

    public class COARef
    {
        public int COARefId { get; set; }

        public int? AccountId { get; set; }

        public int? ReferenceTypeId { get; set; }

    }

    public class COASubsidiary
    {
        public int COASubsidiaryId { get; set; }

        public int? AccountId { get; set; }

        public int? SubsidiaryTypeId { get; set; }

    }

    public class COACostCenter
    {
        public int COACostCenterId { get; set; }

        public int? AccountId { get; set; }

        public int? CostCenterId { get; set; }


    }


    public class SubsidiaryInfoDAO
    {
        public int? SubsidiaryTypeId { get; set; }

        public int? S_Info_Id { get; set; }

        public bool? IsSelect { get; set; }

        public string Description { get; set; }

        public int? EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long SubsidiaryInfoId { get; set; }


    }



    public class ReferenceInfoDAO
    {
        public long ReferenceInfoId { get; set; }

        public int? ReferenceTypeId { get; set; }

        public int? R_Info_Id { get; set; }

        public bool? IsSelect { get; set; }

        public string Description { get; set; }

        public int? EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }


    }
}