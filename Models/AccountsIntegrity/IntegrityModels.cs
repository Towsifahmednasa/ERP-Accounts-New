using System.Collections.Generic;

namespace New_Accounts.Models.AccountsIntegrity
{
    /// <summary>One problem found on one transaction (a transaction with two problems appears twice).</summary>
    public class IntegrityIssue
    {
        public string AreaKey { get; set; }      // delivery | payment | adjustment | market | dreturn
        public string Page { get; set; }         // page name shown on the dashboard
        public string Problem { get; set; }      // token, e.g. NO_SUBSIDIARY
        public string Title { get; set; }        // readable problem name
        public string Category { get; set; }     // Missing voucher | Value mismatch | Subsidiary | Reference | Other
        public int DocId { get; set; }
        public int DocId2 { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Customer { get; set; }
        public decimal Expected { get; set; }
        public decimal Actual { get; set; }
        public string Detail { get; set; }
        public string FixCode { get; set; }      // null = no automatic fix
        public string FixLabel { get; set; }     // button text
        public string Note { get; set; }         // what the fix does / why it is manual
    }

    public class IntegrityFixResult
    {
        public bool Success { get; set; }
        public bool Simulated { get; set; }
        public string Message { get; set; }
        public string DocNo { get; set; }
        public int DocId { get; set; }
    }

    public class IntegrityFixAllResult
    {
        public int Total { get; set; }
        public int Fixed { get; set; }
        public int Failed { get; set; }
        public bool Simulated { get; set; }
        public List<IntegrityFixResult> Failures { get; set; } = new List<IntegrityFixResult>();
    }
}
