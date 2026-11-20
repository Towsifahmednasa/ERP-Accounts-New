-- Delivery Confirmation (LoadingSummary "Sales Confirm" + Auto_Loading partial) -> sp_Sales_DeliverySalesPosting
-- Params: @From DATE, @To DATE, @DocId INT (InvoiceId). {ACC} = accounts DB, {ERP} = ERP master DB.
SET NOCOUNT ON;
DECLARE @GlAR INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 2), 4);

;WITH F AS (
    SELECT INV.InvoiceId, INV.InvoiceNo, CAST(INV.InvoiceDate AS DATE) AS DocDate, INV.CustomerMasterId,
           ISNULL(INV.DeliveryTpGrandTotal,0) AS Header, ISNULL(INV.DeliveryCharge,0) AS Charge,
           ISNULL((SELECT SUM(ISNULL(D.DeliveryNetAmount, ISNULL(D.TotalPrice,0))) FROM dbo.tblInvoiceDetail D WITH (NOLOCK) WHERE D.InvoiceId = INV.InvoiceId),0) AS Net,
           S.VoucherMasterId AS SV, SL.Dr AS SDr, SL.Cr AS SCr, SL.ArLines, SL.ArSub, SL.CrRefCnt, SL.CrRefBad,
           C.VoucherMasterId AS CV, CL.Dr AS CDr, CL.Cr AS CCr,
           SI.SubsidiaryTypeId AS SubType, SI.S_Info_Id AS LedgerCust,
           CAST(CASE WHEN S.VoucherMasterId IS NOT NULL THEN 0 WHEN EXISTS (SELECT 1 FROM {ACC}.dbo.tblDebitCreditVoucherMaster X WITH (NOLOCK)
                                  WHERE X.Narration LIKE 'Sale to %under invoice ' + INV.InvoiceNo + ' for BDT%') THEN 1 ELSE 0 END AS BIT) AS OtherKey
    FROM dbo.tblInvoice INV WITH (NOLOCK)
    OUTER APPLY (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2014 AND CommonPK_ID = INV.InvoiceId AND ISNULL(IsInventoryValuation,0) = 0 ORDER BY VoucherMasterId) S
    OUTER APPLY (SELECT SUM(ISNULL(D.DebitAmountBDT,0)) AS Dr, SUM(ISNULL(D.CreditAmountBDT,0)) AS Cr,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND D.AccountId = @GlAR THEN 1 ELSE 0 END) AS ArLines,
                        MAX(CASE WHEN D.DebitorCredit = 'Debit' AND D.AccountId = @GlAR THEN ISNULL(D.SubsidoryLedgerId,0) END) AS ArSub,
                        SUM(CASE WHEN D.DebitorCredit = 'Credit' AND ISNULL(D.RefInfoInd,0) <> 0 THEN 1 ELSE 0 END) AS CrRefCnt,
                        SUM(CASE WHEN D.DebitorCredit = 'Credit' AND ISNULL(D.RefInfoInd,0) <> 0 AND R.ReferenceInfoId IS NULL THEN 1 ELSE 0 END) AS CrRefBad
                 FROM {ACC}.dbo.tblDebitCreditVoucherDetail D WITH (NOLOCK)
                 LEFT JOIN {ACC}.dbo.tblReferenceInfo R WITH (NOLOCK) ON R.ReferenceInfoId = D.RefInfoInd AND R.ReferenceTypeId = 1016 AND R.R_Info_Id = INV.InvoiceId
                 WHERE D.VoucherMasterId = S.VoucherMasterId) SL
    OUTER APPLY (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK)
                 WHERE TableID = 2014 AND CommonPK_ID = INV.InvoiceId AND ISNULL(IsInventoryValuation,0) = 1 ORDER BY VoucherMasterId) C
    OUTER APPLY (SELECT SUM(ISNULL(DebitAmountBDT,0)) AS Dr, SUM(ISNULL(CreditAmountBDT,0)) AS Cr
                 FROM {ACC}.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK) WHERE VoucherMasterId = C.VoucherMasterId) CL
    LEFT JOIN {ACC}.dbo.tblSubsidiaryInfo SI WITH (NOLOCK) ON SI.SubsidiaryInfoId = SL.ArSub
    WHERE INV.DelivaryInvoiceNo IS NOT NULL AND ISNULL(INV.DeliveryTpGrandTotal,0) > 0
      AND (@DocId IS NULL OR INV.InvoiceId = @DocId)
      AND (@From IS NULL OR INV.InvoiceDate >= @From) AND (@To IS NULL OR INV.InvoiceDate < DATEADD(DAY,1,@To)))
SELECT P.Problem, F.InvoiceId AS DocId, 0 AS DocId2, F.InvoiceNo AS DocNo, F.DocDate, F.CustomerMasterId AS CustomerId,
       CAST(F.Net + F.Charge AS DECIMAL(18,2)) AS Expected, CAST(ISNULL(F.SDr,0) AS DECIMAL(18,2)) AS Actual, P.Detail
FROM F
CROSS APPLY (SELECT V.Problem, V.Detail FROM (VALUES
    ('NO_SALES_VOUCHER',        CASE WHEN F.SV IS NULL AND F.OtherKey = 0 THEN 1 ELSE 0 END, CONCAT('No sales voucher (TableID 2014) for delivered amount ', F.Net + F.Charge)),
    ('SALES_VOUCHER_OTHER_KEY', CASE WHEN F.SV IS NULL AND F.OtherKey = 1 THEN 1 ELSE 0 END, 'A sales voucher for this invoice exists under another TableID / key'),
    ('NO_COGS_VOUCHER',         CASE WHEN F.SV IS NOT NULL AND F.CV IS NULL THEN 1 ELSE 0 END, 'Sales voucher exists but the COGS voucher is missing'),
    ('SALES_UNBALANCED',        CASE WHEN F.SV IS NOT NULL AND F.SDr <> F.SCr THEN 1 ELSE 0 END, CONCAT('Sales voucher Dr ', F.SDr, ' <> Cr ', F.SCr)),
    ('COGS_UNBALANCED',         CASE WHEN F.CV IS NOT NULL AND F.CDr <> F.CCr THEN 1 ELSE 0 END, CONCAT('COGS voucher Dr ', F.CDr, ' <> Cr ', F.CCr)),
    ('HEADER_STALE',            CASE WHEN F.SV IS NOT NULL AND ABS(F.SDr - (F.Net + F.Charge)) <= 1 AND ABS(F.Header - (F.Net + F.Charge)) > 1 THEN 1 ELSE 0 END, CONCAT('Invoice header ', F.Header, ' but lines+charge ', F.Net + F.Charge, ' and voucher ', F.SDr, ' agree')),
    ('VOUCHER_STALE',           CASE WHEN F.SV IS NOT NULL AND ABS(F.Header - (F.Net + F.Charge)) <= 1 AND ABS(F.SDr - (F.Net + F.Charge)) > 1 THEN 1 ELSE 0 END, CONCAT('Voucher ', F.SDr, ' but invoice header ', F.Header, ' and lines+charge ', F.Net + F.Charge, ' agree (accounts adjustment needed)')),
    ('AMOUNT_UNRELATED',        CASE WHEN F.SV IS NOT NULL AND ABS(F.Header - (F.Net + F.Charge)) > 1 AND ABS(F.SDr - (F.Net + F.Charge)) > 1 THEN 1 ELSE 0 END, CONCAT('Header ', F.Header, ' / lines+charge ', F.Net + F.Charge, ' / voucher ', F.SDr, ' all differ')),
    ('NO_AR_LINE',              CASE WHEN F.SV IS NOT NULL AND F.ArLines = 0 THEN 1 ELSE 0 END, 'No Debit line on the Accounts Receivable GL'),
    ('NO_SUBSIDIARY',           CASE WHEN F.SV IS NOT NULL AND F.ArLines > 0 AND ISNULL(F.ArSub,0) = 0 THEN 1 ELSE 0 END, 'AR line has no customer subsidiary ledger'),
    ('WRONG_CUSTOMER_LEDGER',   CASE WHEN F.SV IS NOT NULL AND ISNULL(F.ArSub,0) <> 0 AND (ISNULL(F.SubType,0) <> 1 OR F.LedgerCust <> F.CustomerMasterId) THEN 1 ELSE 0 END, CONCAT('AR ledger belongs to customer ', F.LedgerCust, ', invoice customer is ', F.CustomerMasterId)),
    ('REF_MISSING',             CASE WHEN F.SV IS NOT NULL AND F.CrRefCnt = 0 THEN 1 ELSE 0 END, 'Credit lines carry no invoice reference'),
    ('REF_WRONG',               CASE WHEN F.SV IS NOT NULL AND F.CrRefBad > 0 THEN 1 ELSE 0 END, 'A credit line points to another invoice''s reference')
) V(Problem, Hit, Detail) WHERE V.Hit = 1) P
ORDER BY F.DocDate DESC, F.InvoiceId DESC;
