// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\CreditVoucherController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class CreditVoucherController : LegacyController
    {
        // GET: CreditVoucher
        CreditVoucherDAL dbDal = new CreditVoucherDAL();
        BankPaymentsDAL Debit_Dal = new BankPaymentsDAL();

        public ActionResult CVList()
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


        public ActionResult CVCreate(int MasterId = 0)
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
        public ActionResult SaveCRVoucher(DebitCreditVoucherMaster aMaster)
        {

            ResultResponse aResponse = dbDal.SaveBankPaymentMaster(aMaster,  (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }


        public ActionResult BankAccountList(string param)
        {
            List<DebitVoucherView> aList = Debit_Dal.BankPaymentList(param + "  and mas.VoucherType in ('CreditVoucherBank','CreditVoucherCash' )");
            return Json(aList);

        }

        public ActionResult GetCreditMasterId(int id)
        {
            DebitCreditVoucherMaster aMaster = dbDal.GetCreditVoucherMasterId(id);
            aMaster.DetailsInfo = dbDal.GetCreditVoucherDetailsById(id);
            return Json(aMaster);
        }


    }
}