// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\LCRelatedBankPaymentController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Loan_DAL;
using New_Accounts.DAL.Report;
using New_Accounts.Models.LOAN_DAO;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class LCRelatedBankPaymentController : LegacyController
    {
        // GET: LCRelatedBankPayment

        private readonly LCRelatedBankPaymentDAL _LCRelatedBankPaymentDAL;

        public LCRelatedBankPaymentController()
        {
            this._LCRelatedBankPaymentDAL = new LCRelatedBankPaymentDAL();
        }
        public ActionResult LoadCIInfo(string LCNoID)
        {
            DataTable dt = _LCRelatedBankPaymentDAL.LoadCIInfo(LCNoID);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }
        public ActionResult BankPaymentIndex(int id = 0)
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


        public ActionResult BankPayment_List()
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

        public ActionResult Get_BankChargeList(string param)
        {
            List<LCRelatedBankPaymentMasterListDAO> aList = _LCRelatedBankPaymentDAL.Get_BankChargeList(param);
            return Json(aList);
        }


        //public ActionResult Get_LCRelatedBankPayment_Edit_Mode(int id)
        //{
        //    DataTable dt = _LoanDAL.Get_LoanById(id);
        //    string JSONresult;
        //    JSONresult = JsonConvert.SerializeObject(dt);
        //    return Json(JSONresult);
        //}

        public ActionResult InsUpBankPaymentInfo(BankPaymentMasterDAO model)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(_LCRelatedBankPaymentDAL.Save_LoanInfo(model));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }




    }
}