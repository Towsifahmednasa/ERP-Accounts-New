// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\SubControlController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).

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
    public class SubControlController : LegacyController
    {

        private readonly SubcontrolLedger _subcontrol;
        // GET: SubControl


        public SubControlController()
        {
            this._subcontrol = new SubcontrolLedger();
        }

        public ActionResult SubControlCeate(int id=0)
        {        
            if (Session["LoginUserId"] != null)
            {
                ViewBag.id = id;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }

        }


        public ActionResult SubControlList()
        {
            return View();
        }

        public ActionResult Get_Ledger_DDL()
        {
            DataTable dt = _subcontrol.Load_Ledger_DDl();
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }



        public ActionResult Get_SubControll_List()
        {
            DataTable dt = _subcontrol.GET_SubControll_List("");
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }


        public ActionResult Get_ControlLedger_By_Ledger_DDL(string LedgerId, string mode)
        {
            DataTable dt = _subcontrol.Load_ControlLedger_By_Ledger(LedgerId, mode);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult SaveSubcontrolInformation(SubControlLedger employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(_subcontrol.SaveSubcontrolInformation(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }
         
        }

        public ActionResult Get_SubControlLed_ById(int id)
        {
            DataTable dt = _subcontrol.Load_SubControlLedger_ById(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }
    }
}