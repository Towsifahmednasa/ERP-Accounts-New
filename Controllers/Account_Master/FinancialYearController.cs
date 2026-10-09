// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\FinancialYearController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models.Accounts_Master_M;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class FinancialYearController : LegacyController
    {
        private readonly FinalCialYearDal finalCialYearDal;
        public FinancialYearController()
        {
            this.finalCialYearDal = new FinalCialYearDal();
        }

        // GET: FinancialYear
        public ActionResult FinancialyearList()
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

        public ActionResult FinancialyearCreate(int id=0)
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

        public ActionResult Get_Financialyear(string param)
        {
            List<FinancialYearDao> aList = finalCialYearDal.GetCostcenter(param);
            return Json(aList);
        }

        public ActionResult Get_Financialyear_ById(int id)
        {
            return Json(finalCialYearDal.GetCostCentre_ById(id));
        }

        public ActionResult Save_Financialyear(FinancialYearDao employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(finalCialYearDal.Save_CostCenter(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult FinancialyearLock(int id = 0)
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


        public ActionResult Lock_Financialyear(FinancialYearDao employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(finalCialYearDal.Lock_FinancialYear(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}