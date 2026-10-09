// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\CostcentreDal.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class CostcentreDal
    {
        private DataAccessManager accessManager = new DataAccessManager();
        public ResultResponse Save_CostCenter(CostCentreDao aCostCenter)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                aSqlParameterlist.Add(new SqlParameter("@CostCenterId", aCostCenter.CostCenterId));
                aSqlParameterlist.Add(new SqlParameter("@CostCenterCode", (object)aCostCenter.CostCenterCode ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CostCenterStartDate", (object)aCostCenter.CostCenterStartDate ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsSupplier", (object)aCostCenter.IsSupplier ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@SupplierId", (object)aCostCenter.SupplierId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@BBLCInformationId", (object)aCostCenter.BBLCInformationId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsBuyer", (object)aCostCenter.IsBuyer ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@ByerId", (object)aCostCenter.ByerId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsEmployee", (object)aCostCenter.IsEmployee ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@EmployeeId", (object)aCostCenter.EmployeeId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsSalesContract", (object)aCostCenter.IsSalesContract ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@SalesContractId", (object)aCostCenter.SalesContractId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@MasterLCId", (object)aCostCenter.MasterLCId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Other", (object)aCostCenter.Other ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsActive", (object)aCostCenter.IsActive ?? DBNull.Value));
           
                aSqlParameterlist.Add(new SqlParameter("@InActiveDate", (object)aCostCenter.InActiveDate ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsMLC", (object)aCostCenter.IsMLC ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)aCostCenter.CompanyId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)aCostCenter.FinancialYearId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@OrderId", (object)aCostCenter.OrderId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@OtherCOAId", (object)aCostCenter.OtherCOAId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Description", (object)aCostCenter.Description ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@COAId", (object)aCostCenter.COAId ??DBNull.Value));

                if (aCostCenter.CostCenterId > 0)
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
                        aSqlParameterlist.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    aResponse.isSuccess = accessManager.UpdateData("sp_UD_CostCenter", aSqlParameterlist);
                       aResponse.pk = aCostCenter.CostCenterId;
                
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));

                   aSqlParameterlist.Add(new SqlParameter("@EntryDate", DateTime.Now));

                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_CostCenter", aSqlParameterlist);
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

        public List<CostCentreDao> GetCostcenter()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<CostCentreDao> aList = new List<CostCentreDao>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@AccountName",""));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCostCenter", aParameters);
                while (dr.Read())
                {
                    CostCentreDao aMaster = new CostCentreDao();
                    aMaster.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                    aMaster.Description = dr["Description"].ToString();
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                   
                    aMaster._CostCenterStartDate = dr["CostCenterStartDate"].ToString();
                                   
                    aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
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



        #region   Rana 


        public ResultResponse Save_CostCenter_Rana(CostCentreDao aCostCenter)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                aSqlParameterlist.Add(new SqlParameter("@CostCenterId", aCostCenter.CostCenterId));
                aSqlParameterlist.Add(new SqlParameter("@CostCenterCode", (object)aCostCenter.CostCenterCode ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CostCenterStartDate", (object)aCostCenter.CostCenterStartDate ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsSupplier", (object)aCostCenter.IsSupplier ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@SupplierId", (object)aCostCenter.SupplierId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@BBLCInformationId", (object)aCostCenter.BBLCInformationId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsBuyer", (object)aCostCenter.IsBuyer ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@ByerId", (object)aCostCenter.ByerId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsEmployee", (object)aCostCenter.IsEmployee ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@EmployeeId", (object)aCostCenter.EmployeeId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsSalesContract", (object)aCostCenter.IsSalesContract ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@SalesContractId", (object)aCostCenter.SalesContractId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@MasterLCId", (object)aCostCenter.MasterLCId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Other", (object)aCostCenter.Other ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsActive", (object)aCostCenter.IsActive ?? DBNull.Value));

                aSqlParameterlist.Add(new SqlParameter("@InActiveDate", (object)aCostCenter.InActiveDate ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@IsMLC", (object)aCostCenter.IsMLC ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)aCostCenter.CompanyId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)aCostCenter.FinancialYearId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@OrderId", (object)aCostCenter.OrderId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@OtherCOAId", (object)aCostCenter.OtherCOAId ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@Description", (object)aCostCenter.Description ?? DBNull.Value));
                aSqlParameterlist.Add(new SqlParameter("@COAId", (object)aCostCenter.COAId ?? DBNull.Value));

                if (aCostCenter.CostCenterId > 0)
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
                    aSqlParameterlist.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    aResponse.isSuccess = accessManager.UpdateData("sp_UD_CostCenter", aSqlParameterlist);
                    aResponse.pk = aCostCenter.CostCenterId;

                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.UserName].ToString()));

                    aSqlParameterlist.Add(new SqlParameter("@EntryDate", DateTime.Now));

                    aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_I_CostCenter", aSqlParameterlist);
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


        internal DataTable GetCostcenterR()
        {
            DataTable dataTable = new DataTable();

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                dataTable = accessManager.GetDataTable("sp_GET_AllCostCenter_Rana");
            }
            catch (Exception e)
            {
                accessManager.SqlConnectionClose(true);
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }

            return dataTable;
        }


        public CostCentreDao GetCostCentre_ById_Rana(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                CostCentreDao aMaster = new CostCentreDao();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@CostCenterId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCostCenterById", aSqlParameters);
                while (dr.Read())
                {
                    aMaster.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                    aMaster.Description = dr["Description"].ToString();
                    aMaster._CostCenterStartDate = dr["CostCenterStartDate"].ToString();
                    aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

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


        public List<CompanyInformation> Company_Information()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_DDL_Company");
            List<CompanyInformation> companyInformationList = new List<CompanyInformation>();
            while (dr.Read())
            {
                CompanyInformation companyInformation = new CompanyInformation();
                companyInformation.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                companyInformation.CompanyName = dr["CompanyName"].ToString();
                //companyInformation.CompanyShortName = dr["CompanyName"].ToString();
                companyInformationList.Add(companyInformation);
            }

            return companyInformationList;
        }



        public CompanyInformation Load_Company_Info_ById(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                CompanyInformation aMaster = new CompanyInformation();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@CompanyId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CompanyById", aSqlParameters);
                while (dr.Read())
                {
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();

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




        public ResultResponse DelCostCenterInfoById(int Id)
        {
            ResultResponse aResponse = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                aSqlParameterlist.Add(new SqlParameter("@CostCenterId", Id));

                aResponse.pk = accessManager.SaveDataReturnPrimaryKey("sp_Del_CostCenterInfoById", aSqlParameterlist);

                aResponse.isSuccess = true;
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

        #endregion


        public CostCentreDao GetCostCentre_ById(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                CostCentreDao aMaster = new CostCentreDao();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@CostCenterId", id));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCostCenterById", aSqlParameters);
                while (dr.Read())
                {
                    aMaster.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                    aMaster.Description = dr["Description"].ToString();
                    aMaster._CostCenterStartDate = dr["CostCenterStartDate"].ToString();
                    aMaster.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

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