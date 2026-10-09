// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\BusinessUnit.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tblCompanyUnit")]
    public class BusinessUnit
    {
        [Key]
       public int CompanyUnitId { get; set; }
       public int CompanyId { get; set; }
       public string CompanyUnitName { get; set; }
       public string ShortName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

    }
}