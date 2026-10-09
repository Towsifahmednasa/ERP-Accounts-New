// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\ContraController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class ContraController : LegacyController
    {

        ContraDAL dbDal = new ContraDAL();
        BankPaymentsDAL dbDebit = new BankPaymentsDAL();

        // GET: Contra
        public ActionResult ContraList()
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

        public ActionResult ContraEntry(int MasterId = 0)
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


        [HttpPost]
        public ActionResult SaveContra(DebitCreditVoucherMaster aMaster)
        {

            ResultResponse aResponse = dbDal.SaveContra(aMaster,  (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }


        public ActionResult GetContraMasterId(int id)
        {
            DebitCreditVoucherMaster aMaster = dbDal.GetContraMasterId(id);
            //aMaster.DetailsInfo = dbDebit.GetBankPaymentDetailsById(id);
            return Json(aMaster);
        }


        public ActionResult GetContraList(string param)
        {
            List<DebitVoucherView> aList = dbDebit.BankPaymentList(param + "  and mas.VoucherType='FundTransfer'");
            return Json(aList);

        }

    }
}