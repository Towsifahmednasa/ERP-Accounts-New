// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\TestController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class TestController : LegacyController
    {

        TestDAL dbDal = new TestDAL();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        // GET: Test/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult AddRowForCountry(int tr)
        {
            TempData["trSl"] = tr;

            return PartialView("_addNewCountryRow");
        }

        // GET: Test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        public ActionResult Create(Microsoft.AspNetCore.Http.IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Test/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Microsoft.AspNetCore.Http.IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Microsoft.AspNetCore.Http.IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




        public ActionResult TsetListData(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
        {
            List<TestMasterView> aList = dbDal.GeTestMaster(fromdate, todate, CompanyId);
            return Json(aList);

        }


        [HttpPost]
        public ActionResult SaveTest(TestMaster aMaster)
        {

            ResultResponse aResponse = dbDal.SaveTest(aMaster, Session[SessionCollection.UserName].ToString());
            return Json(aResponse);
        }

    }
}
