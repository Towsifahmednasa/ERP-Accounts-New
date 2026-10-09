// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\SubSubControlLedgerController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class SubSubControlLedgerController : LegacyController
    {

        SubSubControlLedgerDAL dbDal = new SubSubControlLedgerDAL();
        // GET: SubSubControlLedger
        public ActionResult SSCLCreate(int id = 0)
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
        public ActionResult SSCLList()
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


        public ActionResult Save_SSCL(SubSubControlLedgerDAO aMaster)
        {

            if (Session["LoginUserId"] != null)
            {
                return Json(dbDal.Save_SSCL(aMaster));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Get_SSCLList()
        {

            if (Session["LoginUserId"] != null)
            {

                List<SubSubControlLedgerView> aList = dbDal.Get_SubSubControlLedgerList("");
                return Json(aList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }



        public ActionResult Get_SSCLById(int id)
        {
            DataTable dt = dbDal.Get_SSCL_ById(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

    }
}