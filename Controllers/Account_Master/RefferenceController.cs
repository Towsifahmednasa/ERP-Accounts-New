// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\RefferenceController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models.Accounts;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class RefferenceController : LegacyController
    {
        Ref_sub_InfoDAL dbDal = new Ref_sub_InfoDAL();
        // GET: Refference
        public ActionResult ReferenceType()
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

        public ActionResult ReferenceInfo()
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


        public ActionResult SubsidiaryType()
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

        public ActionResult SubsidiaryInfo()
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


        public ActionResult LoadSubsidiaryInfoInPartialView(string SubsidiaryType)
        {
            List<Subsidiary_chk> cc = dbDal.GetAllSubsidiaryInfo(SubsidiaryType);
            return PartialView("_SubsidiaryInfo", cc);
        }


        public ActionResult LoadReferenceInfoInPartialView(string ReferenceType)
        {
            List<RefInfo_chk> cc = dbDal.GetAllReffInfo(ReferenceType);
            return PartialView("_RefCheckInfo", cc);
        }



        public ActionResult LoadSubsidiaryTableInfo(string SubsidiaryType)
        {
            DataTable dt = dbDal.GetSubsidiaryTableInfo(  SubsidiaryType);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }


        public ActionResult LoadRefTableInfo(string ReferenceType)
        {
            DataTable dt = dbDal.GetRefTableInfo(ReferenceType);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult CostCenterInfo()
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
    }
}