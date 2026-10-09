// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\BalanceSheetDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace New_Accounts.DAL.Accounts
{
    public class BalanceSheetDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();

        public DataTable GetBalanceSheetReport(
            int CompanyID,
            int FinancialYearID,
            DateTime FromDate,
            DateTime ToDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> parameterList = new List<SqlParameter>();

                parameterList.Add(new SqlParameter("@CompanyID", CompanyID));
                parameterList.Add(new SqlParameter("@FinancialYearID", FinancialYearID));
                parameterList.Add(new SqlParameter("@FromDate", FromDate));
                parameterList.Add(new SqlParameter("@ToDate", ToDate));

                DataTable dt = _accessManager.GetDataTable(
                    "sp_Rpt_BalanceSheet",
                    parameterList);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public ResultResponse SaveBalanceSheet(BalanceSheetMaster aMaster, int userId)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                // ── Step 1: Get existing MasterId (আলাদা connection এ) ──
                int existingMasterId = GetMasterIdByCompanyId(aMaster.CompanyId ?? 0);

                // ── Step 2: Main connection open ──
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                if (existingMasterId > 0)
                {
                    // UPDATE Master
                    List<SqlParameter> updateParams = new List<SqlParameter>();
                    updateParams.Add(new SqlParameter("@IncomeStatementId", existingMasterId));
                    updateParams.Add(new SqlParameter("@UpdateBy", userId));
                    updateParams.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    _accessManager.UpdateData("sp_U_BalanceSheetMaster", updateParams);

                    // DELETE all existing Details
                    List<SqlParameter> deleteParams = new List<SqlParameter>();
                    deleteParams.Add(new SqlParameter("@IncomeStatementId", existingMasterId));
                    _accessManager.SaveData("sp_D_BalanceSheetDetailByMasterId", deleteParams);

                    aResponse.pk = existingMasterId;
                }
                else
                {
                    // INSERT new Master
                    List<SqlParameter> insertParams = new List<SqlParameter>();
                    insertParams.Add(new SqlParameter("@CompanyId", aMaster.CompanyId ?? (object)DBNull.Value));
                    insertParams.Add(new SqlParameter("@EntryBy", userId));
                    insertParams.Add(new SqlParameter("@EntryDate", DateTime.Now));
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_BalanceSheetMaster", insertParams);
                }

                if (aResponse.pk <= 0)
                {
                    _accessManager.SqlConnectionClose(true);
                    aResponse.isSuccess = false;
                    return aResponse;
                }

                // INSERT all Details fresh
                aResponse.isSuccess = SaveDetails(aMaster.Details, aResponse.pk);

                if (!aResponse.isSuccess)
                {
                    _accessManager.SqlConnectionClose(true);
                }

                return aResponse;
            }
            catch (Exception ex)
            {
                _accessManager.SqlConnectionClose(true);
                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        // ─────────────────────────────────────────────────────────
        // INSERT Details
        // ─────────────────────────────────────────────────────────
        private bool SaveDetails(List<BalanceSheetDetail> details, int masterId)
        {
            try
            {
                bool result = true;

                foreach (var item in details)
                {
                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@IncomeStatementId", masterId));
                    p.Add(new SqlParameter("@OrderSl", (object)(item.OrderSl ?? (object)DBNull.Value)));
                    p.Add(new SqlParameter("@Particular", (object)(item.Particular ?? (object)DBNull.Value)));
                    p.Add(new SqlParameter("@COAFormula", (object)(item.COAFormula ?? (object)DBNull.Value)));
                    p.Add(new SqlParameter("@Remarks", (object)(item.Remarks ?? (object)DBNull.Value)));
                    p.Add(new SqlParameter("@IsBold", (object)(item.IsBold ?? (object)DBNull.Value)));
                    p.Add(new SqlParameter("@IsControlLedgerCode", item.IsControlLedgerCode ?? true));
                    p.Add(new SqlParameter("@IsSubcontrolLedgerCode", item.IsSubcontrolLedgerCode ?? false));
                    p.Add(new SqlParameter("@IsSubSubControlLedgercode", item.IsSubSubControlLedgercode ?? false));
                    p.Add(new SqlParameter("@IsChartOfAccount", item.IsChartOfAccount ?? false));

                    result = _accessManager.SaveData("sp_I_BalanceSheetDetail", p);

                    if (!result) break;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ─────────────────────────────────────────────────────────
        // GET Master Id by CompanyId
        // ─────────────────────────────────────────────────────────
        private int GetMasterIdByCompanyId(int companyId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CompanyId", companyId));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BalanceSheetByCompanyId", p);

                int masterId = 0;
                if (dr.Read())
                {
                    masterId = Convert.ToInt32(dr["IncomeStatementId"]);
                }
                return masterId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        // ─────────────────────────────────────────────────────────
        // GET Details by CompanyId (for loading in View)
        // ─────────────────────────────────────────────────────────
        public List<BalanceSheetDetail> GetDetailsByCompanyId(int companyId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<BalanceSheetDetail> aList = new List<BalanceSheetDetail>();
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CompanyId", companyId));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BalanceSheetDetailByCompanyId", p);

                while (dr.Read())
                {
                    BalanceSheetDetail detail = new BalanceSheetDetail();
                    detail.BalanceSheetDetailId = Convert.ToInt32(dr["IncomeStatementDetailId"]);
                    detail.BalanceSheetId = Convert.ToInt32(dr["IncomeStatementId"]);
                    detail.OrderSl = DBNull.Value.Equals(dr["OrderSl"]) ? 0 : Convert.ToInt32(dr["OrderSl"]);
                    detail.Particular = dr["Particular"].ToString();
                    detail.COAFormula = dr["COAFormula"].ToString();
                    detail.Remarks = dr["Remarks"].ToString();
                    detail.IsBold = DBNull.Value.Equals(dr["IsBold"]) ? false : Convert.ToBoolean(dr["IsBold"]);
                    detail.IsControlLedgerCode = HasColumn(dr, "IsControlLedgerCode") && !DBNull.Value.Equals(dr["IsControlLedgerCode"])
                        ? Convert.ToBoolean(dr["IsControlLedgerCode"])
                        : true;
                    detail.IsSubcontrolLedgerCode = HasColumn(dr, "IsSubcontrolLedgerCode") && !DBNull.Value.Equals(dr["IsSubcontrolLedgerCode"])
                        ? Convert.ToBoolean(dr["IsSubcontrolLedgerCode"])
                        : false;
                    detail.IsSubSubControlLedgercode = HasColumn(dr, "IsSubSubControlLedgercode") && !DBNull.Value.Equals(dr["IsSubSubControlLedgercode"])
                        ? Convert.ToBoolean(dr["IsSubSubControlLedgercode"])
                        : false;
                    detail.IsChartOfAccount = HasColumn(dr, "IsChartOfAccount") && !DBNull.Value.Equals(dr["IsChartOfAccount"])
                        ? Convert.ToBoolean(dr["IsChartOfAccount"])
                        : false;
                    aList.Add(detail);
                }

                return aList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        private bool HasColumn(IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
