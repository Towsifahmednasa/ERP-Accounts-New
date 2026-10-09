// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\CurrencyConversionMsterDao.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;

namespace New_Accounts.Models
{
    public class CurrencyConversionMsterDao
    {
        public DateTime PostingDate { get; set; }
        public List<CurrencyConversionDtls> CurrencyConversionDtls { get; set; }
    }
}