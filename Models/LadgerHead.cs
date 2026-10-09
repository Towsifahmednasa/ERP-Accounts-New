// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\LadgerHead.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models
{
    public class LadgerHead
    {
        public int GLHeadId { get; set; }
        public string GLName { get; set; }
    }

    public class ControllLadger
    {

        public int ControlLId { get; set; }
        public string ControlLName { get; set; }
    }

    public class SubsidaryType
    {

        public int SubsidiryLId { get; set; }
        public string SubsidiryLName { get; set; }
    }

    public class SubsidaryTypes
    {

        public int SubsidiryTypeId { get; set; }
        public string SubsidiryTypeName { get; set; }
    }
    public class SupplierType
    {

        public int SupplierTypeId { get; set; }
        public string SupplierTypeName { get; set; }
    }

    public class ReferenceTypeDao
    {

        public int ReferenceTypeId { get; set; }
        public string ReferenceType { get; set; }
    }

    
    
     public class DDLDAO
    {

        public int ValueId { get; set; }
        public string ValueText { get; set; }
    }

    
    public class ReferenceInfoDao
    {

        public int ReferenceInfoId { get; set; }
        public string ReferenceName { get; set; }
    }


    public class ChartofAccount
    {

        public int AccountId { get; set; }
        public string AccountName { get; set; }
    }

    public class Supplierinfo
    {
        public int SupplierTypeId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }

    
}