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
                string BankNAme = bankBookBankName;

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



                if (category == "coa")
                {
                    allDataTable2 = accountsReport.GetOpeningBalanceReportDALNew(CompanyID, FinancialYearID, BankAccountID, subsidiaryId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }
                else if (category == "subsidiary")
                {
                    allDataTable2 = accountsReport.GetOpeningBalancesubsidiary(CompanyID, FinancialYearID, BankAccountID, subsidiaryId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }
                else if (category == "cost-center" || category == "reference")
                {
                    allDataTable2 = accountsReport.GetOpeningBalancereferenceId(CompanyID, FinancialYearID, BankAccountID, referenceId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }
                else
                {
                    allDataTable2 = accountsReport.GetOpeningBalanceReportDALNew(CompanyID, FinancialYearID, BankAccountID, subsidiaryId,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                }


                /////MonthlyCalculation start

                DataTable dtMonthly = new DataTable();
                // dtMonthly.Columns.Add("MonthlyOpeningBalance");
                dtMonthly.Columns.Add("DebitAmount");
                dtMonthly.Columns.Add("CreditAmount");
                dtMonthly.Columns.Add("DebitAmountBDT");
                dtMonthly.Columns.Add("CreditAmountBDT");
                dtMonthly.Columns.Add("VoucherMonth");
                dtMonthly.Columns.Add("VoucherYear");
                dtMonthly.Columns.Add("MonthYear");


                dtMonthly = accountsReport.GetBankBookReportMonthlySummeryDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();
                dtMonthly.Columns.Add("MonthlyOpeningBalance");
                dtMonthly.Columns.Add("MonthlyClosingBalance");


                string GLName = allDataTable2.Rows[0]["GLName"].ToString();
                decimal openingBal = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"].ToString());
                decimal closingBal = 0;

                decimal debitAmountBDT = 0;
                decimal creditAmountBDT = 0;


                for (int i = 0; i < dtMonthly.Rows.Count; i++)
                {
                    debitAmountBDT = 0;
                    creditAmountBDT = 0;
                    if (i == 0)
                    {
                        dtMonthly.Rows[i]["MonthlyOpeningBalance"] = openingBal;

                        if (GLName == "Assets" || GLName == "Expenses")
                        {
                            debitAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["DebitAmountBDT"].ToString()) + openingBal;
                            creditAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["CreditAmountBDT"].ToString());
                            dtMonthly.Rows[i]["MonthlyClosingBalance"] = (debitAmountBDT - creditAmountBDT).ToString();
                            closingBal = (debitAmountBDT - creditAmountBDT);
                            openingBal = closingBal;
                        }
                        else
                        {
                            debitAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["DebitAmountBDT"].ToString());
                            creditAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["CreditAmountBDT"].ToString()) +
                                              openingBal;
                            dtMonthly.Rows[i]["MonthlyClosingBalance"] = (creditAmountBDT - debitAmountBDT).ToString();
                            closingBal = (creditAmountBDT - debitAmountBDT);
                            openingBal = closingBal;
                        }


                    }
                    else
                    {
                        dtMonthly.Rows[i]["MonthlyOpeningBalance"] = openingBal;

                        if (GLName == "Assets" || GLName == "Expenses")
                        {
                            debitAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["DebitAmountBDT"].ToString()) + openingBal;
                            creditAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["CreditAmountBDT"].ToString());
                            dtMonthly.Rows[i]["MonthlyClosingBalance"] = (debitAmountBDT - creditAmountBDT).ToString();
                            closingBal = (debitAmountBDT - creditAmountBDT);
                            openingBal = closingBal;
                        }
                        else
                        {
                            debitAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["DebitAmountBDT"].ToString());
                            creditAmountBDT = Convert.ToDecimal(dtMonthly.Rows[i]["CreditAmountBDT"].ToString()) +
                                              openingBal;
                            dtMonthly.Rows[i]["MonthlyClosingBalance"] = (creditAmountBDT - debitAmountBDT).ToString();
                            closingBal = (creditAmountBDT - debitAmountBDT);
                            openingBal = closingBal;
                        }
                    }

                }

                /////MonthlyCalculation end
                DataTable allDataTable3 = new DataTable();

                allDataTable3 = accountsReport.GetOpeningBalanceReport2DAL(companyName, FYname, CompanyID, BankAccountID, FinancialYearID,FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "BankBookDataTable";
                    dtMonthly.TableName = "dtMonthly";

                }

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
                string BankNAme = bankBookBankName;

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
                
                return View();

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