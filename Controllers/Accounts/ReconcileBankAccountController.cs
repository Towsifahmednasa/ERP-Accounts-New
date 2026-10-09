// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\ReconcileBankAccountController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class ReconcileBankAccountController : LegacyController
    {

        ReconcileBankAccountDAL dbDal = new ReconcileBankAccountDAL();

        // GET: ReconcileBankAccount
        public ActionResult RBAList()
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

        public ActionResult RBAEntry(int id = 0)
        {

            if (Session["LoginUserId"] != null)
            {
                ViewBag.BankAccountId = id;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        //public ActionResult GetReconcileBankAccountListAll(string param)
        //{
             

        //    List<DebitCreditVoucherDetail> aList = dbDal.GetReconcileBankAccountListAll(param);
        //    return Json(aList);
        //}
        public ActionResult GetReconcileBankAccountListAll(string param)
        {
            List<DebitVoucherView> aList = dbDal.GetReconcileBankAccountListAll(param + "  and mas.VoucherType in ('CreditVoucherBank','DebitVoucherBank' )  and mas.ActionStatus<>'Canceled'  ");
            return Json(aList);

        }


        public ActionResult GetReconcileBankAccountRptAll(string param)
        {
            List<DebitVoucherView> aList = dbDal.GetReconcileBankAccountListAll(param + "  and mas.IsReconcile=1 ");
            return Json(aList);

        }

        [HttpPost]
        public ActionResult SaveReconcileBank(List<ReconcileBankAccount> aMaster)
        {

            ResultResponse aResponse = dbDal.SaveBankPaymentMaster(aMaster, (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }


        public ActionResult ReconcileBankAccountList(string param)
        {
            List <DebitVoucherView> aList = dbDal.GetReconcileBankAccountListAll(param + "  and mas.VoucherType in ('CreditVoucherBank','DebitVoucherBank' ) and mas.ActionStatus<>'Canceled' and mas.IsReconcile<>1");
            return PartialView("_partialRBAccount", aList);
        }

    }
}