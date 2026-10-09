// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\BanckAccountController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class BanckAccountController : LegacyController
    {
        BanckAccountDAL dbDal = new BanckAccountDAL();
        // GET: BanckAccount
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


        public ActionResult Create(int id=0)
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

        [HttpPost]
        public ActionResult SaveBankAccount(BankAccountDAO aMaster)
        {


            if (Session["LoginUserId"] != null)
            {

                ResultResponse aResponse = dbDal.SaveBankAccount(aMaster, Convert.ToInt32(Session[SessionCollection.LoginUserId].ToString()));
            return Json(aResponse);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Get_BankAcoount_Edit(int id)
        {
            return Json(dbDal.GetBankAccount_ById(id));
        }

        public ActionResult BankAccountList(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
        {
            List<BankAccountView> aList = dbDal.BankAccountList(fromdate, todate, CompanyId);
            return Json(aList);

        }

    }
}