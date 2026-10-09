// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\LoanController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.DAL.Loan_DAL;
using New_Accounts.Models.Accounts_Master_M;
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
    public class LoanController : LegacyController
    {
        private readonly LoanDAL _LoanDAL;

        public LoanController()
        {
            this._LoanDAL = new LoanDAL();
        }

        // GET: ChartOfAccount ttt  
        public ActionResult L_Entry(int id = 0)
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
        public ActionResult L_List()
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


        public JsonResult Load_LC_Reference_Number(string companyId, string supplierId)
        {
            List<LCPreparationMaster> bu = _LoanDAL.Load_Reference_No(companyId, supplierId);
            var asd = Json(bu);
            return Json(bu);
        }

       

        public ActionResult InsUpLoanInfo(LoanInfoModelDAO model)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(_LoanDAL.Save_LoanInfo(model));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Get_LoanInfoList(string param)
        {
            List<LoanDataModelListDAO> aList = _LoanDAL.GetLoanInfoList(param);
            return Json(aList);
        }



        public ActionResult Get_LoanById(int id)
        {
            DataTable dt = _LoanDAL.Get_LoanById(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }



    }

}