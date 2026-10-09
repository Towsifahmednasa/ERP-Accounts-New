// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\LedgerGroup.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_LedgerGroup")]
    public class LedgerGroup
    {
        [Key]
        public int LedgerGroupId { get; set; }

        public string LedgerGroupCode { get; set; }

        public string LedgerGroupName { get; set; }
        public  IList<ChartOfAccLayerTwo> ChartOfAccLayerTwoList { get; set; } 
        [NotMapped]
        public int ParentId { get; set; }
        [NotMapped]
        public  int LayerId { get; set; }
        [NotMapped]
        public  bool IsActive { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string Code { get; set; }


        //new 
        public int CompanyId { get; set; }

        public List<ReferanceType> RefDetails { get; set; }
        public List<CostCenter> CostCenters { get; set; }
        public bool IsCostCenter { get; set; }
        public bool IsRefarence { get; set; }
        public int BUId { get; set; }
        public int LayerFiveId { get; set; }
        public int BankId { get; set; }
        public int BranchId { get; set; }
        
    }
}