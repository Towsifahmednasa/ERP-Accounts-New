// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Currency.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class Currency
    {

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyRate { get; set; }

        

       

    }  public class CurrencyRateDAO
    {

 
        public string CurrencyRate { get; set; }

        

       

    }
}