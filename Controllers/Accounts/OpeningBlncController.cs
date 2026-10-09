// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\OpeningBlncController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models.Accounts;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class OpeningBlncController : LegacyController
    {
        // GET: OpeningBlnc

        OpeningBlncDAL dbDal = new OpeningBlncDAL();

        public ActionResult OpningBlncList()
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

        public ActionResult OpningBlncEntry(int id = 0)
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



        public ActionResult Save_OpningBlnc(OpeningBlncDAO employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(dbDal.Save_OpningBlnc(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Get_OpningBlncList(string companyID, string Yearid)
        {
            List<OpeningBlncListDAO> aList = dbDal.Get_OpningBlncList( companyID,  Yearid);
            return Json(aList);
        }


        public ActionResult Get_OpningBlnc_ById(int id)
        {
            return Json(dbDal.Get_OpningBlnc_ById(id));
        }


    }
}