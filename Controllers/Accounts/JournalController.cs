// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\JournalController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class JournalController : LegacyController
    {

        JournalDAL dbDal = new JournalDAL();
        BankPaymentsDAL Debit_Dal = new BankPaymentsDAL();
        // GET: Journal
        public ActionResult JRList()
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


        public ActionResult JRCreate(int MasterId = 0)
        {
            if (Session["LoginUserId"] != null)
            {
                ViewBag.MasterId = MasterId;
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult AddJRRowForCOA(int tr)
        {
            TempData["trSl"] = tr;

            return PartialView("_addJRRowForCOA");
        }


        public ActionResult BankAccountList(string param)
        {
            List<DebitVoucherView> aList = Debit_Dal.BankPaymentList(param + "  and mas.VoucherType in ('Journal')");
            return Json(aList);

        }


        [HttpPost]
        public ActionResult SaveJournal(DebitCreditVoucherMaster aMaster)
        {
            // FI period close: posting into a closed month is a hard stop; parking a draft is allowed.
            if (aMaster.ActionStatus != "Parked" && aMaster.PostingDate.HasValue
                && dbDal.IsPeriodClosed(aMaster.CompanyInfoId ?? 0, aMaster.PostingDate.Value))
            {
                return Json(new ResultResponse { isSuccess = false, pk = 0, msg = "Posting period " + aMaster.PostingDate.Value.ToString("MM/yyyy") + " is closed for financial accounting. Park the voucher or re-open the period." });
            }

            ResultResponse aResponse = dbDal.SaveJournalMaster(aMaster,  (Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);
        }


        // ---- Voucher attachments ----

        [HttpPost]
        public ActionResult UploadVoucherAttachment(int masterId, Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (Session["LoginUserId"] == null) { return Json(new ResultResponse { isSuccess = false, msg = "Not signed in." }); }
            if (masterId <= 0 || file == null || file.Length == 0) { return Json(new ResultResponse { isSuccess = false, msg = "Save the voucher first, then attach the file." }); }
            if (file.Length > 10 * 1024 * 1024) { return Json(new ResultResponse { isSuccess = false, msg = "File is larger than 10 MB." }); }

            byte[] data;
            using (var ms = new System.IO.MemoryStream())
            {
                file.CopyTo(ms);
                data = ms.ToArray();
            }
            int pk = dbDal.SaveVoucherAttachment(new VoucherAttachment
            {
                VoucherMasterId = masterId,
                FileName = System.IO.Path.GetFileName(file.FileName),
                ContentType = file.ContentType,
                FileSize = file.Length,
                FileData = data
            }, Session[SessionCollection.UserName]?.ToString());
            return Json(new ResultResponse { isSuccess = pk > 0, pk = pk });
        }

        public ActionResult VoucherAttachmentList(int masterId)
        {
            return Json(dbDal.GetVoucherAttachments(masterId));
        }

        public ActionResult DownloadVoucherAttachment(int id)
        {
            VoucherAttachment a = dbDal.GetVoucherAttachment(id);
            if (a == null) { return NotFound(); }
            return File(a.FileData, string.IsNullOrEmpty(a.ContentType) ? "application/octet-stream" : a.ContentType, a.FileName);
        }

        [HttpPost]
        public ActionResult DeleteVoucherAttachment(int id)
        {
            return Json(new ResultResponse { isSuccess = dbDal.DeleteVoucherAttachment(id) });
        }


        // ---- FI period close ----

        public ActionResult PeriodClose()
        {
            if (Session["LoginUserId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PeriodCloseList(int companyId, int year)
        {
            return Json(dbDal.GetClosedPeriods(companyId, year));
        }

        public ActionResult PeriodCloseStatus(int companyId, string date)
        {
            DateTime d;
            bool closed = DateTime.TryParse(date, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d)
                && dbDal.IsPeriodClosed(companyId, d);
            return Json(new { closed });
        }

        [HttpPost]
        public ActionResult SetPeriodClose(int companyId, int year, int month, bool isClosed)
        {
            if (Session["LoginUserId"] == null) { return Json(new ResultResponse { isSuccess = false, msg = "Not signed in." }); }
            bool ok = dbDal.SetPeriodClose(companyId, year, month, isClosed, Session[SessionCollection.UserName]?.ToString());
            return Json(new ResultResponse { isSuccess = ok });
        }



        public ActionResult GetJournalMasterId(int id)
        {
            DebitCreditVoucherMaster aMaster = dbDal.GetCreditVoucherMasterId(id);
            aMaster.DetailsInfo = dbDal.GetCreditVoucherDetailsById(id);
            return Json(aMaster);
        }


    }
}