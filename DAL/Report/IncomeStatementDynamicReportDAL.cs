// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\IncomeStatementDynamicReportDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace New_Accounts.DAL.Report
{
    public class IncomeStatementDynamicReportDAL
    {
        private static readonly Regex NumberTokenRegex = new Regex(@"\b\d+(?:\.\d+)?\b", RegexOptions.Compiled);
        private static readonly Regex AllowedExpressionRegex = new Regex(@"^[0-9\.\+\-\*/\(\)\s]+$", RegexOptions.Compiled);

        // ================================================================
        // NEW OPTIMISED GetReport — 2 DB calls only
        // ================================================================
        public List<IncomeStatementDynamicReportRow> GetReport(
            int companyId,
            int financialYearId,
            DateTime fromDate,
            DateTime toDate)
        {
            // STEP 1: Get Income Statement setup rows (1 DB call)
            List<IncomeStatementSetupRow> setupRows = GetIncomeStatementSetupRows(companyId);

            // STEP 2: Get ALL GL Closing Balance (1 DB call)
            Dictionary<string, decimal> glClosingBalances = GetAllGLClosingBalance(
                companyId, financialYearId, fromDate, toDate);

            // STEP 3: Build formulaTotals from GL closing balance
            Dictionary<string, decimal> formulaTotals = BuildFormulaTotalsFromGL(
                setupRows, glClosingBalances);

            // STEP 4: Build report rows (same logic as before)
            List<IncomeStatementDynamicReportRow> reportRows = new List<IncomeStatementDynamicReportRow>();
            Dictionary<int, decimal> rowAmounts = new Dictionary<int, decimal>();

            // Non-bold rows → direct COA amount
            foreach (IncomeStatementSetupRow setupRow in setupRows.Where(x => !x.IsBold))
            {
                rowAmounts[setupRow.OrderSl] = GetSingleFormulaValue(
                    setupRow.COAFormula,
                    formulaTotals,
                    setupRow.CodeType);
            }

            // Bold rows → evaluate formula (e.g. "1-2", "3+4")
            bool resolvedAny;
            int guard = 0;
            do
            {
                resolvedAny = false;
                foreach (IncomeStatementSetupRow setupRow in setupRows.Where(x => x.IsBold))
                {
                    decimal amount;
                    if (TryEvaluateSlFormula(setupRow.COAFormula, rowAmounts, out amount))
                    {
                        if (!rowAmounts.ContainsKey(setupRow.OrderSl) || rowAmounts[setupRow.OrderSl] != amount)
                        {
                            rowAmounts[setupRow.OrderSl] = amount;
                            resolvedAny = true;
                        }
                    }
                }
                guard++;
            } while (resolvedAny && guard < setupRows.Count);

            foreach (IncomeStatementSetupRow setupRow in setupRows)
            {
                decimal amount = 0;
                if (rowAmounts.ContainsKey(setupRow.OrderSl))
                {
                    amount = rowAmounts[setupRow.OrderSl];
                }
                else if (setupRow.IsBold)
                {
                    amount = EvaluateFormula(setupRow.COAFormula, formulaTotals, setupRow.CodeType);
                }

                reportRows.Add(new IncomeStatementDynamicReportRow
                {
                    Sl        = setupRow.OrderSl,
                    Particular = setupRow.Particular,
                    Notes     = setupRow.Remarks,
                    Formula   = setupRow.COAFormula,
                    IsBold    = setupRow.IsBold,
                    Amount    = amount
                });
            }

            return reportRows;
        }

        // ================================================================
        // NEW: Get ALL GL Closing Balance — single DB call
        // sp_GET_AllGLClosingBalance return করবে:
        // AccountId, AccountCode, ControlLCode, SubControlCode,
        // SubSubControlCode, ClosingBalance
        // ================================================================
        private Dictionary<string, decimal> GetAllGLClosingBalance(
            int companyId,
            int financialYearId,
            DateTime fromDate,
            DateTime toDate)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@CompanyId",       companyId),
                    new SqlParameter("@FinancialYearId", financialYearId),
                    new SqlParameter("@FromDate",        fromDate),
                    new SqlParameter("@ToDate",          toDate)
                };

                DataTable dt = accessManager.GetDataTable(
                    "sp_GET_AllGLClosingBalance", parameters);

                // Dictionary — key হবে "CodeType|Code", value হবে ClosingBalance
                // প্রতিটা row থেকে সব level এর key বানাও
                // যাতে formula match করতে পারে
                Dictionary<string, decimal> balances = new Dictionary<string, decimal>();

                foreach (DataRow row in dt.Rows)
                {
                    decimal closing = ToDecimal(row["ClosingBalance"]);

                    // ChartOfAccount level (exact AccountCode)
                    string accountCode = row["AccountCode"].ToString().Trim();
                    if (!string.IsNullOrEmpty(accountCode))
                    {
                        string key = GetFormulaKey("ChartOfAccount", accountCode);
                        if (balances.ContainsKey(key))
                            balances[key] += closing;
                        else
                            balances[key] = closing;
                    }

                    // SubSubControlLedger level
                    string subSubCode = row["SubSubControlCode"].ToString().Trim();
                    if (!string.IsNullOrEmpty(subSubCode))
                    {
                        string key = GetFormulaKey("SubSubControlLedger", subSubCode);
                        if (balances.ContainsKey(key))
                            balances[key] += closing;
                        else
                            balances[key] = closing;
                    }

                    // SubControlLedger level
                    string subCode = row["SubControlCode"].ToString().Trim();
                    if (!string.IsNullOrEmpty(subCode))
                    {
                        string key = GetFormulaKey("SubControlLedger", subCode);
                        if (balances.ContainsKey(key))
                            balances[key] += closing;
                        else
                            balances[key] = closing;
                    }

                    // ControlLedger level
                    string controlCode = row["ControlLCode"].ToString().Trim();
                    if (!string.IsNullOrEmpty(controlCode))
                    {
                        string key = GetFormulaKey("ControlLedger", controlCode);
                        if (balances.ContainsKey(key))
                            balances[key] += closing;
                        else
                            balances[key] = closing;
                    }
                }

                return balances;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        // ================================================================
        // NEW: Build formulaTotals from GL closing balance dictionary
        // পুরনো BuildFormulaTotals এর মতোই কাজ করে
        // কিন্তু DB call নেই — শুধু dictionary lookup
        // ================================================================
        private Dictionary<string, decimal> BuildFormulaTotalsFromGL(
            List<IncomeStatementSetupRow> setupRows,
            Dictionary<string, decimal> glClosingBalances)
        {
            Dictionary<string, decimal> totals = new Dictionary<string, decimal>();

            foreach (IncomeStatementSetupRow row in setupRows.Where(x => !x.IsBold))
            {
                foreach (Match match in NumberTokenRegex.Matches(row.COAFormula ?? string.Empty))
                {
                    string key = GetFormulaKey(row.CodeType, match.Value);
                    if (!totals.ContainsKey(key))
                    {
                        totals[key] = glClosingBalances.ContainsKey(key)
                            ? glClosingBalances[key]
                            : 0;
                    }
                }
            }

            return totals;
        }

        // ================================================================
        // EXISTING CODE (kept for reference)
        // ================================================================

        private List<IncomeStatementSetupRow> GetIncomeStatementSetupRows(int companyId)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@CompanyId", companyId)
                };

                DataTable dt = accessManager.GetDataTable("sp_GET_IncomeStatementDetailByCompanyId", parameters);
                List<IncomeStatementSetupRow> rows = new List<IncomeStatementSetupRow>();

                foreach (DataRow row in dt.Rows)
                {
                    rows.Add(new IncomeStatementSetupRow
                    {
                        OrderSl                   = ToInt(row["OrderSl"]),
                        Particular                = row["Particular"].ToString(),
                        COAFormula                = row["COAFormula"].ToString(),
                        Remarks                   = row["Remarks"].ToString(),
                        IsBold                    = ToBool(row["IsBold"]),
                        IsControlLedgerCode       = GetBool(row, "IsControlLedgerCode", true),
                        IsSubcontrolLedgerCode    = GetBool(row, "IsSubcontrolLedgerCode", false),
                        IsSubSubControlLedgercode = GetBool(row, "IsSubSubControlLedgercode", false),
                        IsChartOfAccount          = GetBool(row, "IsChartOfAccount", false)
                    });
                }

                return rows.OrderBy(x => x.OrderSl).ToList();
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        // ================================================================
        // OLD BuildFormulaTotals — commented out (replaced by new approach)
        // ================================================================
        //private Dictionary<string, decimal> BuildFormulaTotals(
        //    List<IncomeStatementSetupRow> setupRows,
        //    int companyId,
        //    int financialYearId,
        //    DateTime fromDate,
        //    DateTime toDate)
        //{
        //    HashSet<string> formulaKeys = new HashSet<string>();
        //    foreach (IncomeStatementSetupRow row in setupRows)
        //    {
        //        foreach (Match match in NumberTokenRegex.Matches(row.COAFormula ?? string.Empty))
        //        {
        //            formulaKeys.Add(GetFormulaKey(row.CodeType, match.Value));
        //        }
        //    }

        //    Dictionary<string, decimal> totals = new Dictionary<string, decimal>();
        //    foreach (string formulaKey in formulaKeys)
        //    {
        //        string[] parts = formulaKey.Split(new[] { '|' }, 2);
        //        string codeType = parts[0];
        //        string formulaCode = parts.Length > 1 ? parts[1] : string.Empty;
        //        List<CoaInfo> coas = GetCoasByFormulaCode(formulaCode, codeType, companyId);
        //        decimal total = 0;

        //        foreach (CoaInfo coa in coas)
        //        {
        //            decimal opening = GetOpeningBalance(
        //                companyId, financialYearId, coa.AccountId,
        //                fromDate, toDate, coa.AccountName, fromDate);

        //            decimal transactionBalance = GetTransactionBalance(
        //                companyId, financialYearId, coa.AccountId,
        //                fromDate, toDate, coa.AccountName);

        //            total += opening + transactionBalance;
        //        }

        //        totals[formulaKey] = total;
        //    }

        //    return totals;
        //}

        // ================================================================
        // OLD GetCoasByFormulaCode — commented out (no longer needed)
        // ================================================================
        //private List<CoaInfo> GetCoasByFormulaCode(string formulaCode, string codeType, int companyId)
        //{
        //    DataAccessManager accessManager = new DataAccessManager();
        //    try
        //    {
        //        accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<SqlParameter> parameters = new List<SqlParameter>
        //        {
        //            new SqlParameter("@FormulaCode", formulaCode),
        //            new SqlParameter("@CodeType", codeType),
        //            new SqlParameter("@CompanyId", companyId)
        //        };

        //        DataTable dt = accessManager.GetDataTable("sp_GET_IncomeStatementCOAByFormulaCode", parameters);
        //        List<CoaInfo> coas = new List<CoaInfo>();

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            coas.Add(new CoaInfo
        //            {
        //                AccountId = ToInt(row["AccountId"]),
        //                AccountName = row["AccountName"].ToString()
        //            });
        //        }

        //        return coas;
        //    }
        //    finally
        //    {
        //        accessManager.SqlConnectionClose();
        //    }
        //}

        // ================================================================
        // OLD GetOpeningBalance — commented out (replaced by sp_GET_AllGLClosingBalance)
        // ================================================================
        //private decimal GetOpeningBalance(
        //    int companyId, int financialYearId, int accountId,
        //    DateTime fromDate, DateTime toDate,
        //    string accountName, DateTime financialYearStartDate)
        //{
        //    DataAccessManager accessManager = new DataAccessManager();
        //    try
        //    {
        //        accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<SqlParameter> parameters = new List<SqlParameter>
        //        {
        //            new SqlParameter("@CompanyInfoId", companyId),
        //            new SqlParameter("@FinancialYearId", financialYearId),
        //            new SqlParameter("@CoaAccountId", accountId),
        //            new SqlParameter("@FromDate", fromDate),
        //            new SqlParameter("@Todate", toDate),
        //            new SqlParameter("@BankBookBankNAme", accountName),
        //            new SqlParameter("@BankBookFinancialYearStartDate", financialYearStartDate)
        //        };

        //        DataTable dt = accessManager.GetDataTable("sp_GET_COAAccountOpeningBalance", parameters);
        //        if (dt.Rows.Count == 0) return 0;
        //        return ToDecimal(dt.Rows[0]["OpeningBalance"]);
        //    }
        //    finally
        //    {
        //        accessManager.SqlConnectionClose();
        //    }
        //}

        // ================================================================
        // OLD GetTransactionBalance — commented out (replaced by sp_GET_AllGLClosingBalance)
        // ================================================================
        //private decimal GetTransactionBalance(
        //    int companyId, int financialYearId, int accountId,
        //    DateTime fromDate, DateTime toDate, string accountName)
        //{
        //    DataAccessManager accessManager = new DataAccessManager();
        //    try
        //    {
        //        accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<SqlParameter> parameters = new List<SqlParameter>
        //        {
        //            new SqlParameter("@CompanyInfoId", companyId),
        //            new SqlParameter("@FinancialYearId", financialYearId),
        //            new SqlParameter("@BankAccountId", accountId),
        //            new SqlParameter("@FromDate", fromDate),
        //            new SqlParameter("@Todate", toDate),
        //            new SqlParameter("@ChartofAccountName", accountName)
        //        };

        //        DataTable dt = accessManager.GetDataTable(
        //            "sp_GET_TrailBalance_TransectionforPostingDate", parameters);
        //        if (dt.Rows.Count == 0) return 0;
        //        return ToDecimal(dt.Rows[0]["FinalBalance"]);
        //    }
        //    finally
        //    {
        //        accessManager.SqlConnectionClose();
        //    }
        //}

        // ================================================================
        // UNCHANGED — helper methods
        // ================================================================

        private decimal GetSingleFormulaValue(
            string formula,
            Dictionary<string, decimal> values,
            string codeType)
        {
            string token = (formula ?? string.Empty).Trim();
            string key   = GetFormulaKey(codeType, token);
            return values.ContainsKey(key)
                ? values[key]
                : EvaluateFormula(formula, values, codeType);
        }

        private decimal EvaluateFormula(
            string formula,
            Dictionary<string, decimal> values,
            string codeType)
        {
            if (string.IsNullOrWhiteSpace(formula)) return 0;

            string expression = NumberTokenRegex.Replace(formula, match =>
            {
                string key = GetFormulaKey(codeType, match.Value);
                if (!values.ContainsKey(key)) return match.Value;
                return "(" + values[key].ToString(CultureInfo.InvariantCulture) + ")";
            });

            if (!AllowedExpressionRegex.IsMatch(expression)) return 0;

            try
            {
                object result = new DataTable().Compute(expression, string.Empty);
                return Convert.ToDecimal(result, CultureInfo.InvariantCulture);
            }
            catch { return 0; }
        }

        private bool TryEvaluateSlFormula(
            string formula,
            Dictionary<int, decimal> rowAmounts,
            out decimal amount)
        {
            amount = 0;
            if (string.IsNullOrWhiteSpace(formula)) return false;

            bool hasSlReference = false;
            string expression = NumberTokenRegex.Replace(formula, match =>
            {
                int sl;
                if (int.TryParse(match.Value, out sl) && rowAmounts.ContainsKey(sl))
                {
                    hasSlReference = true;
                    return "(" + rowAmounts[sl].ToString(CultureInfo.InvariantCulture) + ")";
                }
                return "0";
            });

            if (!hasSlReference || !AllowedExpressionRegex.IsMatch(expression))
                return false;

            try
            {
                object result = new DataTable().Compute(expression, string.Empty);
                amount = Convert.ToDecimal(result, CultureInfo.InvariantCulture);
                return true;
            }
            catch { amount = 0; return true; }
        }

        private string GetFormulaKey(string codeType, string formulaCode)
        {
            return (string.IsNullOrWhiteSpace(codeType) ? "ControlLedger" : codeType)
                   + "|"
                   + (formulaCode ?? string.Empty).Trim();
        }

        private int ToInt(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            int parsed;
            return int.TryParse(value.ToString(), out parsed) ? parsed : 0;
        }

        private decimal ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            decimal parsed;
            return decimal.TryParse(value.ToString(), out parsed) ? parsed : 0;
        }

        private bool ToBool(object value)
        {
            if (value == null || value == DBNull.Value) return false;
            bool parsed;
            return bool.TryParse(value.ToString(), out parsed) && parsed;
        }

        private bool GetBool(DataRow row, string columnName, bool defaultValue)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return defaultValue;
            return ToBool(row[columnName]);
        }

        // ================================================================
        // UNCHANGED — inner classes
        // ================================================================

        private class IncomeStatementSetupRow
        {
            public int    OrderSl                   { get; set; }
            public string Particular                { get; set; }
            public string COAFormula                { get; set; }
            public string Remarks                   { get; set; }
            public bool   IsBold                    { get; set; }
            public bool   IsControlLedgerCode       { get; set; }
            public bool   IsSubcontrolLedgerCode    { get; set; }
            public bool   IsSubSubControlLedgercode { get; set; }
            public bool   IsChartOfAccount          { get; set; }

            public string CodeType
            {
                get
                {
                    if (IsSubcontrolLedgerCode)       return "SubControlLedger";
                    if (IsSubSubControlLedgercode)    return "SubSubControlLedger";
                    if (IsChartOfAccount)             return "ChartOfAccount";
                    return "ControlLedger";
                }
            }
        }

        private class CoaInfo
        {
            public int    AccountId   { get; set; }
            public string AccountName { get; set; }
        }
    }

    public class IncomeStatementDynamicReportRow
    {
        public int     Sl         { get; set; }
        public string  Particular { get; set; }
        public string  Notes      { get; set; }
        public string  Formula    { get; set; }
        public bool    IsBold     { get; set; }
        public decimal Amount     { get; set; }
    }
}