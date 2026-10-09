// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\CostCentreController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL;
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models;
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
    public class CostCentreController : LegacyController
    {
        private readonly CostcentreDal costcentre;
        public CostCentreController()
        {
            this.costcentre = new CostcentreDal();
        }

        // GET: CostCentre
        public ActionResult CostcentreInformation(int id=0)
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

        public ActionResult CostcentreList()
        {
            return View();
        }

        public ActionResult Get_CostCentre()
        {
            List<CostCentreDao> aList = costcentre.GetCostcenter();
            return Json(aList);
        }


        


        #region   Rana 

        public ActionResult Get_CostCentre_R()
        {
            // Assuming costcentre.GetCostcenterRana() returns a DataTable instead of List<CostCentreDao>
            DataTable dt = costcentre.GetCostcenterR(); // Make sure the method returns a DataTable
            string JSONresult;

            // Serialize the DataTable to JSON format
            JSONresult = JsonConvert.SerializeObject(dt);

            // Return the serialized JSON data
            return Json(JSONresult);
        }



        public ActionResult GetCostCentre_ById_Rana(int id)
        {
            return Json(costcentre.GetCostCentre_ById_Rana(id));
        }

        public ActionResult Save_CostCenter_Rana(CostCentreDao employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(costcentre.Save_CostCenter_Rana(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public JsonResult Load_Company_Info()
        {
            List<CompanyInformation> companies = costcentre.Company_Information();
            var asd = Json(companies);
            return Json(companies);
        }

        public ActionResult Load_Company_Info_ById(int id)
        {
            var companies = costcentre.Load_Company_Info_ById(id);
            return Json(companies);
        }



        public ActionResult Del_CostCenter_Info_By_Id(int id)
        {
            if (Session["LoginUserId"] != null)
            {

                return Json(costcentre.DelCostCenterInfoById(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion





        public ActionResult Get_CostCenter_ById(int id)
        {
            return Json(costcentre.GetCostCentre_ById(id));
        }

        public ActionResult Save_CostCenter(CostCentreDao employee)
        {
            if (Session["LoginUserId"] != null)
            {
                return Json(costcentre.Save_CostCenter(employee));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}