// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Loan_DAL\LCRelatedBankPaymentDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using New_Accounts.DataManager;
using New_Accounts.Models.LOAN_DAO;
using New_Accounts.Models;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;

namespace New_Accounts.DAL.Loan_DAL
{
    public class LCRelatedBankPaymentDAL
    {


        DataAccessManager accessManager = new DataAccessManager();


        internal DataTable LoadCIInfo(string lcId)
        {
            DataTable dataTable = new DataTable();

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

                aSqlParameterList.Add(new SqlParameter("@Param", lcId));
                dataTable = accessManager.GetDataTable("sp_GET_LcCInvoice_MasterLCId", aSqlParameterList);

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

        public List<LCRelatedBankPaymentMasterListDAO> Get_BankChargeList(string param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<LCRelatedBankPaymentMasterListDAO> aList = new List<LCRelatedBankPaymentMasterListDAO>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@parm", param));
                SqlDataReader dr = accessManager.GetSqlDataReader("SP_GetLCRelatedBankPaymentMaster", aParameters);
                while (dr.Read())
                {
                    LCRelatedBankPaymentMasterListDAO aMaster = new LCRelatedBankPaymentMasterListDAO
                    {
                        BankChargeId = dr["BankChargeId"].ToString(),
                        CompanyName = dr["CompanyName"].ToString(),
                        LCNo = dr["LCNo"].ToString(),
                        LCDate = dr["LCDate"].ToString(),
                        CurrencyName = dr["CurrencyName"].ToString(),
                        SupplierName = dr["SupplierName"].ToString(),
                        TypeName = dr["TypeName"].ToString(),
                        SupplierTypeName = dr["SupplierTypeName"].ToString(),
                        LCOpenId = dr["LCOpenId"].ToString(),
                        Conversionrate = dr["Conversionrate"].ToString(),
                        LcBankPayTypeId = dr["LcBankPayTypeId"].ToString(),
                        ChargeBasisof = dr["ChargeBasisof"].ToString(),
                        TotalAmountBDT = dr["TotalAmountBDT"].ToString(),
                        TotalAmountFC = dr["TotalAmountFC"].ToString(),
                        PaymentFrom = dr["PaymentFrom"].ToString(),
                        LoanAccID = dr["LoanAccID"].ToString(),
                        COAID = dr["COAID"].ToString(),
                        EntryBy = dr["EntryBy"].ToString(),
                        EntryDate = dr["EntryDate"].ToString(),
                        Updateby = dr["Updateby"].ToString(),
                        Updatedate = dr["Updatedate"].ToString()

                    };
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

        public ResultResponse Save_LoanInfo(BankPaymentMasterDAO loanInfo)
        {
            ResultResponse response = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();

                // Adding parameters
                sqlParameterList.Add(new SqlParameter("@BankChargeId", loanInfo.BankChargeId));
                sqlParameterList.Add(new SqlParameter("@LCOpenId", (object)loanInfo.LCOpenId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@Conversionrate", (object)loanInfo.Conversionrate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@TotalAmountBDT", (object)loanInfo.TotalAmountBDT ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@TotalAmountFC", (object)loanInfo.TotalAmountFC ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LcBankPayTypeId", (object)loanInfo.LcBankPayTypeId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@PaymentFrom", (object)loanInfo.PaymentFrom ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@ChargeBasisof", (object)loanInfo.ChargeBasisof ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanAccID", (object)loanInfo.LoanAccID ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@COAID", (object)loanInfo.COAID ?? DBNull.Value)); 

                // Check if updating an existing record
                if (loanInfo.BankChargeId > 0)
                {
                    sqlParameterList.Add(new SqlParameter("@UpdatedBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));

                    response.isSuccess = accessManager.UpdateData("SP_UpdateBankPaymentMaster", sqlParameterList);
                    response.pk = loanInfo.BankChargeId;
                }
                else // If a new record
                {
                    sqlParameterList.Add(new SqlParameter("@EntryBy", HttpContext.Current.Session[SessionCollection.LoginUserId].ToString()));

                    response.pk = accessManager.SaveDataReturnPrimaryKey("SP_InsertBankPaymentMaster", sqlParameterList);
                    response.isSuccess = response.pk > 0;
                }


                if (response.isSuccess)
                {
                    if (loanInfo.DetailsInfo != null)
                    {
                        response.isSuccess = SaveBankPaymentDetails(loanInfo.DetailsInfo, response.pk);

                        if (response.isSuccess == false)
                        {
                            accessManager.SqlConnectionClose(true);
                        }
                    }
                    
                }
                // Save additional details if primary key is generated or found

            }
            catch (Exception ex)
            {
                accessManager.SqlConnectionClose(true);
                response.isSuccess = false;
                response.msg = ex.Message;
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }

            return response;
        }
        public bool SaveBankPaymentDetails(List<BankPaymentDetailDAO> aList, int pk)
        {
            try
            {
                bool result = false;
                try
                {
                    foreach (var item in aList)
                    {
                        List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                        aSqlParameterlist.Add(new SqlParameter("@BankChargeId", pk));
                        aSqlParameterlist.Add(new SqlParameter("@CommercialInvoiceId", (object)(item.CommercialInvoiceId ?? (object)DBNull.Value)));
                        aSqlParameterlist.Add(new SqlParameter("@AmountBDT", (object)(item.AmountBDT ?? (object)DBNull.Value))); aSqlParameterlist.Add(new SqlParameter("@AmountFC", (object)(item.AmountFC ?? (object)DBNull.Value)));



                        result = accessManager.SaveData("SP_InsertBankPaymentDetail", aSqlParameterlist);


                        if (result == false)
                        {
                            break;
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


    }
}