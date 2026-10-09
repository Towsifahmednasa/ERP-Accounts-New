// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\SubsubsidairyController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class SubsubsidairyController : LegacyController
    {

        SubsubsidairyDAL dbDal = new SubsubsidairyDAL();
        // GET: Subsubsidairy
        public ActionResult SubsubsidairyCreate(int id=0)
        {
            ViewBag.Id = id;
            return View();
        }


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

        public ActionResult Save_SubsidiaryLedger(SubsidiaryLedgerDAO aMaster)
        {

            if (Session["LoginUserId"] != null)
            {
                return Json(dbDal.Save_SubsidiaryLedger(aMaster));
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }
        }


        public ActionResult Get_SubsidiaryLedgerList()
        {
            if (Session["LoginUserId"] != null)
            {
                DataTable dt = dbDal.Get_Subsubsidairy("");
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}