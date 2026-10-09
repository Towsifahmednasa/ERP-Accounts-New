// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\ReconcileBankAccountDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class ReconcileBankAccountDAL
    {

        DataAccessManager _accessManager = new DataAccessManager();


        //public List<DebitVoucherView> GetReconcileBankAccountListAll(string param)
        //{
        //    try
        //    {


        //        _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

        //        List<DebitVoucherView> aList = new List<DebitVoucherView>();
        //        List<SqlParameter> aParameters = new List<SqlParameter>();
        //        aParameters.Add(new SqlParameter("@param", param));

        //        //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
        //        //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
        //        SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_DebitVoucherList", aParameters);
        //        while (dr.Read())
        //        {
        //            DebitVoucherView aMaster = new DebitVoucherView();
        //            aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
        //            aMaster.CompanyName = dr["CompanyName"].ToString();
        //            aMaster.VoucherCode = dr["VoucherCode"].ToString();
        //            aMaster.VoucherNo = dr["VoucherNo"].ToString();
        //            aMaster.PostingDate = (dr["PostingDate"].ToString());
        //            aMaster.VoucherCategory = (dr["VoucherCategory"].ToString());
        //            aMaster.Narration = (dr["Narration"].ToString());

        //            aMaster.TotalAmountBDT = (dr["TotalAmountBDT"].ToString());
        //            aMaster.EntryBy = (dr["EntryBy"].ToString());
        //            aMaster.EntryDate = (dr["EntryDate"].ToString());
        //            aMaster.UpdateBy = (dr["UpdateBy"].ToString());
        //            aMaster.UpdateDate = (dr["UpdateDate"].ToString());
        //            aMaster.VoucherType = (dr["VoucherType"].ToString());
        //            aMaster.ReconcileDate = (dr["ReconcileDate"].ToString());
        //            aMaster.IsReconcile = Convert.ToBoolean(dr["IsReconcile"].ToString());







        //            aList.Add(aMaster);
        //        }
        //        return aList;

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
        public List<DebitVoucherView> GetReconcileBankAccountListAll(string param)
        {
            try
            {


                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<DebitVoucherView> aList = new List<DebitVoucherView>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@param", param));

                //aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
                //aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_DebitVoucherList", aParameters);
                while (dr.Read())
                {
                    DebitVoucherView aMaster = new DebitVoucherView();
                    aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
                    aMaster.CompanyName = dr["CompanyName"].ToString();
                    aMaster.VoucherCode = dr["VoucherCode"].ToString();
                    aMaster.VoucherNo = dr["VoucherNo"].ToString();
                    aMaster.PostingDate = (dr["PostingDate"].ToString());
                    aMaster.VoucherCategory = (dr["VoucherCategory"].ToString());
                    aMaster.Narration = (dr["Narration"].ToString());
                  
                    aMaster.TotalAmountBDT = (dr["TotalAmountBDT"].ToString());
                    aMaster.EntryBy = (dr["EntryBy"].ToString());
                    aMaster.EntryDate = (dr["EntryDate"].ToString());
                    aMaster.UpdateBy = (dr["UpdateBy"].ToString());
                    aMaster.UpdateDate = (dr["UpdateDate"].ToString());
                    aMaster.VoucherType = (dr["VoucherType"].ToString());
                    aMaster.ReconcileDate = (dr["ReconcileDate"].ToString());
                    aMaster.IsReconcile = Convert.ToBoolean(dr["IsReconcile"].ToString());

                    aMaster.ActionStatus = (dr["ActionStatus"].ToString());
                    aMaster.ApprovedBy = (dr["ApprovedBy"].ToString());
                    aMaster.ApprovedDate = (dr["ApprovedDate"].ToString());

                    aMaster.BankCheckNo = (dr["BankCheckNo"].ToString());
                    aMaster.BankCheckDate = (dr["BankCheckDate"].ToString());






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
        //public List<DebitCreditVoucherDetail> GetReconcileBankAccountListAll(string
        //    param)
        //{
        //    try
        //    {
        //        _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
        //        List<DebitCreditVoucherDetail> aDetails = new List<DebitCreditVoucherDetail>();
        //        List<SqlParameter> aParameters = new List<SqlParameter>();
        //        aParameters.Add(new SqlParameter("@id", param));

        //        SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GET_ReconcileBankAccountList", aParameters);
        //        while (dr.Read())
        //        {
        //            DebitCreditVoucherDetail deta = new DebitCreditVoucherDetail();
        //            deta.VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]);
        //            deta.VoucherMasterId = DBNull.Value.Equals(dr["VoucherMasterId"]) ? 0 : Convert.ToInt32(dr["VoucherMasterId"]);
        //            deta.AccountInfo = dr["AccountInfo"].ToString();





        //            deta.CostCenter = dr["CostCenter"].ToString();
        //            deta.SubsidoryLedger = dr["SubsidoryLedger"].ToString();
        //            deta.RefInfo = dr["RefInfo"].ToString();



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



        public ResultResponse SaveBankPaymentMaster(List<ReconcileBankAccount> aList, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);


                foreach (var item in aList)
                {

                    if (item.ReconcileDate != null)
                    {
                        List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                        aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", item.VoucherMasterId));


                        aSqlParameterlist.Add(new SqlParameter("@ReconcileDate", (object)(item.ReconcileDate ?? (object)DBNull.Value)));

                        aSqlParameterlist.Add(new SqlParameter("@user", user));


                        aResponse.isSuccess = _accessManager.SaveData("sp_UP_ReconcileBankAccount", aSqlParameterlist);
                    }
                   

                    if (aResponse.isSuccess == false)
                    {
                        _accessManager.SqlConnectionClose(true);
                    }
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


    }
}