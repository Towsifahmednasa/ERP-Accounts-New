// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\CostCenter.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_CostCenter")]
    public class CostCenter
    {
        [Key]
        public int CostCenterId { get; set; }

        public int CompanyId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int CostCenterLayerId { get; set; }

        public int ParentId { get; set; }

        public string CostCenterName { get; set; }

        public string Description { get; set; }

        public bool IsBudgetCenter { get; set; }

        public bool IsProfitCenter { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public CompanyInformation Company { get; set; }
        public CostCenterType CostCenterType { get; set; }
        public CostCenterLayer CostCenterLayer { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }

        [NotMapped]
        public string CostCenterTypeName { get; set; }
        [NotMapped]

        public string LayerName { get; set; }
        public int isCostCenter { get; set; }
        public bool isChecked { get; set; }
    }
}