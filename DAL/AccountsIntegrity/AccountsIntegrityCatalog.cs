using System.Collections.Generic;

namespace New_Accounts.DAL.AccountsIntegrity
{
    /// <summary>What the dashboard knows about a problem token: its category, readable name and the automatic fix (if any).</summary>
    internal class ProblemInfo
    {
        public string Category;
        public string Title;
        public string FixCode;   // null = manual
        public string FixLabel;
        public string Note;
    }

    internal class IntegrityArea
    {
        public string Key;
        public string Page;
        public string SqlFile;
    }

    internal static class AccountsIntegrityCatalog
    {
        public const string Missing = "Missing voucher";
        public const string Value = "Value mismatch";
        public const string Subsidiary = "Subsidiary";
        public const string Reference = "Reference";
        public const string Other = "Other";

        // The five transaction pages of Journal_Entries_By_Page.xlsx (Delivery Confirmation covers LoadingSummary + Auto_Loading).
        public static readonly IntegrityArea[] Areas =
        {
            new IntegrityArea { Key = "delivery",   Page = "Delivery Confirmation",          SqlFile = "Detect_Delivery.sql" },
            new IntegrityArea { Key = "payment",    Page = "Customer Payment",               SqlFile = "Detect_Payment.sql" },
            new IntegrityArea { Key = "adjustment", Page = "Invoice Adjustment",             SqlFile = "Detect_Adjustment.sql" },
            new IntegrityArea { Key = "market",     Page = "Non-Transactional Sales Return", SqlFile = "Detect_MarketReturn.sql" },
            new IntegrityArea { Key = "dreturn",    Page = "Delivery Sales Return",          SqlFile = "Detect_DeliveryReturn.sql" },
        };

        private const string DatedToday = " The voucher is dated the day the fix runs.";

        private static ProblemInfo M(string category, string title, string note) =>
            new ProblemInfo { Category = category, Title = title, Note = note };
        private static ProblemInfo F(string category, string title, string fix, string label, string note) =>
            new ProblemInfo { Category = category, Title = title, FixCode = fix, FixLabel = label, Note = note };

        public static ProblemInfo Info(string areaKey, string problem)
        {
            switch (problem)
            {
                // ---- missing voucher -------------------------------------------------------------------------
                case "NO_SALES_VOUCHER":
                    return F(Missing, "Sales voucher missing", "DELIVERY_POST", "Post voucher", "Runs sp_Sales_DeliverySalesPosting (sales and COGS vouchers)." + DatedToday);
                case "NO_COGS_VOUCHER":
                    return F(Missing, "COGS voucher missing", "DELIVERY_POST", "Post COGS", "Re-runs sp_Sales_DeliverySalesPosting; the sales voucher is kept, only the COGS voucher is added. Nothing changes if no cost price exists." + DatedToday);
                case "SALES_VOUCHER_OTHER_KEY":
                    return M(Missing, "Sales voucher saved under another key", "A voucher for this invoice exists under a different TableID / id. Accounts must check it.");
                case "NO_VOUCHER":
                    switch (areaKey)
                    {
                        case "payment":    return F(Missing, "Receipt voucher missing", "PAYMENT_POST", "Post voucher", "Runs sp_Sales_CustomerPaymentPosting. The voucher uses the payment date.");
                        case "adjustment": return F(Missing, "Adjustment voucher missing", "ADJ_POST", "Post voucher", "Runs sp_Sales_InvoiceAdjustmentPosting. The voucher uses the adjustment date.");
                        case "market":     return F(Missing, "Sales return voucher missing", "MARKET_POST", "Post voucher", "Runs sp_SalesReturnPosting (needs the key-collision SP fix). The voucher uses the return date.");
                        case "dreturn":    return F(Missing, "Sales return voucher missing", "DRETURN_POST", "Post voucher", "Runs sp_DeliverySalesReturnPosting (sales return and inventory vouchers)." + DatedToday);
                        default:           return M(Missing, "Voucher missing", "No automatic fix.");
                    }
                case "NO_VOUCHER_BLOCKED":
                    return M(Missing, "Voucher missing (blocked by LC Charges key)", "LC Charges Booking already uses this TableID + id, so the SP would skip it. Fix the SP guard first, then post.");
                case "KEY_COLLISION":
                    return M(Missing, "Skipped by the SP (key collision)", "A delivery-return voucher uses the same TableID + id. Deploy Fix_sp_SalesReturnPosting_KeyCollision.sql, then use Fix on 'Sales return voucher missing'.");
                case "NO_COST_VOUCHER":
                    if (areaKey == "market")
                        return F(Missing, "Cost voucher missing", "MARKET_POST", "Post cost voucher", "Runs sp_SalesReturnPosting; only the missing cost voucher is added (needs the key-collision SP fix).");
                    return M(Missing, "Inventory / cost voucher missing", "Running the SP again would duplicate the sales-return voucher. Accounts must post the cost voucher by hand.");
                case "NO_DETAIL_LINES":
                    return M(Missing, "Voucher has no lines", "The voucher header exists without debit/credit lines. Accounts must repair or delete it.");
                case "NO_AR_LINE":
                    return M(Missing, "No Accounts Receivable line", "The voucher has no line on the Accounts Receivable GL. Accounts must repair it.");
                case "DUPLICATE_VOUCHER":
                case "DUPLICATE_COST":
                    return M(Other, "Duplicate voucher", "More than one voucher exists for one transaction. Accounts must reverse the extra one.");

                // ---- value mismatch -----------------------------------------------------------------------------
                case "HEADER_STALE":
                    return F(Value, "Invoice header differs from lines and voucher", "HEADER_SYNC", "Sync header", "Sets the invoice delivery total to the line total (the voucher already agrees). No accounts entry. Confirm with sales first.");
                case "VOUCHER_STALE":
                    return M(Value, "Voucher differs from invoice", "The invoice is right, the voucher is stale. Accounts must post an adjustment voucher for the difference.");
                case "AMOUNT_UNRELATED":
                    return M(Value, "Header, lines and voucher all differ", "Check the invoice history with sales and accounts.");
                case "AMOUNT_MISMATCH":
                    return M(Value, "Voucher amount differs", "Voucher and transaction amounts differ. Accounts must adjust or reverse and re-post.");
                case "AMOUNT_DR":
                case "AMOUNT_DEDUCTION":
                case "AMOUNT_AR":
                    return M(Value, "Voucher line amount differs", "A voucher line differs from the transaction. Accounts must adjust it.");
                case "AR_AMOUNT":
                    return M(Value, "AR credit differs from receipt", "The Accounts Receivable credit differs from the receipt total. Accounts must adjust it.");
                case "BANKCASH_LINE":
                    return M(Value, "Cash / Bank line wrong", "The Cash/Bank GL line has the wrong account or amount. Accounts must correct the voucher.");
                case "DISCOUNT_LINE":
                case "AIT_LINE":
                    return M(Value, "Discount / AIT line differs", "The Discount or AIT line differs from the receipt. Accounts must correct the voucher.");
                case "COST_AMOUNT":
                    return M(Value, "Cost voucher differs from recomputed cost", "The cost may simply have changed since posting. Accounts decide whether to adjust.");
                case "NET_PLUS_DEDUCTION":
                    return M(Value, "Net + deduction differs from return amount", "The return header is inconsistent. Correct the return document.");
                case "HEADER_VS_DETAIL":
                    return M(Value, "Header total differs from detail total", "The document header and its detail rows disagree. Correct the document.");
                case "VOUCHER_COUNT_DIFFERS":
                    return M(Value, "Number of vouchers differs from payment rows", "A multi-invoice payment has a different number of vouchers than rows. Accounts must check.");
                case "DR_GL_WRONG":
                    return M(Value, "Wrong debit GL", "The debit line is not on the GL for this adjustment type. Accounts must correct the voucher.");
                case "SALES_UNBALANCED":
                case "COGS_UNBALANCED":
                case "UNBALANCED":
                    return M(Value, "Voucher Dr and Cr differ", "The voucher is unbalanced. Accounts must correct it.");

                // ---- subsidiary --------------------------------------------------------------------------------
                case "NO_SUBSIDIARY":
                    return F(Subsidiary, "Customer ledger missing on AR line", "AR_SUBSIDIARY", "Set ledger", "Puts the customer's subsidiary ledger on the Accounts Receivable line. Amounts are unchanged. Edits an approved voucher line.");
                case "WRONG_CUSTOMER_LEDGER":
                    return F(Subsidiary, "AR line in another customer's ledger", "AR_SUBSIDIARY", "Move ledger", "Moves the Accounts Receivable line to the document's customer ledger (usually a duplicate customer record). Edits an approved voucher line.");

                // ---- reference ---------------------------------------------------------------------------------
                case "REF_MISSING":
                    return F(Reference, "Reference missing", "REF_FIX", "Set reference", "Sets the document reference on the voucher lines (creates the reference record if needed). Amounts are unchanged.");
                case "REF_WRONG":
                    return F(Reference, "Reference points to another document", "REF_FIX", "Fix reference", "Re-points the voucher lines to this document's reference. Amounts are unchanged.");
            }
            return M(Other, problem, "No automatic fix.");
        }

        public static readonly Dictionary<string, string> CategoryOrder = new Dictionary<string, string>
        {
            { Missing, "1" }, { Value, "2" }, { Subsidiary, "3" }, { Reference, "4" }, { Other, "5" },
        };
    }
}
