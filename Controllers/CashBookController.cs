// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\CashBookController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.DAL.Report;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class CashBookController : LegacyController
    {
        AccountsReportDAL aAccountsReportDAL = new AccountsReportDAL();
        // GET: CashBook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCashBook_Report(int CompanyID, int CashAccountID, int FinancialYearID, DateTime FromDate, DateTime ToDate, string CashbookNAme, string Monthly, string Narration)
        {
            DateTime CashBookFinancialYearStartDate = DateTime.Now;
            
            DataSet mainDS = new DataSet();

            if (Monthly != "True" && Narration != "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                allDataTable.TableName = "BankBookDataTable";
                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
            }

            if (Narration == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                allDataTable.TableName = "BankBookDataTable";
                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
            }

            if (Monthly == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();
                //   GetBankBookReportBLL



                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();

                /////MonthlyCalculation start

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


                dtMonthly = aAccountsReportDAL.GetBankBookReportMonthlySummeryDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();
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

                allDataTable.TableName = "BankBookDataTable";
                dtMonthly.TableName = "dtMonthly";

                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
                mainDS.Tables.Add(dtMonthly);
            }

            string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            return Content(JSONresult, "application/json");

        }









        public ActionResult GetCashBook_ReportOther(int CompanyID, int CashAccountID, int FinancialYearID, DateTime FromDate, DateTime ToDate, string CashbookNAme, string Monthly, string Narration)
        {
            DateTime CashBookFinancialYearStartDate = DateTime.Now;

            DataSet mainDS = new DataSet();

            if (Monthly != "True" && Narration != "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                allDataTable.TableName = "BankBookDataTable";
                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
            }

            if (Narration == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();


                allDataTable.TableName = "BankBookDataTable";
                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
            }

            if (Monthly == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aAccountsReportDAL.GetCashBookReportNewDaL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();
                //   GetBankBookReportBLL



                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aAccountsReportDAL.GetOpeningBalanceReportDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();

                /////MonthlyCalculation start

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


                dtMonthly = aAccountsReportDAL.GetBankBookReportMonthlySummeryDAL(CompanyID, CashAccountID, FinancialYearID,
                    FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate).Copy();
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

                allDataTable.TableName = "BankBookDataTable";
                dtMonthly.TableName = "dtMonthly";

                mainDS.Tables.Add(allDataTable);
                mainDS.Tables.Add(allDataTable2);
                mainDS.Tables.Add(dtMonthly);
            }

            string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            return Content(JSONresult, "application/json");

        }
    }
}