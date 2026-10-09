// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ChartOfAccLayerSix.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ChartOfAccLayerSix")]
    public class ChartOfAccLayerSix
    {
        [Key]
        public int CALayerSixId { get; set; }

        public int CALayerFiveId { get; set; }

        public string CALayerSixCode { get; set; }
        public string CALayerSixName { get; set; }
        [NotMapped]
        public string Code { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime ActiveDate { get; set; }

        public DateTime? InactiveDate { get; set; }

        public string EntryBy { get; set; }

        public DateTime EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public ChartOfAccLayerFive ChartOfAccLayerFive { get; set; }

        public IList<ChartOfAccLayerSeven> ChartOfAccLayerSevenList { get; set; }
    }
}