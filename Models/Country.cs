// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Country.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string ShortName { get; set; }
    }
}