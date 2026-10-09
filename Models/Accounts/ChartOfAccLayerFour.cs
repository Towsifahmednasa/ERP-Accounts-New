// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ChartOfAccLayerFour.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ChartOfAccLayerFour")]
    public class ChartOfAccLayerFour
    {
        [Key]
        public int CALayerFourId { get; set; }

        public int CALayerThreeId { get; set; }

        public string CALayerFourCode { get; set; }
        public string CALayerFourName { get; set; }
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
        public  ChartOfAccLayerThree ChartOfAccLayerThree { get; set; }
        public  IList<ChartOfAccLayerFive> ChartOfAccLayerFiveList { get; set; } 
    }
}