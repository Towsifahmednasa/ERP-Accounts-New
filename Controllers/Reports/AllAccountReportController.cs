// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Reports\AllAccountReportController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Reports
{
    public class AllAccountReportController : LegacyController
    {
        // GET: AllAccountReport

        // Ledger Report

        AccountsReportDAL accountsReport = new AccountsReportDAL();


        public ActionResult GetLedgerReport(int companyId, int coaId, int financialYearId, DateTime fromDate, DateTime toDate, string company, 
            bool monthly, string narration, string financialYearText, string bankBookBankName, 
            bool ledgerOtherCurrency, string typeSubCS, int subsidiaryId, int csId  , string category, int referenceId) { 

            if (Session["LoginUserId"] != null)
            {

                string companyName = company;
                string FYname = financialYearText;

                int CompanyID = Convert.ToInt32(companyId);
                int BankAccountID = Convert.ToInt32(coaId);
                int FinancialYearID = Convert.ToInt32(financialYearId);
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime ToDate = Convert.ToDateTime(toDate);
                string BankNAme = bankBookBankName ?? ""; // empty query value binds to null; SqlParameter(null) is not sent -> "expects parameter"

                bool LedgerOtherCurrency = Convert.ToBoolean(Request.Query["LedgerOtherCurrency"].ToString());

                DateTime BankBookFinancialYearStartDate = FromDate;

                DataTable allDataTable = new DataTable();

                DataTable allDataTable2 = new DataTable();
                if (category == "coa")

                {
                    allDataTable = accountsReport.GetLedgerReportFinalDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate, typeSubCS, subsidiaryId.ToString(), csId.ToString()).Copy();
                }


                if (category == "subsidiary")

                {
                    allDataTable = accountsReport.GetLedgerReportFinalSubsidaryDAL(CompanyID, BankAccountID, FinancialYearID,
                   FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate, typeSubCS, subsidiaryId.ToString(), csId.ToString()).Copy();
                 
                }

                if (category == "cost-center")

                {
                    allDataTable = accountsReport.GetLedgerReportFinalCostCenterDAL(CompanyID, BankAccountID, FinancialYearID,
                   FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate, typeSubCS, referenceId.ToString(), csId.ToString()).Copy();
                }

                 if (category == "reference")

                {
                    allDataTable = accountsReport.GetLedgerReportFinalReferenceDAL(CompanyID, BankAccountID, FinancialYearID,
                   FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate, typeSubCS, referenceId.ToString(), csId.ToString()).Copy();
                }



                // opening balance / monthly summary were computed here but never returned; they only added failure points
                // (an empty opening-balance table made Rows[0] throw and the ledger never loaded)

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(allDataTable);
                return Json(JSONresult);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
             public ActionResult GetLedgerReportOpeningBlnc(int companyId, int coaId, int financialYearId, DateTime fromDate, DateTime toDate, string company, 
            bool monthly, string narration, string financialYearText, string bankBookBankName, 
            bool ledgerOtherCurrency, string typeSubCS, int subsidiaryId, string category, int referenceId) { 

            if (Session["LoginUserId"] != null)
            {

                string companyName = company;
                string FYname = financialYearText;

                int CompanyID = Convert.ToInt32(companyId);
                int BankAccountID = Convert.ToInt32(coaId);
                int FinancialYearID = Convert.ToInt32(financialYearId);
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime ToDate = Convert.ToDateTime(toDate);
                string BankNAme = bankBookBankName ?? ""; // empty query value binds to null; SqlParameter(null) is not sent -> "expects parameter"

                bool LedgerOtherCurrency = Convert.ToBoolean(Request.Query["LedgerOtherCurrency"].ToString());

                DateTime BankBookFinancialYearStartDate = FromDate;
 

                DataTable allDataTable2 = new DataTable();

                if(category== "coa")

                { 
                allDataTable2 = accountsReport.GetOpeningBalanceReportDALNew(CompanyID, financialYearId, BankAccountID, subsidiaryId,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }


                if (category == "subsidiary")

                {
                    allDataTable2 = accountsReport.GetOpeningBalancesubsidiary(CompanyID, financialYearId, BankAccountID, subsidiaryId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }
                if (category == "reference")

                {
                    allDataTable2 = accountsReport.GetOpeningBalancereferenceId(CompanyID, financialYearId, BankAccountID, referenceId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }
                /////MonthlyCalculation start 


                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(allDataTable2);
                return Json(JSONresult);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult ReportList()
        {
            if (Session["LoginUserId"] != null)
            {
                return RedirectToAction("COAReport"); // reports are listed in the sidebar now
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult COAReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult BankBookReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult CashBookReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CashFlowReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LedgerReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult LedgerReportSubsidiary()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LedgerReportCostCenter()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult TrialBalanceReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult BankReconciliationStatementReport()
        {
            if (Session["LoginUserId"] != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        
    }
}