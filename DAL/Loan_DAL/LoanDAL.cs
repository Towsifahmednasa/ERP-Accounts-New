// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Loan_DAL\LoanDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts_Master_M;
using New_Accounts.Models.LOAN_DAO;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Loan_DAL
{
    public class LoanDAL
    {
        DataAccessManager accessManager = new DataAccessManager();

        internal List<LCPreparationMaster> Load_Reference_No(string companyId, string supplierId)
        {
            List<LCPreparationMaster> ReferenceNOlist = new List<LCPreparationMaster>();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.ControlPanel);

                List<SqlParameter> aParameters = new List<SqlParameter>();

                aParameters.Add(new SqlParameter("@CompanyId", companyId));
                aParameters.Add(new SqlParameter("@SupplierId", supplierId));

                SqlDataReader dr = accessManager.GetSqlDataReader("sp_LC_ReferenceNoDDLForLoan", aParameters);

                while (dr.Read())
                {
                    LCPreparationMaster ReferenceNO = new LCPreparationMaster();
                    ReferenceNO.LCPreparationId = Convert.ToInt32(dr["LCPreparationId"].ToString());
                    ReferenceNO.ReferenceNo = dr["ReferenceNo"].ToString();
                    ReferenceNO.LCDate = dr["LCDate"].ToString();
                    ReferenceNO.CurrencyId =  (dr["CurrencyId"].ToString());
                    ReferenceNO.ConversionRate = (dr["ConversionRate"].ToString());
                    ReferenceNOlist.Add(ReferenceNO);
                }

            }
            catch (Exception ex)
            {
                accessManager.SqlConnectionClose();
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }

            return ReferenceNOlist;
        }



        public ResultResponse Save_LoanInfo(LoanInfoModelDAO loanInfo)
        {
            ResultResponse response = new ResultResponse();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();

                // Adding parameters
                sqlParameterList.Add(new SqlParameter("@LoanInfoID", loanInfo.LoanInfoID));
                sqlParameterList.Add(new SqlParameter("@BusinessUnitId", (object)loanInfo.BusinessUnitId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@ProfitCenterID", (object)loanInfo.ProfitCenterID ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@SupplierID", (object)loanInfo.SupplierID ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LCNoId", (object)loanInfo.LCNoId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@CINoId", (object)loanInfo.CINoId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LCOpenDate", (object)loanInfo.LCOpenDate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanIssuingBankId", (object)loanInfo.LoanIssuingBankId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanIssuingBranchId", (object)loanInfo.LoanIssuingBranchId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@CurrencyId", (object)loanInfo.CurrencyId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@ConversionRate", (object)loanInfo.ConversionRate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanNo", (object)loanInfo.LoanNo ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanTenorDays", (object)loanInfo.LoanTenorDays ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanCategoryID", (object)loanInfo.LoanCategoryID ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanCreateDate", (object)loanInfo.LoanCreateDate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanEffectiveDate", (object)loanInfo.LoanEffectiveDate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanAmount", (object)loanInfo.LoanAmount ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@GracePeriodDays", (object)loanInfo.GracePeriodDays ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@AdvancedInterestPayment", (object)loanInfo.AdvancedInterestPayment ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@InterestTypeId", (object)loanInfo.InterestTypeId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@InterestMethodId", (object)loanInfo.InterestMethodId ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@InstallmentNo", (object)loanInfo.InstallmentNo ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@Funded", (object)loanInfo.Funded ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanAgreement", (object)loanInfo.LoanAgreement ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@ReimbursementBank", (object)loanInfo.ReimbursementBank ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@MarginAdjusted", (object)loanInfo.MarginAdjusted ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@InterestRate", (object)loanInfo.InterestRate ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@LoanPurpose", (object)loanInfo.LoanPurpose ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@InterestAmount", (object)loanInfo.InterestAmount ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@TotalChargeAmount", (object)loanInfo.TotalChargeAmount ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@TotalExpense", (object)loanInfo.TotalExpense ?? DBNull.Value));
                sqlParameterList.Add(new SqlParameter("@TotalPayableAmount", (object)loanInfo.TotalPayableAmount ?? DBNull.Value));
               

                // Check if updating an existing record
                if (loanInfo.LoanInfoID > 0)
                {
                    sqlParameterList.Add(new SqlParameter("@UpdatedBy", HttpContext.Current.Session["LoginUserId"].ToString()));
                
                    response.isSuccess = accessManager.UpdateData("sp_UpdateLoanInfo", sqlParameterList);
                    response.pk = loanInfo.LoanInfoID;
                }
                else // If a new record
                {
                    sqlParameterList.Add(new SqlParameter("@CreatedBy", HttpContext.Current.Session["LoginUserId"].ToString()));
                  
                    response.pk = accessManager.SaveDataReturnPrimaryKey("sp_InsertLoanInfo", sqlParameterList);
                    response.isSuccess = response.pk > 0;
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


        public List<LoanDataModelListDAO> GetLoanInfoList(string param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<LoanDataModelListDAO> aList = new List<LoanDataModelListDAO>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@parm", param));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_LoanInfoList", aParameters);
                while (dr.Read())
                {
                    LoanDataModelListDAO aMaster = new LoanDataModelListDAO
                    {
                        LoanInfoID = Convert.ToInt32(dr["LoanInfoID"]),
                        CompanyName = dr["CompanyName"].ToString(),
                        ProfitCenter = dr["ProfitCenter"].ToString(),
                        Supplier = dr["Supplier"].ToString(),
                        LCNo = dr["LCNo"].ToString(),
                        LCOpenDate = dr["LCOpenDate"].ToString(),
                        LoanIssuingBank = dr["LoanIssuingBank"].ToString(),
                        LoanIssuingBranch = dr["LoanIssuingBranch"].ToString(),
                        Currency = dr["Currency"].ToString(),
                        ConversionRate = dr["ConversionRate"].ToString(),
                        LoanNo = dr["LoanNo"].ToString(),
                        LoanTenorDays = dr["LoanTenorDays"].ToString(),
                        LoanCategory = dr["LoanCategory"].ToString(),
                        LoanCreateDate = dr["LoanCreateDate"].ToString(),
                        LoanEffectiveDate = dr["LoanEffectiveDate"].ToString(),
                        LoanAmount = dr["LoanAmount"].ToString(),
                        GracePeriodDays = dr["GracePeriodDays"].ToString(),
                        AdvancedInterestPayment = dr["AdvancedInterestPayment"].ToString(),
                        InterestType = dr["InterestType"].ToString(),
                        InterestMethod = dr["InterestMethod"].ToString(),
                        InstallmentNo = dr["InstallmentNo"].ToString(),
                        Funded = dr["Funded"].ToString(),
                        LoanAgreement = dr["LoanAgreement"].ToString(),
                        ReimbursementBank = dr["ReimbursementBank"].ToString(),
                        MarginAdjusted = dr["MarginAdjusted"].ToString(),
                        InterestRate = dr["InterestRate"].ToString(),
                        LoanPurpose = dr["LoanPurpose"].ToString(),
                        InterestAmount = dr["InterestAmount"].ToString(),
                        TotalChargeAmount = dr["TotalChargeAmount"].ToString(),
                        TotalExpense = dr["TotalExpense"].ToString(),
                        TotalPayableAmount = dr["TotalPayableAmount"].ToString()
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
        public DataTable Get_LoanById(int Id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                gSqlParameterList.Add(new SqlParameter("@LoanInfoID", Id));
                return accessManager.GetDataTable("sp_GetLoanInfoById", gSqlParameterList);
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