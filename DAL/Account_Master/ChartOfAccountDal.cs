// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\ChartOfAccountDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class ChartOfAccountDal
    {
        private DataAccessManager accessManager = new DataAccessManager();

        public List<COAView> Get_ChartOfAccount_List(string id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<COAView> aList = new List<COAView>();

                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@search", id));
                
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ChartOfAccountParamSearch", gSqlParameterList);
                while (dr.Read())
                {
                    COAView aMaster = new COAView();
                    aMaster.AccountId = Convert.ToInt32(dr["AccountId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.GLName = dr["GLName"].ToString();
                    aMaster.ControlLName = dr["ControlLName"].ToString();
                    aMaster.SubsidiryLName = dr["SubsidiryLName"].ToString();
                    aMaster.SubSubControlLedgerName = dr["SubSubControlLedgerName"].ToString();
                    aMaster.AccountName = (dr["AccountName"].ToString());
                     aMaster.Description = (dr["Description"].ToString());

                    aMaster.CashAccountId = DBNull.Value.Equals(dr["CashAccountId"]) ? 0 : Convert.ToInt32(dr["CashAccountId"]);

                    aMaster.BankAccountId = DBNull.Value.Equals(dr["BankAccountId"]) ? 0 : Convert.ToInt32(dr["BankAccountId"]);
                    try
                    {
                        aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

                    }
                    catch { aMaster.IsActive = false; }

                    try
                    {
                        aMaster.IsDefaultGl = !DBNull.Value.Equals(dr["IsDefaultGl"]) && Convert.ToInt32(dr["IsDefaultGl"]) == 1;
                    }
                    catch { aMaster.IsDefaultGl = false; }

                    try
                    {
                        aMaster.IsSystemDefine = Convert.ToBoolean(dr["IsSystemDefine"].ToString());

                    }
                    catch { aMaster.IsSystemDefine = false; }

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

        public DataTable Load_SubControl_ByControlLedgerId(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@ControlId", id));
                return accessManager.GetDataTable("sp_GET_SubsidiaryDropDown", gSqlParameterList);
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

        public DataTable Load_Company_()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                return accessManager.GetDataTable("sp_DDL_Company");
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


        public DataTable Get_ChartofAccount_ById(int Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@AccountId", Id));
                return accessManager.GetDataTable("sp_GET_ChartofAccount_ById", gSqlParameterList);
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
        public DataTable GetAccountCodebySubSubControlLedgerId(int Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@SubSubControlLedgerId", Id));
                return accessManager.GetDataTable("sp_GET_AccountCodebySubSubControlLedgerId", gSqlParameterList);
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
        public DataTable CheckAccountCodeExists(string accountCode)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@accountCode", accountCode));
                return accessManager.GetDataTable("sp_CheckAccountCodeExists", gSqlParameterList);
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
        public DataTable Load_CompanyUnit(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@CompanyId", id));
                return accessManager.GetDataTable("sp_DDL_CompanyUnit", gSqlParameterList);
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

        public List<ReferanceType> GetAllReferanceType(int glid)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@glid", glid));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllReferanceType_Ddl", aParameters);
            List<ReferanceType> businessUnitList = new List<ReferanceType>();
            while (dr.Read())
            {
                ReferanceType businessUnit = new ReferanceType();
                businessUnit.ReferanceTypeId = Convert.ToInt32(dr["ReferanceTypeId"]);
                businessUnit.ReferanceTypeName = dr["ReferanceTypeName"].ToString();
                businessUnit.isrefrence = Convert.ToInt32(dr["isrefrence"]);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public DataTable GetChartofAccountByCompanyId(string CompanyId)
        {
            try
            {
                DataTable dr = new DataTable();
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyId));
                dr = accessManager.GetDataTable("sp_Rpt_ChartofAccount", aSqlParameterlist);


                return dr;
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

        public List<SubsidiaryType_Ddl> GetAllSubSidaryType(int glid)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@glid", glid));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllSubsidiaryType_Ddl", aParameters);
            List<SubsidiaryType_Ddl> businessUnitList = new List<SubsidiaryType_Ddl>();
            while (dr.Read())
            {
                SubsidiaryType_Ddl businessUnit = new SubsidiaryType_Ddl();
                businessUnit.SubsidiaryTypeId = Convert.ToInt32(dr["SubsidiaryTypeId"]);
                businessUnit.SubsidiaryType = dr["SubsidiaryType"].ToString();
                businessUnit.isrefrence = Convert.ToInt32(dr["isrefrence"]);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<CostCenter_Ddl> GetAllCostCenter(int glid)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@glid", glid));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CostCenter_Ddl", aParameters);
            List<CostCenter_Ddl> businessUnitList = new List<CostCenter_Ddl>();
            while (dr.Read())
            {
                CostCenter_Ddl businessUnit = new CostCenter_Ddl();
                businessUnit.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                businessUnit.CostCenterName = dr["CostCenterName"].ToString();
                businessUnit.isrefrence = Convert.ToInt32(dr["isrefrence"]);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }
        public ResultResponse Save_ChartOfAccount(ChartOfAccount aChartOfAccounts)
        {
            

            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;
                aSqlParameterlist.Add(new SqlParameter("@AccountId", aChartOfAccounts.AccountId));
                aSqlParameterlist.Add(new SqlParameter("@AccountCode", (object)(aChartOfAccounts.AccountCode ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@Description", (object)(aChartOfAccounts.Description ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@AccountName", aChartOfAccounts.AccountName));
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryLId", aChartOfAccounts.SubsidiaryLId));
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)aChartOfAccounts.CompanyId ?? DBNull.Value));         
                aSqlParameterlist.Add(new SqlParameter("@InIncomeStatement", (object)aChartOfAccounts.InIncomeStatement ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@InBalanceSheet", (object)aChartOfAccounts.InBalanceSheet ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CashAccountId", (object)(aChartOfAccounts.CashAccountId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", (object)(aChartOfAccounts.BankAccountId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsEmployee", (object)(aChartOfAccounts.IsEmployee ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@EmployeeId", (object)(aChartOfAccounts.EmployeeId ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@IsReference", (object)(aChartOfAccounts.IsReference ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@IsSubsidiary", (object)(aChartOfAccounts.IsSubsidiary ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@IsCostCenter", (object)(aChartOfAccounts.IsCostCenter ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@IsActive", (object)(aChartOfAccounts.IsActive ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@CreateBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));
                aSqlParameterlist.Add(new SqlParameter("@CreateDate", DateTime.Now));
                SqlParameter msgParam = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
                msgParam.Direction = ParameterDirection.Output;
                aSqlParameterlist.Add(msgParam);
                if (aChartOfAccounts.AccountId > 0)
                {
                   
                    aResponse.isSuccess = accessManager.UpdateData("sp_UP_ChartOfAccount_new", aSqlParameterlist);

                    if (aResponse.isSuccess)
                    {
                        aResponse.pk = aChartOfAccounts.AccountId;
                    }
                    // Set the AccountId in response
                   

                    // Retrieve and assign output message
                    aResponse.msg = msgParam.Value?.ToString() ?? "No message returned.";

                }
                else
                {
                    aResponse.pk =  accessManager.SaveDataReturnPrimaryKey("sp_I_ChartOfAccount", aSqlParameterlist);


                    string resultMessage = msgParam.Value?.ToString() ?? "No message returned.";

                    // Set response based on primary key
                    if (aResponse.pk > 0)
                    {
                        aResponse.isSuccess = true;
                        aResponse.msg = resultMessage; // e.g., "Account successfully inserted."
                    }
                    else
                    {
                        aResponse.isSuccess = false;
                        aResponse.msg = resultMessage; // e.g., "Account name already exists."
                    }

                }
                if (aResponse.pk > 0)
                {
                    int[] idS = aChartOfAccounts.CompanyUnitAll.Split(',').Select(Int32.Parse).ToArray();
                    for (int i = 0; i < idS.Length; i++)
                    {
                        int aId = idS[i];
                        List<SqlParameter> aSQL = new List<SqlParameter>();                      

                        aSQL.Add(new SqlParameter("@BankAccountId", aResponse.pk));
                        aSQL.Add(new SqlParameter("@CompanyId", aId));

                        aResponse.isSuccess = accessManager.SaveData("sp_I_MultipleCOACompanyPermission", aSQL);
                    }


                    if (aChartOfAccounts.IsReference==true)
                    {
                        aResponse.isSuccess = SaveRefDetails(aChartOfAccounts.RefDetails, aResponse.pk);
                    }


                    if (aChartOfAccounts.IsSubsidiary == true)
                    {
                        aResponse.isSuccess = SaveSubDetails(aChartOfAccounts.SubDetails, aResponse.pk);
                    }
                    if (aChartOfAccounts.IsCostCenter == true)
                    {
                        aResponse.isSuccess = SaveCostCenterDetails(aChartOfAccounts.CSDetails, aResponse.pk);
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


        public bool SaveRefDetails(List<COARef> aList, int pk)
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                    aSqlParameterlist.Add(new SqlParameter("@AccountId", pk));
                    aSqlParameterlist.Add(new SqlParameter("@ReferenceTypeId", (object)(item.ReferenceTypeId ?? (object)DBNull.Value)));
                     

                    result = accessManager.SaveData("sp_I_COARefDetail", aSqlParameterlist);


                    if (result == false)
                    {
                        break;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool SaveSubDetails(List<COASubsidiary> aList, int pk)
        {
            try
            {
                bool result = false;


                try
                {
                    if (aList != null) { 
                    foreach (var item in aList)
                    {
                        List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                        aSqlParameterlist.Add(new SqlParameter("@AccountId", pk));
                        aSqlParameterlist.Add(new SqlParameter("@SubsidiaryTypeId", (object)(item.SubsidiaryTypeId ?? (object)DBNull.Value)));


                        result = accessManager.SaveData("sp_I_COASubsidiaryDetail", aSqlParameterlist);


                        if (result == false)
                        {
                            break;
                        }

                    }
                    }
                }
                catch
                {

                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool SaveCostCenterDetails(List<COACostCenter> aList, int pk)
        {
            try
            {
                bool result = false;
                try
                {
                    if (aList != null)
                    {
                        foreach (var item in aList)
                        {
                            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                            aSqlParameterlist.Add(new SqlParameter("@AccountId", pk));
                            aSqlParameterlist.Add(new SqlParameter("@CostCenterId", (object)(item.CostCenterId ?? (object)DBNull.Value)));


                            result = accessManager.SaveData("sp_I_COACostCenterDetail", aSqlParameterlist);


                            if (result == false)
                            {
                                break;
                            }

                        }
                    }
                }
                catch
                {

                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveSubInfoDetails(List<SubsidiaryInfoDAO> aList, int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                bool result = false;

                int SubsidiaryTypeId = Convert.ToInt32(aList.First().SubsidiaryTypeId);

                var selectedIds = string.Join(",",
                    aList.Where(x => x.S_Info_Id != null)
                         .Select(x => x.S_Info_Id.ToString())
                );

                List<SqlParameter> deleteParams = new List<SqlParameter>
        {
            new SqlParameter("@SubsidiaryTypeId", SubsidiaryTypeId),
            new SqlParameter("@SelectedIds", selectedIds)
        };

                result = accessManager.SaveData("sp_DEL_SubsidiaryInfoDtlsNew", deleteParams);

                if (result == false)
                    return false;

                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>
            {
                new SqlParameter("@SubsidiaryTypeId", (object)item.SubsidiaryTypeId ?? DBNull.Value),
                new SqlParameter("@S_Info_Id", (object)item.S_Info_Id ?? DBNull.Value),
                new SqlParameter("@IsSelect", (object)item.IsSelect ?? DBNull.Value),
                new SqlParameter("@Description", (object)item.Description ?? DBNull.Value),
                new SqlParameter("@EntryBy", UserId)
            };

                    result = accessManager.SaveData("sp_I_SubsidiaryInfoDtls", aSqlParameterlist);

                    if (result == false)
                        break;
                }

                return result;
            }
            catch (Exception)
            {
                accessManager.SqlConnectionClose(true);
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        //public bool SaveSubInfoDetails(List<SubsidiaryInfoDAO> aList, int UserId)
        //{
        //    try
        //    {

        //        accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);


        //        bool result = false;


        //        int ReferenceTypeId = 0;
        //        foreach (var item in aList)
        //        {

        //            ReferenceTypeId = Convert.ToInt32(item.SubsidiaryTypeId);
        //        }

        //        List<SqlParameter> asql = new List<SqlParameter>();
        //        asql.Add(new SqlParameter("@ReferenceTypeId", ReferenceTypeId));
        //        result = accessManager.DeleteData("sp_DEL_SubsidiaryInfoDtls", asql);

        //        foreach (var item in aList)
        //        {
        //            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //            aSqlParameterlist.Add(new SqlParameter("@SubsidiaryTypeId", (object)(item.SubsidiaryTypeId ?? (object)DBNull.Value)));
        //            aSqlParameterlist.Add(new SqlParameter("@S_Info_Id", (object)(item.S_Info_Id ?? (object)DBNull.Value)));

        //            aSqlParameterlist.Add(new SqlParameter("@IsSelect", (object)(item.IsSelect ?? (object)DBNull.Value)));


        //            aSqlParameterlist.Add(new SqlParameter("@Description", (object)(item.Description ?? (object)DBNull.Value)));

        //            aSqlParameterlist.Add(new SqlParameter("@EntryBy", UserId));


        //            result = accessManager.SaveData("sp_I_SubsidiaryInfoDtls", aSqlParameterlist);


        //            if (result == false)
        //            {
        //                break;
        //            }

        //        }
        //        return result;
        //    }catch(Exception exception)
        //    {
        //        accessManager.SqlConnectionClose(true);

        //        throw exception;
        //    }
        //    finally
        //    {
        //        accessManager.SqlConnectionClose();
        //    }
        //}


        public bool SaveRefInfoDetails(List<ReferenceInfoDAO> aList, int UserId)
        {
            try
            {

                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                bool result = false;
                int ReferenceTypeId  = 0;
                foreach (var item in aList)
                {
                    ReferenceTypeId = Convert.ToInt32(item.ReferenceTypeId);
                }

                    List<SqlParameter> asql = new List<SqlParameter>();
                asql.Add(new SqlParameter("@ReferenceTypeId",   ReferenceTypeId ));
                result = accessManager.DeleteData("sp_DEL_RefInfoDtls", asql);

                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                    aSqlParameterlist.Add(new SqlParameter("@ReferenceTypeId", (object)(item.ReferenceTypeId ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@R_Info_Id", (object)(item.R_Info_Id ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@IsSelect", (object)(item.IsSelect ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@Description", (object)(item.Description ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", UserId));


                    result = accessManager.SaveData("sp_I_RefInfoDtls", aSqlParameterlist);


                    if (result == false)
                    {
                        break;
                    }

                }
                return result;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);

                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

    }
}