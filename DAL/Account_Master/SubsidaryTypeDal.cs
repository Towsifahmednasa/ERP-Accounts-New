// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\SubsidaryTypeDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class SubsidaryTypeDal
    {
        private DataAccessManager accessManager = new DataAccessManager();
        public ResultResponse Save_SubsidairyTypeType(Subsidarytype employee)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();

                gSqlParameterList.Add(new SqlParameter("@SubsidiaryTypeId", employee.SubsidiaryTypeId));
                gSqlParameterList.Add(new SqlParameter("@SubsidiaryType", employee.SubsidiaryType));
                gSqlParameterList.Add(new SqlParameter("@IsActive", employee.IsActive));

                if (employee.SubsidiaryTypeId > 0)
                {
                    DataTable dt = accessManager.GetDataTable("sp_check_SubsidairyType", gSqlParameterList);
              
                    if (dt.Rows.Count == 0)
                    {
                        gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                        aResponse.isSuccess = accessManager.UpdateData("sp_UD_SubsidairyType", gSqlParameterList);
                        aResponse.pk = employee.SubsidiaryTypeId;
                    }
                    else
                    {
                        aResponse.isSuccess = false;
                    }
                }
                else
                {
                    gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_SubsidairyType", gSqlParameterList);
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
       
        public Subsidarytype GeSubsidairyType_ById(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                Subsidarytype master = new Subsidarytype();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@SubsidiaryTypeId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_SubsidairyType_ById", aSqlParameters);
                while (dr.Read())
                {
                    master.SubsidiaryTypeId = Convert.ToInt32(dr["SubsidiaryTypeId"]);
                    master.SubsidiaryType = dr["SubsidiaryType"].ToString();

                    try
                    {



                        master.IsActive =
                        Convert.ToBoolean(dr["IsActive"] != DBNull.Value ? Convert.ToString(dr["IsActive"]) : "false");

                    }
                    catch
                    {

                    }

                }
                return master;
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



        public List<Subsidarytype> GeSubsidairyType()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<Subsidarytype> aList = new List<Subsidarytype>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_SubsidairyType");
                while (dr.Read())
                {
                    Subsidarytype aMaster = new Subsidarytype();
                    aMaster.SubsidiaryTypeId = Convert.ToInt32(dr["SubsidiaryTypeId"]);
                    aMaster.SubsidiaryType = dr["SubsidiaryType"].ToString();

                    try
                    {



                        aMaster.IsActive =
                        Convert.ToBoolean(dr["IsActive"] != DBNull.Value ? Convert.ToString(dr["IsActive"]) : "false");

                    }
                    catch
                    {

                    }
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
    }
}