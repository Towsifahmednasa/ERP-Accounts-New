// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\SubSubControlLedgerDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
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
    public class SubSubControlLedgerDAL
    {

        private DataAccessManager accessManager = new DataAccessManager();

        public List<SubSubControlLedgerView> Get_SubSubControlLedgerList(string id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SubSubControlLedgerView> aList = new List<SubSubControlLedgerView>();

                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@search", id));

                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubSubControlLedger", gSqlParameterList);
                while (dr.Read())
                {
                    SubSubControlLedgerView aMaster = new SubSubControlLedgerView();
                    aMaster.SubSubControlLedgerId = Convert.ToInt32(dr["SubSubControlLedgerId"]);
                 
                    aMaster.GLName = dr["GLName"].ToString();
                    aMaster.ControlLName = dr["ControlLName"].ToString();
                    aMaster.SubsidiryLName = dr["SubsidiryLName"].ToString();
                    aMaster.SubSubControlLedgerCode = dr["SubSubControlLedgerCode"].ToString();
                    aMaster.SubSubControlLedgerName = (dr["SubSubControlLedgerName"].ToString());
                    aMaster.Description = (dr["Description"].ToString());
                    try
                    {
                        aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

                    }
                    catch { }



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
        public ResultResponse Save_SSCL(SubSubControlLedgerDAO aMaster)
        {


            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;
                aSqlParameterlist.Add(new SqlParameter("@SubSubControlLedgerId", aMaster.SubSubControlLedgerId));
                aSqlParameterlist.Add(new SqlParameter("@Description", (object)(aMaster.Description ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@SubSubControlLedgerName", aMaster.SubSubControlLedgerName));
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryLId", aMaster.SubsidiaryLId));
              
                

                aSqlParameterlist.Add(new SqlParameter("@IsActive", (object)(aMaster.IsActive ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@CreateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                
                if (aMaster.SubSubControlLedgerId > 0)
                {
                    //aSqlParameterList.Add(new SqlParameter("@id", dCTagging.DCTaggingWithUCId));
                    //aSqlParameterList.Add(new SqlParameter("@Name", dCTagging.DistrictCoordinator));
                    //DataTable dt = accessManager.GetDataTable("sp_check_DctaggingWithUc", aSqlParameterList);
                    //if (dt.Rows.Count == 0)
                    //{

                    //    gSqlParameterList.Add(new SqlParameter("@UpdateBy", sessionUser));
                    aResponse.isSuccess = accessManager.UpdateData("sp_UP_SubSubControlLedger", aSqlParameterlist);

                    aResponse.pk = aMaster.SubSubControlLedgerId;
                    
                }
                else
                {

                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_SubSubControlLedger", aSqlParameterlist);
                    if (aResponse.pk > 0)
                    {
                        aResponse.isSuccess = true;
                    }
                }
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


        public DataTable Get_SSCL_ById(int Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@SubSubControlLedgerId", Id));
                return accessManager.GetDataTable("sp_GET_SubSubControlLedgerById", gSqlParameterList);
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