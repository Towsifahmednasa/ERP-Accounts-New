// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\Account_Master\Ref_sub_InfoDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace New_Accounts.DAL.Account_Master
{
    public class Ref_sub_InfoDAL
    {

        private DataAccessManager accessManager = new DataAccessManager();

        public List<Subsidiary_chk> GetAllSubsidiaryInfo(string SubsidiaryType)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SubsidiaryType", SubsidiaryType));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubsidiaryBySubsidiaryType_Ddl", aParameters);
            List<Subsidiary_chk> businessUnitList = new List<Subsidiary_chk>();
            while (dr.Read())
            {
                Subsidiary_chk businessUnit = new Subsidiary_chk();
                businessUnit.S_Info_Id = Convert.ToInt32(dr["S_Info_Id"]);
                businessUnit.S_InfoName = dr["S_InfoName"].ToString();
                businessUnit.isrefrence = Convert.ToInt32(dr["isrefrence"]);
                businessUnit.isReadOnly = Convert.ToInt32(dr["isReadOnly"]);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<RefInfo_chk> GetAllReffInfo(string ReferenceType)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@ReferenceType", ReferenceType));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_RefInfoByRefType_Ddl", aParameters);
            List<RefInfo_chk> businessUnitList = new List<RefInfo_chk>();
            while (dr.Read())
            {
                RefInfo_chk businessUnit = new RefInfo_chk();
                businessUnit.R_Info_Id = Convert.ToInt32(dr["R_Info_Id"]);
                businessUnit.R_InfoName = dr["R_InfoName"].ToString();
                businessUnit.isrefrence = Convert.ToInt32(dr["isrefrence"]);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public DataTable GetSubsidiaryTableInfo(string SubsidiaryType)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@SubsidiaryType", SubsidiaryType));
                return accessManager.GetDataTable("sp_GET_DesSubsidiaryBySubsidiaryType_Ddl", aSqlParameterList);
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


        public DataTable GetRefTableInfo(string ReferenceType)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ReferenceType", ReferenceType));
                return accessManager.GetDataTable("sp_GET_DesRefByRefType_Ddl", aSqlParameterList);
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