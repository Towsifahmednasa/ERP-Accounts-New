// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\CashAccountDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class CashAccountDal
    {
        DataAccessManager _accessManager = new DataAccessManager();
        public ResultResponse SaveCashAccount(CashAccount aCashAccount, int user)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@CashAccountId", aCashAccount.CashAccountId));
                aParameters.Add(new SqlParameter("@AccountName", aCashAccount.AccountName));
                aParameters.Add(new SqlParameter("@Description", aCashAccount.Description));
                aParameters.Add(new SqlParameter("@SubsidiryLId",aCashAccount.SubsidiryLId));
                aParameters.Add(new SqlParameter("@Currency",    aCashAccount.Currency));
                aParameters.Add(new SqlParameter("@Entryby", user));
                aParameters.Add(new SqlParameter("@EntryDate", System.DateTime.Now));
                if (aCashAccount.CashAccountId > 0)
                {

                    List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                    aSqlParameterList.Add(new SqlParameter("@id", aCashAccount.CashAccountId));
                    aSqlParameterList.Add(new SqlParameter("@Type", "Cash"));
                    aSqlParameterList.Add(new SqlParameter("@AccountNo", aCashAccount.AccountName ?? (object)DBNull.Value));

                    DataTable dt = _accessManager.GetDataTable("sp_check_BankCashUp", aSqlParameterList);
                    if (dt.Rows.Count == 0)
                    {
                        aResponse.isSuccess = _accessManager.UpdateData("sp_UD_CashAccount_New", aParameters);
                    }
                    else
                    {
                        aResponse.isSuccess = false;
                        aResponse.pk = 0;
                    }
                    if (aResponse.isSuccess)
                    {
                        List<SqlParameter> aParaChart = new List<SqlParameter>();
                        aParaChart.Add(new SqlParameter("@Description", (object)(aCashAccount.Description ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@AccountName", aCashAccount.AccountName ));
                        aParaChart.Add(new SqlParameter("@SubsidiaryLId", aCashAccount.SubsidiryLId));
                        aParaChart.Add(new SqlParameter("@CompanyId", (object)(aCashAccount.CompanyId ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@CreateBy", user));
                        aParaChart.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        aParaChart.Add(new SqlParameter("@CashAccountId", aCashAccount.CashAccountId));
                        aParaChart.Add(new SqlParameter("@BankAccountId", null ?? (object)DBNull.Value));
                        aResponse.isSuccess = _accessManager.UpdateData("sp_UD_ChartOfAccount_New", aParaChart);
                        if (aResponse.isSuccess)
                        {
                            aResponse.pk = aCashAccount.CashAccountId;
                        }
                    }
                }
                else
                {
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_CashAccount_New", aParameters);
                    if (aResponse.pk > 0)
                    {
                        List<SqlParameter> aParaChart = new List<SqlParameter>();
                        aParaChart.Add(new SqlParameter("@Description", (object)(aCashAccount.Description ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@AccountName", aCashAccount.AccountName));
                        aParaChart.Add(new SqlParameter("@SubsidiaryLId", aCashAccount.SubsidiryLId));
                        aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)(aCashAccount.CompanyId ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@CreateBy", user));
                        aParaChart.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        aParaChart.Add(new SqlParameter("@CashAccountId", (object)(aResponse.pk)));
                        aParaChart.Add(new SqlParameter("@BankAccountId", null ?? (object)DBNull.Value)); 
                        aResponse.isSuccess = _accessManager.SaveData("sp_I_ChartOfAccount_New", aParaChart);
                    }
                    else
                    {
                        _accessManager.SqlConnectionClose(true);
                    }
                }
                if (aResponse.pk > 0)
                {
                    int[] idS = aCashAccount.CompanyName.Split(',').Select(Int32.Parse).ToArray();
                    for (int i = 0; i < idS.Length; i++)
                    {
                        int aId = idS[i];
                        List<SqlParameter> aSQL = new List<SqlParameter>();
                        aSQL.Add(new SqlParameter("@CashAccountId", aResponse.pk));
                        aSQL.Add(new SqlParameter("@CompanyId", aId));
                        aResponse.isSuccess = _accessManager.SaveData("sp_I_MultipleCompanySameCashAccount_New", aSQL);
                    }
                    aResponse.isSuccess = true;
                }
                else
                {
                    aResponse.pk = 0;
                    aResponse.isSuccess = false;
                    aResponse.msg = "Already Exist!";

                }
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

            return aResponse;
        }

        public List<CashAccountView> CashAccountList(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
        {
            try
            {
                if (fromdate == null)
                {
                    fromdate = System.DateTime.Now.AddDays(-1);
                }
                if (todate == null)
                {
                    todate = System.DateTime.Now;
                }
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<CashAccountView> aList = new List<CashAccountView>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@AccountName", CompanyId));
                //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_AllCashAccount", aParameters);
                while (dr.Read())
                {
                    CashAccountView aMaster = new CashAccountView();
                    aMaster.CashAccountId = Convert.ToInt32(dr["CashAccountId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.AccountName = (dr["AccountName"].ToString());
                    aMaster.Description = (dr["Description"].ToString());
                    aMaster.GLName = (dr["GLName"].ToString());
                    aMaster.ControlLName = (dr["ControlLName"].ToString());
                    aMaster.SubsidiryLName = (dr["SubsidiryLName"].ToString());
                    aMaster.SubSubControlLedgerName = (dr["SubSubControlLedgerName"].ToString());

                    aList.Add(aMaster);
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

        public CashAccount GetCashAccount_ById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                CashAccount aMaster = new CashAccount();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@CashAccountId", id));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_CashAccountById_new", aSqlParameters);
                while (dr.Read())
                {

                    aMaster.CashAccountId = Convert.ToInt32(dr["CashAccountId"]);
                    aMaster.AccountName = dr["AccountName"].ToString();
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.Description = dr["Description"].ToString();
                    aMaster.Currency = Convert.ToInt32(dr["CurrencyId"]);
                    aMaster.SubSubControlLedgerId = Convert.ToInt32(dr["SubSubControlLedgerId"]);
                    aMaster.SubsidiryLId = Convert.ToInt32(dr["SubsidiryLId"]);
                    aMaster.ControlLId = Convert.ToInt32(dr["ControlLId"]);
                    aMaster.GLHeadId = Convert.ToInt32(dr["GLHeadId"]);       
                }
                return aMaster;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }
    }
}