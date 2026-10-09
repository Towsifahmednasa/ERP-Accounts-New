// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\CostCenterLayer.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_CostCenterLayer")]
    public class CostCenterLayer
    {
        [Key]
        public int CostCenterLayerId { get; set; }

        public string CostCenterLayerName { get; set; }
    }
}