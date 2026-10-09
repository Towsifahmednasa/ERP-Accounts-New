// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\SubcontrolLedger.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts_Master_M;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Account_Master
{
    public class SubcontrolLedger
    {

        private DataAccessManager accessManager = new DataAccessManager();

        public DataTable Load_Ledger_DDl()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                return accessManager.GetDataTable("sp_GET_LadgerHeadDropDown");
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


        public DataTable Load_ControlLedger_By_Ledger(string Id, string mode)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@GLHeadId", Id));
                //gSqlParameterList.Add(new SqlParameter("@mode", mode));
                return accessManager.GetDataTable("sp_GET_ControllLadgerDropDown", gSqlParameterList);
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

        public DataTable GET_SubControll_List(string Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@SubsidiryLName", Id));
                return accessManager.GetDataTable("sp_GET_SubsidiryLadger", gSqlParameterList);
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

        public ResultResponse SaveSubcontrolInformation(SubControlLedger employee)
        {

            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;
                gSqlParameterList.Add(new SqlParameter("@SubsidiryLId", employee.SubsidiryLId));
                gSqlParameterList.Add(new SqlParameter("@SubsidiryLName", employee.SubsidiryLName));
                gSqlParameterList.Add(new SqlParameter("@ControlLId", employee.ControlLId));
                gSqlParameterList.Add(new SqlParameter("@InIncomeStatement", (object)employee.InIncomeStatement ?? DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@InBalanceSheet", (object)employee.InBalanceSheet ?? DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@IsActive", employee.IsActive));
                if (employee.SubsidiryLId > 0)
                {
                       //HttpContext.Current.Session["LoginUserName"].ToString();
                        gSqlParameterList.Add(new SqlParameter("@UpdateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                    aResponse.isSuccess = accessManager.UpdateData("sp_UD_SubsidiryLadger", gSqlParameterList);
                    aResponse.pk = employee.SubsidiryLId;
                }
                else
                {
                    gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_SubsidiryLadgerHead", gSqlParameterList);
                    if (aResponse.pk > 0)
                    {
                        aResponse.isSuccess = true;
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

        public DataTable Load_SubControlLedger_ById(int Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@SubsidiryLId", Id));
                return accessManager.GetDataTable("sp_GET_SubsidiryLadgerById", gSqlParameterList);
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
    }
}