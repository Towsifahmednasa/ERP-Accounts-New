// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Accounts\AllApprovalDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Accounts
{


    public class AllApprovalDAL
    {

        DataAccessManager _accessManager = new DataAccessManager();

        public ResultResponse ApproveAllVoucher(string MasterId,  bool? AppStatus, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@VoucherMasterId", MasterId));
                aSqlParameterlist.Add(new SqlParameter("@Status", AppStatus));
                aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", user));

                aResponse.isSuccess = _accessManager.UpdateData("sp_UP_AllVoucherApp", aSqlParameterlist);
                if (aResponse.isSuccess == false)
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

 public ResultResponse ApproveLoan(string MasterId,  bool? AppStatus, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@LoanInfoID", MasterId));
                aSqlParameterlist.Add(new SqlParameter("@Status", AppStatus));
                aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", user));

                aResponse.isSuccess = _accessManager.UpdateData("sp_UP_AllLoanApp", aSqlParameterlist);
                if (aResponse.isSuccess == false)
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

        

 public ResultResponse Approve_AllBankCharge(string MasterId,  bool? AppStatus, string user)
        {
            try
            {
                ResultResponse aResponse = new ResultResponse();
                _accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);



                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@BankChargeId", MasterId));
                aSqlParameterlist.Add(new SqlParameter("@Status", AppStatus));
                aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", user));

                aResponse.isSuccess = _accessManager.UpdateData("sp_UP_AllBankChargeApp", aSqlParameterlist);
                if (aResponse.isSuccess == false)
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


            }
}