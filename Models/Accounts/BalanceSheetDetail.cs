// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\BalanceSheetDetail.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Accounts.Models.Accounts
{
    public class BalanceSheetDetail
    {
        public int BalanceSheetDetailId { get; set; }
        public int BalanceSheetId { get; set; }
        public int? OrderSl { get; set; }
        public string Particular { get; set; }
        public string COAFormula { get; set; }
        public string Remarks { get; set; }
        public bool? IsBold { get; set; }
        public bool? IsControlLedgerCode { get; set; }
        public bool? IsSubcontrolLedgerCode { get; set; }
        public bool? IsSubSubControlLedgercode { get; set; }
        public bool? IsChartOfAccount { get; set; }
    }
}
