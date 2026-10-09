// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\JournalDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class JournalDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();


        public ResultResponse SaveJournalMaster(DebitCreditVoucherMaster aDebitCreditVoucherNewMaster, string user)
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
                    aDebitCreditVoucherNewMaster.VoucherType = "CreditVoucherBank";
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
                if (aDebitCreditVoucherNewMaster.VoucherCategoryID == 2)
                {
                    aDebitCreditVoucherNewMaster.VoucherType = "CreditVoucherCash";
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

                aDebitCreditVoucherNewMaster.VoucherType = "Journal";
                aSqlParameterlist.Add(new SqlParameter("@VoucherType", (object)(aDebitCreditVoucherNewMaster.VoucherType ?? (object)DBNull.Value)));


                aSqlParameterlist.Add(new SqlParameter("@EntryBy", user));
                aSqlParameterlist.Add(new SqlParameter("@EntryDate", System.DateTime.Now));

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



        public DebitCreditVoucherMaster GetCreditVoucherMasterId(int id)
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

                    aMaster.ActionStatus = dr["ActionStatus"].ToString();

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

        public List<DebitCreditVoucherDetail> GetCreditVoucherDetailsById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<DebitCreditVoucherDetail> aDetails = new List<DebitCreditVoucherDetail>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_JournalDetailById", aParameters);
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
                    deta.DebitorCredit = dr["DebitorCredit"].ToString();
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


        // ---- Voucher attachments (tblVoucherAttachment) ----

        public int SaveVoucherAttachment(VoucherAttachment a, string user)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@VoucherMasterId", a.VoucherMasterId));
                p.Add(new SqlParameter("@FileName", a.FileName));
                p.Add(new SqlParameter("@ContentType", (object)(a.ContentType ?? (object)DBNull.Value)));
                p.Add(new SqlParameter("@FileSize", a.FileSize));
                p.Add(new SqlParameter("@FileData", a.FileData));
                p.Add(new SqlParameter("@EntryBy", (object)(user ?? (object)DBNull.Value)));
                return _accessManager.SaveDataReturnPrimaryKey("sp_I_VoucherAttachment", p);
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public List<VoucherAttachment> GetVoucherAttachments(int masterId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<VoucherAttachment> aList = new List<VoucherAttachment>();
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@VoucherMasterId", masterId));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_VoucherAttachmentsByMasterId", p);
                while (dr.Read())
                {
                    aList.Add(new VoucherAttachment
                    {
                        AttachmentId = Convert.ToInt32(dr["AttachmentId"]),
                        FileName = dr["FileName"].ToString(),
                        ContentType = dr["ContentType"].ToString(),
                        FileSize = Convert.ToInt64(dr["FileSize"]),
                        EntryBy = dr["EntryBy"].ToString(),
                        EntryDate = DBNull.Value.Equals(dr["EntryDate"]) ? (DateTime?)null : Convert.ToDateTime(dr["EntryDate"])
                    });
                }
                return aList;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public VoucherAttachment GetVoucherAttachment(int attachmentId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@AttachmentId", attachmentId));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_VoucherAttachmentById", p);
                VoucherAttachment a = null;
                while (dr.Read())
                {
                    a = new VoucherAttachment
                    {
                        AttachmentId = Convert.ToInt32(dr["AttachmentId"]),
                        VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]),
                        FileName = dr["FileName"].ToString(),
                        ContentType = dr["ContentType"].ToString(),
                        FileSize = Convert.ToInt64(dr["FileSize"]),
                        FileData = (byte[])dr["FileData"]
                    };
                }
                return a;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public bool DeleteVoucherAttachment(int attachmentId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@AttachmentId", attachmentId));
                return _accessManager.UpdateData("sp_D_VoucherAttachment", p);
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }


        // ---- FI period close (tblFiPeriodClose) ----

        public bool IsPeriodClosed(int companyId, DateTime date)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CompanyInfoId", companyId));
                p.Add(new SqlParameter("@Date", date.Date));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_FiPeriodCloseByDate", p);
                int n = 0;
                while (dr.Read()) { n = Convert.ToInt32(dr[0]); }
                return n > 0;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public List<FiPeriodClose> GetClosedPeriods(int companyId, int year)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<FiPeriodClose> aList = new List<FiPeriodClose>();
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CompanyInfoId", companyId));
                p.Add(new SqlParameter("@PeriodYear", year));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_FiPeriodClose", p);
                while (dr.Read())
                {
                    aList.Add(new FiPeriodClose
                    {
                        PeriodMonth = Convert.ToInt32(dr["PeriodMonth"]),
                        IsClosed = Convert.ToBoolean(dr["IsClosed"]),
                        UpdateBy = dr["UpdateBy"].ToString(),
                        UpdateDate = DBNull.Value.Equals(dr["UpdateDate"]) ? (DateTime?)null : Convert.ToDateTime(dr["UpdateDate"])
                    });
                }
                return aList;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }

        public bool SetPeriodClose(int companyId, int year, int month, bool isClosed, string user)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CompanyInfoId", companyId));
                p.Add(new SqlParameter("@PeriodYear", year));
                p.Add(new SqlParameter("@PeriodMonth", month));
                p.Add(new SqlParameter("@IsClosed", isClosed));
                p.Add(new SqlParameter("@UpdateBy", (object)(user ?? (object)DBNull.Value)));
                return _accessManager.UpdateData("sp_SET_FiPeriodClose", p);
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }
    }
}