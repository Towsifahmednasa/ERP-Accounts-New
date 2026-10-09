// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Reports\AccountingTaskReportController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using New_Accounts.Models;
using New_Accounts.Models.ReportModel;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Reports
{
    public class AccountingTaskReportController : LegacyController
    {
        // GET: AccountingTaskReport/VoucherPrint?MasterId=123
        // HTML voucher print (RDLC-style layout) replacing crpVoucher2.rpt for debit/credit vouchers
        public ActionResult VoucherPrint(string MasterId)
        {
            if (Session[SessionCollection.UserName] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ReportDAL aReportDAL = new ReportDAL();
            DataTable master = aReportDAL.GetDebitVoucherMasterDal(MasterId);
            if (master.Rows.Count == 0)
            {
                return Content("No Data Found");
            }
            DataTable detail = aReportDAL.GetDebitVoucherDetailDal(MasterId);
            DataTable header = aReportDAL.GetReportHeaderDal();

            DataRow m = master.Rows[0];
            string voucherType = m["VoucherType"].ToString();
            bool isDebit = voucherType.StartsWith("Debit");
            bool isPayment = isDebit || voucherType.StartsWith("Credit");

            // bank voucher: just "Bank"; cash voucher: offset account name (credited on debit voucher, debited on credit voucher); journal: none
            string paymentMode = "";
            if (isPayment)
            {
                if (voucherType.EndsWith("Bank"))
                {
                    paymentMode = "Bank";
                }
                else
                {
                    foreach (DataRow d in detail.Rows)
                    {
                        decimal amt = ToDec(d[isDebit ? "CreditAmountBDT" : "DebitAmountBDT"]);
                        if (amt > 0) { paymentMode = d["AccountName"].ToString(); break; }
                    }
                }
            }

            decimal totalDebit = 0, totalCredit = 0;
            foreach (DataRow d in detail.Rows)
            {
                totalDebit += ToDec(d["DebitAmountBDT"]);
                totalCredit += ToDec(d["CreditAmountBDT"]);
            }

            ViewBag.ReportTitle = isDebit ? "DEBIT VOUCHER"
                : voucherType.StartsWith("Credit") ? "CREDIT VOUCHER"
                : voucherType == "Journal" ? "JOURNAL VOUCHER"
                : "FUND TRANSFER";
            ViewBag.PaymentMode = paymentMode;
            ViewBag.ChequeNo = !isPayment ? "" : voucherType.EndsWith("Bank") ? m["BankCheckNo"].ToString() : "Cash";
            ViewBag.TotalDebit = totalDebit;
            ViewBag.TotalCredit = totalCredit;
            ViewBag.TakaInWords = TakaInWords(Math.Max(totalDebit, totalCredit));
            ViewBag.PrintedUser = Session[SessionCollection.UserName].ToString();
            ViewBag.Master = m;
            ViewBag.Detail = detail;
            ViewBag.Header = header.Rows.Count > 0 ? header.Rows[0] : null;
            return View();
        }

        private static decimal ToDec(object value)
        {
            decimal d;
            decimal.TryParse(Convert.ToString(value), out d);
            return d;
        }

        private static readonly string[] WordOnes = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] WordTens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static string TakaInWords(decimal amount)
        {
            long taka = (long)decimal.Truncate(amount);
            int paisa = (int)decimal.Round((amount - taka) * 100);
            string words = "( Taka " + NumberToWordsBD(taka);
            if (paisa > 0) words += " and Paisa " + NumberToWordsBD(paisa);
            return words + " Only )";
        }

        // Bangladeshi units: Crore, Lac, Thousand, Hundred
        private static string NumberToWordsBD(long n)
        {
            if (n == 0) return "Zero";
            if (n >= 10000000) return JoinWords(NumberToWordsBD(n / 10000000), "Crore", n % 10000000);
            if (n >= 100000) return JoinWords(NumberToWordsBD(n / 100000), "Lac", n % 100000);
            if (n >= 1000) return JoinWords(NumberToWordsBD(n / 1000), "Thousand", n % 1000);
            if (n >= 100) return JoinWords(NumberToWordsBD(n / 100), "Hundred", n % 100);
            if (n >= 20) return (WordTens[n / 10] + " " + WordOnes[n % 10]).TrimEnd();
            return WordOnes[n];
        }

        private static string JoinWords(string head, string unit, long rest)
        {
            string s = head + " " + unit;
            return rest > 0 ? s + " " + NumberToWordsBD(rest) : s;
        }

        public ActionResult rptDebitVoucher(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();


           

            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }


        public ActionResult rptCOA(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }



        public ActionResult rptBankBook(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }


        public ActionResult rptCashBook(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }

        public ActionResult rptCashBookOther(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }
        public ActionResult rptCashFlowReport(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }

        public ActionResult rptLedgerReport(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }


        public ActionResult rptLedgerReportSubsidiary(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }


        public ActionResult rptLedgerReportCostCenter(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }


        public ActionResult rptTrialBalance(string MasterId, string reportValue)
        {
            ResultResponse aResponse = new ResultResponse();
            BasicInfoReportsParameter aBasicInfoReportsParameter = new BasicInfoReportsParameter();
            aBasicInfoReportsParameter.ReportValue = reportValue;
            Session[SessionCollection.ReportValue] = aBasicInfoReportsParameter;


            ParamReport aParamReport = new ParamReport();




            aParamReport.MasterId = MasterId;
            Session[SessionCollection.ReportParam] = aParamReport;
            aResponse.isSuccess = true;
            return Json(aResponse);
        }

    }
}