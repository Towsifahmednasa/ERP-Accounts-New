// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\BankPaymentsController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class BankPaymentsController : LegacyController
    {
        BankPaymentsDAL dbDal = new BankPaymentsDAL();
        // GET: BankPayments
        public ActionResult Index()
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


        public ActionResult Create(int MasterId = 0)
        {
            if (Session["LoginUserId"] != null)
            {
                ViewBag.MasterId = MasterId;
            return View();

        }
            else
            {
                return RedirectToAction("Index", "Home");
                  }
}


        public ActionResult AddRowForCOA(int tr)
        {
            TempData["trSl"] = tr;

            return PartialView("_addRowForCOA");
        }


        [HttpPost]
        public ActionResult SaveBankPayment(DebitCreditVoucherMaster aMaster)
        {
            // FI period close: posting into a closed month is a hard stop; parking a draft is allowed.
            if (aMaster.ActionStatus != "Parked" && aMaster.PostingDate.HasValue
                && new JournalDAL().IsPeriodClosed(aMaster.CompanyInfoId ?? 0, aMaster.PostingDate.Value))
            {
                return Json(new ResultResponse { isSuccess = false, pk = 0, msg = "Posting period " + aMaster.PostingDate.Value.ToString("MM/yyyy") + " is closed for financial accounting. Park the voucher or re-open the period." });
            }

            ResultResponse aResponse = dbDal.SaveBankPaymentMaster(aMaster,  (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }


        public ActionResult BankAccountList(string param)
        {
            List<DebitVoucherView> aList = dbDal.BankPaymentList(param + "  and mas.VoucherType in ('DebitVoucherCash','DebitVoucherBank' )");
            return Json(aList);

        }


        public ActionResult GetBankPaymentMasterId(int id)
        {
            DebitCreditVoucherMaster aMaster = dbDal.GetBankPaymentMasterId(id);
            aMaster.DetailsInfo = dbDal.GetBankPaymentDetailsById(id);
            return Json(aMaster);
        }


        public ActionResult GetBankInfoMasterId(int CompanyID, int BankAccountID, int FinancialYearID,
              DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            BankAccountDAO aMaster = dbDal.GetBankAccountInfo_ById(CompanyID, BankAccountID, FinancialYearID,
                FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate);
          
            return Json(aMaster);
        }


        public ActionResult GetCashInfoIndoById(int CompanyID, int CashAccountID, int FinancialYearID,
                              DateTime  FromDate, DateTime ToDate, string CashbookNAme, DateTime CashBookFinancialYearStartDate)
        {
            BankAccountDAO aMaster = dbDal.GetCashInfoIndoById(CompanyID, CashAccountID, FinancialYearID,
                                FromDate, ToDate, CashbookNAme, CashBookFinancialYearStartDate);

            return Json(aMaster);
        }


        public ActionResult GetBankCashInfoMasterId(int CompanyID, int BankAccountID, int FinancialYearID,
            DateTime FromDate, DateTime ToDate, string BankNAme, DateTime BankBookFinancialYearStartDate)
        {
            BankAccountDAO aMaster = dbDal.GetBankCashInfoMasterId(CompanyID, BankAccountID, FinancialYearID,
                FromDate, ToDate, BankNAme, BankBookFinancialYearStartDate);

            return Json(aMaster);
        }



    }
}