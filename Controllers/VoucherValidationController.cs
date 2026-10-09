// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\VoucherValidationController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using New_Accounts.Models;
using New_Accounts.Models.ReportModel;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class VoucherValidationController : LegacyController
    {
        private readonly VoucherValidationDAL _dal = new VoucherValidationDAL();

        public ActionResult Index()
        {
            if (Session["LoginUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new VoucherValidationViewModel();
            return View("~/Views/AllAccountReport/VoucherValidationReport.cshtml", model);
        }

        [HttpGet]
        public JsonResult GetData(DateTime? fromDate, DateTime? toDate, string pageName, string status)
        {
            try
            {
                var filter = new VoucherValidationFilter
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    PageName = pageName,
                    IntegrityStatus = status
                };

                var data = _dal.GetVoucherValidation(filter);
                return Json(data);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = "Failed to load Accounts Integrity Report data", detail = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Reprocess(List<VoucherReprocessRequestDao> model)
        {
            ResultResponse aResponse = _dal.ReprocessVoucherAnomalies(model);
            return Json(aResponse);
        }


        [HttpGet]
        public JsonResult GetPages()
        {
            try
            {
                var data = _dal.GetPageNames();
                return Json(data);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = "Failed to load pages", detail = ex.Message });
            }
        }
    }
}
