// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\HomeController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL;
using New_Accounts.Models;
using New_Accounts.Utility;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace New_Accounts.Controllers
{
    public class HomeController : LegacyController
    {
        DashboardDAL aDashBoardDal = new DashboardDAL();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]

        public ActionResult Login()
        {
            //if (Request.Cookies["LoginUserIdCookie"] != null)
            //{

            //    var c = new HttpCookie("LoginUserIdCookie");
            //    c.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(c);
            //}
            //else
            //{
                
            //}

            //HttpContext.Session.Clear();
            return View("Index"); // legacy: View("Login") - the view never existed (GAP-PLT-001)



        }

        [AllowAnonymous]

        public ActionResult LoginAction(LoginUser login)
        {
            if (CheckLogin(login.userName, login.password))
            {
                ViewData["LoginMsg"] = "Home";
                //return RedirectToAction("Index", "DashBoard");
                return RedirectToAction("Index", "DashBoard");
            }
            else
            {
                //TempData["msg"] = "Please Enter Valid Credentials";
                TempData["LoginMsg"] = "Invalid Credentials"; // legacy used ViewData, lost on redirect (GAP-PLT-002)

                return RedirectToAction("Index", "Home");

            }
        }
        [AllowAnonymous]
        public ActionResult LockScreen(string EmpId, string EmpName)
        {
            ViewBag.EmpName = EmpName;
            ViewBag.EmpId = EmpId;
            return View("LockScreen");
        }
        [NonAction]
        public bool CheckLogin(string userName, string pass)
        {
            try
            {
                HomeDAL aHomeDal = new HomeDAL();

                DataTable dt = aHomeDal.CheckLoginDAL(userName, pass);

                if (dt.Rows.Count > 0)
                {
                    Session[SessionCollection.UserName] = dt.Rows[0]["LoginUserName"].ToString();
                    Session[SessionCollection.LoginUserId] = dt.Rows[0]["LoginUserId"].ToString();
                    Session[SessionCollection.LoginUserType] = dt.Rows[0]["LoginUserType"].ToString();
                    Session[SessionCollection.LoginUserEmpId] = dt.Rows[0]["LoginUserEmpId"].ToString();

                    bool DashPer = false;

                    try
                    {
                        DashPer = Convert.ToBoolean(dt.Rows[0]["IsMainDashboard"].ToString());
                    }
                    catch { }

                    if (DashPer)
                    {
                        Session[SessionCollection.DashboardPage] = "Index";
                    }
                    else
                    {
                        Session[SessionCollection.DashboardPage] = "UserDashboard";
                    }

                    Session[SessionCollection.LoginUserEmpId] = dt.Rows[0]["LoginUserEmpId"].ToString();
                    Session[SessionCollection.ModuleId] = "13";
                    Session["MenuList"] = aDashBoardDal.GetMenuStepOne(Session[SessionCollection.LoginUserId].ToString());

                    UtilityDAL aDal = new UtilityDAL();
                   //string userInfo = aDal.GetUserInfoStrig(dt.Rows[0]["LoginUserEmpId"].ToString());
                    Session[SessionCollection.UserInfo] = dt.Rows[0]["LoginUserName"].ToString();

                    string loginName = dt.Rows[0]["LoginUserName"].ToString();
                    Response.Cookies.Append("LoginUserIdCookie", "EmpId=" + loginName + "&EmpName=" + loginName, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true }); // legacy multi-value cookie, never read

                    return true;
                }
                else
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    return false;
                }
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();
                return false;
            }
            finally
            {

            }
        }




        [AllowAnonymous]




        public ActionResult Logout()
        {

            if (ViewData["LoginMsg"] == null)
            {
                ViewData["LoginMsg"] = "Please Input Correct User Name Password!!";
            }
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}