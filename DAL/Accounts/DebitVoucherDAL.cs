// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\DebitVoucherDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{
    public class DebitVoucherDAL
    {
        DataAccessManager _accessManager = new DataAccessManager();

        public ChartOfAccLayerSeven GetGLDetails(int id)
        {
            try
            {
                ChartOfAccLayerSeven aDetails = new ChartOfAccLayerSeven();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@CALayerSevenId", id));

                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GetGLInfoById", aParameters);
                while (dr.Read())
                {
                    aDetails.CALayerSevenId = Convert.ToInt32(dr["CALayerSevenId"]);
                    aDetails.CALayerSevenName = dr["CALayerSevenName"].ToString();
                    aDetails.CALayerSevenCode = dr["CALayerSevenCode"].ToString();
                    aDetails.Description = dr["Description"].ToString();
                    aDetails.IsReferance = Convert.ToInt32(dr["IsReferance"]);
                }
                return aDetails;
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
        public List<CostCenter> GetCostCenterByGlId(int CALayerSevenId)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CALayerSevenId", CALayerSevenId));
            SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GetCostCenterByGlId", aParameters);
            List<CostCenter> _cList = new List<CostCenter>();
            while (dr.Read())
            {
                CostCenter aCur = new CostCenter();
                aCur.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                aCur.CostCenterName = dr["CostCenterName"].ToString();
                _cList.Add(aCur);
            }
            return _cList;
        }

        public List<ReferanceTypeDetails> GetReferenceTypeByGlId(int CALayerSevenId)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CALayerSevenId", CALayerSevenId));
            SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GetReferenceTypeByGlId", aParameters);
            List<ReferanceTypeDetails> _cList = new List<ReferanceTypeDetails>();
            while (dr.Read())
            {
                ReferanceTypeDetails aCur = new ReferanceTypeDetails();
                aCur.RefDetailsId = Convert.ToInt32(dr["RefDetailsId"]);
                aCur.RefName = dr["RefName"].ToString();
                _cList.Add(aCur);
            }
            return _cList;
        }

        public List<ReferanceTypeDetails> GetRefDetailsByRefTypeId(int RefTypeId)
        {
            _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@RefTypeId", RefTypeId));
            SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GetRefDetailsByRefTypeId", aParameters);
            List<ReferanceTypeDetails> _cList = new List<ReferanceTypeDetails>();
            while (dr.Read())
            {
                ReferanceTypeDetails aCur = new ReferanceTypeDetails();
                aCur.RefDetailsId = Convert.ToInt32(dr["RefDetailsId"]);
                aCur.RefName = dr["RefName"].ToString();
                _cList.Add(aCur);
            }
            return _cList;
        }
        public BankAccount GetBankBranch(int CALayerSevenId)
        {
            try
            {
                BankAccount aDetails = new BankAccount();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@CALayerSevenId", CALayerSevenId));

                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_GetBankBranchByGlId", aParameters);
                while (dr.Read())
                {
                    aDetails.CALayerSevenId = Convert.ToInt32(dr["CALayerSevenId"]);
                    aDetails.BankId = Convert.ToInt32(dr["BankId"]);
                    aDetails.BranchId = Convert.ToInt32(dr["BranchId"]);
                }
                return aDetails;
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

        public ResultResponse SaveVoucher(VoucherMaster aMaster, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);


                if (aMaster.eflag == "ed")
                {
                    aMaster.IsApprove = false;
                }
                else
                {
                    aMaster.IsApprove = true;
                }

                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@VoucherMasterId", aMaster.VoucherMasterId));
                aParameters.Add(new SqlParameter("@CompanyId", aMaster.CompanyId));
                aParameters.Add(new SqlParameter("@BUId", aMaster.BUId));
                aParameters.Add(new SqlParameter("@FinancialYearId", aMaster.FinancialYearId));
                aParameters.Add(new SqlParameter("@VoucherType", aMaster.VoucherType));
                aParameters.Add(new SqlParameter("@VoucherDate", aMaster.VoucherDate));
                aParameters.Add(new SqlParameter("@CurrencyId", aMaster.CurrencyId));
                aParameters.Add(new SqlParameter("@CurrencyConRate", aMaster.CurrencyConRate));
                aParameters.Add(new SqlParameter("@PaymentTypeId", aMaster.PaymentTypeId));
                aParameters.Add(new SqlParameter("@BankId", aMaster.BankId));
                aParameters.Add(new SqlParameter("@BranchId", aMaster.BranchId));
                aParameters.Add(new SqlParameter("@ChequeNo", aMaster.ChequeNo));
                aParameters.Add(new SqlParameter("@ChequeDate", aMaster.ChequeDate));
                aParameters.Add(new SqlParameter("@RefNo", aMaster.RefNo));
                aParameters.Add(new SqlParameter("@RefDate", aMaster.RefDate));
                aParameters.Add(new SqlParameter("@TotalAmount", aMaster.TotalAmount));
                aParameters.Add(new SqlParameter("@Narration", aMaster.Narration));
                aParameters.Add(new SqlParameter("@eflag", aMaster.eflag));

                aParameters.Add(new SqlParameter("@EntryBy", user));
                aParameters.Add(new SqlParameter("@EntryDate", System.DateTime.Now));

                if (aMaster.eflag == "ap")
                {
                    aParameters.Add(new SqlParameter("@IsApprove", true));
                }
                else
                {
                    aParameters.Add(new SqlParameter("@IsApprove", aMaster.IsApprove));
                }

                aResponse.pk = _accessManager.SaveDataReturnPrimaryKey("sp_Save_DebitVoucherMaster", aParameters);
                if (aResponse.pk > 0)
                {
                    aResponse.isSuccess = SaveVoucherDetails(aMaster.VoucherDetails, aResponse.pk, user, aMaster.IsApprove);

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

        public bool SaveVoucherDetails(List<VoucherDetails> aList, int pk, string user, bool isApprove)
        {
            try
            {
                bool result = false;
                foreach (var item in aList)
                {
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@VoucherMasterId", pk));
                    aParameters.Add(new SqlParameter("@VoucherDetailId", item.VoucherDetailId));
                    aParameters.Add(new SqlParameter("@CALayerSevenId", item.CALayerSevenId));
                    aParameters.Add(new SqlParameter("@DrCr", item.DrCr));
                    aParameters.Add(new SqlParameter("@CrAmt", item.CrAmt));
                    aParameters.Add(new SqlParameter("@CrAmtBDT", item.CrAmtBDT));
                    aParameters.Add(new SqlParameter("@DrAmt", item.DrAmt));
                    aParameters.Add(new SqlParameter("@DrAmtBDT", item.DrAmtBDT));
                    aParameters.Add(new SqlParameter("@IsDelete", item.IsDelete));
                    aParameters.Add(new SqlParameter("@Remarks", item.Remarks));
                    aParameters.Add(new SqlParameter("@eflag", item.eflag));

                    int voucherDetailsId = _accessManager.SaveDataReturnPrimaryKey("sp_Save_DebitVoucherDetails", aParameters);

                    if (item.VoucherTransections != null)
                    {
                        List<SqlParameter> aParametersTr = new List<SqlParameter>();
                        aParametersTr.Add(new SqlParameter("@VoucherTransectionId", item.VoucherTransections.VoucherTransectionId));
                        aParametersTr.Add(new SqlParameter("@VoucherDetailId", voucherDetailsId));
                        aParametersTr.Add(new SqlParameter("@Amt", item.VoucherTransections.Amt));
                        aParametersTr.Add(new SqlParameter("@AmtBDT", item.VoucherTransections.AmtBDT));
                        aParametersTr.Add(new SqlParameter("@CostCenterId", item.VoucherTransections.CostCenterId));
                        aParametersTr.Add(new SqlParameter("@RefDetailsId", item.VoucherTransections.RefDetailsId));

                        result = _accessManager.SaveData("sp_Save_DebitVoucherTransaction", aParametersTr);
                    }
                    else
                    {
                        result = true;
                    }


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

        //public List<VoucherMasterView> GetDebitVoucherMaster(DateTime? fromdate, DateTime? todate, int CompanyId = 0, int BUId = 0)
        //{
        //    try
        //    {

        //        if (fromdate == null)
        //        {
        //            fromdate = System.DateTime.Now.AddDays(-1);

        //        }
        //        if (todate == null)
        //        {
        //            todate = System.DateTime.Now;

        //        }
        //        _accessManager.SqlConnectionOpen(DataBase.IGFOODACCDB);

        //        List<VoucherMasterView> aList = new List<VoucherMasterView>();
        //        List<SqlParameter> aParameters = new List<SqlParameter>();
        //        aParameters.Add(new SqlParameter("@CompanyId", CompanyId));
        //        aParameters.Add(new SqlParameter("@BUId", BUId));
        //        aParameters.Add(new SqlParameter("@FromDate", String.Format("{0:MM/dd/yyyy}", fromdate)));
        //        aParameters.Add(new SqlParameter("@ToDate", String.Format("{0:MM/dd/yyyy}", todate)));
        //        SqlDataReader dr = _accessManager.GetSqlDataReader("sp_Get_DebitVoucherList", aParameters);
        //        while (dr.Read())
        //        {
        //            VoucherMasterView aMaster = new VoucherMasterView();
        //            aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
        //            aMaster.VoucherType = dr["VoucherType"].ToString();
        //            aMaster.VoucherCode = dr["VoucherCode"].ToString();
        //            aMaster.VoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
        //            aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"]);
        //            aMaster.Company = dr["CompanyShortName"].ToString();
        //            aMaster.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
        //            aMaster.FinancialYear = dr["FinancialYear"].ToString();
        //            aMaster.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);
        //            aMaster.Currency = dr["CurrencyCode"].ToString();
        //            aMaster.Narration = dr["Narration"].ToString();
        //            aMaster.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
        //            aMaster.TotalAmountBDT = Convert.ToDecimal(dr["TotalAmountBDT"]);
        //            aMaster.PaymentType = dr["PaymentType"].ToString();
        //            aMaster.ChequeNo = dr["ChequeNo"].ToString();
        //            aMaster.ChequeDate = dr["ChequeDate"] as DateTime?;
        //            aMaster.RefNo = dr["RefNo"].ToString();
        //            aMaster.RefDate = dr["RefDate"] as DateTime?;
        //            aMaster.EntryBy = dr["EntryBy"].ToString();
        //            aMaster.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
        //            aMaster.BUId = Convert.ToInt32(dr["BUId"]);
        //            aMaster.BusinessUnit = dr["ShortName"].ToString();
        //            aMaster.IsApprove = Convert.ToBoolean(dr["IsApprove"]);
        //            aMaster.IsReject = Convert.ToBoolean(dr["IsReject"]);
        //            aMaster.IsPage = Convert.ToBoolean(dr["IsPage"]);
        //            aMaster.BankId = DBNull.Value.Equals(dr["BankId"]) ? 0 : Convert.ToInt32(dr["BankId"]);
        //            aMaster.Bank = dr["BankName"].ToString();
        //            aMaster.BranchId = DBNull.Value.Equals(dr["BranchId"]) ? 0 : Convert.ToInt32(dr["BranchId"]);
        //            aMaster.Branch = dr["BranchName"].ToString();

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
        public VoucherMaster GetVoucherMasterById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                VoucherMaster aMaster = new VoucherMaster();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_Get_VoucherMasterById", aParameters);
                while (dr.Read())
                {
                    aMaster.VoucherMasterId = Convert.ToInt32(dr["VoucherMasterId"]);
                    aMaster.VoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
                    aMaster.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                    aMaster.VoucherTypeId = Convert.ToInt32(dr["VoucherTypeId"]);
                    aMaster.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                    aMaster.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);
                    aMaster.CurrencyConRate = Convert.ToDecimal(dr["CurrencyConRate"]);
                    aMaster.Narration = dr["Narration"].ToString();
                    aMaster.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    aMaster.TotalAmountBDT = Convert.ToDecimal(dr["TotalAmountBDT"]);
                    aMaster.PaymentTypeId = DBNull.Value.Equals(dr["PaymentTypeId"]) ? 0 : Convert.ToInt32(dr["PaymentTypeId"]);
                    aMaster.ChequeNo = dr["ChequeNo"].ToString();
                    aMaster.ChequeDate = dr["ChequeDate"] as DateTime?;
                    aMaster.RefNo = dr["RefNo"].ToString();
                    aMaster.RefDate = dr["RefDate"] as DateTime?;
                    aMaster.EntryBy = dr["EntryBy"].ToString();
                    aMaster.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                    aMaster.BUId = Convert.ToInt32(dr["BUId"]);
                    aMaster.IsApprove = Convert.ToBoolean(dr["IsApprove"]);
                    aMaster.IsPage = Convert.ToBoolean(dr["IsPage"]);
                    aMaster.BankId = DBNull.Value.Equals(dr["BankId"]) ? 0 : Convert.ToInt32(dr["BankId"]);
                    aMaster.BranchId = DBNull.Value.Equals(dr["BranchId"]) ? 0 : Convert.ToInt32(dr["BranchId"]);


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

        public List<VoucherDetails> GetVoucherDetailsById(int id)
        {
            try
            {
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<VoucherDetails> aDetails = new List<VoucherDetails>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));

                SqlDataReader dr = _accessManager.GetSqlDataReader("sp_Get_VoucherDetailsById", aParameters);
                while (dr.Read())
                {
                    VoucherDetails deta = new VoucherDetails();
                    deta.VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]);
                    deta.VoucherTransectionId = DBNull.Value.Equals(dr["VoucherTransectionId"]) ? 0 : Convert.ToInt32(dr["VoucherTransectionId"]);
                    deta.CALayerSevenId = Convert.ToInt32(dr["CALayerSevenId"]);
                    deta.DrCr = dr["DrCr"].ToString();
                    deta.DrAmt = DBNull.Value.Equals(dr["DrAmt"]) ? 0 : Convert.ToDecimal(dr["DrAmt"]);
                    deta.DrAmtBDT = DBNull.Value.Equals(dr["DrAmtBDT"]) ? 0 : Convert.ToDecimal(dr["DrAmtBDT"]);
                    deta.CrAmt = DBNull.Value.Equals(dr["CrAmt"]) ? 0 : Convert.ToDecimal(dr["CrAmt"]);
                    deta.CrAmtBDT = DBNull.Value.Equals(dr["CrAmtBDT"]) ? 0 : Convert.ToDecimal(dr["CrAmtBDT"]);
                    deta.Amt = DBNull.Value.Equals(dr["Amt"]) ? 0 : Convert.ToDecimal(dr["Amt"]);
                    deta.AmtBDT = DBNull.Value.Equals(dr["AmtBDT"]) ? 0 : Convert.ToDecimal(dr["AmtBDT"]);
                    deta.SalesOrderChallanMasterId = DBNull.Value.Equals(dr["SalesOrderChallanMasterId"]) ? 0 : Convert.ToInt32(dr["SalesOrderChallanMasterId"]);
                    deta.MRRMasterId = DBNull.Value.Equals(dr["MRRMasterId"]) ? 0 : Convert.ToInt32(dr["MRRMasterId"]);
                    deta.CostCenterId = DBNull.Value.Equals(dr["CostCenterId"]) ? 0 : Convert.ToInt32(dr["CostCenterId"]);
                    deta.RefTypeId = DBNull.Value.Equals(dr["RefTypeId"]) ? 0 : Convert.ToInt32(dr["RefTypeId"]);
                    deta.RefDetailsId = DBNull.Value.Equals(dr["RefDetailsId"]) ? 0 : Convert.ToInt32(dr["RefDetailsId"]);
                    deta.Remarks = dr["Remarks"].ToString();
                    aDetails.Add(deta);
                }
                return aDetails;
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