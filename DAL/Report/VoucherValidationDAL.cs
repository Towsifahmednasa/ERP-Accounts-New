// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\VoucherValidationDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.ReportModel;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
namespace New_Accounts.DAL.Report
{
    public class VoucherValidationDAL
    {
        private readonly DataAccessManager _accessManager = new DataAccessManager();

        public IList<VoucherValidationRow> GetVoucherValidation(VoucherValidationFilter filter)
        {
            var results = new List<VoucherValidationRow>();

            try
            {
                //_accessManager.SqlConnectionOpen(DataBase.ControlPanel);
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@FromDate", (object)filter.FromDate ?? DBNull.Value),
                    new SqlParameter("@Todate", (object)filter.ToDate ?? DBNull.Value),
                    new SqlParameter("@PageName", string.IsNullOrWhiteSpace(filter.PageName) ? (object)DBNull.Value : filter.PageName),
                    new SqlParameter("@IntegrityStatus", string.IsNullOrWhiteSpace(filter.IntegrityStatus) ? (object)DBNull.Value : filter.IntegrityStatus)
                };

                // Stored proc name based on prior discussion.
                //DataTable dt = _accessManager.GetDataTable("sp_ValidateVoucherPostings", parameters);
                DataTable dt = _accessManager.GetDataTable("sp_RPT_AccountsIntigrityReport", parameters);

                IEnumerable<DataRow> rows = dt.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(filter.IntegrityStatus))
                {
                    rows = rows.Where(r => string.Equals(Convert.ToString(r["VoucherLog"]), filter.IntegrityStatus, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(filter.PageName))
                {
                    rows = rows.Where(r => string.Equals(Convert.ToString(r["PageName"]), filter.PageName, StringComparison.OrdinalIgnoreCase));
                }

                foreach (var r in rows)
                {
                    results.Add(new VoucherValidationRow
                    {
                        PageName = Convert.ToString(r["PageName"]),
                        TransRefNo = Convert.ToString(r["TransRefNo"]),
                        TransDate = Convert.ToString(r["TransDate"]),
                        VoucherType = Convert.ToString(r["VoucherType"]),
                        TransactionType = Convert.ToString(r["TranscationType"]),

                        CommonPK_ID = r["CommonPK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["CommonPK_ID"]),
                        TableID = r["TableID"] == DBNull.Value ? 0 : Convert.ToInt32(r["TableID"]),
                        InvoiceId = r["InvoiceId"] == DBNull.Value ? 0 : Convert.ToInt32(r["InvoiceId"]),
                        VoucherMasterId = r["VoucherMasterId"] == DBNull.Value ? 0 : Convert.ToInt32(r["VoucherMasterId"]),
                        TranscationId = r["TranscationId"] == DBNull.Value ? 0 : Convert.ToInt32(r["TranscationId"]),
                                                              
                        VoucherCode = Convert.ToString(r["VoucherCode"]),
                        PostingDate = Convert.ToString(r["PostingDate"]),
                        SourceRefNo = Convert.ToString(r["SourceRefNo"]),
                        TransactionAmount = r["TranscationAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TranscationAmount"]),
                        VoucherAmount = r["VoucherAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(r["VoucherAmount"]),
                        VoucherLog = Convert.ToString(r["VoucherLog"])
                    });
                }
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }

            return results;
        }

        public ResultResponse ReprocessVoucherAnomalies(List<VoucherReprocessRequestDao> items)
        {
            ResultResponse aResponse = new ResultResponse();

            try
            {
                if (items == null || items.Count == 0)
                {
                    aResponse.isSuccess = false;
                    aResponse.pk = 0;
                    return aResponse;
                }

                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                int processedCount = 0;
                bool allSuccess = true;

                foreach (var item in items)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                    aSqlParameterlist.Add(new SqlParameter("@TableId", item.TableId));
                    aSqlParameterlist.Add(new SqlParameter("@CommonPkId", item.CommonPkId));
                    aSqlParameterlist.Add(new SqlParameter("@TransctionErrorType",
                                            (object)(item.TransctionErrorType ?? (object)DBNull.Value)));
                    aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", item.VoucherMasterId));
                    aSqlParameterlist.Add(new SqlParameter("@CusPaymentInvoiceId", item.CusPaymentInvoiceId));
                    aSqlParameterlist.Add(new SqlParameter("@TranscationId", item.TranscationId));

                    bool result = _accessManager.UpdateData("sp_AccIntegrity_AnomaliesCorrection", aSqlParameterlist);

                    if (result)
                    {
                        processedCount++;
                    }
                    else
                    {
                        allSuccess = false;
                    }
                }

                aResponse.pk = processedCount;
                aResponse.isSuccess = allSuccess && processedCount > 0;

                if (!aResponse.isSuccess)
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


        public IList<VoucherValidationPageOption> GetPageNames()
        {
            var list = new List<VoucherValidationPageOption>();
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.ControlPanel);
                var dt = _accessManager.GetDataTable("sp_DDD_AccountsIntigrityPages");
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new VoucherValidationPageOption { PageName = Convert.ToString(row["PageName"]) });
                }
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
            return list;
        }
    }
}
