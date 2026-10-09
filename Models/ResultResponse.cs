// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\ResultResponse.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class ResultResponse
    {
        public bool isSuccess { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
        public int pk { get; set; }
    }
}