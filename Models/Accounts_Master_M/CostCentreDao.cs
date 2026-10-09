// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\CostCentreDao.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class CostCentreDao
    {

        public int CostCenterId { get; set; }

        public string CostCenterCode { get; set; }

        public string Description { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? CostCenterStartDate { get; set; }

        public bool? IsSupplier { get; set; }

        public int? SupplierId { get; set; }

        public int? BBLCInformationId { get; set; }

        public bool? IsBuyer { get; set; }

        public int? ByerId { get; set; }

        public bool? IsSalesContract { get; set; }

        public bool? IsMLC { get; set; }

        public int? SalesContractId { get; set; }

        public int? MasterLCId { get; set; }

        public string Other { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? InActiveDate { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDelete { get; set; }

        public int? FinancialYearId { get; set; }

        public int? COAId { get; set; }

        public int? OrderId { get; set; }

        public int? OtherCOAId { get; set; }

        public bool? IsEmployee { get; set; }

        public int? EmployeeId { get; set; }


        //

        public string CompanyName { get; set; }

        public string _CostCenterStartDate { get; set; }
    }
}