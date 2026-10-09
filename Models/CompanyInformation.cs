// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\CompanyInformation.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace New_Accounts.Models
{
    [Serializable]
    [Table("tbl_CompanyInformation")]
    public class CompanyInformation
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyNameBG { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public int? MinSerial { get; set; }
        public int? MaxSerial { get; set; }
    }
}