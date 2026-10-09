// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\CostCenterType.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_CostCenterType")]
    public class CostCenterType
    {
        [Key]
        public int CostCenterTypeId { get; set; }

        public string CostCenterTypeName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }
        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        //public Company Company { get; set; }
        [NotMapped]
        public  string CompanyName { get; set; }

       
    }
}