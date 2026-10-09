// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Reports\IncomeStatementDynamicReportController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Report;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Reports
{
    public class IncomeStatementDynamicReportController : LegacyController
    {
        private readonly IncomeStatementDynamicReportDAL _reportDal = new IncomeStatementDynamicReportDAL();

        public ActionResult Index()
        {
            if (Session["LoginUserId"] != null)
            {
                return View("~/Views/AllAccountReport/IncomeStatementDynamicReport.cshtml");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetReport(
            int companyId,
            int financialYearId,
            DateTime fromDate,
            DateTime toDate)
        {
            try
            {
                List<IncomeStatementDynamicReportRow> rows = _reportDal.GetReport(
                    companyId,
                    financialYearId,
                    fromDate,
                    toDate);

                return Json(new { isSuccess = true, data = rows });
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
