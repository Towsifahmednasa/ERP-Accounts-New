// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ChartOfAccLayerTwo.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ChartOfAccLayerTwo")]
    public class ChartOfAccLayerTwo
    {

        
        [Key]
        public int CALayerTwoId { get; set; }

        public int LedgerGroupId { get; set; }

        public string CALayerTwoCode { get; set; }
        public string CALayerTwoName { get; set; }
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
        public  LedgerGroup LedgerGroup { get; set; }
        public  IList<ChartOfAccLayerThree> ChartOfAccLayerThreeList { get; set; }

    }
}