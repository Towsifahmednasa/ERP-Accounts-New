// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\DashboardDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL
{

    public class DashboardDAL
    {

        DataAccessManager accessManager = new DataAccessManager();
        private int ModuleId = 13;
        private int IsMVC = 1;
        private SqlConnection sqlConnection = null;
        private SqlTransaction sqlTransaction = null;
        private SqlCommand sqlCommand = null;

        private List<MenuOperation> GetAllMenuUserWise(string loginUserId)
        {
            SqlDataReader dr = null;
            try
            {

                //db connection only for menu generation always from aims server so this connection is manually done without DataAcessManager


                string connectionStr = SqlUserAccess.ConnectionString(DataBase.ControlPanel);

                //string connectionStr = @"data source=AIMS-SERVER\SQLSERVER2014;Initial Catalog=ControlPanelDB;Integrated Security=false; User Id=sa; password=*@bflsql2017#;";
                //string connectionStr = @"data source=MAHABUBMIS;Initial Catalog=ControlPanelDB;Integrated Security=false; User Id=sa; password=sa1234;";


                sqlConnection = new SqlConnection(connectionStr);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                List<MenuOperation> aMenuOperations = new List<MenuOperation>();
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ModuleId", ModuleId));
                aSqlParameterList.Add(new SqlParameter("@IsMVC", IsMVC));
                aSqlParameterList.Add(new SqlParameter("@LoginUserId", loginUserId));

                string StoreProcedure = "sp_Select_Menu_Step_WithUserAccessERPAcc";
                sqlCommand = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = StoreProcedure
                };

                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddRange(aSqlParameterList.ToArray());

                dr = sqlCommand.ExecuteReader();
                sqlCommand.Parameters.Clear();

                MenuOperation aMenuOperation;
                while (dr.Read())
                {
                    aMenuOperation = new MenuOperation();
                    aMenuOperation.SL = dr["SL"] != DBNull.Value ? Convert.ToInt32(dr["SL"]) : 0;
                    aMenuOperation.MenuName = dr["MenuName"] != DBNull.Value ? dr["MenuName"].ToString() : "";
                    aMenuOperation.ControllerName = dr["ControllerName"] != DBNull.Value ? dr["ControllerName"].ToString() : "";
                    aMenuOperation.ActionName = dr["ActionName"] != DBNull.Value ? dr["ActionName"].ToString() : "";
                    aMenuOperation.ModuleId = dr["ModuleId"] != DBNull.Value ? Convert.ToInt32(dr["ModuleId"]) : 1014;
                    aMenuOperation.MenuStep = dr["MenuStep"] != DBNull.Value ? Convert.ToInt32(dr["MenuStep"]) : 1;
                    aMenuOperation.ParantId = dr["ParantId"] != DBNull.Value ? Convert.ToInt32(dr["ParantId"]) : 0;
                    aMenuOperations.Add(aMenuOperation);
                }



                return aMenuOperations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
                sqlCommand.Dispose();
                sqlConnection.Close();

            }
        }
        public IEnumerable<MenuStepOne> GetMenuStepOne(string loginUserId)
        {
            try
            {

                List<MenuStepOne> aMenuStepOneList = new List<MenuStepOne>();

                IEnumerable<MenuOperation> menuOperationsList = GetAllMenuUserWise(loginUserId);

                IEnumerable<MenuOperation> tempStepOneList = menuOperationsList.Where(x =>  x.ParantId == 0); ;
                MenuStepOne aMenuStepOne;
                foreach (var aItem in tempStepOneList)
                {
                    aMenuStepOne = new MenuStepOne();
                    aMenuStepOne.SL = aItem.SL;
                    aMenuStepOne.MenuName = aItem.MenuName;
                    aMenuStepOne.ControllerName = aItem.ControllerName;
                    aMenuStepOne.ActionName = aItem.ActionName;
                    aMenuStepOne.MenuStepTwoList = GetMenuStepTwo(aItem.SL, menuOperationsList);
                    aMenuStepOneList.Add(aMenuStepOne);
                }
                return aMenuStepOneList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<MenuStepTwo> GetMenuStepTwo(int ParantId, IEnumerable<MenuOperation> menuOperationsList)
        {
            try
            {
                List<MenuStepTwo> aStepTwoList = new List<MenuStepTwo>();

                IEnumerable<MenuOperation> tempStepOneList = menuOperationsList.Where(x => x.MenuStep == 2 && x.ParantId == ParantId);
                MenuStepTwo aMenuStepTwo;
                foreach (var aItem in tempStepOneList)
                {
                    aMenuStepTwo = new MenuStepTwo();
                    aMenuStepTwo.SL = aItem.SL;
                    aMenuStepTwo.MenuName = aItem.MenuName;
                    aMenuStepTwo.ControllerName = aItem.ControllerName;
                    aMenuStepTwo.ActionName = aItem.ActionName;
                    aMenuStepTwo.MenuStepThreeList = GetMenuStepThree(aItem.SL, menuOperationsList);
                    aStepTwoList.Add(aMenuStepTwo);
                }


                return aStepTwoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<MenuStepThree> GetMenuStepThree(int ParantId, IEnumerable<MenuOperation> menuOperationsList)
        {
            try
            {
                List<MenuStepThree> aStepThreeList = new List<MenuStepThree>();

                IEnumerable<MenuOperation> tempStepOneList = menuOperationsList.Where(x => x.MenuStep == 3 && x.ParantId == ParantId);
                MenuStepThree aMenuStepThree;
                foreach (var aItem in tempStepOneList)
                {
                    aMenuStepThree = new MenuStepThree();
                    aMenuStepThree.SL = aItem.SL;
                    aMenuStepThree.MenuName = aItem.MenuName;
                    aMenuStepThree.ControllerName = aItem.ControllerName;
                    aMenuStepThree.ActionName = aItem.ActionName;
                    aStepThreeList.Add(aMenuStepThree);
                }
                return aStepThreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApprovalMenuList> GetApprovalMenuData(string user)
        {

            try
            {


                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> SqlList = new List<SqlParameter>();
                List<ApprovalMenuList> aList = new List<ApprovalMenuList>();

                SqlList.Add(new SqlParameter("@user", user));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetApprovalMenuListByUser", SqlList);

                while (dr.Read())
                {
                    ApprovalMenuList aDO = new ApprovalMenuList();
                    aDO.MenuName = dr["MenuName"].ToString();
                    aDO.ControllerName = dr["ControllerName"].ToString();
                    aDO.ActionName = dr["ActionName"].ToString();
                    aDO.pending = dr["pending"].ToString();

                    //aDO.n_MenuName = dr["n_MenuName"].ToString();
                    //aDO.n_ControllerName = dr["n_ControllerName"].ToString();
                    //aDO.n_ActionName = dr["n_ActionName"].ToString();
                    //aDO.n_pending = dr["n_pending"].ToString();
                    aDO.bgcolor = dr["bgcolor"].ToString();
                    aList.Add(aDO);

                }

                return aList;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }


        }

        public List<ApprovalMenuList> GetVoucherApprovalDashboard(string user)
        {

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> SqlList = new List<SqlParameter>();
                List<ApprovalMenuList> aList = new List<ApprovalMenuList>();

                SqlList.Add(new SqlParameter("@user", user));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetVoucherApprovalDashboard", SqlList);

                while (dr.Read())
                {
                    ApprovalMenuList aDO = new ApprovalMenuList();
                    aDO.SL = Convert.ToInt32(dr["SL"]);
                    aDO.MenuName = dr["MenuName"].ToString();
                    aDO.ControllerName = dr["ControllerName"].ToString();
                    aDO.ActionName = dr["ActionName"].ToString();
                    aDO.pending = dr["pending"].ToString();
                    aDO.bgcolor = dr["bgcolor"].ToString();
                    aList.Add(aDO);

                }

                return aList;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }


        }
    }
}