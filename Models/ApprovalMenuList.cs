// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\ApprovalMenuList.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class ApprovalMenuList
    {
        public int SL { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string pending { get; set; }
        public string n_MenuName { get; set; }
        public string n_ControllerName { get; set; }
        public string n_ActionName { get; set; }
        public string n_pending { get; set; }
        public string bgcolor { get; set; }
    }
}