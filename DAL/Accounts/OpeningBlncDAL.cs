// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\OpeningBlncDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class OpeningBlncDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();
        public ResultResponse Save_OpningBlnc(OpeningBlncDAO aOpeningBalance)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

               

                aSqlParameterlist.Add(new SqlParameter("@OpeningBalanceId", aOpeningBalance.OpeningBalanceId));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)aOpeningBalance.FinancialYearId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@ChartOfAccountId", (object)aOpeningBalance.ChartOfAccountId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Amount", (object)aOpeningBalance.Amount ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Currency", (object)aOpeningBalance.Currency ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)aOpeningBalance.CompanyId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@DRorCR", (object)aOpeningBalance.DRorCR ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Remarks", (object)aOpeningBalance.Remarks ?? DBNull.Value));


                if (aOpeningBalance.OpeningBalanceId > 0)
                {
                    // DataTable dt = accessManager.GetDataTable("sp_check_SubsidairyType", aSqlParameterlist);

                    //if (dt.Rows.Count == 0)
                    //{
                    //    gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                    //    aResponse.isSuccess = accessManager.UpdateData("sp_UD_SubsidairyType", gSqlParameterList);
                    //    aResponse.pk = employee.SubsidiaryTypeId;
                    //}
                    //else
                    //{
                    //    aResponse.isSuccess = false;
                    //}

                    aSqlParameterlist.Add(new SqlParameter("@UpdateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
     
                    aResponse.isSuccess = _accessManager.UpdateData("sp_UD_OpeningBalance", aSqlParameterlist);
                    aResponse.pk = aOpeningBalance.OpeningBalanceId;

                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));



                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_OpeningBalance", aSqlParameterlist);
                    if (aResponse.pk > 0)
                    {
                        aResponse.isSuccess = true;
                    }
                }
            }
            catch (Exception exception)
            {
                _accessManager.SqlConnectionClose(true);
                aResponse.isSuccess = false;
                aResponse.msg = exception.Message;

                throw exception;
            }
            finally
            {

                _accessManager.SqlConnectionClose();
            }

            return aResponse;
        }


        public List<OpeningBlncListDAO> Get_OpningBlncList(string companyID, string Yearid)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<OpeningBlncListDAO> aList = new List<OpeningBlncListDAO>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@companyID", companyID));
                aParameters.Add(new SqlParameter("@Yearid", Yearid));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_AllOpeningBalanceCompanyandYearwise", aParameters);
                while (dr.Read())
                {
                    OpeningBlncListDAO aMaster = new OpeningBlncListDAO();
                    aMaster.OpeningBalanceId = Convert.ToInt32(dr["OpeningBalanceId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.FinancialYearDesc = dr["FinancialYearDesc"].ToString();
                    aMaster.AccountName = dr["AccountName"].ToString();
                    aMaster.CurrencyName = dr["CurrencyName"].ToString();
                    aMaster.Remarks = dr["Remarks"].ToString();

                    aMaster.Amount = dr["Amount"].ToString();
                    aMaster.EntryBy = dr["EntryBy"].ToString();
                    aMaster.EntryDate = dr["EntryDate"].ToString();

                    aMaster.UpdateBy = dr["UpdateBy"].ToString();
                    aMaster.UpdateDate = dr["UpdateDate"].ToString();

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


        public OpeningBlncDAO Get_OpningBlnc_ById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                OpeningBlncDAO aMaster = new OpeningBlncDAO();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@OpeningBalanceId", id));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_AllOpeningBalanceById", aSqlParameters);
                while (dr.Read())
                {
                    aMaster.OpeningBalanceId = Convert.ToInt32(dr["OpeningBalanceId"]);
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                    aMaster.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"].ToString());
                    aMaster.ChartOfAccountId = Convert.ToInt32(dr["ChartOfAccountId"].ToString());
                    aMaster.CurrencyId = Convert.ToInt32(dr["CurrencyId"].ToString());
                    aMaster.Amount = Convert.ToDecimal(dr["Amount"].ToString());
                    aMaster.Remarks = dr["Remarks"].ToString();
                     

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