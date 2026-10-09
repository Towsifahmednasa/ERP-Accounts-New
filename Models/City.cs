// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\City.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class City
    {
        public int CityId { get; set; }

        public int? CountryId { get; set; }

        public string CityName { get; set; }
    }
}