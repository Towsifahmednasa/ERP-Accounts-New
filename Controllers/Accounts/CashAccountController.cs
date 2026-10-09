// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\CashAccountController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class CashAccountController : LegacyController
    {

        private readonly CashAccountDal cashAccount;

        public CashAccountController()
        {
            this.cashAccount = new CashAccountDal();
        }


        // GET: CashAccount
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

        public ActionResult Create(int id = 0)
        {
           
            if (Session["LoginUserId"] != null)
            {
                ViewBag.Id = id;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult SaveCashAccount(CashAccount aMaster)
        {          
            if (Session["LoginUserId"] != null)
            {
               // return Json(costcentre.Save_CostCenter(employee));

                ResultResponse aResponse = cashAccount.SaveCashAccount(aMaster, Convert.ToInt32(Session[SessionCollection.LoginUserId].ToString()));
                return Json(aResponse);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public ActionResult Get_CashAcoount_Edit(int id)
        {
            return Json(cashAccount.GetCashAccount_ById(id));
        }

        public ActionResult CashAccountList(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
        {
            List<CashAccountView> aList = cashAccount.CashAccountList(fromdate, todate, CompanyId);
            return Json(aList);

        }
    }
}