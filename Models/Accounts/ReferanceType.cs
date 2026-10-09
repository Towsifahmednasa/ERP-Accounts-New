// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ReferanceType.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_ReferanceType")]
    public class ReferanceType
    {
        [Key]
        public int ReferanceTypeId { get; set; }

        public string ReferanceTypeName { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InactiveDate { get; set; }


        public int CompanyId { get; set; }
        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }
        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public int BUId { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }

        public int isrefrence { get; set; }
        public bool isChecked { get; set; }

    }
}