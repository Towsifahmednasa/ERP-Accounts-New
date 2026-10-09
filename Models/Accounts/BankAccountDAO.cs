// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\BankAccountDAO.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class BankAccountDAO
    {

        public int BankAccountId { get; set; }
        public string BusinessUnit { get; set; }

        public int? BankId { get; set; }

        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string BankBlnc { get; set; }

        public string Description { get; set; }

        public int? CurrencyId { get; set; }

        public int? SubsidiryLId { get; set; }
        public int? SubSubControlLedgerId { get; set; }

        public string AccountNo { get; set; }

        public int? CompanyId { get; set; }

        public string BranchName { get; set; }

        public int? AccountTypeId { get; set; }

        public string BankAccountRouteNumber { get; set; }

        public DateTime? OpeningDate { get; set; }

        public string BankAddress { get; set; }


        //For Edit

        public string RE_OpeningDate { get; set; }
        public int? ControlLId { get; set; }

        public int? GLHeadId { get; set; }

    }



    public class BankAccountView
    {

        public int BankAccountId { get; set; }
        public string CompanyName { get; set; }
        public string BussinessUnit { get; set; }

        public string BankName { get; set; }

        public string AccountName { get; set; }

        public string Description { get; set; }
        public string GLName { get; set; }
        public string ControlLName { get; set; }
        public string SubsidiryLName { get; set; }
        public string SubSubControlLedgerName { get; set; }

        public int? CurrencyId { get; set; }

       

        public string AccountNo { get; set; }

      
        public string BranchName { get; set; }

         
        public string BankAccountRouteNumber { get; set; }

        public string OpeningDate { get; set; }

        public string BankAddress { get; set; }


    }


    public class ChartOfAccounts
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public string AccountName { get; set; }
        public string SSubsidiaryLCode { get; set; }
        public int SubsidiaryLId { get; set; }
        public int? CompanyId { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string CreateBy { get; set; }
        public int? BankAccountId { get; set; }
        public int? CashAccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public string IsActive { get; set; }
        public bool InBalanceSheet { get; set; }
        public bool InIncomeStatement { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        public bool? IsEmployee { get; set; }

    }
}