// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LoginUser.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class LoginUser
    {

        public string userName { get; set; }
        public string password { get; set; }
        public bool isRemember { get; set; }
    }
}