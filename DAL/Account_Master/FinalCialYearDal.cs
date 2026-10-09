// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\FinalCialYearDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts_Master_M;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Account_Master
{
    public class FinalCialYearDal
    {


        private DataAccessManager accessManager = new DataAccessManager();
        public ResultResponse Save_CostCenter(FinancialYearDao aFinancialYearEntry)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", aFinancialYearEntry.FinancialYearId));
                aSqlParameterlist.Add(new SqlParameter("@StartDate", aFinancialYearEntry.StartDate));
                aSqlParameterlist.Add(new SqlParameter("@EndDate", aFinancialYearEntry.EndDate));
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", aFinancialYearEntry.CompanyId));
                if(aFinancialYearEntry.Status== "Active")
                {
                    aSqlParameterlist.Add(new SqlParameter("@ActiveDate", DateTime.Now));
                    aSqlParameterlist.Add(new SqlParameter("@InActiveDate", DBNull.Value));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@ActiveDate", DBNull.Value));
                    aSqlParameterlist.Add(new SqlParameter("@InActiveDate", DateTime.Now));

                }
                aSqlParameterlist.Add(new SqlParameter("@Status", aFinancialYearEntry.Status));
                if (aFinancialYearEntry.FinancialYearId > 0)
                {                   
                    aSqlParameterlist.Add(new SqlParameter("@UpdateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));

                    aResponse.isSuccess = accessManager.UpdateData("sp_UD_FinancialYear_New", aSqlParameterlist);
                    aResponse.pk = aFinancialYearEntry.FinancialYearId;
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_FinancialYear_New", aSqlParameterlist);
                    if (aResponse.pk > 0)
                    {
                        aResponse.isSuccess = true;
                    }
                    if (aResponse.pk == -1)
                    {
                        aResponse.isSuccess = false;
                        aResponse.msg = "Already Exist!";
                    }

                    if (aResponse.pk == -2)
                    {
                        aResponse.isSuccess = false;
                        aResponse.msg = "The financial year is currently active. You cannot insert new records until the financial year is closed or set to inactive for this company.";
                    }
                }
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                aResponse.isSuccess = false;
                aResponse.msg = exception.Message;

                throw exception;
            }
            finally
            {

                accessManager.SqlConnectionClose();
            }

            return aResponse;
        }


        public ResultResponse Lock_FinancialYear (FinancialYearDao aFinancialYearEntry)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", aFinancialYearEntry.FinancialYearId));
           
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", aFinancialYearEntry.CompanyId));
                
                 
                    aSqlParameterlist.Add(new SqlParameter("@UpdateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));

                    aResponse.isSuccess = accessManager.UpdateData("sp_Lock_FinancialYear_New", aSqlParameterlist);
                    aResponse.pk = aFinancialYearEntry.FinancialYearId;
                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = true;
                }
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                aResponse.isSuccess = false;
                aResponse.msg = exception.Message;

                throw exception;
            }
            finally
            {

                accessManager.SqlConnectionClose();
            }

            return aResponse;
        }

        public List<FinancialYearDao> GetCostcenter(string param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<FinancialYearDao> aList = new List<FinancialYearDao>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@FinancialCode", param));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_FinancialYear", aParameters);
                while (dr.Read())
                {
                    FinancialYearDao aMaster = new FinancialYearDao();
                    aMaster.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster._StartDate = dr["StartDate"].ToString();
                    aMaster._EndDate = dr["EndDate"].ToString();
                    aMaster._ActiveDate = dr["ActiveDate"].ToString();
                    aMaster.FinancialYearDesc = dr["FinancialYearDesc"].ToString();
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.EntryBy = dr["EntryBy"].ToString();
                    aMaster.EntryDateStr = dr["EntryDateStr"].ToString();
                    aMaster.Status = dr["Status"].ToString();
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
                accessManager.SqlConnectionClose();
            }
        }


        public FinancialYearDao GetCostCentre_ById(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                FinancialYearDao aMaster = new FinancialYearDao();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@FinancialYearId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_FinancialYearById", aSqlParameters);
                while (dr.Read())
                {
                    aMaster.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                    aMaster._StartDate = dr["StartDate"].ToString();
                    aMaster._EndDate = dr["EndDate"].ToString();
                    aMaster.Status = dr["Status"].ToString();
                }
                return aMaster;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }


    }
}