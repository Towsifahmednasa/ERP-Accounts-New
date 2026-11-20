-- Invoice Adjustment -> sp_Sales_InvoiceAdjustmentPosting (Dr Bad Debt / Promotion Expense, Cr Accounts Receivable)
-- Params: @From DATE, @To DATE, @DocId INT (BadDebtsId). The integration TableID is shared with LC Charges Booking,
-- so adjustment vouchers are recognised by their narration.
SET NOCOUNT ON;
DECLARE @Tbl INT = (SELECT TOP 1 IntegrationTableId FROM {ERP}.dbo.tbl_AccIntegrationTable WHERE LTRIM(RTRIM(UPPER(Table_Name))) = 'TBLINVOICEBADDEBTSMASTER');
DECLARE @GlAR INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2);

;WITH F AS (
    SELECT M.BadDebtsId, M.InvoiceId, M.InvoiceNo, M.AdjustmentType, CAST(M.AdjustmentDate AS DATE) AS DocDate, ISNULL(M.TotalAdjustment,0) AS Amt,
           (SELECT SUM(AdjustmentAmount) FROM dbo.tblInvoiceBadDebtsDetails WITH (NOLOCK) WHERE BadDebtsId = M.BadDebtsId) AS DetailSum,
           I.CustomerMasterId,
           V.Vouchers, V.Dr, V.Cr,
           (SELECT COUNT(*) FROM {ACC}.dbo.tblDebitCreditVoucherMaster X WITH (NOLOCK)
             WHERE X.TableID = @Tbl AND X.CommonPK_ID = M.BadDebtsId AND X.Narration NOT LIKE '%adjusted against Invoice No.%') AS OtherOnKey,
           CASE WHEN V.VoucherMasterId IS NULL THEN NULL WHEN EXISTS (SELECT 1 FROM {ACC}.dbo.tblDebitCreditVoucherDetail D WITH (NOLOCK)
                WHERE D.VoucherMasterId = V.VoucherMasterId AND D.DebitorCredit = 'Debit'
                  AND D.AccountId = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity
                                     WHERE PageName = 'Invoice Adjustment' AND FlagId = CASE WHEN M.AdjustmentType = 'Promotional Expense' THEN 2 ELSE 1 END)) THEN 1 ELSE 0 END AS DrGlOk,
           AR.SubsidoryLedgerId AS ArSub, AR.RefInfoInd, AR.AnyAr,
           S.SubsidiaryTypeId AS SubType, S.S_Info_Id AS LedgerCust, R.ReferenceTypeId AS RefType, R.R_Info_Id AS RefDoc
    FROM dbo.tblInvoiceBadDebtsMaster M WITH (NOLOCK)
    LEFT JOIN dbo.tblInvoice I WITH (NOLOCK) ON I.InvoiceId = M.InvoiceId
    OUTER APPLY (SELECT COUNT(*) AS Vouchers, MIN(VM.VoucherMasterId) AS VoucherMasterId, SUM(ISNULL(DD.Dr,0)) AS Dr, SUM(ISNULL(DD.Cr,0)) AS Cr
                 FROM {ACC}.dbo.tblDebitCreditVoucherMaster VM WITH (NOLOCK)
                 OUTER APPLY (SELECT SUM(ISNULL(DebitAmountBDT,0)) AS Dr, SUM(ISNULL(CreditAmountBDT,0)) AS Cr
                              FROM {ACC}.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK) WHERE VoucherMasterId = VM.VoucherMasterId) DD
                 WHERE VM.TableID = @Tbl AND VM.CommonPK_ID = M.BadDebtsId AND VM.Narration LIKE '%adjusted against Invoice No.%') V
    OUTER APPLY (SELECT TOP 1 1 AS AnyAr, D.SubsidoryLedgerId, D.RefInfoInd FROM {ACC}.dbo.tblDebitCreditVoucherDetail D WITH (NOLOCK)
                 WHERE D.VoucherMasterId = V.VoucherMasterId AND D.DebitorCredit = 'Credit' AND D.AccountId = @GlAR) AR
    LEFT JOIN {ACC}.dbo.tblSubsidiaryInfo S WITH (NOLOCK) ON S.SubsidiaryInfoId = AR.SubsidoryLedgerId
    LEFT JOIN {ACC}.dbo.tblReferenceInfo R WITH (NOLOCK) ON R.ReferenceInfoId = AR.RefInfoInd
    WHERE (@DocId IS NULL OR M.BadDebtsId = @DocId)
      AND (@From IS NULL OR M.AdjustmentDate >= @From) AND (@To IS NULL OR M.AdjustmentDate < DATEADD(DAY,1,@To)))
SELECT P.Problem, F.BadDebtsId AS DocId, F.InvoiceId AS DocId2, CONCAT('ADJ-', F.BadDebtsId, ' / ', F.InvoiceNo) AS DocNo, F.DocDate, F.CustomerMasterId AS CustomerId,
       CAST(F.Amt AS DECIMAL(18,2)) AS Expected, CAST(ISNULL(F.Dr,0) AS DECIMAL(18,2)) AS Actual, P.Detail
FROM F
CROSS APPLY (SELECT V.Problem, V.Detail FROM (VALUES
    ('NO_VOUCHER',            CASE WHEN F.Vouchers = 0 AND F.Amt > 0 AND F.OtherOnKey = 0 THEN 1 ELSE 0 END, CONCAT(F.AdjustmentType, ' of ', F.Amt, ' has no voucher')),
    ('NO_VOUCHER_BLOCKED',    CASE WHEN F.Vouchers = 0 AND F.Amt > 0 AND F.OtherOnKey > 0 THEN 1 ELSE 0 END, 'No voucher, and another document (LC Charges) already holds this TableID + id, so the SP would skip it. Fix the SP guard first.'),
    ('DUPLICATE_VOUCHER',     CASE WHEN F.Vouchers > 1 THEN 1 ELSE 0 END, CONCAT(F.Vouchers, ' vouchers for one adjustment')),
    ('UNBALANCED',            CASE WHEN F.Vouchers > 0 AND F.Dr <> F.Cr THEN 1 ELSE 0 END, CONCAT('Voucher Dr ', F.Dr, ' <> Cr ', F.Cr)),
    ('AMOUNT_MISMATCH',       CASE WHEN F.Vouchers > 0 AND (ABS(F.Dr - F.Amt) > 1 OR ABS(F.Cr - F.Amt) > 1) THEN 1 ELSE 0 END, CONCAT('Adjustment ', F.Amt, ' / voucher Dr ', F.Dr, ' Cr ', F.Cr)),
    ('HEADER_VS_DETAIL',      CASE WHEN ABS(ISNULL(F.DetailSum,0) - F.Amt) > 1 THEN 1 ELSE 0 END, CONCAT('Header ', F.Amt, ' <> detail total ', ISNULL(F.DetailSum,0))),
    ('DR_GL_WRONG',           CASE WHEN F.Vouchers > 0 AND ISNULL(F.DrGlOk,1) = 0 THEN 1 ELSE 0 END, 'No debit line on the Bad Debt / Promotion Expense GL for this adjustment type'),
    ('NO_AR_LINE',            CASE WHEN F.Vouchers > 0 AND F.AnyAr IS NULL THEN 1 ELSE 0 END, 'No credit line on the Accounts Receivable GL'),
    ('NO_SUBSIDIARY',         CASE WHEN F.Vouchers > 0 AND F.AnyAr = 1 AND ISNULL(F.ArSub,0) = 0 THEN 1 ELSE 0 END, 'AR credit line has no customer subsidiary ledger'),
    ('WRONG_CUSTOMER_LEDGER', CASE WHEN F.Vouchers > 0 AND ISNULL(F.ArSub,0) <> 0 AND (ISNULL(F.SubType,0) <> 1 OR F.LedgerCust <> F.CustomerMasterId) THEN 1 ELSE 0 END, CONCAT('AR ledger belongs to customer ', F.LedgerCust, ', invoice customer is ', F.CustomerMasterId)),
    ('REF_MISSING',           CASE WHEN F.Vouchers > 0 AND F.AnyAr = 1 AND ISNULL(F.RefInfoInd,0) = 0 THEN 1 ELSE 0 END, 'AR credit line carries no invoice reference'),
    ('REF_WRONG',             CASE WHEN F.Vouchers > 0 AND ISNULL(F.RefInfoInd,0) <> 0 AND (ISNULL(F.RefType,0) <> 1016 OR F.RefDoc <> F.InvoiceId) THEN 1 ELSE 0 END, 'AR credit line points to another invoice''s reference')
) V(Problem, Hit, Detail) WHERE V.Hit = 1) P
ORDER BY F.DocDate DESC, F.BadDebtsId DESC;
