// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DataManager\DataBase.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.DataManager
{
    public class DataBase
    {

        // ZAS
        //public static string ControlPanel = @"CSTL_ERPDB";
        //public static string CSTL_ERP_DB = @"CSTL_ACCDB";

        //FC

        //public static string ControlPanel = @"CSTL_ERPDB_FC";
        //public static string CSTL_ERP_DB = @"CSTL_ACCDB_FC";

        // Unicorn
        public static string ControlPanel = @"CSTL_ERPDB_Unicorn";
        public static string CSTL_ERP_DB = @"CSTL_ACCDB_Unicorn";
    }
}