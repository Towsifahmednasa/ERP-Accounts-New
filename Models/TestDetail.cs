// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\TestDetail.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class TestDetail
    {

        public int? TestDetailId { get; set; }

        public int? TestMasterId { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public decimal? Amount { get; set; }

        public string Remarks { get; set; }
    }
}