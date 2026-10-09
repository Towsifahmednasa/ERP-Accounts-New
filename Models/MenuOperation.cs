// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\MenuOperation.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class MenuOperation
    {
        public int SL { get; set; }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public bool IsPage { get; set; }
        public string MenuName { get; set; }

        public int? ParantId { get; set; }

        public string ParantMenu { get; set; }

        public int MenuStep { get; set; }

        public string MenuStepName { get; set; }

        public int MenuTypeId { get; set; }

        public string MenuType { get; set; }

        public bool IsMVC { get; set; }

        public string URL { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsActive { get; set; }

        public bool IsApprovalPage { get; set; }

        public List<MenuPermission> menuPermissionList { get; set; }
    }
}