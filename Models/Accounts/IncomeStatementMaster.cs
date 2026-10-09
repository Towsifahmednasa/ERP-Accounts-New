// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\IncomeStatementMaster.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Accounts.Models.Accounts
{
    public class IncomeStatementMaster
    {
        public int IncomeStatementId { get; set; }
        public int? CompanyId { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        // Details list
        public List<IncomeStatementDetail> Details { get; set; }
            = new List<IncomeStatementDetail>();
    }
}
