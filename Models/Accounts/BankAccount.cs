// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\Accounts\BankAccount.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Accounts.Models.Accounts
{
    [Table("tbl_BankAccount")]
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        public int? CompanyId { get; set; }

        public int? BankId { get; set; }

        public int BranchId { get; set; }

        public string AccountName { get; set; }

        public string Description { get; set; }

        public int? SubsidiryLId { get; set; }

        public string AccountNo { get; set; }

        public int CALayerSevenId { get; set; }

        public string BankAccountCode { get; set; }

        public int? CurrencyId { get; set; }
        [NotMapped]

        public int LayerFourId { get; set; }

        [NotMapped]
        public string LayerSevenCode { get; set; }
    
        [NotMapped]
        public string BankName { get; set; }
        [NotMapped]
        public string CurrencyName { get; set; }
        [NotMapped]
        public string Bank { get; set; }
        [NotMapped]
        public string Branch { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
    }
}