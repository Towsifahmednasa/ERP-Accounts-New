-- Delivery Sales Return: DeliverySalesReturn.aspx, tblCustomerReturnMaster -> sp_DeliverySalesReturnPosting
-- Voucher 1 (TableID 2018, "Sales Return from customer..."): Dr Sales Return, Cr Deduction Income, Cr Accounts Receivable
-- Voucher 2 (TableID 2023, "Delivery Sales Return, Inventory Valuation..."): Dr Finished Goods, Cr COGS (stock value > 0)
-- Params: @From DATE, @To DATE, @DocId INT (CustReturnId). This SP has no duplicate guard, so duplicates are reported too.
SET NOCOUNT ON;
DECLARE @GlRet  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1020), 4170),
        @GlDed  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1032), 5968),
        @GlAR   INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 2), 2112),
        @GlInv  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1016), 4156),
        @GlCogs INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1021), 4162);

;WITH R AS (
    SELECT CRM.CustReturnId, INV.InvoiceId, INV.InvoiceNo, CAST(CRM.EntryDate AS DATE) AS DocDate, INV.CustomerMasterId,
           ISNULL(CRM.TotalAmount,0) AS ExpAmt, ISNULL(CRM.DeductionCharge,0) AS Ded,
           ISNULL(CRM.NetReturnAmount, ISNULL(CRM.TotalAmount,0) - ISNULL(CRM.DeductionCharge,0)) AS NetHeader,
           (SELECT SUM(ISNULL(ReturnAmount,0)) FROM dbo.tblCustomerReturnDetail WITH (NOLOCK) WHERE CustReturnId = CRM.CustReturnId) AS DetailSum,
           ISNULL((SELECT SUM(ISNULL((CT.UnitPrice * RTND.ReturnQty) + (CT.VATPerUnit * RTND.ReturnQty), 0))
                   FROM dbo.tblCustomerReturnDetail RTND WITH (NOLOCK)
                   JOIN dbo.tblInvoiceDetail INVD WITH (NOLOCK) ON INVD.InvoiceDetailId = RTND.InvoiceDetailId
                   JOIN dbo.tblDCStore DCS WITH (NOLOCK) ON DCS.DCStoreId = INVD.DCStoreId
                   JOIN dbo.tblCentralStore CT WITH (NOLOCK) ON DCS.TempReceiveId = CT.ReceiveId
                   WHERE RTND.CustReturnId = CRM.CustReturnId),0) AS Cost
    FROM dbo.tblCustomerReturnMaster CRM WITH (NOLOCK)
    JOIN dbo.tblInvoice INV WITH (NOLOCK) ON INV.InvoiceId = CRM.InvoiceId
    WHERE (@DocId IS NULL OR CRM.CustReturnId = @DocId)
      AND (@From IS NULL OR CRM.EntryDate >= @From) AND (@To IS NULL OR CRM.EntryDate < DATEADD(DAY,1,@To))),
E AS (SELECT R.*, CASE WHEN R.NetHeader <= 0 AND R.ExpAmt > 0 THEN R.ExpAmt - R.Ded ELSE R.NetHeader END AS ExpNet FROM R),
F AS (
    SELECT E.*, V1.VoucherMasterId AS V1, V1.N AS V1Count, L1.Dr AS V1Dr, L1.Cr AS V1Cr, L1.DrRet, L1.CrDed, L1.CrAR, L1.ArSub, L1.DrRef,
           S.SubsidiaryTypeId AS SubType, S.S_Info_Id AS LedgerCust, RF.ReferenceTypeId AS RefType, RF.R_Info_Id AS RefDoc,
           V2.VoucherMasterId AS V2, V2.N AS V2Count, L2.Dr AS V2Dr, L2.Cr AS V2Cr, L2.DrInv, L2.CrCogs
    FROM E
    OUTER APPLY (SELECT TOP 1 VoucherMasterId,
                        (SELECT COUNT(*) FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                         WHERE TableID = 2018 AND CommonPK_ID = E.CustReturnId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Sales Return from customer%') AS N
                 FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2018 AND CommonPK_ID = E.CustReturnId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Sales Return from customer%' ORDER BY VoucherMasterId) V1
    OUTER APPLY (SELECT TOP 1 VoucherMasterId,
                        (SELECT COUNT(*) FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                         WHERE TableID = 2023 AND CommonPK_ID = E.CustReturnId AND Narration LIKE 'Delivery Sales Return, Inventory Valuation%') AS N
                 FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2023 AND CommonPK_ID = E.CustReturnId AND Narration LIKE 'Delivery Sales Return, Inventory Valuation%' ORDER BY VoucherMasterId) V2
    OUTER APPLY (SELECT SUM(ISNULL(DebitAmountBDT,0)) AS Dr, SUM(ISNULL(CreditAmountBDT,0)) AS Cr,
                        SUM(CASE WHEN DebitorCredit = 'Debit' AND AccountId = @GlRet THEN DebitAmountBDT ELSE 0 END) AS DrRet,
                        SUM(CASE WHEN DebitorCredit = 'Credit' AND AccountId = @GlDed THEN CreditAmountBDT ELSE 0 END) AS CrDed,
                        SUM(CASE WHEN DebitorCredit = 'Credit' AND AccountId = @GlAR THEN CreditAmountBDT ELSE 0 END) AS CrAR,
                        MAX(CASE WHEN DebitorCredit = 'Credit' AND AccountId = @GlAR THEN ISNULL(SubsidoryLedgerId,0) END) AS ArSub,
                        MAX(CASE WHEN DebitorCredit = 'Debit' AND AccountId = @GlRet THEN RefInfoInd END) AS DrRef
                 FROM {ACC}.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK) WHERE VoucherMasterId = V1.VoucherMasterId) L1
    OUTER APPLY (SELECT SUM(ISNULL(DebitAmountBDT,0)) AS Dr, SUM(ISNULL(CreditAmountBDT,0)) AS Cr,
                        SUM(CASE WHEN DebitorCredit = 'Debit' AND AccountId = @GlInv THEN DebitAmountBDT ELSE 0 END) AS DrInv,
                        SUM(CASE WHEN DebitorCredit = 'Credit' AND AccountId = @GlCogs THEN CreditAmountBDT ELSE 0 END) AS CrCogs
                 FROM {ACC}.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK) WHERE VoucherMasterId = V2.VoucherMasterId) L2
    LEFT JOIN {ACC}.dbo.tblSubsidiaryInfo S WITH (NOLOCK) ON S.SubsidiaryInfoId = L1.ArSub
    LEFT JOIN {ACC}.dbo.tblReferenceInfo RF WITH (NOLOCK) ON RF.ReferenceInfoId = L1.DrRef)
SELECT P.Problem, F.CustReturnId AS DocId, F.InvoiceId AS DocId2, CONCAT('RTN-', F.CustReturnId, ' / ', F.InvoiceNo) AS DocNo, F.DocDate, F.CustomerMasterId AS CustomerId,
       CAST(F.ExpAmt AS DECIMAL(18,2)) AS Expected, CAST(ISNULL(F.V1Dr,0) AS DECIMAL(18,2)) AS Actual, P.Detail
FROM F
CROSS APPLY (SELECT V.Problem, V.Detail FROM (VALUES
    ('NO_VOUCHER',            CASE WHEN F.ExpAmt > 0 AND F.V1 IS NULL THEN 1 ELSE 0 END, CONCAT('No sales-return voucher for ', F.ExpAmt)),
    ('DUPLICATE_VOUCHER',     CASE WHEN F.V1Count > 1 THEN 1 ELSE 0 END, CONCAT(F.V1Count, ' sales-return vouchers for one return')),
    ('NO_COST_VOUCHER',       CASE WHEN F.Cost > 0 AND F.V2 IS NULL THEN 1 ELSE 0 END, CONCAT('No inventory-valuation voucher for ', F.Cost)),
    ('DUPLICATE_COST',        CASE WHEN F.V2Count > 1 THEN 1 ELSE 0 END, CONCAT(F.V2Count, ' inventory-valuation vouchers for one return')),
    ('UNBALANCED',            CASE WHEN F.V1 IS NOT NULL AND F.V1Dr <> F.V1Cr THEN 1 ELSE 0 END, CONCAT('Voucher Dr ', F.V1Dr, ' <> Cr ', F.V1Cr)),
    ('AMOUNT_DR',             CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.DrRet,0) - F.ExpAmt) > 1 THEN 1 ELSE 0 END, CONCAT('Sales Return debit ', ISNULL(F.DrRet,0), ' <> ', F.ExpAmt)),
    ('AMOUNT_DEDUCTION',      CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.CrDed,0) - F.Ded) > 1 THEN 1 ELSE 0 END, CONCAT('Deduction credit ', ISNULL(F.CrDed,0), ' <> ', F.Ded)),
    ('AMOUNT_AR',             CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.CrAR,0) - F.ExpNet) > 1 THEN 1 ELSE 0 END, CONCAT('AR credit ', ISNULL(F.CrAR,0), ' <> net return ', F.ExpNet)),
    ('COST_AMOUNT',           CASE WHEN F.V2 IS NOT NULL AND (F.V2Dr <> F.V2Cr OR ABS(ISNULL(F.DrInv,0) - F.Cost) > 1 OR ABS(ISNULL(F.CrCogs,0) - F.Cost) > 1) THEN 1 ELSE 0 END, CONCAT('Valuation voucher Dr ', F.V2Dr, ' Cr ', F.V2Cr, ' vs recomputed ', F.Cost)),
    ('NET_PLUS_DEDUCTION',    CASE WHEN ABS(F.ExpNet + F.Ded - F.ExpAmt) > 1 THEN 1 ELSE 0 END, CONCAT('Net ', F.ExpNet, ' + deduction ', F.Ded, ' <> return amount ', F.ExpAmt)),
    ('HEADER_VS_DETAIL',      CASE WHEN F.DetailSum IS NOT NULL AND ABS(F.ExpAmt - F.DetailSum) > 1 THEN 1 ELSE 0 END, CONCAT('Return amount ', F.ExpAmt, ' <> detail total ', F.DetailSum)),
    ('NO_SUBSIDIARY',         CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.ArSub,0) = 0 THEN 1 ELSE 0 END, 'AR credit line has no customer subsidiary ledger'),
    ('WRONG_CUSTOMER_LEDGER', CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.ArSub,0) <> 0 AND (ISNULL(F.SubType,0) <> 1 OR F.LedgerCust <> F.CustomerMasterId) THEN 1 ELSE 0 END, CONCAT('AR ledger belongs to customer ', F.LedgerCust, ', invoice customer is ', F.CustomerMasterId)),
    ('REF_MISSING',           CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.DrRef,0) = 0 THEN 1 ELSE 0 END, 'Sales Return debit line carries no invoice reference'),
    ('REF_WRONG',             CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.DrRef,0) <> 0 AND (ISNULL(F.RefType,0) <> 1016 OR F.RefDoc <> F.InvoiceId) THEN 1 ELSE 0 END, 'Sales Return debit line points to another invoice''s reference')
) V(Problem, Hit, Detail) WHERE V.Hit = 1) P
ORDER BY F.DocDate DESC, F.CustReturnId DESC;
