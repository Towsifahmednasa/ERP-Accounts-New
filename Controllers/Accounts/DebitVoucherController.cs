// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\DebitVoucherController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
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
    public class DebitVoucherController : LegacyController
    {
        DebitVoucherDAL dbDal = new DebitVoucherDAL();
        // GET: DebitVoucher
        public ActionResult AddRowForVoucher(int tr)
        {
            TempData["trSl"] = tr;

            return PartialView("_addNewGLRow");
        }
        public ActionResult Create(int VoucherId = 0, string eflag = "ed")
        {
            ViewBag.VoucherId = VoucherId;
            ViewBag.eflag = eflag;
            return View();
        }
        public ActionResult GetGLDetails(int glId)
        {
            ChartOfAccLayerSeven aDetails = dbDal.GetGLDetails(glId);
            return Json(aDetails);
        }
        public ActionResult LoadCostCenterByGlId(int CALayerSevenId)
        {
            List<CostCenter> cu = dbDal.GetCostCenterByGlId(CALayerSevenId);
            return Json(cu);
        }
        public ActionResult LoadReferenceTypeByGlId(int CALayerSevenId)
        {
            List<ReferanceTypeDetails> cu = dbDal.GetReferenceTypeByGlId(CALayerSevenId);
            return Json(cu);
        }
        public ActionResult LoadRefDetailsByRefTypeId(int RefTypeId)
        {
            List<ReferanceTypeDetails> cu = dbDal.GetRefDetailsByRefTypeId(RefTypeId);
            return Json(cu);
        }

        public ActionResult GetBankBranch(int CALayerSevenId)
        {
            BankAccount aDetails = dbDal.GetBankBranch(CALayerSevenId);
            return Json(aDetails);
        }
        [HttpPost]
        public ActionResult SaveVoucher(VoucherMaster aMaster)
        {

            ResultResponse aResponse = dbDal.SaveVoucher(aMaster, Session[SessionCollection.UserName].ToString());
            return Json(aResponse);
        }
        //public ActionResult Index(DateTime? fromdate, DateTime? todate, int CompanyId = 0, int BUId = 0)
        //{
        //    List<VoucherMasterView> aList = dbDal.GetDebitVoucherMaster(fromdate, todate, CompanyId, BUId);
        //    return View(aList);
        //}
        //public ActionResult GetVoucherByMasterId(int id)
        //{
        //    VoucherMaster aMaster = dbDal.GetVoucherMasterById(id);
        //    aMaster.VoucherDetails = dbDal.GetVoucherDetailsById(id);
        //    return Json(aMaster);
        //}
        //public ActionResult DebitVoucherListData(DateTime? fromdate, DateTime? todate, int CompanyId = 0, int BUId = 0)
        //{
        //    List<VoucherMasterView> aList = dbDal.GetDebitVoucherMaster(fromdate, todate, CompanyId, BUId);
        //    return Json(aList);

        //}

    }
}