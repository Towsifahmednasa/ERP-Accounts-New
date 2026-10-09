// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\BanckAccountDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class BanckAccountDAL
    {

        DataAccessManager _accessManager = new DataAccessManager();

        public ResultResponse SaveBankAccount(BankAccountDAO aMaster, int user)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {            
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@BankAccountId", aMaster.BankAccountId));
                aParameters.Add(new SqlParameter("@CompanyId", aMaster.CompanyId ?? (object)DBNull.Value));             
                aParameters.Add(new SqlParameter("@BankId", aMaster.BankId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@BranchName", aMaster.BranchName ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@AccountTypeId", aMaster.AccountTypeId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@CurrencyId", aMaster.CurrencyId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@AccountName", aMaster.AccountName ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@AccountNo", aMaster.AccountNo ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@BankAccountRouteNumber", aMaster.BankAccountRouteNumber ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@OpeningDate", aMaster.OpeningDate ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@BankAddress", aMaster.BankAddress ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@Description", aMaster.Description ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@SubsidiryLId", aMaster.SubsidiryLId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@Entryby", user));
                aParameters.Add(new SqlParameter("@EntryDate", System.DateTime.Now));
                if (aMaster.BankAccountId > 0)
                {
                    List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                    aSqlParameterList.Add(new SqlParameter("@id", aMaster.BankAccountId));
                    aSqlParameterList.Add(new SqlParameter("@Type", "Bank"));
                    aSqlParameterList.Add(new SqlParameter("@AccountNo", aMaster.AccountNo ?? (object)DBNull.Value));

                    DataTable dt = _accessManager.GetDataTable("sp_check_BankCashUp", aSqlParameterList);
                    if (dt.Rows.Count == 0)
                    {
                        aResponse.isSuccess = _accessManager.UpdateData("sp_UD_BankAccount_New", aParameters);
                    }
                    else
                    {
                        aResponse.pk = 0;
                    }
                    if (aResponse.isSuccess)
                    {                
                        List<SqlParameter> aParaChart = new List<SqlParameter>();
                        aParaChart.Add(new SqlParameter("@Description", (object)(aMaster. AccountName ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@AccountName", aMaster.AccountNo));
                        aParaChart.Add(new SqlParameter("@SubsidiaryLId", aMaster.SubsidiryLId));
                        aParaChart.Add(new SqlParameter("@CompanyId", (object)(aMaster.CompanyId ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@CreateBy", user));
                        aParaChart.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        aParaChart.Add(new SqlParameter("@CashAccountId", null ?? (object)DBNull.Value));
                        aParaChart.Add(new SqlParameter("@BankAccountId", (object)(aMaster.BankAccountId)));
                        aResponse.isSuccess = _accessManager.UpdateData("sp_UD_ChartOfAccount_New", aParaChart);
                        if (aResponse.isSuccess)
                        {
                            aResponse.pk = aMaster.BankAccountId;                        
                        }                     
                    }
                }
                else
                {
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_BankAccount_New", aParameters);
                    if (aResponse.pk > 0)
                    {
                        List<SqlParameter> aParaChart = new List<SqlParameter>();
                        aParaChart.Add(new SqlParameter("@Description", (object)(aMaster.AccountName ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@AccountName", aMaster.AccountNo));

                        aParaChart.Add(new SqlParameter("@SubsidiaryLId", aMaster.SubsidiryLId));
                        aParaChart.Add(new SqlParameter("@CompanyId", (object)(aMaster.CompanyId ?? (object)DBNull.Value)));
                        aParaChart.Add(new SqlParameter("@CreateBy", user));
                        aParaChart.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        aParaChart.Add(new SqlParameter("@CashAccountId", null ?? (object)DBNull.Value));
                        aParaChart.Add(new SqlParameter("@BankAccountId", (object)(aResponse.pk)));
                        aResponse.isSuccess = _accessManager.SaveData("sp_I_ChartOfAccount_New", aParaChart);    
                        


                    }
                    else
                    {
                        _accessManager.SqlConnectionClose(true);
                    }
                }
                if (aResponse.pk > 0)
                {
                    int[] idS = aMaster.BusinessUnit.Split(',').Select(Int32.Parse).ToArray();
                    for (int i = 0; i < idS.Length; i++)
                    {
                        int aId = idS[i];
                        List<SqlParameter> aSQL = new List<SqlParameter>();
                        aSQL.Add(new SqlParameter("@BankAccountId", aResponse.pk));
                        aSQL.Add(new SqlParameter("@CompanyUnitId", aId));
                        aResponse.isSuccess = _accessManager.SaveData("sp_I_BankAccount_Unit", aSQL);
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

        public List<BankAccountView> BankAccountList(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
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
                List<BankAccountView> aList = new List<BankAccountView>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@AccountName", CompanyId));
                //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_AllBankAccount", aParameters);
                while (dr.Read())
                {
                    BankAccountView aMaster = new BankAccountView();
                    aMaster.BankAccountId = Convert.ToInt32(dr["BankAccountId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.BussinessUnit = dr["BussinessUnit"].ToString();
                    aMaster.BankName = dr["BankName"].ToString();
                    aMaster.AccountName = (dr["AccountName"].ToString());
                    aMaster.AccountNo = (dr["AccountNo"].ToString());
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

        public BankAccountDAO GetBankAccount_ById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                BankAccountDAO aMaster = new BankAccountDAO();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@BankAccountId", id));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BankAccountById", aSqlParameters);
                while (dr.Read())
                {
                    
                    aMaster.BankAccountId = Convert.ToInt32(dr["BankAccountId"]);
        
                    aMaster.BankId = Convert.ToInt32(dr["BankId"]);

                    aMaster.AccountName = dr["AccountName"].ToString();

                    aMaster.BusinessUnit = dr["BusinessUnit"].ToString();
              
                    aMaster.Description = dr["Description"].ToString();

                    aMaster.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);

                    aMaster.SubsidiryLId = Convert.ToInt32(dr["SubsidiryLId"]);
                    aMaster.SubSubControlLedgerId = Convert.ToInt32(dr["SubSubControlLedgerId"]);

                    aMaster.ControlLId = Convert.ToInt32(dr["ControlLId"]);

                    aMaster.GLHeadId = Convert.ToInt32(dr["GLHeadId"]);

                    aMaster.AccountNo = dr["AccountNo"].ToString();

                   // aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"]);

                    aMaster.BranchName = dr["BranchName"].ToString();

                    aMaster.AccountTypeId = Convert.ToInt32(dr["AccountTypeId"]);

                    aMaster.BankAccountRouteNumber = dr["BankAccountRouteNumber"].ToString();

                    aMaster.RE_OpeningDate =dr["OpeningDate"].ToString();

                    aMaster.BankAddress = dr["BankAddress"].ToString();

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