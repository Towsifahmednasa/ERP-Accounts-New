// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Report\AccountsReportDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Report
{
    public class AccountsReportDAL
    {


        DataAccessManager _accessManager = new DataAccessManager();



        public DataTable GetChartofAccountDAL(string CompanyID)
        {
            try
            {
                DataTable dr = new DataTable();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                dr = _accessManager.GetDataTable("sp_Rpt_ChartofAccount", aSqlParameterlist);


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
        //public DataTable GetBankBookReportDAL(int CompanyID, int BankAccountID, int FinancialYearID,
        //      DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet)
        //{
        //    try
        //    {
        //        DataTable dr = new DataTable();
        //        _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //        aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
        //        aSqlParameterlist.Add(new SqlParameter("@BankAccountId", BankAccountID));
        //        aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
        //        aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
        //        aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
        //        aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", BankNAme));

        //        dr = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_CashBook", aSqlParameterlist);


        //        return dr;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        _accessManager.SqlConnectionClose();
        //    }
        //}

        public DataTable GetOpeningBalanceReportDALNew(int CompanyID,int financialYearId, int BankAccountID, int SubsidiaryID,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {

            DataTable dt = null;
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@CoaAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", financialYearId));
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryID", SubsidiaryID));
                aSqlParameterlist.Add(new SqlParameter("@OpeningBalance", "0"));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@BankBookBankNAme", BankNAme));
                aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));

                dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance", aSqlParameterlist);

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
       public DataTable GetOpeningBalancesubsidiary(int CompanyID, int financialYearId, int BankAccountID, int SubsidiaryID,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {

            DataTable dt = null;
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", financialYearId));
                aSqlParameterlist.Add(new SqlParameter("@CoaAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryID", SubsidiaryID));
                aSqlParameterlist.Add(new SqlParameter("@OpeningBalance", "0"));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@BankBookBankNAme", BankNAme));
                aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));

                dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance_supplier", aSqlParameterlist);

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




        public DataTable GetCOAForTrailBalanceDaLTB(string GL, string Control, string Subsidary,int CompanyID)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@GL", GL));
            aSqlParameterlist.Add(new SqlParameter("@Control", Control));
            aSqlParameterlist.Add(new SqlParameter("@Subsidary", Subsidary));
            aSqlParameterlist.Add(new SqlParameter("@comId", CompanyID));


            DataTable dt = _accessManager.GetDataTable("sp_COAForTrailBalanceParameterwise_New", aSqlParameterlist);
            _accessManager.SqlConnectionClose(); 
            return dt;
        }

        public DataTable GetTrialBalanceReportDaLTB(int CompanyID, int BankAccountID, int FinancialYearID,
               DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@BankAccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
            aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
            aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", BankNAme));

            DataTable dt = _accessManager.GetDataTable("sp_GET_TrailBalance_Transection", aSqlParameterlist);
            _accessManager.SqlConnectionClose();
            return dt;
        }
        public DataTable GetOpeningBalanceReportBLLTB(int CompanyID, int BankAccountID, int FinancialYearID,
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
                _accessManager.SqlConnectionClose();

            }
            catch (Exception)
            {

                // throw;
            }

            return dt;

        }

        public DataTable GetInventoryOpeningBalanceBLLTB(DateTime Fdate, int CompanyID, int FinancialYearID, int BankAccountID
   )
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@Fdate", Fdate));

            DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalance_NEW2", aSqlParameterlist);
            _accessManager.SqlConnectionClose();
            return dt;
        }

        public DataTable GetInventoryClosingBalanceBLLTB(DateTime Fdate, int CompanyID, int FinancialYearID, int BankAccountID
    )
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@Fdate", Fdate));

            DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalance_NEW", aSqlParameterlist);
            _accessManager.SqlConnectionClose();
            return dt;
        }

        public DataTable GetComProfitBLLTB(int CompanyID, int FinancialYearID)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));

            DataTable dt = _accessManager.GetDataTable("sp_GET_CompanyProfit", aSqlParameterlist);
            _accessManager.SqlConnectionClose();
            return dt;
        }

        public DataTable GetOpeningBalancereferenceId(int CompanyID, int financialYearId, int BankAccountID, int referenceId,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {

            DataTable dt = null;
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", financialYearId));
                aSqlParameterlist.Add(new SqlParameter("@CoaAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@referenceId", referenceId));
                aSqlParameterlist.Add(new SqlParameter("@OpeningBalance", "0"));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@BankBookBankNAme", BankNAme));
                aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));

                dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_Summary_ForOppBalance_reference", aSqlParameterlist);

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




        public DataTable GetBankBookReportBlankDAL(int CompanyID, int BankAccountID, int FinancialYearID,
         DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet)
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
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet)
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




        public DataTable GetCashBookReportNewDaL(int CompanyID, int CashAccountID, int FinancialYearID,
      DateTime FromDate, DateTime ToDate, string CashbookNAme, DateTime CashBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@BankAccountId", CashAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@ChartofAccountName", CashbookNAme));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", CashBookFinancialYearStartDate));
                DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_CashBook", aSqlParameterlist);


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



        public DataTable GetAllCashFlowData(DateTime fdate, DateTime todate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                //aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                //aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@fdate", fdate));
                aSqlParameterlist.Add(new SqlParameter("@todate", todate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_CashFlowSummaryBDTCurrencyAllCompany", aSqlParameterlist);


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



        public DataTable GetCashFlowOtherDAL(string FinancialYearID, string CompanyID, DateTime fdate, DateTime todate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@fdate", fdate));
                aSqlParameterlist.Add(new SqlParameter("@todate", todate));
                DataTable dt = _accessManager.GetDataTable("sp_GET_CashFlowSummaryOtherCurrency", aSqlParameterlist);


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

        public DataTable GetCashFlowBDTDAL(string FinancialYearID, string CompanyID, DateTime fdate, DateTime todate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@fdate", fdate));
                aSqlParameterlist.Add(new SqlParameter("@todate", todate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_CashFlowSummaryBDTCurrency", aSqlParameterlist);


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

        public DataTable GetLedgerReportDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet)
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
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));
                   
                DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerOnly", aSqlParameterlist);


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

        public DataTable GetLedgerReportFinalDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet, string   TypeSubCS, string SubsidiaryID, string CsID)
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

                if(TypeSubCS== "undefined")
                {
                    aSqlParameterlist.Add(new SqlParameter("@TypeSubCS", ""));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@TypeSubCS", TypeSubCS));
                }

        
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryID", SubsidiaryID));
                aSqlParameterlist.Add(new SqlParameter("@CsID", CsID));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));


                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerOnly", aSqlParameterlist);
                DataTable dt = _accessManager.GetDataTable("gl_Rpt_Cr_Proc_GL", aSqlParameterlist);
                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerAcc", aSqlParameterlist);


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

        public DataTable GetLedgerReportFinalSubsidaryDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet, string   TypeSubCS, string SubsidiaryID, string CsID)
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

                if(TypeSubCS== "undefined")
                {
                    aSqlParameterlist.Add(new SqlParameter("@TypeSubCS", ""));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@TypeSubCS", TypeSubCS));
                }

        
                aSqlParameterlist.Add(new SqlParameter("@SubsidiaryID", SubsidiaryID));
                aSqlParameterlist.Add(new SqlParameter("@CsID", CsID));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));


                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerOnly", aSqlParameterlist);
                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerAcc_Subsidary", aSqlParameterlist);
                DataTable dt = _accessManager.GetDataTable("gl_Rpt_Cr_Proc_Subsidiary_Ledger", aSqlParameterlist);


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
        
        public DataTable GetLedgerReportFinalReferenceDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet, string Typereference, string referenceId, string CsID)
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

                if(Typereference == "undefined")
                {
                    aSqlParameterlist.Add(new SqlParameter("@Typereference", ""));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@Typereference", Typereference));
                }

        
                aSqlParameterlist.Add(new SqlParameter("@referenceId", referenceId));
                aSqlParameterlist.Add(new SqlParameter("@CsID", CsID));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));


                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerOnly", aSqlParameterlist);
                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerAcc_reference", aSqlParameterlist);
                DataTable dt = _accessManager.GetDataTable("gl_Rpt_Cr_Proc_reference", aSqlParameterlist);


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
        
        public DataTable GetLedgerReportFinalCostCenterDAL(int CompanyID, int BankAccountID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDatet, string Typereference, string referenceId, string CsID)
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

                if(Typereference == "undefined")
                {
                    aSqlParameterlist.Add(new SqlParameter("@Typereference", ""));
                }
                else
                {
                    aSqlParameterlist.Add(new SqlParameter("@Typereference", Typereference));
                }

        
                aSqlParameterlist.Add(new SqlParameter("@referenceId", referenceId));
                aSqlParameterlist.Add(new SqlParameter("@CsID", CsID));
                // aSqlParameterlist.Add(new SqlParameter("@Rt", Rt));
                // aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", _CashBookBankBookFinancialYearStartDate));


                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerOnly", aSqlParameterlist);
                //DataTable dt = _accessManager.GetDataTable("sp_Rpt_ChartofAccountTransaction_LedgerAcc_reference", aSqlParameterlist);
                DataTable dt = _accessManager.GetDataTable("gl_Rpt_Cr_Proc_Costcenter", aSqlParameterlist);


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

        public DataTable GetOpeningBalanceReport2DAL(string C, string F, int CompanyID, int BankAccountID, int FinancialYearID,
                DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@C", C));
                aSqlParameterlist.Add(new SqlParameter("@F", F));
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
                aSqlParameterlist.Add(new SqlParameter("@CoaAccountId", BankAccountID));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));
                aSqlParameterlist.Add(new SqlParameter("@BankBookBankNAme", BankNAme));
                aSqlParameterlist.Add(new SqlParameter("@BankBookFinancialYearStartDate", BankBookFinancialYearStartDate));


                DataTable dt = _accessManager.GetDataTable("sp_GET_COAAccountOpeningBalanceForNonTractionLedger", aSqlParameterlist);


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


        public DataTable GetCOAForTrailBalanceDaL(int CompanyID, int FinancialYearID,
             DateTime FromDate, DateTime ToDate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));

                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
                aSqlParameterlist.Add(new SqlParameter("@FromDate", FromDate));
                aSqlParameterlist.Add(new SqlParameter("@Todate", ToDate));



                DataTable dt = _accessManager.GetDataTable("sp_COAForTrailBalance", aSqlParameterlist);


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


        public DataTable GetCOAForTrailBalanceDaL(string GL, string Control, string Subsidary)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@GL", GL));
                aSqlParameterlist.Add(new SqlParameter("@Control", Control));
                aSqlParameterlist.Add(new SqlParameter("@Subsidary", Subsidary));


                DataTable dt = _accessManager.GetDataTable("sp_COAForTrailBalanceParameterwise", aSqlParameterlist);


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



        public DataTable GetCOAForTrailBalanceDaL_New(string GL, string Control, string Subsidary, string subsubsidiary, string comId)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@GL", GL));
                aSqlParameterlist.Add(new SqlParameter("@Control", Control));
                aSqlParameterlist.Add(new SqlParameter("@Subsidary", Subsidary));
                aSqlParameterlist.Add(new SqlParameter("@SubSubControlLedgerId", subsubsidiary));
                aSqlParameterlist.Add(new SqlParameter("@comId", comId));


                DataTable dt = _accessManager.GetDataTable("sp_COAForTrailBalanceParameterwise", aSqlParameterlist);


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

        public DataTable GetTrialBalanceReportOtherDaL(int CompanyID, int BankAccountID, int FinancialYearID,
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

                DataTable dt = _accessManager.GetDataTable("sp_GET_TrailBalance_Transection", aSqlParameterlist);


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


        public DataTable GetTrialBalanceReportDaL(string Parameter, string Parameter2, string Company, string financialyear, string fromdate, string todate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@Parameter", Parameter));
                aSqlParameterlist.Add(new SqlParameter("@Parameter2", Parameter2));
                aSqlParameterlist.Add(new SqlParameter("@Company", Company));
                aSqlParameterlist.Add(new SqlParameter("@financialyear", financialyear));
                aSqlParameterlist.Add(new SqlParameter("@fromdate", fromdate));
                aSqlParameterlist.Add(new SqlParameter("@todate", todate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_TrailBalance", aSqlParameterlist);


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


        public DataTable GetTrialBalanceReportDaL(int CompanyID, int BankAccountID, int FinancialYearID,
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

                DataTable dt = _accessManager.GetDataTable("sp_GET_TrailBalance_Transection", aSqlParameterlist);


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

        
        public DataTable GetTrialBalanceReportForPostingDateDaL(int CompanyID, int BankAccountID, int FinancialYearID,
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

                DataTable dt = _accessManager.GetDataTable("sp_GET_TrailBalance_TransectionforPostingDate", aSqlParameterlist);


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


        public DataTable GetInventoryOpeningBalanceDAL(DateTime Fdate, int CompanyID, int FinancialYearID, int BankAccountID
          )
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@Fdate", Fdate));

                DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalance_NEW2", aSqlParameterlist);


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

        public DataTable GetInventoryClosingBalanceDAL(DateTime Fdate, int CompanyID, int FinancialYearID, int BankAccountID
         )
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@Fdate", Fdate));

               DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalance_NEW", aSqlParameterlist);


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

        public DataTable GetInventoryClosingBalanceDAL(int CompanyID, int FinancialYearID, int BankAccountID
         )

        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));

           
                DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalance", aSqlParameterlist);


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

        public DataTable GetComProfitDAL(int CompanyID, int FinancialYearID)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));

           
                DataTable dt = _accessManager.GetDataTable("sp_GET_CompanyProfit", aSqlParameterlist);


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

        public DataTable GetInventoryClosingBalanceNewDAL(int CompanyID, int FinancialYearID, int BankAccountID
     , DateTime todate)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@AccountId", BankAccountID));
            aSqlParameterlist.Add(new SqlParameter("@todate", BankAccountID));

         

                DataTable dt = _accessManager.GetDataTable("sp_GET_ClosingBalanceForTB", aSqlParameterlist);


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


        public DataTable GetAllStock_IncomeandBalanceSheetDAL(int CompanyID, int FinancialYearID, string Opening, DateTime Date, int Coa)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CompanyId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@OpeningClosing", Opening));
            aSqlParameterlist.Add(new SqlParameter("@Date", Date));
            aSqlParameterlist.Add(new SqlParameter("@CoaID", Coa));

          

                DataTable dt = _accessManager.GetDataTable("sp_AllStockMonthly_ForIncomeBalance", aSqlParameterlist);


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


    }

}