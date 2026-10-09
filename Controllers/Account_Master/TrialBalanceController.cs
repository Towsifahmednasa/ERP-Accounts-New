// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\TrialBalanceController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
namespace New_Accounts.Controllers.Account_Master
{
    public class TrialBalanceController : LegacyController
    {
        AccountsReportDAL accountsReport = new AccountsReportDAL();
        private readonly BalanceSheetDynamicReportDAL _reportDal = new BalanceSheetDynamicReportDAL();

        // GET: TrialBalance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTrialBalance_Report(int CompanyID, int FinancialYearID, DateTime FromDate, DateTime ToDate, string glHead, string controlLadger, string subsidiary, string subsubsidiary, string ComName, string FinYear, string TrailBalanceType, string ledgerType = "coa", int subsidiaryInfoId = 0, int referenceInfoId = 0, int costCenterId = 0)
        {
            DataSet mainDS = new DataSet();
            try
            {

                
                DateTime BankBookFinancialYearStartDate = FromDate;
                string TY = TrailBalanceType;


                #region TB_TREE

                if (TY == "TB_TREE")
                {
                    List<TrialBalanceTreeRow> rows = _reportDal.GetTrialBalanceTreeRows(
                        CompanyID,
                        FinancialYearID,
                        FromDate,
                        ToDate,
                        glHead,
                        controlLadger,
                        subsidiary,
                        subsubsidiary,
                        ledgerType,
                        subsidiaryInfoId,
                        referenceInfoId,
                        costCenterId);

                    return Json(new
                    {
                        isSuccess = true,

                        // New format
                        data = rows,

                        // Existing cshtml success block jodi dtMainTable use kore,
                        // eta dile old/new duita tei support korbe.
                        dtMainTable = rows
                    });
                }

                #endregion


                #region TB
                if (TY == "TBOT")
                {
                    BankBookFinancialYearStartDate = FromDate;

                    DataTable dtMainTable = new DataTable();
                    dtMainTable.Columns.Add("GLName");
                    dtMainTable.Columns.Add("ControlLName");
                    dtMainTable.Columns.Add("SubsidiryLName");
                    dtMainTable.Columns.Add("AccountId");
                    dtMainTable.Columns.Add("AccountCode");
                    dtMainTable.Columns.Add("AccountName");
                    dtMainTable.Columns.Add("DebitAmount");
                    dtMainTable.Columns.Add("CreditAmount");
                    dtMainTable.Columns.Add("FromDate");
                    dtMainTable.Columns.Add("ToDate");
                    dtMainTable.Columns.Add("TBCompanyName");
                    dtMainTable.Columns.Add("TBFinacialYear");
                    dtMainTable.Columns.Add("OpeningBalance");
                    dtMainTable.Columns.Add("Finalbalance");

                    DataTable coaForTrailBalanceDataTable = new DataTable();
                    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaLTB(glHead, controlLadger, subsidiary, CompanyID);

                    DataTable allDataTable;
                    DataTable allDataTable2;

                    decimal dcFinalbalance = 0;
                    decimal dcDebitAmount = 0;
                    decimal dcCreditAmount = 0;
                    decimal dcOpeningBalance = 0;

                    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                    {
                        allDataTable = new DataTable();
                        allDataTable2 = new DataTable();

                        allDataTable = accountsReport.GetTrialBalanceReportDaLTB(CompanyID,
                          Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                          FinancialYearID,
                          FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                          BankBookFinancialYearStartDate);

                        allDataTable2 = accountsReport.GetOpeningBalanceReportBLLTB(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                                FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate);

                        DataTable dtOpening = accountsReport.GetInventoryOpeningBalanceBLLTB(FromDate, CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()));
                        DataTable dtClosing = accountsReport.GetInventoryClosingBalanceBLLTB(ToDate, CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()));

                        //DataTable dtComProfit = accountsReport.GetComProfitBLLTB(CompanyID, FinancialYearID);

                        //if (dtClosing.Rows.Count > 0)
                        //{
                        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                        //    {
                        //        dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                        //        dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                        //        dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                        //        dcOpeningBalance = Convert.ToDecimal(dtOpening.Rows[0]["Amount"]);
                        //    }
                        //}
                        //else
                        //{
                        //    dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                        //                    Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                        //    dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                        //    dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                        //    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);
                        //}

                        //if (dtComProfit.Rows.Count > 0)
                        //{
                        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                        //    {
                        //        dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                        //        dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                        //    }
                        //}

                        //if (dcFinalbalance != 0 || dcDebitAmount != 0 || dcCreditAmount != 0 || dcOpeningBalance != 0)
                        {
                            DataRow dr = dtMainTable.NewRow();

                            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();

                            // ✅ Session এর বদলে parameter
                            dr["FromDate"] = FromDate.ToString("dd-MMM-yyyy");
                            dr["ToDate"] = ToDate.ToString("dd-MMM-yyyy");
                            dr["TBCompanyName"] = ComName;
                            dr["TBFinacialYear"] = FinYear;

                            dr["OpeningBalance"] = dcOpeningBalance;
                            dr["Finalbalance"] = dcFinalbalance.ToString();
                            dr["DebitAmount"] = allDataTable.Rows[0]["TotalDebit"].ToString();
                            dr["CreditAmount"] = allDataTable.Rows[0]["TotalCredit"].ToString();

                            dtMainTable.Rows.Add(dr);
                        }
                    }

                    dtMainTable.TableName = "dtMainTable";

                    DataView dv = dtMainTable.DefaultView;
                    dv.Sort = "AccountName";
                    dtMainTable = dv.ToTable();

                    mainDS.Tables.Add(dtMainTable);
                }
                if (TY == "TB")
                {


                    DataTable dtMainTable = new DataTable();
                    dtMainTable.Columns.Add("PsotingDate");
                    dtMainTable.Columns.Add("GLName");

                    dtMainTable.Columns.Add("ControlLName");
                    dtMainTable.Columns.Add("SubsidiryLName");
                    dtMainTable.Columns.Add("AccountId");
                    dtMainTable.Columns.Add("AccountCode");
                    dtMainTable.Columns.Add("AccountName");
                    dtMainTable.Columns.Add("DebitAmount");
                    dtMainTable.Columns.Add("CreditAmount");
                    dtMainTable.Columns.Add("FromDate");
                    dtMainTable.Columns.Add("ToDate");
                    dtMainTable.Columns.Add("TBCompanyName");
                    dtMainTable.Columns.Add("TBFinacialYear");
                    dtMainTable.Columns.Add("OpeningBalance");
                    dtMainTable.Columns.Add("Finalbalance");

                    DataTable coaForTrailBalanceDataTable = new DataTable();
                    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL_New(glHead, controlLadger, subsidiary, subsubsidiary, CompanyID.ToString()).Copy();

                    DataTable allDataTable;

                    DataTable allDataTable2;

                    decimal dcFinalbalance = 0;
                    decimal dcDebitAmount = 0;
                    decimal dcCreditAmount = 0;
                    decimal dcOpeningBalance = 0;


                    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                    {
                        allDataTable = new DataTable();
                        allDataTable2 = new DataTable();


                        allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                            Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                            FinancialYearID,
                            FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                            BankBookFinancialYearStartDate).Copy();

                        decimal opening = 0;
                        decimal final = 0;

                        if (allDataTable2 != null && allDataTable2.Rows.Count > 0)
                        {
                            decimal.TryParse(allDataTable2.Rows[0]["OpeningBalance"]?.ToString(), out opening);
                        }

                        if (allDataTable != null && allDataTable.Rows.Count > 0)
                        {
                            decimal.TryParse(allDataTable.Rows[0]["FinalBalance"]?.ToString(), out final);
                        }

                        dcFinalbalance = opening + final;
                        //dcFinalbalance =
                        //    (decimal.TryParse(allDataTable2.Rows[0]["OpeningBalance"]?.ToString(), out decimal opening) ? opening : 0)
                        //    +
                        //    (decimal.TryParse(allDataTable.Rows[0]["FinalBalance"]?.ToString(), out decimal final) ? final : 0);

                        allDataTable = accountsReport.GetTrialBalanceReportForPostingDateDaL(CompanyID,
                          Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                          FinancialYearID,
                          FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                          BankBookFinancialYearStartDate).Copy();
                        {
                            DataRow dr = dtMainTable.NewRow();


                            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                            dr["FromDate"] = FromDate.ToString("dd-MMM-yyyy");
                            dr["ToDate"] = ToDate.ToString("dd-MMM-yyyy");

                            dr["TBCompanyName"] = ComName;
                            dr["TBFinacialYear"] = FinYear;
                            dr["OpeningBalance"] = dcOpeningBalance;
                            dr["Finalbalance"] = dcFinalbalance.ToString();
                            dr["DebitAmount"] = allDataTable.Rows[0]["TotalDebit"].ToString();
                            dr["CreditAmount"] = allDataTable.Rows[0]["TotalCredit"].ToString();
                            dr["PsotingDate"] = allDataTable.Rows[0]["PostingDate"].ToString();
                            dtMainTable.Rows.Add(dr);
                        }
                    }

                    dtMainTable.TableName = "dtMainTable";




                    DataView dv = dtMainTable.DefaultView;
                    dv.Sort = "AccountName";
                    dtMainTable = dv.ToTable();


                    mainDS.Tables.Add(dtMainTable);
                }
                #endregion

                #region 2
                if (TY == "TTT")
                {

                    if (CompanyID == 4 && FinancialYearID == 933)
                    {

                        DataTable dtMainTable = new DataTable();
                        dtMainTable.Columns.Add("GLName");
                        dtMainTable.Columns.Add("ControlLName");
                        dtMainTable.Columns.Add("SubsidiryLName");
                        dtMainTable.Columns.Add("AccountId");
                        dtMainTable.Columns.Add("AccountCode");
                        dtMainTable.Columns.Add("AccountName");
                        dtMainTable.Columns.Add("DebitAmount");
                        dtMainTable.Columns.Add("CreditAmount");
                        dtMainTable.Columns.Add("FromDate");
                        dtMainTable.Columns.Add("ToDate");
                        dtMainTable.Columns.Add("TBCompanyName");
                        dtMainTable.Columns.Add("TBFinacialYear");

                        DataTable coaForTrailBalanceDataTable = new DataTable();
                        coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL_New(glHead, controlLadger, subsidiary, subsubsidiary, CompanyID.ToString()).Copy();

                        DataTable allDataTable;

                        DataTable allDataTable2;

                        decimal dcFinalbalance = 0;
                        decimal dcDebitAmount = 0;
                        decimal dcCreditAmount = 0;
                        decimal dcOpeningBalance = 0;


                        for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                        {

                            allDataTable = accountsReport.GetTrialBalanceReportDaL(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate).Copy();




                            allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate).Copy();

                            dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                             Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);


                            dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);



                            DataTable dtClosing = accountsReport.GetInventoryClosingBalanceDAL(CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString())).Copy();
                            DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();

                            if (dtClosing.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                                {
                                    dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                                    dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                                    dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                                }
                            }
                            else
                            {
                                dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                              Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                                dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                                dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);
                            }
                            if (dtComProfit.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                                {
                                    dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                                }
                            }
                            if (dcFinalbalance != 0)
                            {

                                DataRow dr = dtMainTable.NewRow();

                                dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                                dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                                dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                                dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                                dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                                dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                                dr["FromDate"] = (Session["COAFromDate"]);
                                dr["ToDate"] = (Session["COAToDate"]);
                                dr["TBCompanyName"] = (Session["TBCompanyName"]);
                                dr["TBFinacialYear"] = (Session["TBFinacialYear"]);


                                if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                                    coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                                {
                                    dr["DebitAmount"] = (dcOpeningBalance + dcDebitAmount) - dcCreditAmount;
                                    dr["CreditAmount"] = 0;
                                }
                                else
                                {
                                    dr["DebitAmount"] = 0;
                                    dr["CreditAmount"] = (dcOpeningBalance + dcCreditAmount) - dcDebitAmount;
                                }
                                dtMainTable.Rows.Add(dr);
                            }
                        }


                        dtMainTable.TableName = "dtMainTable";


                        DataView dv = dtMainTable.DefaultView;
                        dv.Sort = "AccountName";
                        dtMainTable = dv.ToTable();


                        mainDS.Tables.Add(dtMainTable);
                    }
                    else
                    {


                        DataTable dtMainTable = new DataTable();
                        dtMainTable.Columns.Add("GLName");
                        dtMainTable.Columns.Add("ControlLName");
                        dtMainTable.Columns.Add("SubsidiryLName");
                        dtMainTable.Columns.Add("AccountId");
                        dtMainTable.Columns.Add("AccountCode");
                        dtMainTable.Columns.Add("AccountName");
                        dtMainTable.Columns.Add("DebitAmount");
                        dtMainTable.Columns.Add("CreditAmount");
                        dtMainTable.Columns.Add("FromDate");
                        dtMainTable.Columns.Add("ToDate");
                        dtMainTable.Columns.Add("TBCompanyName");
                        dtMainTable.Columns.Add("TBFinacialYear");

                        DataTable coaForTrailBalanceDataTable = new DataTable();
                        coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL_New(glHead, controlLadger, subsidiary, subsubsidiary, CompanyID.ToString()).Copy();
                        DataTable allDataTable;

                        DataTable allDataTable2;

                        decimal dcFinalbalance = 0;
                        decimal dcDebitAmount = 0;
                        decimal dcCreditAmount = 0;
                        decimal dcOpeningBalance = 0;


                        for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                        {

                            allDataTable = accountsReport.GetTrialBalanceReportOtherDaL(CompanyID,
                           Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                           FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                           BankBookFinancialYearStartDate).Copy();




                            allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate).Copy();

                            dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                             Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);


                            dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);



                            DataTable dtClosing = accountsReport.GetInventoryClosingBalanceNewDAL(CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), ToDate).Copy();
                            DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();


                            if (dtClosing.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                                {


                                    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513")
                                    {
                                        DataTable dtClosingStockofRawMaterial =
                                      accountsReport.GetAllStock_IncomeandBalanceSheetDAL(CompanyID, FinancialYearID, "Opening", Convert.ToDateTime(FromDate), 2).Copy();
                                        dcFinalbalance = Convert.ToDecimal(dtClosingStockofRawMaterial.Rows[0]["Amount"]);
                                    }



                                    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514")
                                    {
                                        DataTable dtClosingStockofFinishedGoods =
                                      accountsReport.GetAllStock_IncomeandBalanceSheetDAL(CompanyID, FinancialYearID, "Opening", Convert.ToDateTime(FromDate), 4).Copy();
                                        dcFinalbalance = Convert.ToDecimal(dtClosingStockofFinishedGoods.Rows[0]["Amount"]);
                                    }



                                    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                                    {
                                        DataTable dtClosingWorkinProcess = accountsReport.GetAllStock_IncomeandBalanceSheetDAL(CompanyID, FinancialYearID, "Opening", Convert.ToDateTime(FromDate), 6).Copy();
                                        dcFinalbalance = Convert.ToDecimal(dtClosingWorkinProcess.Rows[0]["Amount"]);
                                    }
                                }
                            }

                            else
                            {
                                dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                                Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                                dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                                dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                            }
                            if (dtComProfit.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                                {
                                    dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                                }
                            }
                            if (dcFinalbalance != 0)
                            {

                                DataRow dr = dtMainTable.NewRow();

                                dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                                dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                                dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                                dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                                dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                                dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                                dr["FromDate"] = (Session["COAFromDate"]);
                                dr["ToDate"] = (Session["COAToDate"]);
                                dr["TBCompanyName"] = (Session["TBCompanyName"]);
                                dr["TBFinacialYear"] = (Session["TBFinacialYear"]);

                                if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                                  coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                                {
                                    dr["DebitAmount"] = dcFinalbalance;
                                    dr["CreditAmount"] = 0;
                                }
                                else
                                {
                                    dr["DebitAmount"] = 0;
                                    dr["CreditAmount"] = dcFinalbalance;
                                }
                                dtMainTable.Rows.Add(dr);



                            }
                        }
                        dtMainTable.TableName = "dtMainTable";

                        mainDS.Tables.Add(dtMainTable);
                    }
                }

                #endregion

                #region TBOT
                //if (TY == "TB")
                //{


                //    DataTable dtMainTable = new DataTable();
                //    dtMainTable.Columns.Add("GLName");
                //    dtMainTable.Columns.Add("ControlLName");
                //    dtMainTable.Columns.Add("SubsidiryLName");
                //    dtMainTable.Columns.Add("AccountId");
                //    dtMainTable.Columns.Add("AccountCode");
                //    dtMainTable.Columns.Add("AccountName");
                //    dtMainTable.Columns.Add("DebitAmount");
                //    dtMainTable.Columns.Add("CreditAmount");

                //    dtMainTable.Columns.Add("FromDate");
                //    dtMainTable.Columns.Add("ToDate");
                //    dtMainTable.Columns.Add("TBCompanyName");
                //    dtMainTable.Columns.Add("TBFinacialYear");
                //    dtMainTable.Columns.Add("OpeningBalance");
                //    dtMainTable.Columns.Add("Finalbalance");

                //    DataTable coaForTrailBalanceDataTable = new DataTable();
                //    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL_New(glHead, controlLadger, subsidiary, subsubsidiary, CompanyID.ToString()).Copy();
                //    DataTable allDataTable;

                //    DataTable allDataTable2;

                //    decimal dcFinalbalance = 0;
                //    decimal dcDebitAmount = 0;
                //    decimal dcCreditAmount = 0;
                //    decimal dcOpeningBalance = 0;

                    
                //    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                //    {
                //        allDataTable = new DataTable();
                //        allDataTable2 = new DataTable();


                        
                //        allDataTable = accountsReport.GetTrialBalanceReportDaL(CompanyID,
                //            Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                //            FinancialYearID,
                //            FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                //            BankBookFinancialYearStartDate).Copy();
                        


                //        allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                //            Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                //            FinancialYearID,
                //            FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                //            BankBookFinancialYearStartDate).Copy();

                //        dcFinalbalance = SafeDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                //                         SafeDecimal(allDataTable.Rows[0]["FinalBalance"]);


                //        //DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();
                //        //DataTable dtOpening = accountsReport.GetInventoryOpeningBalanceDAL(FromDate, CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString())).Copy();
                //        //DataTable dtClosing = accountsReport.GetInventoryClosingBalanceDAL(ToDate, CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString())).Copy();



                //        //if (dtClosing.Rows.Count > 0)
                //        //{
                //        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                //        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                //        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                //        //    {
                //        //        dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                //        //        dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                //        //        dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                //        //        dcOpeningBalance = Convert.ToDecimal(dtOpening.Rows[0]["Amount"]);
                //        //    }
                //        //}

                //        //else
                //        //{
                //            dcFinalbalance = SafeDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                //                            SafeDecimal(allDataTable.Rows[0]["FinalBalance"]);
                //            dcDebitAmount = SafeDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                //            dcCreditAmount = SafeDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                //            dcOpeningBalance = SafeDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                //        //}
                //        //if (dtComProfit.Rows.Count > 0)
                //        //{
                //        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                //        //    {
                //        //        dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                //        //                           Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                //        //        dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                //        //                         Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                //        //    }
                //        //}

                //        //if (dcFinalbalance != 0 || dcOpeningBalance > 0 || dcDebitAmount > 0 || dcCreditAmount > 0)
                //        {
                //            DataRow dr = dtMainTable.NewRow();

                //            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"]?.ToString() ?? "";
                //            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"]?.ToString() ?? "";
                //            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"]?.ToString() ?? "";
                //            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"]?.ToString() ?? "";
                //            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"]?.ToString() ?? "";
                //            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"]?.ToString() ?? "";

                //            dr["FromDate"] = Session["COAFromDate"] ?? DBNull.Value;
                //            dr["ToDate"] = Session["COAToDate"] ?? DBNull.Value;
                //            dr["TBCompanyName"] = Session["TBCompanyName"] ?? "";
                //            dr["TBFinacialYear"] = Session["TBFinacialYear"] ?? "";

                //            dr["OpeningBalance"] = dcOpeningBalance;
                //            dr["Finalbalance"] = dcFinalbalance.ToString();

                //            dr["DebitAmount"] = allDataTable.Rows.Count > 0 ? allDataTable.Rows[0]["TotalDebit"]?.ToString() ?? "0" : "0";
                //            dr["CreditAmount"] = allDataTable.Rows.Count > 0 ? allDataTable.Rows[0]["TotalCredit"]?.ToString() ?? "0" : "0";



                //            //if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                //            //    coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                //            //{
                //            //    dr["DebitAmount"] = (dcOpeningBalance + Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString())) - Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                //            //    dr["CreditAmount"] = 0;
                //            //}
                //            //else
                //            //{
                //            //    dr["DebitAmount"] = 0;
                //            //    dr["CreditAmount"] = (dcOpeningBalance + Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString()) )- Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                //            //}

                //            decimal totalDebit = 0;
                //            decimal totalCredit = 0;

                //            if (allDataTable.Rows.Count > 0)
                //            {
                //                decimal.TryParse(allDataTable.Rows[0]["TotalDebit"]?.ToString(), out totalDebit);
                //                decimal.TryParse(allDataTable.Rows[0]["TotalCredit"]?.ToString(), out totalCredit);
                //            }

                //            decimal total = dcFinalbalance+ dcOpeningBalance;// + totalDebit + totalCredit;

                //              if (total != 0)
                //            {
                //                dtMainTable.Rows.Add(dr);
                //            }
                //        }
                //    }
                //    dtMainTable.TableName = "dtMainTable";

                //    DataView dv = dtMainTable.DefaultView;
                //    dv.Sort = "AccountName";
                //    dtMainTable = dv.ToTable();
                //    mainDS.Tables.Add(dtMainTable);

                //}
                #endregion

                #region 4
                if (TY == "TBO")
                {

                    DataTable dtMainTable = new DataTable();
                    dtMainTable.Columns.Add("GLName");
                    dtMainTable.Columns.Add("ControlLName");
                    dtMainTable.Columns.Add("SubsidiryLName");
                    dtMainTable.Columns.Add("AccountId");
                    dtMainTable.Columns.Add("AccountCode");
                    dtMainTable.Columns.Add("AccountName");
                    dtMainTable.Columns.Add("DebitAmount");
                    dtMainTable.Columns.Add("CreditAmount");

                    dtMainTable.Columns.Add("FromDate");
                    dtMainTable.Columns.Add("ToDate");
                    dtMainTable.Columns.Add("TBCompanyName");
                    dtMainTable.Columns.Add("TBFinacialYear");


                    DataTable coaForTrailBalanceDataTable = new DataTable();
                    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL(CompanyID, FinancialYearID,
                        FromDate, ToDate).Copy();

                    DataTable allDataTable;

                    DataTable allDataTable2;

                    decimal dcFinalbalance = 0;
                    decimal dcDebitAmount = 0;
                    decimal dcCreditAmount = 0;
                    decimal dcOpeningBalance = 0;

                    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                    {
                        allDataTable = new DataTable();
                        allDataTable2 = new DataTable();

                        try
                        {
                            allDataTable = accountsReport.GetTrialBalanceReportOtherDaL(CompanyID,
                         Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                         FinancialYearID,
                         FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                         BankBookFinancialYearStartDate).Copy();


                            allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                                FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate).Copy();

                            dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                             Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);




                            DataTable dtClosing = accountsReport.GetInventoryClosingBalanceDAL(CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString())).Copy();
                            DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();

                            if (dtClosing.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                                {
                                    dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                                    dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                                    dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);
                                }
                            }

                            else
                            {
                                dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                                Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                                dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                                dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                            }
                            if (dtComProfit.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                                {
                                    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                                }
                            }

                            DataRow dr = dtMainTable.NewRow();

                            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                            dr["FromDate"] = (Session["COAFromDate"]);
                            dr["ToDate"] = (Session["COAToDate"]);
                            dr["TBCompanyName"] = (Session["TBCompanyName"]);
                            dr["TBFinacialYear"] = (Session["TBFinacialYear"]);


                            if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                                coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                            {
                                dr["DebitAmount"] = dcFinalbalance;
                                dr["CreditAmount"] = 0;
                            }
                            else
                            {
                                dr["DebitAmount"] = 0;
                                dr["CreditAmount"] = dcFinalbalance;
                            }
                            dtMainTable.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {
                            var accid = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                        }
                    }
                    dtMainTable.TableName = "dtMainTable";

                    DataView dv = dtMainTable.DefaultView;
                    dv.Sort = "AccountName";
                    dtMainTable = dv.ToTable();
                    mainDS.Tables.Add(dtMainTable);
                }
                #endregion

                #region 5
                if (TY == "TBS")
                {




                    DataTable dtMainTable = new DataTable();
                    dtMainTable.Columns.Add("GLName");
                    dtMainTable.Columns.Add("ControlLName");
                    dtMainTable.Columns.Add("SubsidiryLName");
                    dtMainTable.Columns.Add("AccountId");
                    dtMainTable.Columns.Add("AccountCode");
                    dtMainTable.Columns.Add("AccountName");
                    dtMainTable.Columns.Add("DebitAmount");
                    dtMainTable.Columns.Add("CreditAmount");

                    dtMainTable.Columns.Add("FromDate");
                    dtMainTable.Columns.Add("ToDate");
                    dtMainTable.Columns.Add("TBCompanyName");
                    dtMainTable.Columns.Add("TBFinacialYear");


                    DataTable coaForTrailBalanceDataTable = new DataTable();
                    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL(CompanyID, FinancialYearID,
                        FromDate, ToDate).Copy();

                    DataTable allDataTable;

                    DataTable allDataTable2;

                    decimal dcFinalbalance = 0;
                    decimal dcDebitAmount = 0;
                    decimal dcCreditAmount = 0;
                    decimal dcOpeningBalance = 0;

                    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                    {
                        allDataTable = new DataTable();
                        allDataTable2 = new DataTable();

                        try
                        {
                            allDataTable = accountsReport.GetTrialBalanceReportOtherDaL(CompanyID,
                         Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                         FinancialYearID,
                         FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                         BankBookFinancialYearStartDate).Copy();



                            allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                                Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()),
                                FinancialYearID,
                                FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                                BankBookFinancialYearStartDate).Copy();

                            dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                             Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);




                            DataTable dtClosing = accountsReport.GetInventoryClosingBalanceNewDAL(CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), ToDate).Copy();
                            DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();

                            if (dtClosing.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                                    coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                                {
                                    dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                                    dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                                    dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);
                                }
                            }

                            else
                            {
                                dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                                Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                                dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                                dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                                dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                            }
                            if (dtComProfit.Rows.Count > 0)
                            {
                                if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                                {
                                    dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                                    dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                                }
                            }

                            DataRow dr = dtMainTable.NewRow();

                            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                            dr["FromDate"] = (Session["COAFromDate"]);
                            dr["ToDate"] = (Session["COAToDate"]);
                            dr["TBCompanyName"] = (Session["TBCompanyName"]);
                            dr["TBFinacialYear"] = (Session["TBFinacialYear"]);


                            if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                                coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                            {
                                dr["DebitAmount"] = dcFinalbalance;
                                dr["CreditAmount"] = 0;
                            }
                            else
                            {
                                dr["DebitAmount"] = 0;
                                dr["CreditAmount"] = dcFinalbalance;
                            }
                            dtMainTable.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {
                            var accid = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                        }
                    }
                    dtMainTable.TableName = "dtMainTable";

                    DataView dv = dtMainTable.DefaultView;
                    dv.Sort = "AccountName";
                    dtMainTable = dv.ToTable();
                    mainDS.Tables.Add(dtMainTable);
                }

                #endregion

                #region 6

                if (TY == "TBLast")
                {


                    DataTable dtMainTable = new DataTable();
                    dtMainTable.Columns.Add("GLName");
                    dtMainTable.Columns.Add("ControlLName");
                    dtMainTable.Columns.Add("SubsidiryLName");
                    dtMainTable.Columns.Add("AccountId");
                    dtMainTable.Columns.Add("AccountCode");
                    dtMainTable.Columns.Add("AccountName");
                    dtMainTable.Columns.Add("DebitAmount");
                    dtMainTable.Columns.Add("CreditAmount");
                    dtMainTable.Columns.Add("FromDate");
                    dtMainTable.Columns.Add("ToDate");
                    dtMainTable.Columns.Add("TBCompanyName");
                    dtMainTable.Columns.Add("TBFinacialYear");

                    DataTable coaForTrailBalanceDataTable = new DataTable();
                    coaForTrailBalanceDataTable = accountsReport.GetCOAForTrailBalanceDaL_New(glHead, controlLadger, subsidiary, subsubsidiary, CompanyID.ToString()).Copy();

                    DataTable allDataTable;

                    DataTable allDataTable2;

                    decimal dcFinalbalance = 0;
                    decimal dcDebitAmount = 0;
                    decimal dcCreditAmount = 0;
                    decimal dcOpeningBalance = 0;


                    for (int i = 0; i < coaForTrailBalanceDataTable.Rows.Count; i++)
                    {

                        allDataTable = accountsReport.GetTrialBalanceReportDaL(CompanyID,
                            Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                            FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                            BankBookFinancialYearStartDate).Copy();




                        allDataTable2 = accountsReport.GetOpeningBalanceReportDAL(CompanyID,
                            Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString()), FinancialYearID,
                            FromDate, ToDate, coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString(),
                            BankBookFinancialYearStartDate).Copy();

                        dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                         Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);


                        dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);



                        //DataTable dtClosing = accountsReport.GetInventoryClosingBalanceDAL(CompanyID, FinancialYearID, Convert.ToInt32(coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString())).Copy();
                        //DataTable dtComProfit = accountsReport.GetComProfitDAL(CompanyID, FinancialYearID).Copy();

                        //if (dtClosing.Rows.Count > 0)
                        //{
                        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "513" ||
                        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "514" ||
                        //        coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "515")
                        //    {
                        //        dcFinalbalance = Convert.ToDecimal(dtClosing.Rows[0]["Amount"]);
                        //        dcDebitAmount = Convert.ToDecimal(Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) - Convert.ToDecimal(dtClosing.Rows[0]["Amount"]));
                        //        dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                        //        dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);

                        //    }
                        //}
                        //else
                        {
                            dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) +
                                          Convert.ToDecimal(allDataTable.Rows[0]["FinalBalance"]);
                            dcDebitAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalDebit"].ToString());
                            dcCreditAmount = Convert.ToDecimal(allDataTable.Rows[0]["TotalCredit"].ToString());
                            dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]);
                        }
                        //if (dtComProfit.Rows.Count > 0)
                        //{
                        //    if (coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString() == "152")
                        //    {
                        //        // dcOpeningBalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                        //        dcFinalbalance = Convert.ToDecimal(allDataTable2.Rows[0]["OpeningBalance"]) + Convert.ToDecimal(dtComProfit.Rows[0]["Amount"]);
                        //    }
                        //}
                        //if (dcFinalbalance != 0)
                        //    if (dcFinalbalance != 0 || dcDebitAmount != 0 || dcCreditAmount != 0 || dcOpeningBalance != 0)
                        //|| dcDebitAmount > 0 || dcCreditAmount > 0
                        {

                            DataRow dr = dtMainTable.NewRow();

                            dr["GLName"] = coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString();
                            dr["ControlLName"] = coaForTrailBalanceDataTable.Rows[i]["ControlLName"].ToString();
                            dr["SubsidiryLName"] = coaForTrailBalanceDataTable.Rows[i]["SubsidiryLName"].ToString();
                            dr["AccountId"] = coaForTrailBalanceDataTable.Rows[i]["AccountId"].ToString();
                            dr["AccountCode"] = coaForTrailBalanceDataTable.Rows[i]["AccountCode"].ToString();
                            dr["AccountName"] = coaForTrailBalanceDataTable.Rows[i]["AccountName"].ToString();
                            dr["FromDate"] = (Session["COAFromDate"]);
                            dr["ToDate"] = (Session["COAToDate"]);
                            dr["TBCompanyName"] = (Session["TBCompanyName"]);
                            dr["TBFinacialYear"] = (Session["TBFinacialYear"]);


                            //if (coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Assets" ||
                            //    coaForTrailBalanceDataTable.Rows[i]["GLName"].ToString() == "Expenses")
                            //{
                            //    dr["DebitAmount"] = (dcOpeningBalance + dcDebitAmount) - dcCreditAmount;
                            //    dr["CreditAmount"] = 0;
                            //}
                            //else
                            //{
                            //    dr["DebitAmount"] = 0;
                            //    dr["CreditAmount"] = (dcOpeningBalance + dcCreditAmount) - dcDebitAmount;
                            //}
                            dtMainTable.Rows.Add(dr);
                        }
                    }


                    dtMainTable.TableName = "dtMainTable";


                    DataView dv = dtMainTable.DefaultView;
                    dv.Sort = "AccountName";
                    dtMainTable = dv.ToTable();


                    mainDS.Tables.Add(dtMainTable);

                }
                #endregion
            }
            catch (Exception exception)
            {
                throw new Exception("Error converting field value: " + exception.Message);
            }

            string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            return Content(JSONresult, "application/json");
        }

        private decimal SafeDecimal(object value)
        {
            return value == DBNull.Value || value == null ? 0 : Convert.ToDecimal(value);
        }

    }
}
