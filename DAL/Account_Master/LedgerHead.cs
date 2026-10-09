// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\LedgerHead.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Account_Master
{
    public class LedgerHead
    {
        private DataAccessManager accessManager = new DataAccessManager();
        public DataTable CheckLoginDAL(string userName)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@GLName", userName));
                return accessManager.GetDataTable("sp_GET_LadgerHead", aSqlParameterList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
    }
}