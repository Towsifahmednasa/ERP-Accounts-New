// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\TestDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class TestDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();

        public List<TestMasterView> GeTestMaster(DateTime? fromdate, DateTime? todate, int CompanyId = 0)
        {
            try
            {

                if (fromdate == null)
                {
                    fromdate = System.DateTime.Now.AddDays(-1);

                }
                if (todate == null)
                {
                    todate = System.DateTime.Now;

                }
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<TestMasterView> aList = new List<TestMasterView>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@CompanyId", CompanyId));
             
                aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_Get_TestList", aParameters);
                while (dr.Read())
                {
                    TestMasterView aMaster = new TestMasterView();
                    aMaster.TestId = Convert.ToInt32(dr["TestId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.CurrencyRate = Convert.ToDecimal(dr["CurrencyRate"].ToString());
                    aMaster.VoucherDate = (dr["VoucherDate"].ToString());
                    aMaster.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());
                    aMaster.EntryBy = (dr["EntryBy"].ToString());

                    aMaster.EntryDate = (dr["EntryDate"].ToString());
                  

                    aList.Add(aMaster);
                }
                return aList;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }




        public ResultResponse SaveTest(TestMaster aMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

 

                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@TestMasterId", aMaster.TestMasterId));
                aParameters.Add(new SqlParameter("@CompanyId", aMaster.CompanyId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@UnitId", aMaster.UnitId ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@VoucherDate", aMaster.VoucherDate ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@CurrencyRate", aMaster.CurrencyRate ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@PaymentType", aMaster.PaymentType ?? (object)DBNull.Value));
                aParameters.Add(new SqlParameter("@TotalAmount", aMaster.TotalAmount ?? (object)DBNull.Value));

                aParameters.Add(new SqlParameter("@Entryby", user));
                aParameters.Add(new SqlParameter("@EntryDate", System.DateTime.Now));

               

                aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_TestSave", aParameters);
                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = SaveTestDetails(aMaster.TestDetails, aResponse.pk, user);

                    if (aResponse.isSuccess == false)
                    {
                        _accessManager.SqlConnectionClose(true);
                    }
                }
                else
                {
                    _accessManager.SqlConnectionClose(true);
                }
                return aResponse;
            }
            catch (Exception ex)
            {
                _accessManager.SqlConnectionClose(true);
                throw ex;
            }

            finally
            {
                _accessManager.SqlConnectionClose();
            }
        }


        public bool SaveTestDetails(List<TestDetail> aList, int pk, string user )
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@TestMasterId", pk));
                   // aParameters.Add(new SqlParameter("@TestDetailId", item.TestDetailId ?? (object)DBNull.Value));

                    aParameters.Add(new SqlParameter("@CountryId", item.CountryId ?? (object)DBNull.Value));
                    aParameters.Add(new SqlParameter("@CityId", item.CityId ?? (object)DBNull.Value));
                    aParameters.Add(new SqlParameter("@Amount", item.Amount ?? (object)DBNull.Value));
                    aParameters.Add(new SqlParameter("@Remarks", item.Remarks ?? (object)DBNull.Value));



                    result = _accessManager.SaveData("sp_I_TestSaveDetail", aParameters);
                   

                    if (result == false)
                    {
                        break;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}