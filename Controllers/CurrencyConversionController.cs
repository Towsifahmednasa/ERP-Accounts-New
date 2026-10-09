// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\CurrencyConversionController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
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
    public class CurrencyConversionController : LegacyController
    {
        // GET: CurrencyConversion

        BankPaymentsDAL Debit_Dal = new BankPaymentsDAL();
        public ActionResult CCCreate(int MasterId = 0)
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


      

        // Action to return currency rates as JSON via AJAX, filtered by date
        public ActionResult Load_CurrencyConversionList(string date)
        {
            DataTable dt = Debit_Dal.Load_CurrencyConversionList(date);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }
      

        [HttpPost]
        public ActionResult SaveCurrencyConversion(CurrencyConversionMsterDao aMaster)
        {
            ResultResponse aResponse = Debit_Dal.SaveCurrencyConversion(aMaster, (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }
        // T 

    }
}