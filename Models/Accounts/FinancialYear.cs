// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\FinancialYear.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class FinancialYear
    {
        public int FinancialYearId { get; set; }
        public string FinancialYearDesc { get; set; }
    }

    public class AccNo_DDL
    {
        public int AccountId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
    }
    public class BankAccount_Ddl
    {
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
    }

    public class CashAccount_Ddl
    {
        public int CashAccountId { get; set; }
        public string CashAccountName { get; set; }
    }

    public class COA_Ddl
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string SubControlName { get; set; }
    }

    public class CostCenter_Ddl
    {
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public int isrefrence { get; set; }

    }


    public class Reference_Ddl
    {
        public int ReferenceInfoId { get; set; }
        public string ReferenceInfo { get; set; }
    }

    public class Subsidiary_Ddl
    {
        public int SubsidiaryInfoId { get; set; }
        public string SubsidiaryInfo { get; set; }
    }

    public class SubsidiaryType_Ddl
    {
        public int SubsidiaryTypeId { get; set; }
        public string SubsidiaryType { get; set; }
        public int isrefrence { get; set; }
    }

    public class Subsidiary_chk
    {
        public int S_Info_Id { get; set; }
        public string S_InfoName { get; set; }
        public int isrefrence { get; set; }
        public int isReadOnly { get; set; }
    }

    public class RefInfo_chk
    {
        public int R_Info_Id { get; set; }
        public string R_InfoName { get; set; }
        public int isrefrence { get; set; }
    }
}