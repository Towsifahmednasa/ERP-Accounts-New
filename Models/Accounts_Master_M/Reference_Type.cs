// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts_Master_M\Reference_Type.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts_Master_M
{
    public class Reference_Type
    {
        public int ReferenceTypeId { get; set; }

        public string ReferenceType { get; set; }

        public int? EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Ref_TableName { get; set; }
        public Boolean? IsActive { get; set; }
    }
}