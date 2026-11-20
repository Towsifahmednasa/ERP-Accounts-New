// Accounts Integrity Dashboard: finds missing / wrong vouchers of the five sales transaction pages and fixes them one
// transaction at a time. Every fix runs in its own transaction on the Sales DB (the accounts DB sits on the same
// server): it re-checks that the problem still exists, applies the fix, checks again that the problem is gone and only
// then commits. "Simulate" does all of that and rolls back, so a fix can be tried without saving anything.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using New_Accounts.DataManager;
using New_Accounts.Models.AccountsIntegrity;

namespace New_Accounts.DAL.AccountsIntegrity
{
    public class AccountsIntegrityDAL
    {
        private static readonly ConcurrentDictionary<string, string> SqlCache = new ConcurrentDictionary<string, string>();
        private static bool _logTableReady;

        private static string Acc => DataBase.CSTL_ERP_DB;     // accounts DB (legacy name of the setting)
        private static string Erp => DataBase.ControlPanel;    // ERP master DB
        private static string ConnectionString => SqlUserAccess.ConnectionString(DataBase.SalesDb);
        private static string SqlDirectory => Path.Combine(AppContext.BaseDirectory, "Sql", "AccountsIntegrity");

        private static string Prepare(string sql) => sql.Replace("{ACC}", Acc).Replace("{ERP}", Erp);

        private static string DetectSql(IntegrityArea area) =>
            SqlCache.GetOrAdd(area.SqlFile, f => Prepare(File.ReadAllText(Path.Combine(SqlDirectory, f))));

        internal static IntegrityArea FindArea(string key) => AccountsIntegrityCatalog.Areas.FirstOrDefault(a => a.Key == key);

        // ------------------------------------------------------------------------------------------------------------
        // detect
        // ------------------------------------------------------------------------------------------------------------
        public List<IntegrityIssue> GetIssues(DateTime? from, DateTime? to, string areaKey = null)
        {
            var issues = new List<IntegrityIssue>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var customers = LoadCustomers(conn);
                foreach (var area in AccountsIntegrityCatalog.Areas.Where(a => areaKey == null || a.Key == areaKey))
                    issues.AddRange(Detect(conn, null, area, from, to, null, customers));
            }
            return issues
                .OrderBy(i => Array.FindIndex(AccountsIntegrityCatalog.Areas, a => a.Key == i.AreaKey))
                .ThenBy(i => AccountsIntegrityCatalog.CategoryOrder[i.Category])
                .ThenBy(i => i.Problem)
                .ThenByDescending(i => i.DocDate)
                .ToList();
        }

        internal List<IntegrityIssue> Detect(SqlConnection conn, SqlTransaction tx, IntegrityArea area, DateTime? from, DateTime? to, int? docId,
                                             Dictionary<int, string> customers = null)
        {
            var list = new List<IntegrityIssue>();
            using (var cmd = new SqlCommand("DECLARE @From DATE = @pFrom, @To DATE = @pTo, @DocId INT = @pDocId;\n" + DetectSql(area), conn, tx))
            {
                cmd.CommandTimeout = 180;
                cmd.Parameters.AddWithValue("@pFrom", (object)from?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pTo", (object)to?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pDocId", (object)docId ?? DBNull.Value);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string problem = Convert.ToString(rdr["Problem"]);
                        var info = AccountsIntegrityCatalog.Info(area.Key, problem);
                        int doc2 = rdr["DocId2"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["DocId2"]);
                        string fix = info.FixCode, label = info.FixLabel, note = info.Note;
                        if (fix == "PAYMENT_POST" && doc2 <= 0)
                        {
                            fix = null; label = null;
                            note = "This payment covers several invoices; accounts must post it by hand.";
                        }
                        int custId = rdr["CustomerId"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["CustomerId"]);
                        string customer = "";
                        if (customers != null) customers.TryGetValue(custId, out customer);
                        list.Add(new IntegrityIssue
                        {
                            AreaKey = area.Key,
                            Page = area.Page,
                            Problem = problem,
                            Title = info.Title,
                            Category = info.Category,
                            DocId = Convert.ToInt32(rdr["DocId"]),
                            DocId2 = doc2,
                            DocNo = Convert.ToString(rdr["DocNo"]),
                            DocDate = rdr["DocDate"] == DBNull.Value ? "" : Convert.ToDateTime(rdr["DocDate"]).ToString("yyyy-MM-dd"),
                            Customer = customer ?? "",
                            Expected = rdr["Expected"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Expected"]),
                            Actual = rdr["Actual"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Actual"]),
                            Detail = Convert.ToString(rdr["Detail"]),
                            FixCode = fix,
                            FixLabel = label,
                            Note = note,
                        });
                    }
                }
            }
            return list;
        }

        private static Dictionary<int, string> LoadCustomers(SqlConnection conn)
        {
            var d = new Dictionary<int, string>();
            using (var cmd = new SqlCommand("SELECT CustomerMasterId, CustomerCode, CustomerName FROM dbo.tblCustMaster WITH (NOLOCK)", conn))
            using (var rdr = cmd.ExecuteReader())
                while (rdr.Read())
                    d[Convert.ToInt32(rdr[0])] = (Convert.ToString(rdr[2]) + " (" + Convert.ToString(rdr[1]) + ")").Trim();
            return d;
        }

        // ------------------------------------------------------------------------------------------------------------
        // fix
        // ------------------------------------------------------------------------------------------------------------
        public IntegrityFixResult Fix(string areaKey, string problem, int docId, int docId2, string user, bool simulate)
        {
            var result = new IntegrityFixResult { DocId = docId, Simulated = simulate };
            string docNo = null, fixCode = null;
            try
            {
                var area = FindArea(areaKey);
                if (area == null) throw new InvalidOperationException("Unknown page.");
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        try
                        {
                            Execute(conn, tx, area, problem, docId, docId2, simulate, result, out docNo, out fixCode);
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                        if (result.Success && !simulate) tx.Commit(); else tx.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex is SqlException ? "Database error, nothing was changed: " + ex.Message : ex.Message;
            }
            result.DocNo = docNo;
            WriteLog(user, areaKey, problem, fixCode, docId, docId2, docNo, result);
            return result;
        }

        internal void Execute(SqlConnection conn, SqlTransaction tx, IntegrityArea area, string problem, int docId, int docId2, bool simulate,
                             IntegrityFixResult result, out string docNo, out string fixCode)
        {
            docNo = null; fixCode = null;
            // 1. the problem must still be there (somebody may have fixed it meanwhile)
            var before = Detect(conn, tx, area, null, null, docId).Where(i => i.Problem == problem).ToList();
            if (before.Count == 0) { result.Message = "This problem no longer exists; reload the dashboard."; return; }
            var issue = before[0];
            docNo = issue.DocNo; fixCode = issue.FixCode;
            if (issue.FixCode == null) { result.Message = "No automatic fix for this problem. " + issue.Note; return; }
            docId2 = issue.DocId2;

            if (issue.FixCode == "MARKET_POST" && !IsMarketReturnSpFixed(conn, tx))
            {
                result.Message = "Deploy Fix_sp_SalesReturnPosting_KeyCollision.sql first; the current stored procedure would skip this return again.";
                return;
            }

            // 2. apply the fix
            int changed;
            using (var cmd = new SqlCommand(Prepare(AccountsIntegrityFixSql.For(issue.FixCode, area.Key)), conn, tx))
            {
                cmd.CommandTimeout = 180;
                cmd.Parameters.AddWithValue("@DocId", docId);
                cmd.Parameters.AddWithValue("@DocId2", docId2);
                // a posting SP may return result sets of its own; the number we want is the last single value
                changed = 0;
                using (var rdr = cmd.ExecuteReader())
                {
                    do { while (rdr.Read()) if (rdr.FieldCount == 1 && !rdr.IsDBNull(0)) changed = Convert.ToInt32(rdr[0]); }
                    while (rdr.NextResult());
                }
            }
            if (changed <= 0) { result.Message = "Nothing was changed (the voucher line to fix was not found)."; return; }

            // 3. the problem must be gone now
            bool stillThere = Detect(conn, tx, area, null, null, docId).Any(i => i.Problem == problem);
            if (stillThere)
            {
                result.Message = StillThereHint(issue.FixCode) + " Nothing was saved.";
                return;
            }
            result.Success = true;
            result.Message = simulate ? "Simulation OK: this fix would resolve the problem (nothing was saved)." : "Fixed.";
        }

        private static string StillThereHint(string fixCode)
        {
            switch (fixCode)
            {
                case "DELIVERY_POST": return "The posting did not create the voucher (for COGS: no cost price available).";
                case "MARKET_POST": return "The stored procedure did not create the voucher.";
                case "PAYMENT_POST":
                case "ADJ_POST":
                case "DRETURN_POST": return "The posting stored procedure did not create the voucher.";
                default: return "The problem is still there after the fix.";
            }
        }

        // The key-collision fix of sp_SalesReturnPosting is recognisable by its narration filter.
        private static bool IsMarketReturnSpFixed(SqlConnection conn, SqlTransaction tx)
        {
            using (var cmd = new SqlCommand("SELECT CASE WHEN OBJECT_DEFINITION(OBJECT_ID('dbo.sp_SalesReturnPosting')) LIKE @p THEN 1 ELSE 0 END", conn, tx))
            {
                cmd.Parameters.AddWithValue("@p", "%Narration LIKE 'Market Sales Return%");
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        public IntegrityFixAllResult FixAll(string areaKey, string problem, DateTime? from, DateTime? to, string user, bool simulate, int max = 1000)
        {
            var res = new IntegrityFixAllResult { Simulated = simulate };
            var todo = GetIssues(from, to, areaKey).Where(i => i.Problem == problem && i.FixCode != null).Take(max).ToList();
            res.Total = todo.Count;
            foreach (var i in todo)
            {
                var r = Fix(areaKey, problem, i.DocId, i.DocId2, user, simulate);
                if (r.Success) res.Fixed++;
                else { res.Failed++; if (res.Failures.Count < 20) res.Failures.Add(r); }
            }
            return res;
        }

        // ------------------------------------------------------------------------------------------------------------
        // fix log (created on first use in the Sales DB)
        // ------------------------------------------------------------------------------------------------------------
        private const string LogTableSql = """
IF OBJECT_ID('dbo.tblAccountsIntegrityFixLog', 'U') IS NULL
CREATE TABLE dbo.tblAccountsIntegrityFixLog (
    LogId     BIGINT IDENTITY(1,1) PRIMARY KEY,
    FixedAt   DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    FixedBy   NVARCHAR(100) NULL,
    AreaKey   VARCHAR(30) NOT NULL,
    Problem   VARCHAR(60) NOT NULL,
    FixCode   VARCHAR(40) NULL,
    DocId     INT NOT NULL,
    DocId2    INT NULL,
    DocNo     NVARCHAR(100) NULL,
    Success   BIT NOT NULL,
    Simulated BIT NOT NULL,
    Message   NVARCHAR(500) NULL);
""";

        private void WriteLog(string user, string areaKey, string problem, string fixCode, int docId, int docId2, string docNo, IntegrityFixResult r)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (!_logTableReady)
                    {
                        using (var c = new SqlCommand(LogTableSql, conn)) c.ExecuteNonQuery();
                        _logTableReady = true;
                    }
                    using (var cmd = new SqlCommand(@"INSERT INTO dbo.tblAccountsIntegrityFixLog (FixedBy, AreaKey, Problem, FixCode, DocId, DocId2, DocNo, Success, Simulated, Message)
                                                      VALUES (@u, @a, @p, @f, @d, @d2, @n, @s, @sim, @m)", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", (object)user ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@a", areaKey ?? "");
                        cmd.Parameters.AddWithValue("@p", problem ?? "");
                        cmd.Parameters.AddWithValue("@f", (object)fixCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@d", docId);
                        cmd.Parameters.AddWithValue("@d2", docId2);
                        cmd.Parameters.AddWithValue("@n", (object)docNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@s", r.Success);
                        cmd.Parameters.AddWithValue("@sim", r.Simulated);
                        cmd.Parameters.AddWithValue("@m", (object)(r.Message != null && r.Message.Length > 500 ? r.Message.Substring(0, 500) : r.Message) ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // the log must never break a fix that already finished
            }
        }
    }
}
