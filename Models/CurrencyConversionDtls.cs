// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Models\CurrencyConversionDtls.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
namespace New_Accounts.Models
{
    public class CurrencyConversionDtls
    {
        public int? CurrencyId { get; set; }  // Serial Number, usually for display purposes
        // Name of the currency (e.g., USD, Euro)
        public decimal? CurrencyRate { get; set; }  // Conversion rate for the currency
    }
}