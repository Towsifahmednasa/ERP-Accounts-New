// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\ReportDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Report
{
    public class ReportDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();

        public DataTable GetReportHeaderDal()
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
               
                    dr = _accessManager.GetDataTable("sp_GET_AllReportHeaderReport");
               

                return dr;
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


        public DataTable GetDebitVoucherMasterDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherMaster", parameters);


                return dr;
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


        public DataTable GetDebitVoucherDetailDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherDetail", parameters);


                return dr;
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


        /// <summary>
        /// voucher details to help my query
        /// 
        /// </summary>
        /// <param name="VoucherMasterId"></param>
        /// <returns></returns>
        public DataTable GetCreditVoucherDetailDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherDetail", parameters);


                return dr;
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


        public DataTable GetJournalDetailDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherDetail", parameters);


                return dr;
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


        public DataTable GetFundTransferDetailDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherDetail", parameters);


                return dr;
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

        public DataTable GetCostcenterDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherMultiCostCenter", parameters);


                return dr;
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


        public DataTable GetInvoiceDal(string VoucherMasterId)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@VoucherMasterId", VoucherMasterId));
                dr = _accessManager.GetDataTable("sp_Rpt_DebitCreditVoucherMultiInvoice", parameters);


                return dr;
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