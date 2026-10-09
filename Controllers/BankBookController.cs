// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\BankBookController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class BankBookController : LegacyController
    {
        BankBookDAL aBankBookDAL = new BankBookDAL();
        // GET: BankBook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBankBook_Report(int CompanyID, int BankAccountID, int FinancialYearID, DateTime FromDate, DateTime ToDate, string BankNAme, string Monthly, string Narration)
        {
            DateTime BankBookFinancialYearStartDate = DateTime.Now;
            Monthly = "False";
            Narration = "False";
            DataSet mainDS = new DataSet();

            if (Monthly != "True" && Narration != "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aBankBookDAL.GetBankBookReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aBankBookDAL.GetOpeningBalanceReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();


                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "BankBookDataTable";
                    DataView dv = allDataTable.DefaultView;
                    dv.Sort = "AccountName";
                    allDataTable = dv.ToTable();
                    mainDS.Tables.Add(allDataTable);
                    mainDS.Tables.Add(allDataTable2);
                }
                else
                {

                    DataTable allDataTable3 = new DataTable();
                    allDataTable3 = aBankBookDAL.GetBankBookReportBlankDAL(CompanyID, BankAccountID, FinancialYearID,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                    allDataTable3.TableName = "BankBookDataTable";
                    mainDS.Tables.Add(allDataTable3);
                    mainDS.Tables.Add(allDataTable2);
                    
                }
            }

            if (Narration == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aBankBookDAL.GetBankBookReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aBankBookDAL.GetOpeningBalanceReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();


                if (allDataTable.Rows.Count > 0)
                {


                    allDataTable.TableName = "BankBookDataTable";

                    mainDS.Tables.Add(allDataTable);
                    mainDS.Tables.Add(allDataTable2);
                }
                else
                {


                    DataTable allDataTable3 = new DataTable();
                    allDataTable3 = aBankBookDAL.GetBankBookReportBlankDAL(CompanyID, BankAccountID, FinancialYearID,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                    allDataTable3.TableName = "BankBookDataTable";
                    mainDS.Tables.Add(allDataTable3);
                    mainDS.Tables.Add(allDataTable2);
                }
            }
            if (Monthly == "True")
            {
                DataTable allDataTable = new DataTable();
                allDataTable = aBankBookDAL.GetBankBookReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                DataTable allDataTable2 = new DataTable();
                allDataTable2 = aBankBookDAL.GetOpeningBalanceReportDAL(CompanyID, BankAccountID, FinancialYearID,
                    FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

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


                dtMonthly = aBankBookDAL.GetBankBookReportMonthlySummeryDAL(CompanyID, BankAccountID, FinancialYearID,
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
                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "BankBookDataTable";


                    dtMonthly.TableName = "dtMonthly";

                    mainDS.Tables.Add(allDataTable);
                    mainDS.Tables.Add(allDataTable2);
                    mainDS.Tables.Add(dtMonthly);
                }
                else
                {


                    DataTable allDataTable3 = new DataTable();
                    allDataTable3 = aBankBookDAL.GetBankBookReportBlankDAL(CompanyID, BankAccountID, FinancialYearID,
                        FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate).Copy();

                    allDataTable3.TableName = "BankBookDataTable";
                    mainDS.Tables.Add(allDataTable3);
                    mainDS.Tables.Add(allDataTable2);

                }
            }

            //string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            //return Json(JSONresult);
            string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            return Content(JSONresult, "application/json");

        }
    }
}