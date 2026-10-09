// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\ContraDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class ContraDAL
    {


        DataAccessManager _accessManager = new DataAccessManager();

        public ResultResponse SaveContra(DebitCreditVoucherMaster aDebitCreditVoucherNewMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                 
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                DateTime entryDate = DateTime.Now;

                aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", aDebitCreditVoucherNewMaster.VoucherMasterId));
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", (object)(aDebitCreditVoucherNewMaster.CompanyInfoId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)(aDebitCreditVoucherNewMaster.FinancialYearId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@PostingDate", (object)(aDebitCreditVoucherNewMaster.PostingDate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@VoucherNo", (object)(aDebitCreditVoucherNewMaster.VoucherNo ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CurrencyId", (object)(aDebitCreditVoucherNewMaster.CurrencyId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CurrencyCovRate", (object)(aDebitCreditVoucherNewMaster.CurrencyCovRate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@VoucherType", "FundTransfer"));
                aSqlParameterlist.Add(new SqlParameter("@VoucherCategoryID", (object)(aDebitCreditVoucherNewMaster.TransferType ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@CashAccountId", (object)(aDebitCreditVoucherNewMaster.CashAccountId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", (object)(aDebitCreditVoucherNewMaster.BankAccountId ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@ChequeNo", (object)(aDebitCreditVoucherNewMaster.ChequeNo ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@ChequeDate", (object)(aDebitCreditVoucherNewMaster.ChequeDate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@Narration", (object)(aDebitCreditVoucherNewMaster.Narration ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CashRefNo", (object)(aDebitCreditVoucherNewMaster.CashRefNo ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CashRefDate", (object)(aDebitCreditVoucherNewMaster.CashRefDate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@TotalAmount", (object)(aDebitCreditVoucherNewMaster.TotalAmount ?? (object)DBNull.Value)));

                aSqlParameterlist.Add(new SqlParameter("@EntryBy", user));
                aSqlParameterlist.Add(new SqlParameter("@EntryDate", entryDate));


               

                if (aDebitCreditVoucherNewMaster.VoucherMasterId > 0)
                {
                    aResponse.isSuccess = _accessManager.UpdateData("sp_UD_VoucherMaster_New_Contra", aSqlParameterlist);

                    aResponse.pk = aDebitCreditVoucherNewMaster.VoucherMasterId;
                }
                //ll
                else
                {
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_VoucherMaster_Contra", aSqlParameterlist);
                }

                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = true;

                    if (aResponse.isSuccess == false)
                    {
                        _accessManager.SqlConnectionClose(true);
                    }
                }
                else
                {
                    _accessManager.SqlConnectionClose(true);
                }
                return aResponse;
            }
            catch (Exception ex)
            {
                _accessManager.SqlConnectionClose(true);
                throw ex;
            }

            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }


        public DebitCreditVoucherMaster GetContraMasterId(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                DebitCreditVoucherMaster aMaster = new DebitCreditVoucherMaster();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BankPaymentMasterByIdContra", aParameters);
                while (dr.Read())
                {
                    aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
                    aMaster.CompanyInfoId = DBNull.Value.Equals(dr["CompanyInfoId"]) ? 0 : Convert.ToInt32(dr["CompanyInfoId"]);
                    aMaster.FinancialYearId = DBNull.Value.Equals(dr["FinancialYearId"]) ? 0 : Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster.PostingDateStr = dr["PostingDateStr"].ToString();
                    aMaster.VoucherNo = dr["VoucherNo"].ToString();
                    aMaster.CurrencyId = DBNull.Value.Equals(dr["CurrencyId"]) ? 0 : Convert.ToInt32(dr["CurrencyId"]);
                    aMaster.CurrencyCovRate = DBNull.Value.Equals(dr["CurrencyCovRate"]) ? 0 : Convert.ToDecimal(dr["CurrencyCovRate"]);
                    aMaster.VoucherCategoryID = DBNull.Value.Equals(dr["VoucherCategoryID"]) ? 0 : Convert.ToInt32(dr["VoucherCategoryID"]);
                    aMaster.BankAccountId = DBNull.Value.Equals(dr["BankAccountId"]) ? 0 : Convert.ToInt32(dr["BankAccountId"]);


                    aMaster.ChequeNo = dr["ChequeNo"].ToString();
                    aMaster.ChequeDateStr = dr["ChequeDateStr"].ToString();

                    aMaster.CashAccountId = DBNull.Value.Equals(dr["CashAccountId"]) ? 0 : Convert.ToInt32(dr["CashAccountId"]);


                    aMaster.RefNo = dr["RefNo"].ToString();
                    aMaster.RefDateStr = dr["RefDateStr"].ToString();


                    aMaster.Narration = dr["Narration"].ToString();
                    aMaster.TotalAmountBDT = DBNull.Value.Equals(dr["TotalAmountBDT"]) ? 0 : Convert.ToDecimal(dr["TotalAmountBDT"]);


                    aMaster.ToCompanyInfoId = DBNull.Value.Equals(dr["ToCompanyInfoId"]) ? 0 : Convert.ToInt32(dr["ToCompanyInfoId"]);


                    aMaster.ToFinancialYearId = DBNull.Value.Equals(dr["ToFinancialYearId"]) ? 0 : Convert.ToInt32(dr["ToFinancialYearId"]);


                    aMaster.FromCoaId = DBNull.Value.Equals(dr["FromCoaId"]) ? 0 : Convert.ToInt32(dr["FromCoaId"]);


                    aMaster.ToCoaId = DBNull.Value.Equals(dr["ToCoaId"]) ? 0 : Convert.ToInt32(dr["ToCoaId"]);


                }
                return aMaster;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

    }
}