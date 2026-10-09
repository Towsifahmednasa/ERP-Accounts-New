// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\MenuPermission.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class MenuPermission
    {
        public int DisSL { get; set; }
        public int LoginUserId { get; set; }
        public int SL { get; set; }
        public bool Status { get; set; }
        public int ModuleId { get; set; }
        public DateTime PermissionDate { get; set; }
        public string PermissionGivenBy { get; set; }
    }
}