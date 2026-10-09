// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\IncomeStatementController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class IncomeStatementController : LegacyController
    {
        IncomeStatementDAL _dal = new IncomeStatementDAL();

        public ActionResult IncomeStatementReport()
        {
            return View("~/Views/AllAccountReport/IncomeStatementReport.cshtml");
        }

        [HttpPost]
        public ActionResult IncomeStatementReport(
            int CompanyID,
            int FinancialYearID,
            DateTime FromDate,
            DateTime ToDate)
        {
            DataTable dt = _dal.GetIncomeStatementReport(
                CompanyID,
                FinancialYearID,
                FromDate,
                ToDate);

            return View("~/Views/AllAccountReport/IncomeStatementReport.cshtml", dt);
        }

        public ActionResult ISCreate()
        {
            if (Session["LoginUserId"] != null)
                return View("~/Views/IncomeStatement/ISCreate.cshtml");
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult GetByCompany(int companyId)
        {
            try
            {
                List<IncomeStatementDetail> data = _dal.GetDetailsByCompanyId(companyId);
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
        public ActionResult SaveIncomeStatement(IncomeStatementMaster aMaster)
        {
            try
            {
                int userId = Convert.ToInt32(Session[SessionCollection.LoginUserId]);
                ResultResponse result = _dal.SaveIncomeStatement(aMaster, userId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new ResultResponse { isSuccess = false, msg = ex.Message });
            }
        }
    }
}