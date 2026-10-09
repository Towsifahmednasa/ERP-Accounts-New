// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LOAN_DAO\LCRelatedBankPayment.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.LOAN_DAO
{
    public class LCRelatedBankPayment
    {

        public int BankChargeId { get; set; }

        public int LCOpenId { get; set; }

        public int LcBankPayTypeId { get; set; }
    }
}