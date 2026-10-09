// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\MenuStepThree.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class MenuStepThree
    {
        public int SL { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string Permission { get; set; }
    }
}