// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Controllers\UtilityController.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using New_Accounts.DAL;
using New_Accounts.DAL.Account_Master;
using New_Accounts.Models;
using New_Accounts.Models.Accounts;
using New_Accounts.Models.Accounts_Master_M;
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Controllers
{
    public class UtilityController : LegacyController
    {


        // GET: Utility
        UtilityDAL _utilityDal = new UtilityDAL();
        ChartOfAccountDal _coa = new ChartOfAccountDal();
        public JsonResult LoadCompanyInfo()
        {
            List<CompanyInformation> companies = _utilityDal.CompanyInformation();
            var asd = Json(companies);
            return Json(companies);
        }

        public JsonResult LoadERPVoucherIntegration(string param)
        {
            List<CompanyInformation> companies = _utilityDal.LoadERPVoucherIntegration(param);
            var asd = Json(companies);
            return Json(companies);
        }


        public JsonResult LoadCompanyUnitByCompanyId(int companyId)
        {
            List<BusinessUnit> bu = _utilityDal.CompanyUnitByCompanyId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }


        public JsonResult GetFinansialYearComId(int companyId)
        {
            List<FinancialYear> bu = _utilityDal.GetFinansialYearComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetFinansialYearNotLockedComId(int companyId)
        {
            List<FinancialYear> bu = _utilityDal.GetFinansialYearNotLockedComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetFinansialYearSingle(int companyId)
        {
            List<FinancialYear> bu = _utilityDal.GetFinansialYearSingle(companyId);
            var asd = Json(bu);
            return Json(bu);
        }


        public JsonResult GetAccountNObyBankCom(int companyId, int BankNameId)
        {
            List<AccNo_DDL> bu = _utilityDal.GetAccountNObyBankCom(companyId, BankNameId);
            var asd = Json(bu);
            return Json(bu);
        }


        public JsonResult GetCashAccountbyCom(int companyId)
        {
            List<AccNo_DDL> bu = _utilityDal.GetCashAccountbyCom(companyId);
            var asd = Json(bu);
            return Json(bu);
        }


       

        public JsonResult GetDaterangebyFiscal(int companyId, int FinancialYearId)
        {
            

            FinancialYearDDL aMaster = _utilityDal.GetDaterangebyFiscal(companyId, FinancialYearId);
            //aMaster.DetailsInfo = dbDebit.GetBankPaymentDetailsById(id);
            return Json(aMaster);
        }


        public JsonResult GetBankAccountComId(int companyId)
        {
            List<BankAccount_Ddl> bu = _utilityDal.GetBankAccountComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetBankCashAccountComId(int companyId)
        {
            List<BankAccount_Ddl> bu = _utilityDal.GetBankCashAccountComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetAllBankCashAccountComId(int companyId)
        {
            List<BankAccount_Ddl> bu = _utilityDal.GetAllBankCashAccountComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetCashAccountComId(int companyId)
        {
            List<CashAccount_Ddl> bu = _utilityDal.GetCashAccountComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetCOAComId(int companyId)
        {
            List<COA_Ddl> bu = _utilityDal.GetCOAComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetAllCOAComId(int companyId)
        {
            List<COA_Ddl> bu = _utilityDal.GetALLCOAComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetCOASubSubControlLedgerId(int SubSubControlLedgerId)
        {
            List<COA_Ddl> bu = _utilityDal.GetCOASubSubControlLedgerId(SubSubControlLedgerId);
            var asd = Json(bu);
            return Json(bu);
        }
        public JsonResult GetCOAALLComId(int companyId)
        {
            List<COA_Ddl> bu = _utilityDal.GetCOAALLComId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetCOAALLComIdAuto(int companyId, string term = "", int take = 50)
        {
            List<COA_Ddl> bu = _utilityDal.SearchCOAALLComId(companyId, term, take);
            return Json(bu);
        }

        public JsonResult GetCOAALLComIdByAccountId(int companyId, int accountId)
        {
            COA_Ddl coa = _utilityDal.GetCOAALLComIdByAccountId(companyId, accountId);
            return Json(coa);
        }

        public JsonResult GetCostCenterByCOAId(int AccountId)
        {
            List<CostCenter_Ddl> bu = _utilityDal.GetCostCenterByCOAId(AccountId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetCostCenterByCOAIdAuto(int accountId, string term = "", int take = 50)
        {
            List<CostCenter_Ddl> bu = _utilityDal.SearchCostCenterByCOAId(accountId, term, take);
            return Json(bu);
        }

        public JsonResult GetCostCenterByCOAIdById(int accountId, int costCenterId)
        {
            CostCenter_Ddl item = _utilityDal.GetCostCenterByCOAIdById(accountId, costCenterId);
            return Json(item);
        }


        public JsonResult GetRefNoByCOAId(int AccountId)
        {
            List<Reference_Ddl> bu = _utilityDal.GetRefNoByCOAId(AccountId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetRefNoByCOAIdAuto(int accountId, string term = "", int take = 50)
        {
            List<Reference_Ddl> bu = _utilityDal.SearchRefNoByCOAId(accountId, term, take);
            return Json(bu);
        }

        public JsonResult GetRefNoByCOAIdById(int accountId, int referenceInfoId)
        {
            Reference_Ddl item = _utilityDal.GetRefNoByCOAIdById(accountId, referenceInfoId);
            return Json(item);
        }

        public JsonResult GetSubsidoryLedgerByCOAId(int AccountId)
        {
            List<Subsidiary_Ddl> bu = _utilityDal.GetSubsidoryLedgerByCOAId(AccountId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetSubsidoryLedgerByCOAIdAuto(int accountId, string term = "", int take = 50)
        {
            List<Subsidiary_Ddl> bu = _utilityDal.SearchSubsidoryLedgerByCOAId(accountId, term, take);
            return Json(bu);
        }

        public JsonResult GetSubsidoryLedgerByCOAIdById(int accountId, int subsidiaryInfoId)
        {
            Subsidiary_Ddl item = _utilityDal.GetSubsidoryLedgerByCOAIdById(accountId, subsidiaryInfoId);
            return Json(item);
        }


        public JsonResult LoadCountry()
        {
            List<Country> companies = _utilityDal.CountryInformation();
            var asd = Json(companies);
            return Json(companies);
        }

        public JsonResult LoadCitybyCountry(int companyId)
        {
            List<City> bu = _utilityDal.CitybyCountryId(companyId);
            var asd = Json(bu);
            return Json(bu);
        }

        public ActionResult GetBankName()
        {
            List<BankMaster> aList = _utilityDal.GetBankName();
            return Json(aList);
        }

        public ActionResult GetCurrency()
        {
            List<Currency> cu = _utilityDal.GetCurrency();
            return Json(cu);
        }


        public ActionResult GetAccountType()
        {
            List<AccountType_> cu = _utilityDal.GetAccountType();
            return Json(cu);
        }

        public ActionResult GetLadgerHead()
        {
            List<LadgerHead> cu = _utilityDal.GetLadgerHead();
            return Json(cu);
        }

        public ActionResult GetRefferenceType()
        {
            List<ReferanceType> cu = _coa.GetAllReferanceType(0);
            return Json(cu);
        }


        public ActionResult GetSubsidiaryType()
        {
            List<SubsidiaryType_Ddl> cu = _coa.GetAllSubSidaryType(0);
            return Json(cu);
        }


        public ActionResult GetReferanceType()
        {
            List<ReferanceType> cu = _coa.GetAllReferanceType(0);
            return Json(cu);
        }
        public JsonResult LoadControlLedgerByLedgerHeadId(int LedgerHeadId)
        {
            List<ControllLadger> bu = _utilityDal.ControlLedgerByLedgerHeadId(LedgerHeadId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult LoadSubsidaryType()
        {
            List<SubsidaryTypes> bu = _utilityDal.LoadSubsidaryType();
            var asd = Json(bu);
            return Json(bu);
        } 
        
       
        public JsonResult LoadReferenceType()
        {
            List<ReferenceTypeDao> bu = _utilityDal.LoadReferenceType();
            var asd = Json(bu);
            return Json(bu);
        }
        
        public JsonResult LoadSupplierList()
        {
            List<DDLDAO> bu = _utilityDal.LoadSupplierList();
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult LoadSupplierType()
        {
            List<SupplierType> bu = _utilityDal.LoadSupplierType();
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult LoadLoanCategoryList()
        {
            List<DDLDAO> loanCategories = _utilityDal.LoadLoanCategoryList();
            return Json(loanCategories);
        }
        public JsonResult LoadCurrencyList()
        {
            List<DDLDAO> loanCategories = _utilityDal.LoadCurrencyList();
            return Json(loanCategories);
        }

        // Load Interest Types
        public JsonResult LoadInterestTypeList()
        {
            List<DDLDAO> interestTypes = _utilityDal.LoadInterestTypeList();
            return Json(interestTypes);
        }
        public JsonResult LoadloanAccountList(int LCNoID)
        {
            List<DDLDAO> interestTypes = _utilityDal.LoadloanAccountList(LCNoID);
            return Json(interestTypes);
        }
        public JsonResult LoadPaymentTypeList()
        {
            List<DDLDAO> interestTypes = _utilityDal.LoadPaymentTypeList();
            return Json(interestTypes);
        }
        public ActionResult GetBankNameforLoan()
        {
            List<BankMaster> aList = _utilityDal.GetBankNameNew();
            return Json(aList);
        }


        // Load Interest Methods
        public JsonResult LoadInterestMethodList()
        {
            List<DDLDAO> interestMethods = _utilityDal.LoadInterestMethodList();
            return Json(interestMethods);
        }
        public JsonResult GetReferenceByReferenceTypeId(int ReferenceTypeId)
        {
            List<ReferenceInfoDao> bu = _utilityDal.LoadReferenceByTypeId(ReferenceTypeId);
            var asd = Json(bu);
            return Json(bu);
        }
        
        public JsonResult GetReferenceByReferenceTypeComWiseId(int ReferenceTypeId, int BusinessUnitId)
        {
            List<ReferenceInfoDao> bu = _utilityDal.LoadReferenceByTypeIdComWise(ReferenceTypeId, BusinessUnitId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetReferenceByReferenceTypeComWiseIdSearch(int ReferenceTypeId, int BusinessUnitId, string SearchText = "")
        {
            List<ReferenceInfoDao> items = _utilityDal.LoadReferenceByReferenceTypeComWiseIdSearch(
                ReferenceTypeId,
                BusinessUnitId,
                SearchText
            );

            var result = items.Select(x => new
            {
                id = x.ReferenceInfoId,
                text = x.ReferenceName
            }).ToList();

            return Json(result);
        }
        
        public JsonResult getLoadProfitCenterList()
        {
            List<DDLDAO> bu = _utilityDal.getLoadProfitCenterList();
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult GetProfitCenterListSearch(int BusinessUnitId, string SearchText = "")
        {
            var data = _utilityDal.LoadProfitCenterListSearch(BusinessUnitId, SearchText);

            var result = data.Select(x => new
            {
                id = x.ValueId,
                text = x.ValueText
            }).ToList();

            return Json(result);
        }



        public JsonResult LoadBankWiseBranch(string bankId)
        {
            List<BankWiseBranchList> companies = _utilityDal.LoadBankWiseBranch(bankId);
            var asd = Json(companies);
            return Json(companies);
        }

  
        public JsonResult GetSubsidoryLedgerBySupplierTypeId(int SupplierTypeId, string SearchText = "")
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                List<SubsidaryType> allItems = _utilityDal.LoadSubsidaryByTypeId(SupplierTypeId);
                return Json(allItems);
            }

            var select2Items = _utilityDal.LoadSubsidaryByTypeIdSearch(SupplierTypeId, SearchText)
                .Select(x => new
                {
                    id = x.SubsidiryLId,
                    text = x.SubsidiryLName
                })
                .ToList();

            return Json(select2Items);
        }


        public JsonResult LoadSubsidaryByControlLedgerId(int ControlLedgerId)
        {
            List<SubsidaryType> bu = _utilityDal.SubsidaryByControlLedgerId(ControlLedgerId);
            var asd = Json(bu);
            return Json(bu);
        }


        public JsonResult LoadSubSubControlLedgerId(int SubControlLedgerId)
        {
            List<SubSubControlLedgerView> bu = _utilityDal.LoadSubSubControlLedgerId(SubControlLedgerId);
            var asd = Json(bu);
            return Json(bu);
        }


        public JsonResult LoadChartofAccountId(int subId)
        {
            List<ChartofAccount> bu = _utilityDal.ChartOfAccountById(subId);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult SupplierTypewiseSupplier(int SupplierTypeID)
        {
            List<Supplierinfo> bu = _utilityDal.SupplierTypewiseSupplier(SupplierTypeID);
            var asd = Json(bu);
            return Json(bu);
        }

        public JsonResult SupplierwiseLC(int SupplierID)
        {
            List<Supplierinfo> bu = _utilityDal.SupplierwiseLC(SupplierID);
            var asd = Json(bu);
            return Json(bu);
        }
        
        public JsonResult LoadCINoByLCNo(int LCNoID)
        {
            List<DDLDAO> bu = _utilityDal.LoadCINoByLCNo(LCNoID);
            var asd = Json(bu);
            return Json(bu);
        }

    }

}
