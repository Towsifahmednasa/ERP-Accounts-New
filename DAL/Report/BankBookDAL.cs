// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\BankBookDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Report
{
    public class BankBookDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();


        public DataTable GetBankBookReportBlankDAL(int CompanyID, int BankAccountID, int FinancialYearID,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {

            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", BankNAme));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));

                DataTable dt = _accessManager.GetDataTable("sp_SP_BankBookBlank", aSqlParameterlist);
                return dt;

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


        public DataTable GetBankBookReportDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            DataTable dt = null;
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", BankNAme));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));

                dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_CashBook", aSqlParameterlist);
                return dt;
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


        public DataTable GetOpeningBalanceReportDAL(int CompanyID, int BankAccountID, int FinancialYearID,
           DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {

            DataTable dt = null;
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@CoaAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@BankBookBankNAme", BankNAme));
                aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));

                dt = _accessManager.GetDataTable("sp_GET_COAAccountOpeningBalance", aSqlParameterlist);

                return dt;
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


        public DataTable GetBankBookReportMonthlySummeryDAL(int CompanyID, int BankAccountID, int FinancialYearID,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", BankNAme));


                dr = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_MonthlySummery", aSqlParameterlist);
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