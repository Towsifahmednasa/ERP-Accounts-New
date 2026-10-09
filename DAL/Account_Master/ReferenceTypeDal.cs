// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\ReferenceTypeDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts_Master_M;
using New_Accounts.Utility;

namespace New_Accounts.DAL.Account_Master
{
    public class ReferenceTypeDal
    {
        private DataAccessManager accessManager = new DataAccessManager();
        public ResultResponse Save_ReferenceType(Reference_Type employee)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
  
                gSqlParameterList.Add(new SqlParameter("@ReferenceTypeId", employee.ReferenceTypeId));
                gSqlParameterList.Add(new SqlParameter("@ReferenceType", employee.ReferenceType));
                gSqlParameterList.Add(new SqlParameter("@IsActive", employee.IsActive));

                if (employee.ReferenceTypeId > 0)
                {


                    List<SqlParameter> bSqlParameterList = new List<SqlParameter>();


                    bSqlParameterList.Add(new SqlParameter("@ReferenceTypeId", employee.ReferenceTypeId));
                    bSqlParameterList.Add(new SqlParameter("@ReferenceType", employee.ReferenceType));

                    DataTable dt = accessManager.GetDataTable("sp_check_ReferenceType", bSqlParameterList);
                    if (dt.Rows.Count == 0)
                    {
                        gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                        aResponse.isSuccess = accessManager.UpdateData("sp_UD_ReferenceType", gSqlParameterList);
                        aResponse.pk = employee.ReferenceTypeId;
                    }
                    else
                    {
                        aResponse.isSuccess = false;
                    }
                }
                else
                {
                    gSqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_ReferenceType", gSqlParameterList);
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
        //public List<Reference_Type> GeReferenceType_ById(int id)
        //{
        //    try
        //    {
        //        accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<Reference_Type> aList = new List<Reference_Type>();
        //        List<SqlParameter> aParameters = new List<SqlParameter>();
        //        aParameters.Add(new SqlParameter("@ReferenceTypeId", id));
        //        SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_ReferenceType_ById", aParameters);
        //        while (dr.Read())
        //        {
        //            Reference_Type aMaster = new Reference_Type();
        //            aMaster.ReferenceTypeId = Convert.ToInt32(dr["ReferenceTypeId"]);
        //            aMaster.ReferenceType = dr["ReferenceType"].ToString();
        //            aMaster.EntryBy = Convert.ToInt32(dr["EntryBy"].ToString());
        //            aMaster.EntryDate = Convert.ToDateTime(dr["EntryDate"].ToString());
        //            aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
        //            aList.Add(aMaster);
        //        }
        //        return aList;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        accessManager.SqlConnectionClose();
        //    }
        //}






        public Reference_Type GeReferenceType_ById(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                Reference_Type master = new Reference_Type();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@ReferenceTypeId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_ReferenceType_ById", aSqlParameters);
                while (dr.Read())
                {
                    master.ReferenceTypeId = Convert.ToInt32(dr["ReferenceTypeId"]);
                    master.ReferenceType = dr["ReferenceType"].ToString();
                    //aMaster.EntryBy = Convert.ToInt32(dr["EntryBy"].ToString());
                    //aMaster.EntryDate = Convert.ToDateTime(dr["EntryDate"].ToString());
                    master.IsActive = //Convert.ToBoolean(dr["IsActive"].ToString());

                    Convert.ToBoolean(dr["IsActive"] != DBNull.Value ? Convert.ToString(dr["IsActive"]) : "false");
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




        public List<Reference_Type> GeReferenceType()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<Reference_Type> aList = new List<Reference_Type>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_ReferenceType");
                while (dr.Read())
                {
                    Reference_Type aMaster = new Reference_Type();
                    aMaster.ReferenceTypeId = Convert.ToInt32(dr["ReferenceTypeId"]);
                    aMaster.ReferenceType = dr["ReferenceType"].ToString();
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