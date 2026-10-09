// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\ChartOfAccountController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models.Accounts;
using New_Accounts.Models.Accounts_Master_M;
using New_Accounts.Utility;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class ChartOfAccountController : LegacyController
    {

        private readonly ChartOfAccountDal chartOfAccountDal;

        public ChartOfAccountController()
        {
            this.chartOfAccountDal = new ChartOfAccountDal();
        }

        // GET: ChartOfAccount ttt  
        public ActionResult ChartOfAccountCreate(int id=0)
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
        public ActionResult ChartOfAccountList()
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

        public ActionResult GetChartofAccount_ByCompanyId(int id)
        {
            DataTable dt = chartOfAccountDal.GetChartofAccountByCompanyId(Convert.ToString(id));
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult LoadReferanceInPartialView(int glid)
        {
            List<ReferanceType> cc = chartOfAccountDal.GetAllReferanceType(glid);
            return PartialView("_RefPartial", cc);
        }


        public ActionResult LoadSubSidaryInPartialView(int glid)
        {
            List<SubsidiaryType_Ddl> cc = chartOfAccountDal.GetAllSubSidaryType(glid);
            return PartialView("_SubSidaryPartial", cc);
        }

        public ActionResult LoadCostCenterInPartialView(int glid)
        {
            List<CostCenter_Ddl> cc = chartOfAccountDal.GetAllCostCenter(glid);
            return PartialView("_CostCenterPartial", cc);
        }

        public ActionResult Save_ChartOfAccount(ChartOfAccount employee)
        {
        
            if (Session["LoginUserId"] != null)
            {
                return Json(chartOfAccountDal.Save_ChartOfAccount(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Get_ChartofAccount_List(string param)
        {

            if (Session["LoginUserId"] != null)
            {

                List<COAView> aList = chartOfAccountDal.Get_ChartOfAccount_List(param);
            return Json(aList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult Get_Company_DDL()
        {
            DataTable dt = chartOfAccountDal.Load_Company_();
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult Get_CompanyUnit_DDL(int id)
        {
            DataTable dt = chartOfAccountDal.Load_CompanyUnit(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }


        public ActionResult Get_Sub_ControlLed_ById(int id)
        {
            DataTable dt = chartOfAccountDal.Load_SubControl_ByControlLedgerId(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public ActionResult Get_ChartofAccount_ById(int id)
        {
            DataTable dt = chartOfAccountDal.Get_ChartofAccount_ById(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }

        public JsonResult CheckAccountCodeExists(string accountCode)
        {
            DataTable dt = chartOfAccountDal.CheckAccountCodeExists(accountCode);
            int count = dt.Rows.Count;

            return Json(new { count = count });
        }




        public ActionResult GetAccountCodebySubSubControlLedgerId(int id)
        {
            DataTable dt = chartOfAccountDal.GetAccountCodebySubSubControlLedgerId(id);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }


        public ActionResult SaveSubInfoDetails(List<SubsidiaryInfoDAO> entity)
        {
            try
            {

                int entryBy = Convert.ToInt32(Session[SessionCollection.LoginUserId]) ;
                var aResponse = chartOfAccountDal.SaveSubInfoDetails(entity, entryBy);

                return Json(aResponse);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult SaveRefInfoDetails(List<ReferenceInfoDAO> entity)
        {
            try
            {

                int entryBy = Convert.ToInt32(Session[SessionCollection.LoginUserId]);
                var aResponse = chartOfAccountDal.SaveRefInfoDetails(entity, entryBy);

                return Json(aResponse);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}