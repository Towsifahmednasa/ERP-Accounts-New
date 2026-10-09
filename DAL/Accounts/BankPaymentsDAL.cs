// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\BankPaymentsDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{


    public class BankPaymentsDAL
    {

        DataAccessManager _accessManager = new DataAccessManager();
        public List<DebitVoucherView> BankPaymentList( string param)
        {
            try
            {

                 
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<DebitVoucherView> aList = new List<DebitVoucherView>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@param", param));

                //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_DebitVoucherList", aParameters);
                while (dr.Read())
                {
                    DebitVoucherView aMaster = new DebitVoucherView();
                    aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.VoucherCode = dr["VoucherCode"].ToString();
                    aMaster.VoucherNo = dr["VoucherNo"].ToString();
                    aMaster.PostingDate = (dr["PostingDate"].ToString());
                    aMaster.VoucherCategory = (dr["VoucherCategory"].ToString());
                    aMaster.Narration = (dr["Narration"].ToString());

                    aMaster.TotalAmountBDT = (dr["TotalAmountBDT"].ToString());
                    aMaster.EntryBy = (dr["EntryBy"].ToString());
                    aMaster.EntryDate = (dr["EntryDate"].ToString());
                    aMaster.UpdateBy = (dr["UpdateBy"].ToString());
                    aMaster.UpdateDate = (dr["UpdateDate"].ToString());
                    aMaster.VoucherType = (dr["VoucherType"].ToString());
                    aMaster.ActionStatus = (dr["ActionStatus"].ToString());







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
                _accessManager.SqlConnectionClose();
            }
        }

        public ResultResponse SaveBankPaymentMaster(DebitCreditVoucherMaster aDebitCreditVoucherNewMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", aDebitCreditVoucherNewMaster.VoucherMasterId));
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", (object)(aDebitCreditVoucherNewMaster.CompanyInfoId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)(aDebitCreditVoucherNewMaster.FinancialYearId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@PostingDate", (object)(aDebitCreditVoucherNewMaster.PostingDate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@VoucherNo", (object)(aDebitCreditVoucherNewMaster.VoucherNo ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@VoucherCategoryID", (object)(aDebitCreditVoucherNewMaster.VoucherCategoryID ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CurrencyId", (object)(aDebitCreditVoucherNewMaster.CurrencyId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@CurrencyCovRate", (object)(aDebitCreditVoucherNewMaster.CurrencyCovRate ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@Narration", (object)(aDebitCreditVoucherNewMaster.Narration ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsCostCenter", (object)(aDebitCreditVoucherNewMaster.IsCostCenter ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsBuyerCostCenter", (object)(aDebitCreditVoucherNewMaster.IsBuyerCostCenter ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsOtherCostCenter", (object)(aDebitCreditVoucherNewMaster.IsOtherCostCenter ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@ReceiveFromIsBuyer", (object)(aDebitCreditVoucherNewMaster.ReceiveFromIsBuyer ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@ByerId", (object)(aDebitCreditVoucherNewMaster.ByerId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@PaytoAndReceiveFromIsOther", (object)(aDebitCreditVoucherNewMaster.PaytoAndReceiveFromIsOther ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@Other", (object)(aDebitCreditVoucherNewMaster.Other ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@OtherAdress", (object)(aDebitCreditVoucherNewMaster.OtherAdress ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@PaytoIsSupplier", (object)(aDebitCreditVoucherNewMaster.PaytoIsSupplier ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@SupplierId", (object)(aDebitCreditVoucherNewMaster.SupplierId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsInvoice", (object)(aDebitCreditVoucherNewMaster.IsInvoice ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@TotalAmountBDT", (object)(aDebitCreditVoucherNewMaster.TotalAmountBDT ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@TotalAmount", (object)(aDebitCreditVoucherNewMaster.TotalAmount ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@Remarks", (object)(aDebitCreditVoucherNewMaster.Remarks ?? (object)DBNull.Value)));
             
                aSqlParameterlist.Add(new SqlParameter("@UpdateDate", (object)(aDebitCreditVoucherNewMaster.UpdateBy ?? (object)DBNull.Value)));
                
                
             aSqlParameterlist.Add(new SqlParameter("@IsExportInvoice", (aDebitCreditVoucherNewMaster.IsExportInvoice ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@IsImportInvoice", (aDebitCreditVoucherNewMaster.IsImportInvoice ?? (object)DBNull.Value)));


                if (aDebitCreditVoucherNewMaster.VoucherCategoryID == 1)
                {
                    aDebitCreditVoucherNewMaster.VoucherType = "DebitVoucherBank";
                    aSqlParameterlist.Add(new SqlParameter("@BankCheckNo", (object)(aDebitCreditVoucherNewMaster.ChequeNo ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@BankCheckDate", (object)(aDebitCreditVoucherNewMaster.ChequeDate ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@BankAccountId", (aDebitCreditVoucherNewMaster.BankAccountId ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@ChequeNo", (aDebitCreditVoucherNewMaster.ChequeNo ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@ChequeDate", (aDebitCreditVoucherNewMaster.ChequeDate ?? (object)DBNull.Value)));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@BankCheckNo", (object)(null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@BankCheckDate", (object)(null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@BankAccountId", (null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@ChequeNo", (null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@ChequeDate", (null ?? (object)DBNull.Value)));
                }
                if (aDebitCreditVoucherNewMaster.VoucherCategoryID ==  2 )
                {
                    aDebitCreditVoucherNewMaster.VoucherType = "DebitVoucherCash";
                    aSqlParameterlist.Add(new SqlParameter("@CashRefNo", (object)(aDebitCreditVoucherNewMaster.RefNo ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@CashRefDate", (object)(aDebitCreditVoucherNewMaster.RefDate ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@RefNo", (aDebitCreditVoucherNewMaster.RefNo ?? (object)DBNull.Value))); aSqlParameterlist.Add(new SqlParameter("@RefDate", (aDebitCreditVoucherNewMaster.RefDate ?? (object)DBNull.Value)));



                    aSqlParameterlist.Add(new SqlParameter("@CashAccountId", (aDebitCreditVoucherNewMaster.CashAccountId ?? (object)DBNull.Value)));

                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@CashRefNo", (object)(null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@CashRefDate", (object)(null ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@RefNo", (null ?? (object)DBNull.Value))); aSqlParameterlist.Add(new SqlParameter("@RefDate", (null ?? (object)DBNull.Value)));



                    aSqlParameterlist.Add(new SqlParameter("@CashAccountId", (null ?? (object)DBNull.Value)));

                }


                aSqlParameterlist.Add(new SqlParameter("@VoucherType", (object)(aDebitCreditVoucherNewMaster.VoucherType ?? (object)DBNull.Value)));


                aSqlParameterlist.Add(new SqlParameter("@EntryBy", user));
                aSqlParameterlist.Add(new SqlParameter("@EntryDate",    System.DateTime.Now ));

                // Park feature: 'Parked' keeps the voucher out of the approval queue; anything else behaves as before ('Posted').
                aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aDebitCreditVoucherNewMaster.ActionStatus == "Parked" ? "Parked" : "Posted"));


                if (aDebitCreditVoucherNewMaster.VoucherMasterId > 0)
                {
                    aResponse.isSuccess = _accessManager.UpdateData("sp_UD_VoucherMaster_New", aSqlParameterlist);

                    aResponse.pk = aDebitCreditVoucherNewMaster.VoucherMasterId;
                }
                //ll
                else
                {
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_VoucherMaster", aSqlParameterlist);
                }
                
                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = SaveBankPaymentDetails(aDebitCreditVoucherNewMaster.DetailsInfo, aResponse.pk, aDebitCreditVoucherNewMaster.VoucherCategoryID);

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



        public bool SaveCurrencyConversionDetails(List<CurrencyConversionDtls> aList, DateTime? PostingDate, string user)
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                   
                    aSqlParameterlist.Add(new SqlParameter("@PostingDate", (object)( PostingDate ?? (object)DBNull.Value)));

                     
                    aSqlParameterlist.Add(new SqlParameter("@CurrencyRate", (object)(item.CurrencyRate ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@CurrencyId", (object)(item.CurrencyId ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@user", (object)(user ?? (object)DBNull.Value)));
                   


                    result = _accessManager.SaveData("sp_iNS_CurrencyConversion", aSqlParameterlist);


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


        


        public bool SaveBankPaymentDetails(List<DebitCreditVoucherDetail> aList, int pk, int? vCAt)
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                    aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", pk));
                    aSqlParameterlist.Add(new SqlParameter("@AccountId", (object)(item.AccountId ?? (object)DBNull.Value)));

                    
                        item.DebitorCredit = "Debit";
                        item.CreditAmountBDT = 0;
                        item.CreditAmount = 0;

                    
                    aSqlParameterlist.Add(new SqlParameter("@DebitorCredit", (object)(item.DebitorCredit ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@DebitAmountBDT", (object)(item.DebitAmountBDT ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@CreditAmountBDT", (object)(item.CreditAmountBDT ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@DebitAmount", (object)(item.DebitAmount ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@CreditAmount", (object)(item.CreditAmount ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@VoucherCategoryId", (object)(null ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@CostCenterId", (object)(item.CostCenterId ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@RefInfoInd", (object)(item.RefInfoInd ?? (object)DBNull.Value)));

                    aSqlParameterlist.Add(new SqlParameter("@SubsidoryLedgerId", (object)(item.SubsidoryLedgerId ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@Remarks", (object)(item.Remarks ?? (object)DBNull.Value)));
                   


                    result = _accessManager.SaveData("sp_I_VoucherDetail_new", aSqlParameterlist);


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



        public DebitCreditVoucherMaster GetBankPaymentMasterId(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                DebitCreditVoucherMaster aMaster = new DebitCreditVoucherMaster();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BankPaymentMasterById", aParameters);
                while (dr.Read())
                {
                    aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]); 
                    aMaster.CompanyInfoId =  DBNull.Value.Equals(dr["CompanyInfoId"]) ? 0 : Convert.ToInt32(dr["CompanyInfoId"]);
                    aMaster.FinancialYearId = DBNull.Value.Equals(dr["FinancialYearId"]) ? 0 : Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster.PostingDateStr = dr["PostingDateStr"].ToString();
                    aMaster.VoucherNo = dr["VoucherNo"].ToString(); 
                    aMaster.CurrencyId = DBNull.Value.Equals(dr["CurrencyId"]) ? 0 : Convert.ToInt32(dr["CurrencyId"]);
                    aMaster.CurrencyCovRate =  DBNull.Value.Equals(dr["CurrencyCovRate"]) ? 0 : Convert.ToDecimal(dr["CurrencyCovRate"]);
                    aMaster.VoucherCategoryID = DBNull.Value.Equals(dr["VoucherCategoryID"]) ? 0 : Convert.ToInt32(dr["VoucherCategoryID"]); 
                    aMaster.BankAccountId = DBNull.Value.Equals(dr["BankAccountId"]) ? 0 : Convert.ToInt32(dr["BankAccountId"]);


                    aMaster.ChequeNo = dr["ChequeNo"].ToString();
                    aMaster.ChequeDateStr = dr["ChequeDateStr"].ToString();

                    aMaster.CashAccountId = DBNull.Value.Equals(dr["CashAccountId"]) ? 0 : Convert.ToInt32(dr["CashAccountId"]);


                    aMaster.RefNo = dr["RefNo"].ToString();
                    aMaster.RefDateStr = dr["RefDateStr"].ToString();


                    aMaster.Narration = dr["Narration"].ToString();
                   

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

        public List<DebitCreditVoucherDetail> GetBankPaymentDetailsById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<DebitCreditVoucherDetail> aDetails = new List<DebitCreditVoucherDetail>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BankPaymentDetailById", aParameters);
                while (dr.Read())
                {
                    DebitCreditVoucherDetail deta = new DebitCreditVoucherDetail();
                    deta.VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]);
                    deta.VoucherMasterId = DBNull.Value.Equals(dr["VoucherMasterId"]) ? 0 : Convert.ToInt32(dr["VoucherMasterId"]);
                    deta.AccountId = DBNull.Value.Equals(dr["AccountId"]) ? 0 : Convert.ToInt32(dr["AccountId"]);

                    deta.CostCenterId = DBNull.Value.Equals(dr["CostCenterId"]) ? 0 : Convert.ToInt32(dr["CostCenterId"]);

                    deta.RefInfoInd = DBNull.Value.Equals(dr["RefInfoInd"]) ? 0 : Convert.ToInt32(dr["RefInfoInd"]);

                    deta.SubsidoryLedgerId = DBNull.Value.Equals(dr["SubsidoryLedgerId"]) ? 0 : Convert.ToInt32(dr["SubsidoryLedgerId"]);


                    
                    deta.Amount = DBNull.Value.Equals(dr["Amount"]) ? 0 : Convert.ToDecimal(dr["Amount"]);
                     
                    deta.Remarks = dr["Remarks"].ToString();
                    aDetails.Add(deta);
                }
                return aDetails;
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
        private AccountsReportDAL accountsReport = new AccountsReportDAL();

        public string GetClosingBlnc(int CompanyID, int BankAccountID, int FinancialYearID,
              DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@AccountId", BankAccountID));
                aSqlParameters.Add(new SqlParameter("@CompanyId", CompanyID));
                aSqlParameters.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameters.Add(new SqlParameter("@ToDate", ToDate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_AccountClosingBalance_TrialBalanceWise", aSqlParameters);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ClosingBalance"] != DBNull.Value)
                {
                    decimal closing = Convert.ToDecimal(dt.Rows[0]["ClosingBalance"]);
                    return closing.ToString("0.00");
                }
                return "0.00";
            }
            catch (Exception)
            {
                return "0.00";
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public string GetCashClosingBlnc(int CompanyID, int CashAccountID, int FinancialYearID,
              DateTime FromDate, DateTime ToDate, string CashbookNAme, DateTime CashBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@AccountId", CashAccountID));
                aSqlParameters.Add(new SqlParameter("@CompanyId", CompanyID));
                aSqlParameters.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameters.Add(new SqlParameter("@ToDate", ToDate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_AccountClosingBalance_TrialBalanceWise", aSqlParameters);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ClosingBalance"] != DBNull.Value)
                {
                    decimal closing = Convert.ToDecimal(dt.Rows[0]["ClosingBalance"]);
                    return closing.ToString("0.00");
                }
                return "0.00";
            }
            catch (Exception)
            {
                return "0.00";
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }


        public BankAccountDAO GetBankAccountInfo_ById(int CompanyID, int BankAccountID, int FinancialYearID,
              DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                BankAccountDAO aMaster = new BankAccountDAO();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@AccountId", BankAccountID));
              
                DataTable dt = _accessManager.GetDataTable("sp_GET_BankInfoByAccountId", aSqlParameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    aMaster.BankName = dt.Rows[0]["BankName"] != null ? dt.Rows[0]["BankName"].ToString() : "";
                    aMaster.BranchName = dt.Rows[0]["BranchName"] != null ? dt.Rows[0]["BranchName"].ToString() : "";
                }

                aMaster.BankBlnc = GetClosingBlnc(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate);

                return aMaster;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }




        public BankAccountDAO GetBankCashInfoMasterId(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                BankAccountDAO aMaster = new BankAccountDAO();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@AccountId", BankAccountID));
                DataTable dtBankCash = new DataTable();
                  dtBankCash = _accessManager.GetDataTable("sp_GET_BankInfoByAccountId", aSqlParameters);

                if (dtBankCash.Rows.Count == 0)
                {
                    dtBankCash = _accessManager.GetDataTable("sp_GET_CashInfoByAccountId", aSqlParameters);

                    aMaster.BankBlnc = GetCashClosingBlnc(CompanyID, BankAccountID, FinancialYearID,
             FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate);
                }
                else
                {

                    aMaster.BankBlnc = GetClosingBlnc(CompanyID, BankAccountID, FinancialYearID,
             FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate);
                }


            
                
                return aMaster;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }



        public BankAccountDAO GetCashInfoIndoById(int CompanyID, int CashAccountID, int FinancialYearID,
                              DateTime FromDate, DateTime ToDate, string CashbookNAme, DateTime CashBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                BankAccountDAO aMaster = new BankAccountDAO();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@AccountId", CashAccountID));

                DataTable dt = _accessManager.GetDataTable("sp_GET_CashInfoByAccountId", aSqlParameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    aMaster.BankName = dt.Rows[0]["BankName"] != null ? dt.Rows[0]["BankName"].ToString() : "";
                    aMaster.BranchName = dt.Rows[0]["BranchName"] != null ? dt.Rows[0]["BranchName"].ToString() : "";
                }

                aMaster.BankBlnc = GetCashClosingBlnc(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate);

                return aMaster;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public ResultResponse SaveCurrencyConversion(CurrencyConversionMsterDao aDebitCreditVoucherNewMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@PostingDate", aDebitCreditVoucherNewMaster.PostingDate));
                 
               aSqlParameterlist.Add(new SqlParameter("@UpdateBy", user));
                 
              


               
                    aResponse.isSuccess = _accessManager.UpdateData("sp_del_CurrencyConversion", aSqlParameterlist);

                    
                 
                if (aResponse.isSuccess )
                {
                    aResponse.isSuccess = SaveCurrencyConversionDetails(aDebitCreditVoucherNewMaster.CurrencyConversionDtls, aDebitCreditVoucherNewMaster.PostingDate, user);

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

        internal DataTable Load_CurrencyConversionList(string date)
        {
            DataTable dataTable = new DataTable();
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@date", date));
                dataTable = _accessManager.GetDataTable("sp_GET_AllCurrencyConversionByDate", aParameters);
            }
            catch
            {
                _accessManager.SqlConnectionClose(true);
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
            return dataTable;
        }

    }
}