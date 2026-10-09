// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\SubsubsidairyDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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


    public class SubsubsidairyDAL
    {




        private DataAccessManager accessManager = new DataAccessManager();

        public ResultResponse Save_SubsidiaryLedger(SubsidiaryLedgerDAO aMaster)
        {
            int pk = 0;

            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                
                DateTime entryDtae = DateTime.Now;
                aSqlParameterlist.Add(new SqlParameter("@SubsidiryLId", aMaster.SubsidiryLId));
                aSqlParameterlist.Add(new SqlParameter("@SubsidiryLName", (object)(aMaster.SubsidiryLName ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@ChartofAccountId", aMaster.ChartofAccountId));
                aSqlParameterlist.Add(new SqlParameter("@IsActive", aMaster.IsActive));
                //gSqlParameterList.Add(new SqlParameter("@DivisionId", dCTagging.IsActive));
                if (aMaster.SubsidiryLId > 0)
                {
                    //aSqlParameterList.Add(new SqlParameter("@id", dCTagging.DCTaggingWithUCId));
                    //aSqlParameterList.Add(new SqlParameter("@Name", dCTagging.DistrictCoordinator));
                    //DataTable dt = accessManager.GetDataTable("sp_check_DctaggingWithUc", aSqlParameterList);
                    //if (dt.Rows.Count == 0)
                    //{

                    //    gSqlParameterList.Add(new SqlParameter("@UpdateBy", sessionUser));
                    //    aInformation.isSuccess = accessManager.UpdateData("sp_UD_DCTaggingWithUC", gSqlParameterList);
                    //    pk = dCTagging.DCTaggingWithUCId;
                    //}
                    //else
                    //{
                    //    aInformation.isSuccess = false;
                    //}
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));
                    
                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_SubsidiaryLedger", aSqlParameterlist);
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


        public DataTable Get_Subsubsidairy(string search)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ControlLName", search));
                return accessManager.GetDataTable("sp_GET_SubsubsidairyList", aSqlParameterList);
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