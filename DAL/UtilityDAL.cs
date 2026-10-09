// Ported unchanged from Accounts\CSTL_Pro_to_Pay\DAL\UtilityDAL.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DataManager;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Models.Accounts_Master_M;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;

namespace New_Accounts.DAL
{
    public class UtilityDAL
    {
        DataAccessManager accessManager = new DataAccessManager();
        private static readonly ObjectCache CoaCache = MemoryCache.Default;
        private const int CoaCacheMinutes = 10;
        public List<CompanyInformation> CompanyInformation()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_DDL_Company");
            List<CompanyInformation> companyInformationList = new List<CompanyInformation>();
            while (dr.Read())
            {
                CompanyInformation companyInformation = new CompanyInformation();
                companyInformation.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                companyInformation.CompanyName = dr["CompanyName"].ToString();
                companyInformation.CompanyShortName = dr["CompanyName"].ToString();
                companyInformationList.Add(companyInformation);
            }

            return companyInformationList;
        }


        public List<CompanyInformation> LoadERPVoucherIntegration(string param)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@param", param));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ERPVoucherIntegrationByType", aParameters);
            List<CompanyInformation> companyInformationList = new List<CompanyInformation>();
            while (dr.Read())
            {
                CompanyInformation companyInformation = new CompanyInformation();
               
                companyInformation.CompanyName = dr["CompanyName"].ToString();
               
                companyInformationList.Add(companyInformation);
            }

            return companyInformationList;
        }


        public List<Country> CountryInformation()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCountryDropDown");
            List<Country> CountryList = new List<Country>();
            while (dr.Read())
            {
                Country Country_dao = new Country();
                Country_dao.CountryId = Convert.ToInt32(dr["CountryId"]);
                Country_dao.CountryName = dr["CountryName"].ToString();

                CountryList.Add(Country_dao);
            }

            return CountryList;
        }

        public List<BusinessUnit> CompanyUnitByCompanyId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_DDL_CompanyUnit", aParameters);
            List<BusinessUnit> businessUnitList = new List<BusinessUnit>();
            while (dr.Read())
            {
                BusinessUnit businessUnit = new BusinessUnit();
                businessUnit.CompanyUnitId = Convert.ToInt32(dr["CompanyUnitId"]);
                businessUnit.CompanyUnitName = dr["CompanyUnitName"].ToString();
                businessUnit.ShortName = dr["CompanyUnitName"].ToString();
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<FinancialYear> GetFinansialYearComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_FinancialYearforOpeningBalanceDropDown", aParameters);
            List<FinancialYear> businessUnitList = new List<FinancialYear>();
            while (dr.Read())
            {
                FinancialYear businessUnit = new FinancialYear();
                businessUnit.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                businessUnit.FinancialYearDesc = dr["FinancialYearDesc"].ToString();
            
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<FinancialYear> GetFinansialYearNotLockedComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_FinancialYearforNotLock", aParameters);
            List<FinancialYear> businessUnitList = new List<FinancialYear>();
            while (dr.Read())
            {
                FinancialYear businessUnit = new FinancialYear();
                businessUnit.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                businessUnit.FinancialYearDesc = dr["FinancialYearDesc"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<FinancialYear> GetFinansialYearSingle(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_FinancialYearSingle", aParameters);
            List<FinancialYear> businessUnitList = new List<FinancialYear>();
            while (dr.Read())
            {
                FinancialYear businessUnit = new FinancialYear();
                //businessUnit.FinancialYearId = Convert.ToInt32(dr["FinancialYearId"]);
                businessUnit.FinancialYearDesc = dr["FinancialYearDesc"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }
        public List<AccNo_DDL> GetAccountNObyBankCom(int companyId, int BankNameId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            aParameters.Add(new SqlParameter("@bankID", BankNameId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllBankAccountUnderCompanyID", aParameters);
            List<AccNo_DDL> businessUnitList = new List<AccNo_DDL>();
            while (dr.Read())
            {
                AccNo_DDL businessUnit = new AccNo_DDL();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountNo = dr["AccountNo"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<AccNo_DDL> GetCashAccountbyCom(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
         
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCashAccountUnderCompanyID", aParameters);
            List<AccNo_DDL> businessUnitList = new List<AccNo_DDL>();
            while (dr.Read())
            {
                AccNo_DDL businessUnit = new AccNo_DDL();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        internal List<SubsidaryTypes> LoadSubsidaryType()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubsidiaryTypeDDL", aParameters);

            List<SubsidaryTypes> SubsidaryTypeList = new List<SubsidaryTypes>();
            while (dr.Read())
            {
                SubsidaryTypes SubsidaryType = new SubsidaryTypes();
                SubsidaryType.SubsidiryTypeId = Convert.ToInt32(dr["SubsidiaryTypeId"]);
                SubsidaryType.SubsidiryTypeName = dr["SubsidiaryType"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }

        internal List<ReferenceTypeDao> LoadReferenceType()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ReferenceTypeDDL", aParameters);

            List<ReferenceTypeDao> SubsidaryTypeList = new List<ReferenceTypeDao>();
            while (dr.Read())
            {
                ReferenceTypeDao SubsidaryType = new ReferenceTypeDao();
                SubsidaryType.ReferenceTypeId = Convert.ToInt32(dr["ReferenceTypeId"]);
                SubsidaryType.ReferenceType = dr["ReferenceType"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }
        
        internal List<DDLDAO> LoadSupplierList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SupplierDDL", aParameters);

            List<DDLDAO> SubsidaryTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO SubsidaryType = new DDLDAO();
                SubsidaryType.ValueId = Convert.ToInt32(dr["ValueId"]);
                SubsidaryType.ValueText = dr["ValueText"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }


        internal List<SupplierType> LoadSupplierType()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_AllSupplierType", aParameters);

            List<SupplierType> SubsidaryTypeList = new List<SupplierType>();
            while (dr.Read())
            {
                SupplierType SubsidaryType = new SupplierType();
                SubsidaryType.SupplierTypeId = Convert.ToInt32(dr["SupplierTypeId"]);
                SubsidaryType.SupplierTypeName= dr["SupplierTypeName"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }
        internal List<DDLDAO> LoadLoanCategoryList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_LoanCategoryDDL", aParameters);

            List<DDLDAO> loanCategoryList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO loanCategory = new DDLDAO();
                loanCategory.ValueId = Convert.ToInt32(dr["ValueId"]);
                loanCategory.ValueText = dr["ValueText"].ToString();

                loanCategoryList.Add(loanCategory);
            }

            return loanCategoryList;
        }
        internal List<DDLDAO> LoadCurrencyList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CurrencyDDL", aParameters);

            List<DDLDAO> loanCategoryList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO loanCategory = new DDLDAO();
                loanCategory.ValueId = Convert.ToInt32(dr["ValueId"]);
                loanCategory.ValueText = dr["ValueText"].ToString();

                loanCategoryList.Add(loanCategory);
            }

            return loanCategoryList;
        }
        internal List<DDLDAO> LoadInterestTypeList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_InterestTypeDDL", aParameters);

            List<DDLDAO> interestTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO interestType = new DDLDAO();
                interestType.ValueId = Convert.ToInt32(dr["ValueId"]);
                interestType.ValueText = dr["ValueText"].ToString();

                interestTypeList.Add(interestType);
            }

            return interestTypeList;
        }
        internal List<DDLDAO> LoadloanAccountList(int LCNoID)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@LCNoID", LCNoID));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_LoanInfobyLCNoID", aParameters);

            List<DDLDAO> interestTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO interestType = new DDLDAO();
                interestType.ValueId = Convert.ToInt32(dr["ValueId"]);
                interestType.ValueText = dr["ValueText"].ToString();

                interestTypeList.Add(interestType);
            }

            return interestTypeList;
        }
        internal List<DDLDAO> LoadPaymentTypeList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_LcBankPayType", aParameters);

            List<DDLDAO> interestTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO interestType = new DDLDAO();
                interestType.ValueId = Convert.ToInt32(dr["ValueId"]);
                interestType.ValueText = dr["ValueText"].ToString();

                interestTypeList.Add(interestType);
            }

            return interestTypeList;
        }
        internal List<DDLDAO> LoadInterestMethodList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_InterestMethodDDL", aParameters);

            List<DDLDAO> interestTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO interestType = new DDLDAO();
                interestType.ValueId = Convert.ToInt32(dr["ValueId"]);
                interestType.ValueText = dr["ValueText"].ToString();

                interestTypeList.Add(interestType);
            }

            return interestTypeList;
        }

        
        internal List<DDLDAO> getLoadProfitCenterList()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ProfitCenterDDL", aParameters);

            List<DDLDAO> interestTypeList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO interestType = new DDLDAO();
                interestType.ValueId = Convert.ToInt32(dr["ValueId"]);
                interestType.ValueText = dr["ValueText"].ToString();

                interestTypeList.Add(interestType);
            }

            return interestTypeList;
        }

        internal List<DDLDAO> LoadProfitCenterListSearch(int businessUnitId, string searchText)
        {
            SqlDataReader dr = null;

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@BusinessUnitId", businessUnitId));
                aParameters.Add(new SqlParameter("@SearchText", string.IsNullOrWhiteSpace(searchText) ? "" : searchText.Trim()));

                dr = accessManager.GetSqlDataReader("sp_GET_ProfitCenterDDL", aParameters);

                List<DDLDAO> profitCenterList = new List<DDLDAO>();

                while (dr.Read())
                {
                    DDLDAO item = new DDLDAO();
                    item.ValueId = Convert.ToInt32(dr["ValueId"]);
                    item.ValueText = dr["ValueText"].ToString();

                    profitCenterList.Add(item);
                }

                return profitCenterList;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                accessManager.SqlConnectionClose();
            }
        }


        internal List<SubsidaryType> LoadSubsidaryByTypeId(int supplierTypeId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            aParameters.Add(new SqlParameter("@SubsidiaryTypeId", supplierTypeId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubsidiaryByTypeDDL", aParameters);

            List<SubsidaryType> SubsidaryTypeList = new List<SubsidaryType>();
            while (dr.Read())
            {
                SubsidaryType SubsidaryType = new SubsidaryType();


                SubsidaryType.SubsidiryLId = Convert.ToInt32(dr["SubsidiaryInfoId"]);
                SubsidaryType.SubsidiryLName = dr["SubsidiaryName"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }



        internal List<SubsidaryType> LoadSubsidaryByTypeIdSearch(int supplierTypeId, string searchText)
        {
            SqlDataReader dr = null;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@SubsidiaryTypeId", supplierTypeId));
                aParameters.Add(new SqlParameter("@SearchText", string.IsNullOrWhiteSpace(searchText) ? "" : searchText.Trim()));

                dr = accessManager.GetSqlDataReader("sp_GET_SubsidiaryByTypeDDL", aParameters);

                List<SubsidaryType> subsidaryTypeList = new List<SubsidaryType>();

                while (dr.Read())
                {
                    SubsidaryType subsidaryType = new SubsidaryType();

                    subsidaryType.SubsidiryLId = Convert.ToInt32(dr["SubsidiaryInfoId"]);
                    subsidaryType.SubsidiryLName = dr["SubsidiaryName"].ToString();

                    subsidaryTypeList.Add(subsidaryType);
                }

                return subsidaryTypeList;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                accessManager.SqlConnectionClose();
            }
        }

        internal List<ReferenceInfoDao> LoadReferenceByTypeId(int ReferenceTypeId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            aParameters.Add(new SqlParameter("@ReferenceTypeId", ReferenceTypeId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ReferenceByTypeDDL", aParameters);

            List<ReferenceInfoDao> SubsidaryTypeList = new List<ReferenceInfoDao>();
            while (dr.Read())
            {
                ReferenceInfoDao SubsidaryType = new ReferenceInfoDao();


                SubsidaryType.ReferenceInfoId = Convert.ToInt32(dr["ReferenceInfoId"]);
                SubsidaryType.ReferenceName = dr["ReferenceName"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }
        internal List<ReferenceInfoDao> LoadReferenceByTypeIdComWise(int ReferenceTypeId, int BusinessUnitId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();

            aParameters.Add(new SqlParameter("@ReferenceTypeId", ReferenceTypeId));
            aParameters.Add(new SqlParameter("@BusinessUnitId", BusinessUnitId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ReferenceByTypeDDLCompanyWise", aParameters);

            List<ReferenceInfoDao> SubsidaryTypeList = new List<ReferenceInfoDao>();
            while (dr.Read())
            {
                ReferenceInfoDao SubsidaryType = new ReferenceInfoDao();


                SubsidaryType.ReferenceInfoId = Convert.ToInt32(dr["ReferenceInfoId"]);
                SubsidaryType.ReferenceName = dr["ReferenceName"].ToString();

                SubsidaryTypeList.Add(SubsidaryType);
            }

            return SubsidaryTypeList;
        }

        internal List<ReferenceInfoDao> LoadReferenceByReferenceTypeComWiseIdSearch(int referenceTypeId, int businessUnitId, string searchText)
        {
            SqlDataReader dr = null;

            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@ReferenceTypeId", referenceTypeId));
                aParameters.Add(new SqlParameter("@BusinessUnitId", businessUnitId));
                aParameters.Add(new SqlParameter("@SearchText", string.IsNullOrWhiteSpace(searchText) ? "" : searchText.Trim()));

                dr = accessManager.GetSqlDataReader("sp_GET_ReferenceByTypeDDLCompanyWise", aParameters);

                List<ReferenceInfoDao> referenceList = new List<ReferenceInfoDao>();

                while (dr.Read())
                {
                    ReferenceInfoDao reference = new ReferenceInfoDao();
                    reference.ReferenceInfoId = Convert.ToInt32(dr["ReferenceInfoId"]);
                    reference.ReferenceName = dr["ReferenceName"].ToString();

                    referenceList.Add(reference);
                }

                return referenceList;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                accessManager.SqlConnectionClose();
            }
        }
         
        public FinancialYearDDL GetDaterangebyFiscal(int companyId, int FinancialYearId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                FinancialYearDDL aMaster = new FinancialYearDDL();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@ComID", companyId));
                aParameters.Add(new SqlParameter("@financialYear", FinancialYearId));

                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_FinancialYearDateRange", aParameters);
                while (dr.Read())
                {
                    aMaster.StartDate = (dr["StartDate"].ToString());
                   
                    aMaster.EndDate = dr["EndDate"].ToString();
                    


                }
                return aMaster;
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


        public List<BankAccount_Ddl> GetBankAccountComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_BankAccountComId", aParameters);
            List<BankAccount_Ddl> businessUnitList = new List<BankAccount_Ddl>();
            while (dr.Read())
            {
                BankAccount_Ddl businessUnit = new BankAccount_Ddl();
                businessUnit.BankAccountId = Convert.ToInt32(dr["BankAccountId"]);
                businessUnit.BankAccountName = dr["BankAccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<BankAccount_Ddl> GetBankCashAccountComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_BankCashAccountComId", aParameters);
            List<BankAccount_Ddl> businessUnitList = new List<BankAccount_Ddl>();
            while (dr.Read())
            {
                BankAccount_Ddl businessUnit = new BankAccount_Ddl();
                businessUnit.BankAccountId = Convert.ToInt32(dr["BankAccountId"]);
                businessUnit.BankAccountName = dr["BankAccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }
        public List<BankAccount_Ddl> GetAllBankCashAccountComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ALLBankCashAccountComId", aParameters);
            List<BankAccount_Ddl> businessUnitList = new List<BankAccount_Ddl>();
            while (dr.Read())
            {
                BankAccount_Ddl businessUnit = new BankAccount_Ddl();
                businessUnit.BankAccountId = Convert.ToInt32(dr["BankAccountId"]);
                businessUnit.BankAccountName = dr["BankAccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<CashAccount_Ddl> GetCashAccountComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CashAccountComId", aParameters);
            List<CashAccount_Ddl> businessUnitList = new List<CashAccount_Ddl>();
            while (dr.Read())
            {
                CashAccount_Ddl businessUnit = new CashAccount_Ddl();
                businessUnit.CashAccountId = Convert.ToInt32(dr["CashAccountId"]);
                businessUnit.CashAccountName = dr["CashAccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }



        public List<COA_Ddl> GetCOAComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_COAComId", aParameters);
            List<COA_Ddl> businessUnitList = new List<COA_Ddl>();
            while (dr.Read())
            {
                COA_Ddl businessUnit = new COA_Ddl();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();
                businessUnit.SubControlName = dr["SubControlName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<COA_Ddl> GetALLCOAComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ALLCOAComId", aParameters);
            List<COA_Ddl> businessUnitList = new List<COA_Ddl>();
            while (dr.Read())
            {
                COA_Ddl businessUnit = new COA_Ddl();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<COA_Ddl> GetCOASubSubControlLedgerId(int SubSubControlLedgerId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SubSubControlLedgerId", SubSubControlLedgerId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_COASubSubControlLedgerId", aParameters);
            List<COA_Ddl> businessUnitList = new List<COA_Ddl>();
            while (dr.Read())
            {
                COA_Ddl businessUnit = new COA_Ddl();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<COA_Ddl> GetCOAALLComId(int companyId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CompanyId", companyId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_COAALLComId", aParameters);
            List<COA_Ddl> businessUnitList = new List<COA_Ddl>();
            while (dr.Read())
            {
                COA_Ddl businessUnit = new COA_Ddl();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();
                businessUnit.SubControlName = dr["SubControlName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        private List<COA_Ddl> GetCOAALLComIdCached(int companyId)
        {
            var cacheKey = "COAALLComId_" + companyId;
            var cachedList = CoaCache.Get(cacheKey) as List<COA_Ddl>;
            if (cachedList != null)
            {
                return cachedList;
            }

            var freshList = GetCOAALLComId(companyId);
            CoaCache.Set(cacheKey, freshList, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(CoaCacheMinutes)
            });

            return freshList;
        }

        public List<COA_Ddl> SearchCOAALLComId(int companyId, string term, int take = 50)
        {
            var allCoa = GetCOAALLComIdCached(companyId);
            if (string.IsNullOrWhiteSpace(term))
            {
                return allCoa.Take(take).ToList();
            }

            var searchText = term.Trim();
            return allCoa.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.AccountName) && x.AccountName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(x.SubControlName) && x.SubControlName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0))
                .Take(take)
                .ToList();
        }

        public COA_Ddl GetCOAALLComIdByAccountId(int companyId, int accountId)
        {
            if (accountId <= 0)
            {
                return null;
            }

            return GetCOAALLComIdCached(companyId).FirstOrDefault(x => x.AccountId == accountId);
        }
        public List<CostCenter_Ddl> GetCostCenterByCOAId(int AccountId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@AccountId", AccountId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CostCenterByCOAId", aParameters);
            List<CostCenter_Ddl> businessUnitList = new List<CostCenter_Ddl>();
            while (dr.Read())
            {
                CostCenter_Ddl businessUnit = new CostCenter_Ddl();
                businessUnit.CostCenterId = Convert.ToInt32(dr["CostCenterId"]);
                businessUnit.CostCenterName = dr["CostCenterName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        private List<CostCenter_Ddl> GetCostCenterByCOAIdCached(int accountId)
        {
            if (accountId <= 0)
            {
                return new List<CostCenter_Ddl>();
            }

            var cacheKey = "CostCenterByCOAId_" + accountId;
            var cachedList = CoaCache.Get(cacheKey) as List<CostCenter_Ddl>;
            if (cachedList != null)
            {
                return cachedList;
            }

            var freshList = GetCostCenterByCOAId(accountId);
            CoaCache.Set(cacheKey, freshList, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(CoaCacheMinutes)
            });

            return freshList;
        }

        public List<CostCenter_Ddl> SearchCostCenterByCOAId(int accountId, string term, int take = 50)
        {
            var allItems = GetCostCenterByCOAIdCached(accountId);
            if (string.IsNullOrWhiteSpace(term))
            {
                return allItems.Take(take).ToList();
            }

            var searchText = term.Trim();
            return allItems
                .Where(x => !string.IsNullOrWhiteSpace(x.CostCenterName) &&
                            x.CostCenterName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Take(take)
                .ToList();
        }

        public CostCenter_Ddl GetCostCenterByCOAIdById(int accountId, int costCenterId)
        {
            if (costCenterId <= 0)
            {
                return null;
            }

            return GetCostCenterByCOAIdCached(accountId).FirstOrDefault(x => x.CostCenterId == costCenterId);
        }


        public List<Reference_Ddl> GetRefNoByCOAId(int AccountId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@AccountId", AccountId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_RefByCOAId", aParameters);
            List<Reference_Ddl> businessUnitList = new List<Reference_Ddl>();
            while (dr.Read())
            {
                Reference_Ddl businessUnit = new Reference_Ddl();
                businessUnit.ReferenceInfoId = Convert.ToInt32(dr["ReferenceInfoId"]);
                businessUnit.ReferenceInfo = dr["ReferenceInfo"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        private List<Reference_Ddl> GetRefNoByCOAIdCached(int accountId)
        {
            if (accountId <= 0)
            {
                return new List<Reference_Ddl>();
            }

            var cacheKey = "RefNoByCOAId_" + accountId;
            var cachedList = CoaCache.Get(cacheKey) as List<Reference_Ddl>;
            if (cachedList != null)
            {
                return cachedList;
            }

            var freshList = GetRefNoByCOAId(accountId);
            CoaCache.Set(cacheKey, freshList, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(CoaCacheMinutes)
            });

            return freshList;
        }

        public List<Reference_Ddl> SearchRefNoByCOAId(int accountId, string term, int take = 50)
        {
            var allItems = GetRefNoByCOAIdCached(accountId);
            if (string.IsNullOrWhiteSpace(term))
            {
                return allItems.Take(take).ToList();
            }

            var searchText = term.Trim();
            return allItems
                .Where(x => !string.IsNullOrWhiteSpace(x.ReferenceInfo) &&
                            x.ReferenceInfo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Take(take)
                .ToList();
        }

        public Reference_Ddl GetRefNoByCOAIdById(int accountId, int referenceInfoId)
        {
            if (referenceInfoId <= 0)
            {
                return null;
            }

            return GetRefNoByCOAIdCached(accountId).FirstOrDefault(x => x.ReferenceInfoId == referenceInfoId);
        }

        public List<Subsidiary_Ddl> GetSubsidoryLedgerByCOAId(int AccountId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@AccountId", AccountId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubsidoryLedgerByCOAId", aParameters);
            List<Subsidiary_Ddl> businessUnitList = new List<Subsidiary_Ddl>();
            while (dr.Read())
            {
                Subsidiary_Ddl businessUnit = new Subsidiary_Ddl();
                businessUnit.SubsidiaryInfoId = Convert.ToInt32(dr["SubsidiaryInfoId"]);
                businessUnit.SubsidiaryInfo = dr["SubsidiaryInfo"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        private List<Subsidiary_Ddl> GetSubsidoryLedgerByCOAIdCached(int accountId)
        {
            if (accountId <= 0)
            {
                return new List<Subsidiary_Ddl>();
            }

            var cacheKey = "SubsidoryLedgerByCOAId_" + accountId;
            var cachedList = CoaCache.Get(cacheKey) as List<Subsidiary_Ddl>;
            if (cachedList != null)
            {
                return cachedList;
            }

            var freshList = GetSubsidoryLedgerByCOAId(accountId);
            CoaCache.Set(cacheKey, freshList, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(CoaCacheMinutes)
            });

            return freshList;
        }

        public List<Subsidiary_Ddl> SearchSubsidoryLedgerByCOAId(int accountId, string term, int take = 50)
        {
            var allItems = GetSubsidoryLedgerByCOAIdCached(accountId);
            if (string.IsNullOrWhiteSpace(term))
            {
                return allItems.Take(take).ToList();
            }

            var searchText = term.Trim();
            return allItems
                .Where(x => !string.IsNullOrWhiteSpace(x.SubsidiaryInfo) &&
                            x.SubsidiaryInfo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Take(take)
                .ToList();
        }

        public Subsidiary_Ddl GetSubsidoryLedgerByCOAIdById(int accountId, int subsidiaryInfoId)
        {
            if (subsidiaryInfoId <= 0)
            {
                return null;
            }

            return GetSubsidoryLedgerByCOAIdCached(accountId).FirstOrDefault(x => x.SubsidiaryInfoId == subsidiaryInfoId);
        }
        public List<ControllLadger> ControlLedgerByLedgerHeadId(int LedgerHeadId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@GLHeadId", LedgerHeadId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ControllLadgerDropDown", aParameters);
            List<ControllLadger> businessUnitList = new List<ControllLadger>();
            while (dr.Read())
            {
                ControllLadger businessUnit = new ControllLadger();
                businessUnit.ControlLId = Convert.ToInt32(dr["ControlLId"]);
                businessUnit.ControlLName = dr["ControlLName"].ToString();
           
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<Supplierinfo> SupplierTypewiseSupplier(int SupplierTypeID)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SupplierTypeID", SupplierTypeID));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SupplierbyType", aParameters);
            List<Supplierinfo> businessUnitList = new List<Supplierinfo>();
            while (dr.Read())
            {
                Supplierinfo businessUnit = new Supplierinfo();
                businessUnit.SupplierId = Convert.ToInt32(dr["SupplierId"]);
                businessUnit.SupplierName = dr["SupplierName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<Supplierinfo> SupplierwiseLC(int SupplierTypeID)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SupplierTypeID", SupplierTypeID));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SupplierbyType", aParameters);
            List<Supplierinfo> businessUnitList = new List<Supplierinfo>();
            while (dr.Read())
            {
                Supplierinfo businessUnit = new Supplierinfo();
                businessUnit.SupplierId = Convert.ToInt32(dr["SupplierId"]);
                businessUnit.SupplierName = dr["SupplierName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }
        

        public List<DDLDAO> LoadCINoByLCNo(int LCNoID)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@LCNoID", LCNoID));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_CINoByLCNo", aParameters);
            List<DDLDAO> businessUnitList = new List<DDLDAO>();
            while (dr.Read())
            {
                DDLDAO businessUnit = new DDLDAO();
                businessUnit.ValueId = Convert.ToInt32(dr["ValueId"]);
                businessUnit.ValueText = dr["ValueText"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        public List<SubsidaryType> SubsidaryByControlLedgerId(int ControlLedgerId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@ControlId", ControlLedgerId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubsidiaryDropDown", aParameters);
            List<SubsidaryType> businessUnitList = new List<SubsidaryType>();
            while (dr.Read())
            {
                SubsidaryType businessUnit = new SubsidaryType();
                businessUnit.SubsidiryLId = Convert.ToInt32(dr["SubsidiryLId"]);
                businessUnit.SubsidiryLName = dr["SubsidiryLName"].ToString();
          

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }


        public List<SubSubControlLedgerView> LoadSubSubControlLedgerId(int SubControlLedgerId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);

            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SubControlLedgerId", SubControlLedgerId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_SubSubControlLedgerDropDown", aParameters);
            List<SubSubControlLedgerView> businessUnitList = new List<SubSubControlLedgerView>();

            while (dr.Read())
            {
                SubSubControlLedgerView businessUnit = new SubSubControlLedgerView();
                businessUnit.SubSubControlLedgerId = Convert.ToInt32(dr["SubSubControlLedgerId"]);
                businessUnit.SubSubControlLedgerName = dr["SubSubControlLedgerName"].ToString();

                businessUnitList.Add(businessUnit);
            }

            return businessUnitList;
        }

        public List<ChartofAccount> ChartOfAccountById(int subId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@SubsidiaryId", subId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_ChartOfAccountDropDownById", aParameters);
            List<ChartofAccount> businessUnitList = new List<ChartofAccount>();
            while (dr.Read())
            {
                ChartofAccount businessUnit = new ChartofAccount();
                businessUnit.AccountId = Convert.ToInt32(dr["AccountId"]);
                businessUnit.AccountName = dr["AccountName"].ToString();

                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }
        public List<City> CitybyCountryId(int CountryId)
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@CountryId", CountryId));
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCitybyCountryId", aParameters);
            List<City> CityList = new List<City>();
            while (dr.Read())
            {
                City _City = new City();
                _City.CityId = Convert.ToInt32(dr["CityId"]);
                _City.CityName = dr["CityName"].ToString();

                CityList.Add(_City);
            }
            return CityList;
        }
        public string GetUserInfoStrig(string id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@id", id));
                string result = string.Empty;
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_Get_UserInfoString", aParameters);
                while (dr.Read())
                {
                    result = dr["EmpName"].ToString();
                }
                return result;
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
        public bool CheckMenuAccess(string controllerName, string actionName, string userName)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
                aSqlParameterList.Add(new SqlParameter("@ModuleId", "13"));
                aSqlParameterList.Add(new SqlParameter("@LoginUserName", userName));
                aSqlParameterList.Add(new SqlParameter("@ControllerName", controllerName));
                aSqlParameterList.Add(new SqlParameter("@ActionName", actionName));


                DataTable dt = accessManager.GetDataTable("sp_CheckMenuAccess", aSqlParameterList);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }


                return false;
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


        public List<BankMaster> GetBankName()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
                List<SqlParameter> aParameters = new List<SqlParameter>();

                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllBankInformationDropDown");
                List<BankMaster> _divList = new List<BankMaster>();
                while (dr.Read())
                {
                    BankMaster aDivision = new BankMaster();
                    aDivision.BankId = Convert.ToInt32(dr["BankId"]);
                    aDivision.BankName = dr["BankName"].ToString().Trim();
                    _divList.Add(aDivision);
                }
                return _divList;


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


        internal List<BankWiseBranchList> LoadBankWiseBranch(string bankId)
        {
            accessManager.SqlConnectionOpen(DataBase.ControlPanel);

            List<SqlParameter> aParameters = new List<SqlParameter>();
            aParameters.Add(new SqlParameter("@BankId", bankId));

            SqlDataReader dr = accessManager.GetSqlDataReader("sp_DDL_BankBranchDDL", aParameters);

            List<BankWiseBranchList> incoTermList = new List<BankWiseBranchList>();

            while (dr.Read())
            {
                BankWiseBranchList incoTerm = new BankWiseBranchList();

                incoTerm.BankBranchId = Convert.ToInt32(dr["BankBranchId"]);
                incoTerm.BranchName = dr["BranchName"].ToString();

                incoTermList.Add(incoTerm);
            }

            return incoTermList;
        }

        public List<BankMaster> GetBankNameNew()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.ControlPanel);
                List<SqlParameter> aParameters = new List<SqlParameter>();

                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllBankInformationDropDown");
                List<BankMaster> _divList = new List<BankMaster>();
                while (dr.Read())
                {
                    BankMaster aDivision = new BankMaster();
                    aDivision.BankId = Convert.ToInt32(dr["BankId"]);
                    aDivision.BankName = dr["BankName"].ToString().Trim();
                    _divList.Add(aDivision);
                }
                return _divList;


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


        public List<Currency> GetCurrency()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_AllCurrencyDropDown");
            List<Currency> _cList = new List<Currency>();
            while (dr.Read())
            {
                Currency aCur = new Currency();
                aCur.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);
                aCur.CurrencyName = dr["CurrencyName"].ToString(); 
                aCur.CurrencyRate = dr["CurrencyRate"].ToString(); 

                _cList.Add(aCur);
            }
            return _cList;
        }


        public List<AccountType_> GetAccountType()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_DDL_AccountType");
            List<AccountType_> _cList = new List<AccountType_>();
            while (dr.Read())
            {
                AccountType_ aCur = new AccountType_();
                aCur.AccountTypeId = Convert.ToInt32(dr["AccountTypeId"]);
                aCur.AccountType = dr["AccountType"].ToString(); ;

                _cList.Add(aCur);
            }
            return _cList;
        }


        public List<LadgerHead> GetLadgerHead()
        {
            accessManager.SqlConnectionOpen(DataBase.CSTL_ERP_DB);
            SqlDataReader dr = accessManager.GetSqlDataReader("sp_GET_LadgerHeadDropDown");
            List<LadgerHead> _cList = new List<LadgerHead>();
            while (dr.Read())
            {
                LadgerHead aCur = new LadgerHead();
                aCur.GLHeadId = Convert.ToInt32(dr["GLHeadId"]);
                aCur.GLName = dr["GLName"].ToString(); ;

                _cList.Add(aCur);
            }
            return _cList;
        }

       



    }
}
