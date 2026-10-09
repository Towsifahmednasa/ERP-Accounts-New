// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\BalanceSheetController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class BalanceSheetController : LegacyController
    {
        BalanceSheetDAL _dal = new BalanceSheetDAL();

        public ActionResult BalanceSheetReport()
        {
            return View("~/Views/AllAccountReport/BalanceSheetReport.cshtml");
        }

        [HttpPost]
        public ActionResult BalanceSheetReport(
            int CompanyID,
            int FinancialYearID,
            DateTime FromDate,
            DateTime ToDate)
        {
            DataTable dt = _dal.GetBalanceSheetReport(
                CompanyID,
                FinancialYearID,
                FromDate,
                ToDate);

            return View("~/Views/AllAccountReport/BalanceSheetReport.cshtml", dt);
        }


        public ActionResult BSCreate()
        {
            if (Session["LoginUserId"] != null)
                return View("~/Views/BalanceSheet/BSCreate.cshtml");
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult GetByCompany(int companyId)
        {
            try
            {
                List<BalanceSheetDetail> data = _dal.GetDetailsByCompanyId(companyId);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────────────────
        // POST: Save Income Statement
        // ─────────────────────────────────────────────────────────
        [HttpPost]
        public ActionResult SaveBalanceSheet(BalanceSheetMaster aMaster)
        {
            try
            {
                int userId = Convert.ToInt32(Session[SessionCollection.LoginUserId]);
                ResultResponse result = _dal.SaveBalanceSheet(aMaster, userId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new ResultResponse { isSuccess = false, msg = ex.Message });
            }
        }
    }
}