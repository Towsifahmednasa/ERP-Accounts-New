// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\DashboardController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL;
using New_Accounts.Models;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class DashboardController : LegacyController
    {
        // GET: Dashboard
        // GET: Dashboard
        public ActionResult Index()
        {
            string loginUserEmpId = Session["loginuserempid"] as string;

            if (string.IsNullOrEmpty(loginUserEmpId))
            {
                return RedirectToAction(nameof(HomeController.Login), "Home");
            }
            else
            {
                string userName = Session[SessionCollection.UserName].ToString();
                DashboardDAL aDal = new DashboardDAL();
                List<ApprovalMenuList> aList = new List<ApprovalMenuList>(); //aDal.GetApprovalMenuData(userName);
                return View(Session[SessionCollection.DashboardPage].ToString(), aList);
            }

        }


        public ActionResult UserDashboard()
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

        public ActionResult MenuGeneration()
        {
            try
            {
                string loginUserEmpId = Session["loginuserempid"] as string;

                if (string.IsNullOrEmpty(loginUserEmpId))
                {
                    return RedirectToAction(nameof(HomeController.Login), "Home");
                }
                else
                {
                    DashboardDAL aDashBoardDal = new DashboardDAL();
                    return PartialView("_SideNav",
                               Session.Get<List<MenuStepOne>>("MenuList"));
                    //return PartialView("_SideNav",
                    //    aDashBoardDal.GetMenuStepOne("1"));
                    // return PartialView("_SideNav", "");
                    //Session[SessionCollection.LoginUserId].ToString())

                }
            }
            catch (Exception exception)
            {
                string message = exception.ToString();

                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ApprovalDashBoard()
        {
            //if (AccessAuthoraization.CheckAccess(
            //     Request.RequestContext.RouteData.Values["Controller"].ToString(),
            //     Request.RequestContext.RouteData.Values["Action"].ToString()) == false || Session[SessionCollection.UserName] == null)


            //{
            //    return RedirectToAction("Index", "Home"); 
            //}

            string userName = Session[SessionCollection.UserName].ToString();
            DashboardDAL aDal = new DashboardDAL();
            List<ApprovalMenuList> aList = aDal.GetApprovalMenuData(userName);
            return View("ApprovalDashBoard", aList);
        }

        public ActionResult VoucherApprovalDashBoard()
        {
            string userName = Session[SessionCollection.UserName].ToString();
            DashboardDAL aDal = new DashboardDAL();
            List<ApprovalMenuList> aList = aDal.GetVoucherApprovalDashboard(userName);
            return View("VoucherApprovalDashBoard", aList);
        }



        public ActionResult LoadFilterInPartialView()
        {
          
            return PartialView("_FilterMasterInfo");
        }

    }
}