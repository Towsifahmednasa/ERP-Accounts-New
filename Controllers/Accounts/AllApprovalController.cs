// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Accounts\AllApprovalController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.DAL.Loan_DAL;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Models.LOAN_DAO;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Accounts
{
    public class AllApprovalController : LegacyController
    {

        BankPaymentsDAL dbDal = new BankPaymentsDAL();
        AllApprovalDAL appDal = new AllApprovalDAL();
        LoanDAL _LoanDAL = new LoanDAL(); 
        // GET: AllApproval
        public ActionResult AllVoucherApproval()
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

        
        public ActionResult LoanApproval()
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



        public ActionResult BankChargeApproval()
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


        public ActionResult BankAccountList(string param)
        {

            if (param == "")
            {
              //  param=" and mas.PostingDate = '" +  DateTime.Now+"'";
            }


            List<DebitVoucherView> aList = dbDal.BankPaymentList(param + "  and  ISNULL( mas.ActionStatus,'Posted')   = 'Posted'");
            return Json(aList);

        }

        


        public ActionResult LoadLoanList(string param)
        {

            if (param == "")
            {
              //  param=" and mas.PostingDate = '" +  DateTime.Now+"'";
            }


            List<LoanDataModelListDAO> aList = _LoanDAL.GetLoanInfoList(param + "  and  ISNULL( ln.ApproveStatus,'Posted')   = 'Posted'");
            return Json(aList);

        }


        [HttpPost]
        public   ActionResult Approve_AllVoucher(string MyArry, bool? rbValue)
        {

            ResultResponse aResponse = appDal.ApproveAllVoucher(MyArry, rbValue,(Session[SessionCollection.UserName].ToString()));
            return Json(aResponse);


        }

        [HttpPost]
        public   ActionResult Approve_AllLoan(string MyArry, bool? rbValue)
        {

            ResultResponse aResponse = appDal.ApproveLoan(MyArry, rbValue,(Session[SessionCollection.LoginUserId].ToString()));
            return Json(aResponse);


        }

        [HttpPost]
        public   ActionResult Approve_AllBankCharge(string MyArry, bool? rbValue)
        {

            ResultResponse aResponse = appDal.Approve_AllBankCharge(MyArry, rbValue,(Session[SessionCollection.LoginUserId].ToString()));
            return Json(aResponse);


        }
    }
}