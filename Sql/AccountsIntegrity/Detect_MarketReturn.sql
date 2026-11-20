-- Non-Transactional (Market) Sales Return: SalesReturn.aspx, tblReturnInvoice -> sp_SalesReturnPosting
-- Voucher 1 (TableID 2018, "Market Sales Return..."): Dr Sales Return, Cr Deduction Income, Cr Accounts Receivable
-- Voucher 2 (TableID 2023, "Cost reversal..."): Dr Finished Goods, Cr COGS
-- Params: @From DATE, @To DATE, @DocId INT (ReturnInvoiceId).
SET NOCOUNT ON;
DECLARE @GlRet  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1020), 4170),
        @GlDed  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1032), 5968),
        @GlAR   INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2), 2112),
        @GlInv  INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1018), 4156),
        @GlCogs INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1019), 4162);

;WITH R AS (
    SELECT RI.ReturnInvoiceId, RI.ReturnInvoiceNo, CAST(RI.ReturnInvoiceDate AS DATE) AS DocDate, RI.CustomerMasterId,
           ISNULL(RI.DeductionCharge,0) AS Ded, ISNULL(RI.NetReturnAmount,0) AS NetHeader,
           (SELECT SUM(ISNULL(NetAmount,TotalPrice)) FROM dbo.tblReturnInvoiceDetail WITH (NOLOCK) WHERE ReturnInvoiceId = RI.ReturnInvoiceId) AS DetailSum,
           ISNULL(RI.TpGrandTotal, ISNULL(RI.TpTotal,0)) AS HeaderAmt,
           ISNULL((SELECT SUM(ISNULL(RIND.TotalQuantity, RIND.Quantity) * ISNULL(NULLIF(RIND.CostPrice,0), ISNULL(DCS.CostPerUnit,0)))
                   FROM dbo.tblReturnInvoiceDetail RIND WITH (NOLOCK)
                   OUTER APPLY (SELECT TOP 1 CostPerUnit FROM dbo.tblDCStore WITH (NOLOCK) WHERE ProductCode = RIND.ProductCode AND CostPerUnit > 0 ORDER BY DCStoreId DESC) DCS
                   WHERE RIND.ReturnInvoiceId = RI.ReturnInvoiceId),0) AS Cost
    FROM dbo.tblReturnInvoice RI WITH (NOLOCK)
    WHERE (@DocId IS NULL OR RI.ReturnInvoiceId = @DocId)
      AND (@From IS NULL OR RI.ReturnInvoiceDate >= @From) AND (@To IS NULL OR RI.ReturnInvoiceDate < DATEADD(DAY,1,@To))),
E AS (SELECT R.*, CASE WHEN R.HeaderAmt > 0 THEN R.HeaderAmt ELSE ISNULL(R.DetailSum,0) END AS ExpAmt FROM R),
E2 AS (SELECT E.*, CASE WHEN E.NetHeader > 0 THEN E.NetHeader WHEN E.ExpAmt - E.Ded > 0 THEN E.ExpAmt - E.Ded ELSE 0 END AS ExpNet FROM E),
F AS (
    SELECT E2.*, V1.VoucherMasterId AS V1, L1.Dr AS V1Dr, L1.Cr AS V1Cr, L1.DrRet, L1.CrDed, L1.CrAR, L1.ArSub, L1.DrRef,
           S.SubsidiaryTypeId AS SubType, S.S_Info_Id AS LedgerCust, RF.ReferenceTypeId AS RefType, RF.R_Info_Id AS RefDoc,
           V2.VoucherMasterId AS V2, L2.Dr AS V2Dr, L2.Cr AS V2Cr, L2.DrInv, L2.CrCogs,
           (SELECT COUNT(*) FROM {ACC}.dbo.tblDebitCreditVoucherMaster X WITH (NOLOCK)
             WHERE X.TableID = 2018 AND X.CommonPK_ID = E2.ReturnInvoiceId AND ISNULL(X.IsInventoryValuation,0) = 0 AND X.Narration NOT LIKE 'Market Sales Return%') AS OtherOnKey
    FROM E2
    OUTER APPLY (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2018 AND CommonPK_ID = E2.ReturnInvoiceId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Market Sales Return%' ORDER BY VoucherMasterId) V1
    OUTER APPLY (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2023 AND CommonPK_ID = E2.ReturnInvoiceId AND IsInventoryValuation = 1 AND Narration LIKE 'Cost reversal%' ORDER BY VoucherMasterId) V2
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
SELECT P.Problem, F.ReturnInvoiceId AS DocId, 0 AS DocId2, F.ReturnInvoiceNo AS DocNo, F.DocDate, F.CustomerMasterId AS CustomerId,
       CAST(F.ExpAmt AS DECIMAL(18,2)) AS Expected, CAST(ISNULL(F.V1Dr,0) AS DECIMAL(18,2)) AS Actual, P.Detail
FROM F
CROSS APPLY (SELECT V.Problem, V.Detail FROM (VALUES
    ('NO_VOUCHER',            CASE WHEN F.ExpAmt > 0 AND F.V1 IS NULL THEN 1 ELSE 0 END, CONCAT('No sales-return voucher for ', F.ExpAmt)),
    ('KEY_COLLISION',         CASE WHEN F.V1 IS NULL AND F.OtherOnKey > 0 THEN 1 ELSE 0 END, 'A delivery-return voucher already uses this TableID + id, so the SP skipped this return'),
    ('NO_COST_VOUCHER',       CASE WHEN F.Cost > 0 AND F.V2 IS NULL THEN 1 ELSE 0 END, CONCAT('No cost-reversal voucher for ', F.Cost)),
    ('UNBALANCED',            CASE WHEN F.V1 IS NOT NULL AND F.V1Dr <> F.V1Cr THEN 1 ELSE 0 END, CONCAT('Voucher Dr ', F.V1Dr, ' <> Cr ', F.V1Cr)),
    ('AMOUNT_DR',             CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.DrRet,0) - F.ExpAmt) > 1 THEN 1 ELSE 0 END, CONCAT('Sales Return debit ', ISNULL(F.DrRet,0), ' <> ', F.ExpAmt)),
    ('AMOUNT_DEDUCTION',      CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.CrDed,0) - F.Ded) > 1 THEN 1 ELSE 0 END, CONCAT('Deduction credit ', ISNULL(F.CrDed,0), ' <> ', F.Ded)),
    ('AMOUNT_AR',             CASE WHEN F.V1 IS NOT NULL AND ABS(ISNULL(F.CrAR,0) - F.ExpNet) > 1 THEN 1 ELSE 0 END, CONCAT('AR credit ', ISNULL(F.CrAR,0), ' <> net return ', F.ExpNet)),
    ('COST_AMOUNT',           CASE WHEN F.V2 IS NOT NULL AND (F.V2Dr <> F.V2Cr OR ABS(ISNULL(F.DrInv,0) - F.Cost) > 1 OR ABS(ISNULL(F.CrCogs,0) - F.Cost) > 1) THEN 1 ELSE 0 END, CONCAT('Cost voucher Dr ', F.V2Dr, ' Cr ', F.V2Cr, ' vs recomputed cost ', F.Cost, ' (cost may have changed since posting)')),
    ('NET_PLUS_DEDUCTION',    CASE WHEN ABS(F.ExpNet + F.Ded - F.ExpAmt) > 1 THEN 1 ELSE 0 END, CONCAT('Net ', F.ExpNet, ' + deduction ', F.Ded, ' <> return amount ', F.ExpAmt)),
    ('HEADER_VS_DETAIL',      CASE WHEN F.DetailSum IS NOT NULL AND ABS(F.ExpAmt - F.DetailSum) > 1 THEN 1 ELSE 0 END, CONCAT('Return amount ', F.ExpAmt, ' <> detail total ', F.DetailSum)),
    ('NO_SUBSIDIARY',         CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.ArSub,0) = 0 THEN 1 ELSE 0 END, 'AR credit line has no customer subsidiary ledger'),
    ('WRONG_CUSTOMER_LEDGER', CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.ArSub,0) <> 0 AND (ISNULL(F.SubType,0) <> 1 OR F.LedgerCust <> F.CustomerMasterId) THEN 1 ELSE 0 END, CONCAT('AR ledger belongs to customer ', F.LedgerCust, ', return customer is ', F.CustomerMasterId)),
    ('REF_MISSING',           CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.DrRef,0) = 0 THEN 1 ELSE 0 END, 'Sales Return debit line carries no return reference'),
    ('REF_WRONG',             CASE WHEN F.V1 IS NOT NULL AND ISNULL(F.DrRef,0) <> 0 AND (ISNULL(F.RefType,0) <> 1018 OR F.RefDoc <> F.ReturnInvoiceId) THEN 1 ELSE 0 END, 'Sales Return debit line points to another document''s reference')
) V(Problem, Hit, Detail) WHERE V.Hit = 1) P
ORDER BY F.DocDate DESC, F.ReturnInvoiceId DESC;
