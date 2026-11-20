// Accounts Integrity Dashboard: voucher problems of Delivery Confirmation, Customer Payment, Invoice Adjustment,
// Non-Transactional Sales Return and Delivery Sales Return, with a Fix button per transaction.
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using New_Accounts.Compat;
using New_Accounts.DAL.AccountsIntegrity;
using New_Accounts.Utility;

namespace New_Accounts.Controllers
{
    public class AccountsIntegrityController : LegacyController
    {
        private readonly AccountsIntegrityDAL _dal = new AccountsIntegrityDAL();

        public ActionResult Index()
        {
            if (Session["LoginUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "Accounts Integrity Dashboard";
            return View();
        }

        [HttpGet]
        public JsonResult GetIssues(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var issues = _dal.GetIssues(fromDate, toDate);
                var pages = AccountsIntegrityCatalog.Areas.Select(a => new { Key = a.Key, Page = a.Page }).ToList();
                return Json(new { Pages = pages, Issues = issues, GeneratedAt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = "Failed to load the integrity data", detail = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Fix(string area, string problem, int docId, int docId2, bool simulate)
        {
            return Json(_dal.Fix(area, problem, docId, docId2, UserName(), simulate));
        }

        [HttpPost]
        public JsonResult FixAll(string area, string problem, DateTime? fromDate, DateTime? toDate, bool simulate)
        {
            return Json(_dal.FixAll(area, problem, fromDate, toDate, UserName(), simulate));
        }

        private string UserName() => (Session[SessionCollection.UserName] as string) ?? "";
    }
}
