// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ChartOfAccLayerFive.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ChartOfAccLayerFive")]
    public class ChartOfAccLayerFive
    {
        [Key]
        public int CALayerFiveId { get; set; }

        public int CALayerFourId { get; set; }

        public string CALayerFiveCode { get; set; }
        public string CALayerFiveName { get; set; }
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
        public ChartOfAccLayerFour ChartOfAccLayerFour { get; set; }
        public IList<ChartOfAccLayerSix> ChartOfAccLayerSixList { get; set; }
    }
}