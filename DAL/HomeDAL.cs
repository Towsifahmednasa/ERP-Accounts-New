// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\HomeDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL
{
    public class HomeDAL
    {

        private DataAccessManager accessManager = new DataAccessManager();


        public DataTable CheckLoginDAL(string userName, string pass)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.ControlPanel);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@loginName", userName));
                //aSqlParameterList.Add(new SqlParameter("@ModuleId", 2));
                //aSqlParameterList.Add(new SqlParameter("@LoginPassword", EncryptDecrypt.EncryptText(pass)));
                aSqlParameterList.Add(new SqlParameter("@Password", (pass)));
                return accessManager.GetDataTable("sp_GET_UserInfo_Acc", aSqlParameterList);
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