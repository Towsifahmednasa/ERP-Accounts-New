// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\ControlLedgerDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class ControlLedgerDal
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

        public DataTable Get_ControlLedger(string search)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ControlLName", search));
                return accessManager.GetDataTable("sp_GET_ControllLadger", aSqlParameterList);
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

        public DataTable Get_ControlLedger_ById(int search)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ControlLId", search));
                return accessManager.GetDataTable("sp_GET_ControllLadgerById", aSqlParameterList);
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

        public ResultResponse SaveEmployeeInformation(ControlLedger employee)
        {
            int pk = 0;
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
         
                gSqlParameterList.Add(new SqlParameter("@ControlLId", employee.ControlLId));
                gSqlParameterList.Add(new SqlParameter("@GLHeadId", employee.GLHeadId));
                gSqlParameterList.Add(new SqlParameter("@ControlLName", employee.ControlLName));
                gSqlParameterList.Add(new SqlParameter("@InBalanceSheet", (object)employee.InBalanceSheet ?? DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@InIncomeStatement", (object)employee.InIncomeStatement ?? DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@IsActive", employee.IsActive));

                if (employee.ControlLId > 0)
                {
                    //aSqlParameterList.Add(new SqlParameter("@id", employee.EmpInfoId));
                    //aSqlParameterList.Add(new SqlParameter("@Name", employee.EmpMasterCode));
                    //DataTable dt = accessManager.GetDataTable("sp_check_EmployeeInformation", aSqlParameterList);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    gSqlParameterList.Add(new SqlParameter("@UpdateBy", sessionUser));
                    //    aInformation.isSuccess = accessManager.UpdateData("sp_Update_EmployeeInformation", gSqlParameterList);
                    //}
                    //else
                    //{
                    //    aInformation.isSuccess = false;
                    //}

                    gSqlParameterList.Add(new SqlParameter("@UpdateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                    aResponse.isSuccess = accessManager.UpdateData("sp_UD_ControllLadger", gSqlParameterList);
                    aResponse.pk = employee.ControlLId;
                }
                else
                {
                    gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                    aResponse.pk =   accessManager.SaveDataReturnPrimaryKey("sp_I_ControllLadger", gSqlParameterList);
                    if (pk > 0)
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


    }
}