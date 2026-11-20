-- Voucher print: add SubSubControlLedger name column
USE CSTL_ACCDB_Unicorn;
GO
ALTER PROCEDURE [dbo].[sp_Rpt_DebitCreditVoucherDetail] 

	@VoucherMasterId INT
	
AS
BEGIN
SELECT distinct DebitorCredit, ISNULL(DebitAmount,0) DebitAmount,CreditAmount,DebitAmountBDT,CreditAmountBDT,coa.AccountCode,coa.AccountName, VoucherDetailId, cs.CostCenterCode+' : '+cs.Description CostCenter, SubsidiaryDescriptiond RefInfo, ssl.SubSubControlLedgerName SubSubControlLedger,case when isnull(tblSubCust.CustomerName,'') <>'' then  tblSubCust.CustomerName else tblSubSup.SupplierName  end   SubsidoryLedger, 
	CASE WHEN ISNULL(dtl.LCChargesPaymentDetailsId, 0) > 0 THEN ISNULL(CH.ChargeHeadName, dtl.Remarks) ELSE dtl.Remarks END AS Remarks 
	FROM tblDebitCreditVoucherDetail dtl
	LEFT JOIN dbo.tblChartOfAccounts coa ON coa.AccountId = dtl.AccountId
	LEFT JOIN dbo.tblSubSubControlLedger ssl ON ssl.SubSubControlLedgerId = coa.SubsidiaryLId
	LEFT JOIN dbo.tblCostCenter cs ON cs.CostCenterId = dtl.CostCenterId 

	-- LC Charges Payment Details & Charge Heads Join (LCChargesPaymentDetailsId wise)
	LEFT JOIN CSTL_ERPDB_Unicorn..tbl_LCCharges_PaymentDetails LCD ON dtl.LCChargesPaymentDetailsId = LCD.LCChargesPaymentDetailsId
	LEFT JOIN CSTL_ERPDB_Unicorn..Table_LC_ChargeHeads CH ON LCD.ChargeHeadId = CH.ChargeHeadId

	--left join (select distinct ref.SubsidiaryInfoId,cust.CustomerCode+' : '+cust.CustomerName  CustomerName  from    tblSubsidiaryInfo ref  with (nolock) 
	  
	-- inner join SalesDisDB_Unicorn_DB..tblCustMaster cust  with (nolock)  on cust.CustomerMasterId=ref.S_Info_Id
	--  where ref.IsSelect=1 and ref.SubsidiaryTypeId=1
	--) tblSubCust on tblSubCust.SubsidiaryInfoId=dtl.SubsidoryLedgerId

	
		left join (select distinct ref.SubsidiaryInfoId, case when  ref.Description is null then  ISNULL(cust.CustomerCode+' : ','')+cust.CustomerName  else ref.Description end CustomerName   from    tblSubsidiaryInfo ref  with (nolock)  
	 left join  SalesDisDB_Unicorn_DB..tblCustMaster cust    with (nolock)  on cust.CustomerMasterId=ref.S_Info_Id
	  where ref.IsSelect=1 and ref.SubsidiaryTypeId=1
	) tblSubCust on tblSubCust.SubsidiaryInfoId=dtl.SubsidoryLedgerId
	left join (select distinct ref.SubsidiaryInfoId, case when  ref.Description is null then  ISNULL(sup.SupplierCode+' : ','')+sup.SupplierName  else ref.Description end SupplierName   from    tblSubsidiaryInfo ref  with (nolock)  
	 left join  tblSupplier sup  with (nolock)  on sup.SupplierId=ref.S_Info_Id
	  where ref.IsSelect=1 and ref.SubsidiaryTypeId=2
	) tblSubSup on tblSubSup.SubsidiaryInfoId=dtl.SubsidoryLedgerId

	
	left join (select distinct ref.SubsidiaryInfoId,emp.EmpMasterCode+' : '+emp.EmpName  EmpName  from    tblSubsidiaryInfo ref  with (nolock) 
	  
	 inner join SalesDisDB_Unicorn_DB..tblEmpGeneralInfo emp  with (nolock)  on emp.EmpInfoId=ref.S_Info_Id
	  
	  where ref.IsSelect=1 and ref.SubsidiaryTypeId=3
	) tblSubEmp on tblSubEmp.SubsidiaryInfoId=dtl.SubsidoryLedgerId

	
	--left join (select distinct ref.SubsidiaryInfoId,ref.Description  SubsidiaryDescription  from [dbo].tblSubsidiaryInfo ref with (nolock) 
	  
	--) tblSubDescription on tblSubDescription.SubsidiaryInfoId=dtl.SubsidoryLedgerId


	
	left join (select distinct ref.ReferenceInfoId,ref.Description  SubsidiaryDescriptiond  from [dbo].tblReferenceInfo ref with (nolock)  
	   
	) tblRefDescription on tblRefDescription.ReferenceInfoId=dtl.RefInfoInd
	 where VoucherMasterId=@VoucherMasterId

	 order by DebitorCredit desc

END
GO
