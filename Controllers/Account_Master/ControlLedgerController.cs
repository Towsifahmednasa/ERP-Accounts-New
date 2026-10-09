// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\ControlLedgerController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models.Accounts_Master_M;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class ControlLedgerController : LegacyController
    {
        // GET: ControlLedger
        private readonly ControlLedgerDal controlLedger;
        public ControlLedgerController()
        {
            this.controlLedger = new ControlLedgerDal();
        }

        public ActionResult ControlLedgerList()
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

        public ActionResult ControlLedgerCreate(int id = 0)
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

        public ActionResult Get_Ledger_DDL()
        {
            DataTable dt = controlLedger.Load_Ledger_DDl();
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }


        public ActionResult Get_ControlLedger_List()
        {
            DataTable dt = controlLedger.Get_ControlLedger("");
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult Save_EmployeeInformation(ControlLedger employee)
        {

            if (Session["LoginUserId"] != null)
            {
                return Json(controlLedger.SaveEmployeeInformation(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }

            
        }


      
        public ActionResult Get_ControlLed_ById(int id)
        {
            DataTable dt = controlLedger.Get_ControlLedger_ById(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

    }
}