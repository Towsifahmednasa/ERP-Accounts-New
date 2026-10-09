// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\LedgerHeadController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Account_Master;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class LedgerHeadController : LegacyController
    {

        private readonly LedgerHead ledgerHead;

        public LedgerHeadController()
        {
            this.ledgerHead = new LedgerHead();       
        }

        // GET: LedgerHead
        public ActionResult LedgerHeadList()
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
        public ActionResult Get_LedgerHead_List()
        {
            DataTable dt = ledgerHead.CheckLoginDAL("");
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            return Json(JSONresult);
        }
    }
}