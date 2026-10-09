// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Budget\BudgetInfoDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Budget;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Budget
{
    public class BudgetInfoDAL
    {

        DataAccessManager _accessManager = new DataAccessManager();
        public List<BudgetMonthAmountDAO> BudgetMonthAmountDetails(string comID, string finID, string id)
        {
            try
            {


                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<BudgetMonthAmountDAO> aList = new List<BudgetMonthAmountDAO>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@comID", comID));
                aParameters.Add(new SqlParameter("@finID", finID));
                aParameters.Add(new SqlParameter("@id", id));


                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BudgetMonthAmountDetails", aParameters);
                while (dr.Read())
                {
                    BudgetMonthAmountDAO aMaster = new BudgetMonthAmountDAO();
                   
                    aMaster.MYId = dr["MYId"].ToString();
                    aMaster.Period = dr["Period"].ToString();
                    aMaster.Amount = dr["Amount"].ToString();
                    



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

        public ResultResponse SaveBudgetInfoDAL(BudgetMasterDAO aMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@BudgetMasterId", aMaster.BudgetMasterId));
          
                aSqlParameterlist.Add(new SqlParameter("@CompanyId", (object)(aMaster.CompanyId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@COAId", (object)(aMaster.COAId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", (object)(aMaster.FinancialYearId ?? (object)DBNull.Value)));
                aSqlParameterlist.Add(new SqlParameter("@TotalAmount", (object)(aMaster.TotalAmount ?? (object)DBNull.Value)));
             

                aSqlParameterlist.Add(new SqlParameter("@EntryBy", user));
            


                if (aMaster.BudgetMasterId > 0)
                {
                    aResponse.isSuccess = _accessManager.UpdateData("sp_Up_BudgetMaster_Bgt", aSqlParameterlist);

                    aResponse.pk = aMaster.BudgetMasterId;
                }
                //ll
                else
                {
                    aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_I_BudgetMaster_Bgt", aSqlParameterlist);
                }

                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = SaveBankPaymentDetails(aMaster.DetailsInfo, aResponse.pk);

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



        public bool SaveBankPaymentDetails(List<BudgetDetailDAO> aList, int pk)
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                    aSqlParameterlist.Add(new SqlParameter("@BudgetMasterId", pk));
                    aSqlParameterlist.Add(new SqlParameter("@Period", (object)(item.Period ?? (object)DBNull.Value)));





                    aSqlParameterlist.Add(new SqlParameter("@Amount", (object)(item.Amount ?? (object)DBNull.Value)));
                  

                     



                    result = _accessManager.SaveData("sp_I_BudgetDetail_Bgt", aSqlParameterlist);


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
        //public List<BudgetDetailDAO> GetBankPaymentDetailsById(int id)
        //{
        //    try
        //    {
        //        _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<BudgetDetailDAO> aDetails = new List<BudgetDetailDAO>();
        //        List<SqlParameter> aParameters = new List<SqlParameter>();
        //        aParameters.Add(new SqlParameter("@id", id));

        //        SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BankPaymentDetailById", aParameters);
        //        while (dr.Read())
        //        {
        //            BudgetDetailDAO deta = new BudgetDetailDAO();
        //            deta.VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]);
        //            deta.VoucherMasterId = DBNull.Value.Equals(dr["VoucherMasterId"]) ? 0 : Convert.ToInt32(dr["VoucherMasterId"]);
        //            deta.AccountId = DBNull.Value.Equals(dr["AccountId"]) ? 0 : Convert.ToInt32(dr["AccountId"]);

        //            deta.CostCenterId = DBNull.Value.Equals(dr["CostCenterId"]) ? 0 : Convert.ToInt32(dr["CostCenterId"]);

        //            deta.RefInfoInd = DBNull.Value.Equals(dr["RefInfoInd"]) ? 0 : Convert.ToInt32(dr["RefInfoInd"]);

        //            deta.SubsidoryLedgerId = DBNull.Value.Equals(dr["SubsidoryLedgerId"]) ? 0 : Convert.ToInt32(dr["SubsidoryLedgerId"]);



        //            deta.Amount = DBNull.Value.Equals(dr["Amount"]) ? 0 : Convert.ToDecimal(dr["Amount"]);

        //            deta.Remarks = dr["Remarks"].ToString();
        //            aDetails.Add(deta);
        //        }
        //        return aDetails;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        _accessManager.SqlConnectionClose();
        //    }
        //}
        public BudgetMasterDAO GetBudgetMasterId(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                BudgetMasterDAO aMaster = new BudgetMasterDAO();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BudgetMasterById", aParameters);
                while (dr.Read())
                {
                    aMaster.BudgetMasterId = Convert.ToInt32(dr["BudgetMasterId"]);
                    aMaster.CompanyId = DBNull.Value.Equals(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
                    aMaster.FinancialYearId = DBNull.Value.Equals(dr["FinancialYearId"]) ? 0 : Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());


                    aMaster.COAId = DBNull.Value.Equals(dr["COAId"]) ? 0 : Convert.ToInt32(dr["COAId"]);
                    aMaster.GLHeadId = DBNull.Value.Equals(dr["GLHeadId"]) ? 0 : Convert.ToInt32(dr["GLHeadId"]);
                    aMaster.ControlLId = DBNull.Value.Equals(dr["ControlLId"]) ? 0 : Convert.ToInt32(dr["ControlLId"]);
                    aMaster.SubsidiryLId = DBNull.Value.Equals(dr["SubsidiryLId"]) ? 0 : Convert.ToInt32(dr["SubsidiryLId"]);
                    aMaster.SubSubControlLedgerId = DBNull.Value.Equals(dr["SubSubControlLedgerId"]) ? 0 : Convert.ToInt32(dr["SubSubControlLedgerId"]);





                }
                return aMaster;
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
        public List<BudgetDAO> BudgetList(string param)
        {
            try
            {


                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<BudgetDAO> aList = new List<BudgetDAO>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@param", param));

                //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_BudgetList_Bgt", aParameters);
                while (dr.Read())
                {
                    BudgetDAO aMaster = new BudgetDAO();
                    aMaster.BudgetMasterId = Convert.ToInt32(dr["BudgetMasterId"]);
                    aMaster.FinancialYearDesc = dr["FinancialYearDesc"].ToString();
                    aMaster.COAIds = dr["COAIds"].ToString();
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                 


                    aMaster.TotalAmount = (dr["TotalAmount"].ToString());
               
                    aMaster.EntryBy = (dr["EntryBy"].ToString());
                    aMaster.EntryDate = (dr["EntryDate"].ToString());
                    aMaster.UpdateBy = (dr["UpdateBy"].ToString());
                    aMaster.UpdateDate = (dr["UpdateDate"].ToString());
 






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


    }


}