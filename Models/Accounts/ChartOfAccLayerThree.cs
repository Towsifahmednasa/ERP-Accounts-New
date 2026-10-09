// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ChartOfAccLayerThree.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ChartOfAccLayerThree")]
    public class ChartOfAccLayerThree
    {
        [Key]
        public int CALayerThreeId { get; set; }

        public int CALayerTwoId { get; set; }

        public string CALayerThreeCode { get; set; }
        public string CALayerThreeName { get; set; }
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

        public  ChartOfAccLayerTwo ChartOfAccLayerTwo { get; set; }
        public  IList<ChartOfAccLayerFour> ChartOfAccLayerFourList { get; set; } 
    }
}