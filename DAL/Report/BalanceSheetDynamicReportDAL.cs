// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\BalanceSheetDynamicReportDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class BalanceSheetDynamicReportDAL
    {
        private static readonly Regex NumberTokenRegex =
            new Regex(@"\b\d+(?:\.\d+)?\b", RegexOptions.Compiled);

        private static readonly Regex AllowedExpressionRegex =
            new Regex(@"^[0-9\.\+\-\*/\(\)\s]+$", RegexOptions.Compiled);

        public List<BalanceSheetDynamicReportRow> GetReport(
            int companyId,
            int financialYearId,
            DateTime fromDate,
            DateTime toDate)
        {
            List<BalanceSheetSetupRow> setupRows = GetBalanceSheetSetupRows(companyId);

            setupRows = setupRows
                .GroupBy(x => x.OrderSl)
                .Select(g => g.First())
                .OrderBy(x => x.OrderSl)
                .ToList();

            Dictionary<string, decimal> glClosingBalances;
            Dictionary<string, string> glNameMap;

            GetAllGLClosingBalance(
                companyId,
                financialYearId,
                fromDate,
                toDate,
                out glClosingBalances,
                out glNameMap);

            Dictionary<string, string> formulaGlNames;

            Dictionary<string, decimal> formulaTotals = BuildFormulaTotalsFromGL(
                setupRows,
                glClosingBalances,
                glNameMap,
                out formulaGlNames);

            Dictionary<int, decimal> rowAmounts = new Dictionary<int, decimal>();
            Dictionary<int, string> rowGlNames = new Dictionary<int, string>();

            // Non-bold rows: direct GL/COA amount
            foreach (BalanceSheetSetupRow setupRow in setupRows.Where(x => !x.IsBold).OrderBy(x => x.OrderSl))
            {
                decimal amount = GetSingleFormulaValue(
                    setupRow.COAFormula,
                    formulaTotals,
                    setupRow.CodeType);

                rowAmounts[setupRow.OrderSl] = amount;
                rowGlNames[setupRow.OrderSl] =
                    GetSetupRowGlName(setupRow, formulaGlNames);
            }

            // Bold rows: SL based formula calculation
            bool resolvedAny;
            int guard = 0;

            do
            {
                resolvedAny = false;

                foreach (BalanceSheetSetupRow setupRow in setupRows.Where(x => x.IsBold).OrderBy(x => x.OrderSl))
                {
                    decimal amount;

                    if (TryEvaluateSlFormula(setupRow.COAFormula, rowAmounts, out amount))
                    {
                        if (!rowAmounts.ContainsKey(setupRow.OrderSl) ||
                            rowAmounts[setupRow.OrderSl] != amount)
                        {
                            rowAmounts[setupRow.OrderSl] = amount;
                            resolvedAny = true;
                        }

                        string glName = GetReferencedRowGlName(
                            setupRow.COAFormula,
                            rowGlNames);

                        if (!string.IsNullOrWhiteSpace(glName))
                        {
                            rowGlNames[setupRow.OrderSl] = glName;
                        }
                    }
                }

                guard++;

            } while (resolvedAny && guard < setupRows.Count);

            List<BalanceSheetDynamicReportRow> reportRows =
                new List<BalanceSheetDynamicReportRow>();

            foreach (BalanceSheetSetupRow setupRow in setupRows.OrderBy(x => x.OrderSl))
            {
                decimal amount = 0;

                if (rowAmounts.ContainsKey(setupRow.OrderSl))
                {
                    amount = rowAmounts[setupRow.OrderSl];
                }
                else if (setupRow.IsBold)
                {
                    amount = EvaluateFormula(
                        setupRow.COAFormula,
                        formulaTotals,
                        setupRow.CodeType);
                }

                reportRows.Add(new BalanceSheetDynamicReportRow
                {
                    Sl = setupRow.OrderSl,
                    DisplayOrder = setupRow.OrderSl,
                    Particular = setupRow.Particular,
                    Notes = setupRow.Remarks,
                    Formula = setupRow.COAFormula,
                    IsBold = setupRow.IsBold,
                    IsSectionHeader = false,
                    GLName = rowGlNames.ContainsKey(setupRow.OrderSl)
                                ? rowGlNames[setupRow.OrderSl]
                                : string.Empty,
                    Amount = amount
                });
            }

            return FormatBalanceSheetRows(reportRows);
        }


        public List<TrialBalanceTreeRow> GetTrialBalanceTreeRows(
    int companyId,
    int financialYearId,
    DateTime fromDate,
    DateTime toDate,
    string glHead,
    string controlLadger,
    string subsidiary,
    string subsubsidiary,
    string ledgerType,
    int subsidiaryInfoId,
    int referenceInfoId,
    int costCenterId)
        {
            List<TrialBalanceTreeRow> rows = new List<TrialBalanceTreeRow>();

            int glHeadId = ToFilterInt(glHead);
            int controlLedgerId = ToFilterInt(controlLadger);
            int subsidiaryId = ToFilterInt(subsidiary);
            int subSubControlLedgerId = ToFilterInt(subsubsidiary);

            DataAccessManager accessManager = new DataAccessManager();

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@CompanyId", companyId),
            new SqlParameter("@FinancialYearId", financialYearId),
            new SqlParameter("@FromDate", fromDate),
            new SqlParameter("@ToDate", toDate),
            new SqlParameter("@GLHeadId", glHeadId),
            new SqlParameter("@ControlLId", controlLedgerId),
            new SqlParameter("@SubsidiryLId", subsidiaryId),
            new SqlParameter("@SubSubControlLedgerId", subSubControlLedgerId),
            new SqlParameter("@SubsidiaryInfoId", subsidiaryInfoId),
            new SqlParameter("@ReferenceInfoId", referenceInfoId),
            new SqlParameter("@CostCenterId", costCenterId)
        };

                DataTable dt = accessManager.GetDataTable(
                    "sp_GET_AllGLClosingBalance",
                    parameters);

                foreach (DataRow row in dt.Rows)
                {
                    int rowGLHeadId = ToInt(row["GLHeadId"]);
                    int rowControlLId = ToInt(row["ControlLId"]);
                    int rowSubsidiryLId = ToInt(row["SubsidiryLId"]);
                    int rowSubSubControlLedgerId = ToInt(row["SubSubControlLedgerId"]);

                    // ===============================
                    // Filter by selected hierarchy
                    // ===============================
                    if (glHeadId > 0 && rowGLHeadId != glHeadId)
                    {
                        continue;
                    }

                    if (controlLedgerId > 0 && rowControlLId != controlLedgerId)
                    {
                        continue;
                    }

                    if (subsidiaryId > 0 && rowSubsidiryLId != subsidiaryId)
                    {
                        continue;
                    }

                    if (subSubControlLedgerId > 0 && rowSubSubControlLedgerId != subSubControlLedgerId)
                    {
                        continue;
                    }

                    decimal closingBalance = ToDecimal(row["ClosingBalance"]);
                    string balanceType = GetSafeString(row, "BalanceType");

                    decimal balanceDebit = GetBalanceDebit(balanceType, closingBalance);
                    decimal balanceCredit = GetBalanceCredit(balanceType, closingBalance);

                    // zero balance row skip korte chaile eta keep koro
                    if (balanceDebit == 0 && balanceCredit == 0)
                    {
                        continue;
                    }

                    rows.Add(new TrialBalanceTreeRow
                    {
                        AccountId = ToInt(row["AccountId"]),
                        AccountCode = GetSafeString(row, "AccountCode"),
                        AccountName = GetSafeString(row, "AccountName"),

                        GLHeadId = rowGLHeadId,
                        GLName = GetSafeString(row, "GLName"),

                        ControlLId = rowControlLId,
                        ControlLCode = GetSafeString(row, "ControlLCode"),
                        ControlLName = GetSafeString(row, "ControlLName"),

                        SubsidiryLId = rowSubsidiryLId,
                        SubControlCode = GetSafeString(row, "SubControlCode"),
                        SubsidiryLName = GetSafeString(row, "SubsidiryLName"),

                        SubSubControlLedgerId = rowSubSubControlLedgerId,
                        SubSubControlCode = GetSafeString(row, "SubSubControlCode"),
                        SubSubControlLedgerName = GetSafeString(row, "SubSubControlLedgerName"),

                        OpeningBalance = ToDecimal(row["OpeningBalance"]),

                        // Original period debit/credit
                        Debit = ToDecimal(row["Debit"]),
                        Credit = ToDecimal(row["Credit"]),

                        // Closing balance from SP
                        ClosingBalance = closingBalance,
                        BalanceType = balanceType,

                        // Final report display debit/credit
                        BalanceDebit = balanceDebit,
                        BalanceCredit = balanceCredit,

                        SideOrder = GetActualSideOrder(balanceDebit, balanceCredit)
                    });
                }

                // ===============================
                // Dr side first, Cr side later
                // ===============================
                rows = rows
                    .OrderBy(x => x.SideOrder)
                    .ThenBy(x => x.GLHeadId)
                    .ThenBy(x => x.ControlLCode)
                    .ThenBy(x => x.SubControlCode)
                    .ThenBy(x => x.SubSubControlCode)
                    .ThenBy(x => x.AccountCode)
                    .ToList();

                return rows;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }


        private decimal GetBalanceDebit(string balanceType, decimal closingBalance)
        {
            balanceType = (balanceType ?? string.Empty).Trim().ToLower();

            // Dr positive => Debit
            if (balanceType == "dr" && closingBalance >= 0)
            {
                return closingBalance;
            }

            // Cr negative => Debit
            if (balanceType == "cr" && closingBalance < 0)
            {
                return Math.Abs(closingBalance);
            }

            return 0;
        }

        private decimal GetBalanceCredit(string balanceType, decimal closingBalance)
        {
            balanceType = (balanceType ?? string.Empty).Trim().ToLower();

            // Cr positive => Credit
            if (balanceType == "cr" && closingBalance >= 0)
            {
                return closingBalance;
            }

            // Dr negative => Credit
            if (balanceType == "dr" && closingBalance < 0)
            {
                return Math.Abs(closingBalance);
            }

            return 0;
        }

        private int GetActualSideOrder(decimal balanceDebit, decimal balanceCredit)
        {
            if (balanceDebit > 0)
            {
                return 1; // Debit side first
            }

            if (balanceCredit > 0)
            {
                return 2; // Credit side later
            }

            return 3;
        }

        private List<BalanceSheetDynamicReportRow> FormatBalanceSheetRows(
            List<BalanceSheetDynamicReportRow> rows)
        {
            List<BalanceSheetDynamicReportRow> orderedRows = rows
                .GroupBy(x => x.Sl)
                .Select(g => g.First())
                .OrderBy(x => x.Sl)
                .ToList();

            List<BalanceSheetDynamicReportRow> finalRows =
                new List<BalanceSheetDynamicReportRow>();

            BalanceSheetDynamicReportRow totalLiabilityRow = orderedRows
                .FirstOrDefault(x =>
                    NormalizeText(x.Particular)
                    .Equals("Current liabilities", StringComparison.OrdinalIgnoreCase));

            decimal totalLiability = totalLiabilityRow != null
                ? totalLiabilityRow.Amount
                : 0;

            BalanceSheetDynamicReportRow totalEquityLiabilityRow = orderedRows
                .FirstOrDefault(x =>
                    NormalizeText(x.Particular)
                    .Equals("Total Equity & Liabilities", StringComparison.OrdinalIgnoreCase));

            finalRows.Add(new BalanceSheetDynamicReportRow
            {
                Sl = 0,
                DisplayOrder = 0,
                Particular = "Assets",
                Notes = string.Empty,
                Formula = string.Empty,
                IsBold = true,
                IsSectionHeader = true,
                GLName = string.Empty,
                Amount = 0
            });

            foreach (BalanceSheetDynamicReportRow row in orderedRows.Where(x => x.Sl >= 1 && x.Sl <= 12))
            {
                row.DisplayOrder = row.Sl;
                finalRows.Add(row);
            }

            finalRows.Add(new BalanceSheetDynamicReportRow
            {
                Sl = 0,
                DisplayOrder = 12.5m,
                Particular = "Liability",
                Notes = string.Empty,
                Formula = string.Empty,
                IsBold = true,
                IsSectionHeader = true,
                GLName = string.Empty,
                Amount = 0
            });

            foreach (BalanceSheetDynamicReportRow row in orderedRows.Where(x => x.Sl >= 13 && x.Sl <= 23))
            {
                row.DisplayOrder = row.Sl;
                finalRows.Add(row);
            }

            finalRows.Add(new BalanceSheetDynamicReportRow
            {
                Sl = 25,
                DisplayOrder = 23.5m,
                Particular = "Total Liability",
                Notes = string.Empty,
                Formula = "13",
                IsBold = true,
                IsSectionHeader = false,
                GLName = string.Empty,
                Amount = totalLiability
            });

            if (totalEquityLiabilityRow != null)
            {
                totalEquityLiabilityRow.DisplayOrder = 24;
                finalRows.Add(totalEquityLiabilityRow);
            }

            return finalRows
                .OrderBy(x => x.DisplayOrder)
                .ToList();
        }

        private void GetAllGLClosingBalance(
            int companyId,
            int financialYearId,
            DateTime fromDate,
            DateTime toDate,
            out Dictionary<string, decimal> balances,
            out Dictionary<string, string> glNames)
        {
            balances = new Dictionary<string, decimal>();
            glNames = new Dictionary<string, string>();

            DataAccessManager accessManager = new DataAccessManager();

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@CompanyId", companyId),
                    new SqlParameter("@FinancialYearId", financialYearId),
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                DataTable dt = accessManager.GetDataTable(
                    "sp_GET_AllGLClosingBalance",
                    parameters);

                foreach (DataRow row in dt.Rows)
                {
                    decimal closing = ToDecimal(row["ClosingBalance"]);
                    string glName = GetSafeString(row, "GLName");

                    AddBalanceAndName(
                        balances,
                        glNames,
                        "ChartOfAccount",
                        GetSafeString(row, "AccountCode"),
                        closing,
                        glName);

                    AddBalanceAndName(
                        balances,
                        glNames,
                        "SubSubControlLedger",
                        GetSafeString(row, "SubSubControlCode"),
                        closing,
                        glName);

                    AddBalanceAndName(
                        balances,
                        glNames,
                        "SubControlLedger",
                        GetSafeString(row, "SubControlCode"),
                        closing,
                        glName);

                    AddBalanceAndName(
                        balances,
                        glNames,
                        "ControlLedger",
                        GetSafeString(row, "ControlLCode"),
                        closing,
                        glName);
                }
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        private void AddBalanceAndName(
            Dictionary<string, decimal> balances,
            Dictionary<string, string> glNames,
            string codeType,
            string code,
            decimal closing,
            string glName)
        {
            code = (code ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            string key = GetFormulaKey(codeType, code);

            if (balances.ContainsKey(key))
            {
                balances[key] += closing;
            }
            else
            {
                balances[key] = closing;
            }

            if (!glNames.ContainsKey(key) && !string.IsNullOrWhiteSpace(glName))
            {
                glNames[key] = glName;
            }
        }

        private Dictionary<string, decimal> BuildFormulaTotalsFromGL(
            List<BalanceSheetSetupRow> setupRows,
            Dictionary<string, decimal> glClosingBalances,
            Dictionary<string, string> glNameMap,
            out Dictionary<string, string> formulaGlNames)
        {
            Dictionary<string, decimal> totals =
                new Dictionary<string, decimal>();

            formulaGlNames =
                new Dictionary<string, string>();

            foreach (BalanceSheetSetupRow row in setupRows.Where(x => !x.IsBold))
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

                    if (!formulaGlNames.ContainsKey(key) &&
                        glNameMap.ContainsKey(key))
                    {
                        formulaGlNames[key] = glNameMap[key];
                    }
                }
            }

            return totals;
        }

        private List<BalanceSheetSetupRow> GetBalanceSheetSetupRows(int companyId)
        {
            DataAccessManager accessManager = new DataAccessManager();

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@CompanyId", companyId)
                };

                DataTable dt = accessManager.GetDataTable(
                    "sp_GET_BalanceSheetDetailByCompanyId",
                    parameters);

                List<BalanceSheetSetupRow> rows =
                    new List<BalanceSheetSetupRow>();

                foreach (DataRow row in dt.Rows)
                {
                    rows.Add(new BalanceSheetSetupRow
                    {
                        OrderSl = ToInt(row["OrderSl"]),
                        Particular = GetSafeString(row, "Particular"),
                        COAFormula = GetSafeString(row, "COAFormula"),
                        Remarks = GetSafeString(row, "Remarks"),
                        IsBold = ToBool(row["IsBold"]),
                        IsControlLedgerCode = GetBool(row, "IsControlLedgerCode", true),
                        IsSubcontrolLedgerCode = GetBool(row, "IsSubcontrolLedgerCode", false),
                        IsSubSubControlLedgercode = GetBool(row, "IsSubSubControlLedgercode", false),
                        IsChartOfAccount = GetBool(row, "IsChartOfAccount", false)
                    });
                }

                return rows
                    .OrderBy(x => x.OrderSl)
                    .ToList();
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        private string GetSetupRowGlName(
            BalanceSheetSetupRow setupRow,
            Dictionary<string, string> formulaGlNames)
        {
            foreach (Match match in NumberTokenRegex.Matches(setupRow.COAFormula ?? string.Empty))
            {
                string key = GetFormulaKey(setupRow.CodeType, match.Value);

                if (formulaGlNames.ContainsKey(key))
                {
                    return formulaGlNames[key];
                }
            }

            return string.Empty;
        }

        private string GetReferencedRowGlName(
            string formula,
            Dictionary<int, string> rowGlNames)
        {
            foreach (Match match in NumberTokenRegex.Matches(formula ?? string.Empty))
            {
                int sl;

                if (int.TryParse(match.Value, out sl) &&
                    rowGlNames.ContainsKey(sl) &&
                    !string.IsNullOrWhiteSpace(rowGlNames[sl]))
                {
                    return rowGlNames[sl];
                }
            }

            return string.Empty;
        }

        private decimal GetSingleFormulaValue(
            string formula,
            Dictionary<string, decimal> values,
            string codeType)
        {
            string token = (formula ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                return 0;
            }

            string key = GetFormulaKey(codeType, token);

            if (values.ContainsKey(key))
            {
                return values[key];
            }

            return EvaluateFormula(formula, values, codeType);
        }

        private decimal EvaluateFormula(
            string formula,
            Dictionary<string, decimal> values,
            string codeType)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                return 0;
            }

            string expression = NumberTokenRegex.Replace(formula, match =>
            {
                string key = GetFormulaKey(codeType, match.Value);

                if (!values.ContainsKey(key))
                {
                    return "0";
                }

                return "(" + values[key].ToString(CultureInfo.InvariantCulture) + ")";
            });

            if (!AllowedExpressionRegex.IsMatch(expression))
            {
                return 0;
            }

            try
            {
                object result = new DataTable().Compute(expression, string.Empty);
                return Convert.ToDecimal(result, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        private bool TryEvaluateSlFormula(
            string formula,
            Dictionary<int, decimal> rowAmounts,
            out decimal amount)
        {
            amount = 0;

            if (string.IsNullOrWhiteSpace(formula))
            {
                return false;
            }

            bool hasSlReference = false;

            string expression = NumberTokenRegex.Replace(formula, match =>
            {
                int sl;

                if (int.TryParse(match.Value, out sl) &&
                    rowAmounts.ContainsKey(sl))
                {
                    hasSlReference = true;
                    return "(" + rowAmounts[sl].ToString(CultureInfo.InvariantCulture) + ")";
                }

                return "0";
            });

            if (!hasSlReference || !AllowedExpressionRegex.IsMatch(expression))
            {
                return false;
            }

            try
            {
                object result = new DataTable().Compute(expression, string.Empty);
                amount = Convert.ToDecimal(result, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                amount = 0;
                return true;
            }
        }

        private string GetFormulaKey(string codeType, string formulaCode)
        {
            return (string.IsNullOrWhiteSpace(codeType) ? "ControlLedger" : codeType)
                   + "|"
                   + (formulaCode ?? string.Empty).Trim();
        }

        private string NormalizeText(string text)
        {
            return Regex.Replace((text ?? string.Empty).Trim(), @"\s+", " ");
        }

        private string GetSafeString(DataRow row, string columnName)
        {
            if (row == null ||
                row.Table == null ||
                !row.Table.Columns.Contains(columnName) ||
                row[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return row[columnName].ToString().Trim();
        }

        private int ToFilterInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            int parsed;
            return int.TryParse(value.Trim(), out parsed) ? parsed : 0;
        }

       

        private int ToInt(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            int parsed;
            return int.TryParse(value.ToString(), out parsed) ? parsed : 0;
        }

        private decimal ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            decimal parsed;

            return decimal.TryParse(
                value.ToString(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out parsed)
                ? parsed
                : 0;
        }

        private bool ToBool(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool)value;
            }

            string text = value.ToString().Trim();

            if (text == "1")
            {
                return true;
            }

            if (text == "0")
            {
                return false;
            }

            bool parsed;
            return bool.TryParse(text, out parsed) && parsed;
        }

        private bool GetBool(DataRow row, string columnName, bool defaultValue)
        {
            if (row == null ||
                row.Table == null ||
                !row.Table.Columns.Contains(columnName) ||
                row[columnName] == DBNull.Value)
            {
                return defaultValue;
            }

            return ToBool(row[columnName]);
        }

        private class BalanceSheetSetupRow
        {
            public int OrderSl { get; set; }
            public string Particular { get; set; }
            public string COAFormula { get; set; }
            public string Remarks { get; set; }
            public bool IsBold { get; set; }
            public bool IsControlLedgerCode { get; set; }
            public bool IsSubcontrolLedgerCode { get; set; }
            public bool IsSubSubControlLedgercode { get; set; }
            public bool IsChartOfAccount { get; set; }

            public string CodeType
            {
                get
                {
                    if (IsSubcontrolLedgerCode)
                    {
                        return "SubControlLedger";
                    }

                    if (IsSubSubControlLedgercode)
                    {
                        return "SubSubControlLedger";
                    }

                    if (IsChartOfAccount)
                    {
                        return "ChartOfAccount";
                    }

                    return "ControlLedger";
                }
            }
        }
    }

    public class BalanceSheetDynamicReportRow
    {
        public int Sl { get; set; }

        public decimal DisplayOrder { get; set; }

        public string Particular { get; set; }

        public string Notes { get; set; }

        public string Formula { get; set; }

        public bool IsBold { get; set; }

        public bool IsSectionHeader { get; set; }

        public string GLName { get; set; }

        public decimal Amount { get; set; }
    }


    public class TrialBalanceTreeRow
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        public int GLHeadId { get; set; }
        public string GLName { get; set; }

        public int ControlLId { get; set; }
        public string ControlLCode { get; set; }
        public string ControlLName { get; set; }

        public int SubsidiryLId { get; set; }
        public string SubControlCode { get; set; }
        public string SubsidiryLName { get; set; }

        public int SubSubControlLedgerId { get; set; }
        public string SubSubControlCode { get; set; }
        public string SubSubControlLedgerName { get; set; }

        public decimal OpeningBalance { get; set; }

        // Original period debit/credit
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public decimal ClosingBalance { get; set; }
        public string BalanceType { get; set; }

        // Final display amount
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }

        public int SideOrder { get; set; }
    }
}
