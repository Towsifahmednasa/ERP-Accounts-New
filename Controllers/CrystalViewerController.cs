// Replaces CrystalViewer/CrystalViewer.aspx (WebForms + SAP Crystal Reports 13 runtime). Crystal Reports has no
// .NET Core / .NET 10 runtime, so the 22 .rpt reports cannot be rendered by this application
// (docs/migration/blocked-pages.md). The legacy report pages still stage their parameters in Session
// (AccountingTaskReportController rpt* actions) and open this URL in a new window; this page explains the
// situation and shows the staged parameters so the request is not silently lost.
using Microsoft.AspNetCore.Mvc;
using New_Accounts.Compat;

namespace New_Accounts.Controllers
{
    public class CrystalViewerController : LegacyController
    {
        public ActionResult Index(string page)
        {
            ViewBag.Title = "Crystal report";
            ViewBag.Page = page;
            ViewBag.ReportValue = Session["reportvalue"] as string;
            ViewBag.ReportParam = Session["reportparam"] as string;
            ViewBag.Query = Request.QueryString.Value;
            return View();
        }
    }
}
