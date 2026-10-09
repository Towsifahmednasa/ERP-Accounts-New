// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\Account_Master\CashFlowController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL.Accounts;
using New_Accounts.DAL.Report;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Utility;
using Newtonsoft.Json;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers.Account_Master
{
    public class CashFlowController : LegacyController
    {
        AccountsReportDAL aAccountsReportDAL = new AccountsReportDAL();
        // GET: CashFlow
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCashFlow_Report(int CompanyID, int FinancialYearID,  DateTime FromDate, DateTime ToDate)
        {
            DateTime CashBookFinancialYearStartDate = DateTime.Now;

            DataSet mainDS = new DataSet();
            DataTable allDataTable = new DataTable();

            allDataTable = aAccountsReportDAL.GetCashFlowBDTDAL(FinancialYearID.ToString(), CompanyID.ToString(), FromDate, ToDate).Copy();
            allDataTable.TableName = "BankBookDataTable";


            DataView dv = allDataTable.DefaultView;

            dv.Sort = "SubsidiryLName";
            //    dv.Sort = "AccountName";
            allDataTable = dv.ToTable();

            mainDS.Tables.Add(allDataTable);

            string JSONresult = JsonConvert.SerializeObject(mainDS, Formatting.None);
            return Content(JSONresult, "application/json");

        }
    }
}