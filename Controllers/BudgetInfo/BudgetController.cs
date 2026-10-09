// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\BudgetInfo\BudgetController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Budget;
using New_Accounts.Models;
using New_Accounts.Models.Budget;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.BudgetInfo
{
    public class BudgetController : LegacyController
    {
        // GET: Budget
        BudgetInfoDAL dbDal = new BudgetInfoDAL();
        public ActionResult BudgetList()
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

        public ActionResult BudgetEntry(int id = 0)
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


        public ActionResult BudgetMonthAmountDetails(string comID, string finID, string id)
        {
            List<BudgetMonthAmountDAO> aList = dbDal.BudgetMonthAmountDetails(comID, finID, id);
            return PartialView("_partialBudgetDtlsInfo", aList);
        }


        [HttpPost]
        public ActionResult SaveBudgetInfo(BudgetMasterDAO aMaster)
        {
            if (Session["LoginUserId"] != null)
            {
                ResultResponse aResponse = dbDal.SaveBudgetInfoDAL(aMaster, (Session[SessionCollection.UserName].ToString()));
                return Json(aResponse);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult GetBudgetList(string param)
        {

            if (Session["LoginUserId"] != null)
            {
                List<BudgetDAO> aList = dbDal.BudgetList(param);
                return Json(aList);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }



        public ActionResult GetBudgetMasterId(int id)
        {
            BudgetMasterDAO aMaster = dbDal.GetBudgetMasterId(id);
            //aMaster.DetailsInfo = dbDal.GetBankPaymentDetailsById(id);
            return Json(aMaster);
        }


    }
}