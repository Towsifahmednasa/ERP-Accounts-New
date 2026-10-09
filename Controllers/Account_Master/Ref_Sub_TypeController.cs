// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\Ref_Sub_TypeController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models.Accounts_Master_M;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class Ref_Sub_TypeController : LegacyController
    {
        private readonly ReferenceTypeDal reference_;
        private readonly SubsidaryTypeDal subsidaryType;

        public Ref_Sub_TypeController()
        {
            this.reference_ = new ReferenceTypeDal();
            this.subsidaryType = new SubsidaryTypeDal();
        }

        // GET: Ref_Sub_Type
        public ActionResult ReferenceType(int id=0)
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

        public ActionResult ReferenceTypeList()
        {
            return View();
        }


        public ActionResult Get_ReferenceType( )
        {
            List<Reference_Type> aList = reference_.GeReferenceType();
            return Json(aList);
        }

        public ActionResult GeReferenceType_ById(int id)
        {
            return Json(reference_.GeReferenceType_ById(id));          
        }

        public ActionResult Save_ReferenceType(Reference_Type employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(reference_.Save_ReferenceType(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }
        }

        #region
        public ActionResult SubsidairyType(int id = 0)
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

        public ActionResult SubsidairyList()
        {
            return View();
        }


        public ActionResult Get_SubsidairyType()
        {
            List<Subsidarytype> aList = subsidaryType.GeSubsidairyType();
            return Json(aList);
        }

     
        public ActionResult GetSubsidairyType_ById(int id)
        {
            return Json(subsidaryType.GeSubsidairyType_ById(id));
        }


        public ActionResult Save_SubsidairyType(Subsidarytype employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(subsidaryType.Save_SubsidairyTypeType(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

    }
}