// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\ReferanceTypeDetails.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;

namespace New_Accounts.Models.Accounts
{
    public class ReferanceTypeDetails
    {
        public int RefDetailsId { get; set; }

        public int ReferanceTypeId { get; set; }

        public int? SupplierId { get; set; }

        public bool? IsSupplier { get; set; }

        public int? CustomerId { get; set; }

        public bool? ISCustomer { get; set; }

        public int? EmployeeId { get; set; }

        public bool? IsEmployee { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? ModuleId { get; set; }

        public bool? IsModule { get; set; }
        public string RefName { get; set; }
    }
}