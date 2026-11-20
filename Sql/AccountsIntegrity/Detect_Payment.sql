-- Customer Payment -> sp_Sales_CustomerPaymentPosting (TableID 2017, CommonPK_ID = CustPayId, one voucher per detail row)
-- Params: @From DATE, @To DATE, @DocId INT (CustPayId). Line-level checks only for payments with exactly 1 detail row and 1 voucher.
SET NOCOUNT ON;
DECLARE @GlDisc INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1023),
        @GlAIT  INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1024),
        @GlAR   INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2);

;WITH E AS (
    SELECT CD.CustPayId, COUNT(*) AS DetailRows, SUM(ISNULL(CD.PaymentAmount,0)) AS Pay, SUM(ISNULL(CD.Discount,0)) AS Disc, SUM(ISNULL(CD.AIT,0)) AS AIT,
           MIN(CD.InvoiceId) AS InvoiceId, MIN(ISNULL(CD.BankAccId,0)) AS BankAccId, MIN(ISNULL(CD.CashAccId,0)) AS CashAccId
    FROM dbo.tblCustPayDetail CD WITH (NOLOCK)
    WHERE ISNULL(CD.PaymentAmount,0) + ISNULL(CD.Discount,0) + ISNULL(CD.AIT,0) > 0 AND (@DocId IS NULL OR CD.CustPayId = @DocId)
    GROUP BY CD.CustPayId),
F AS (
    SELECT E.*, P.PaymentDate, P.CustomerMasterId AS PayCustomer, INV.InvoiceNo, INV.CustomerMasterId AS InvCustomer,
           VM.Vouchers, VM.VMaster, VL.Lines, VL.Dr, VL.Cr, VL.BankCashAmt, VL.DiscAmt, VL.AitAmt, VL.ArCr, VL.ArSub, VL.DrNoRef, VL.DrBadRef,
           SI.SubsidiaryTypeId AS SubType, SI.S_Info_Id AS LedgerCust,
           CAST(CASE WHEN EXISTS (SELECT 1 FROM {ACC}.dbo.tblReferenceInfo R WITH (NOLOCK) WHERE R.ReferenceTypeId = 1016 AND R.R_Info_Id = E.InvoiceId) THEN 1 ELSE 0 END AS BIT) AS RefExists,
           CAST(CASE WHEN E.DetailRows = 1 AND VM.Vouchers = 1 THEN 1 ELSE 0 END AS BIT) AS Single
    FROM E
    JOIN dbo.tblCustomerPay P WITH (NOLOCK) ON P.CustPayId = E.CustPayId
    LEFT JOIN dbo.tblInvoice INV WITH (NOLOCK) ON INV.InvoiceId = E.InvoiceId
    OUTER APPLY (SELECT COUNT(*) AS Vouchers, SUM(ISNULL(TotalAmountBDT,0)) AS VMaster
                 FROM {ACC}.dbo.tblDebitCreditVoucherMaster WITH (NOLOCK) WHERE TableID = 2017 AND CommonPK_ID = E.CustPayId) VM
    OUTER APPLY (SELECT COUNT(D.VoucherMasterId) AS Lines, SUM(ISNULL(D.DebitAmountBDT,0)) AS Dr, SUM(ISNULL(D.CreditAmountBDT,0)) AS Cr,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND COA.AccountId IS NOT NULL THEN D.DebitAmountBDT ELSE 0 END) AS BankCashAmt,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND D.AccountId = @GlDisc THEN D.DebitAmountBDT ELSE 0 END) AS DiscAmt,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND D.AccountId = @GlAIT THEN D.DebitAmountBDT ELSE 0 END) AS AitAmt,
                        SUM(CASE WHEN D.DebitorCredit = 'Credit' AND D.AccountId = @GlAR THEN D.CreditAmountBDT ELSE 0 END) AS ArCr,
                        MAX(CASE WHEN D.DebitorCredit = 'Credit' AND D.AccountId = @GlAR THEN ISNULL(D.SubsidoryLedgerId,0) END) AS ArSub,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND ISNULL(D.RefInfoInd,0) = 0 THEN 1 ELSE 0 END) AS DrNoRef,
                        SUM(CASE WHEN D.DebitorCredit = 'Debit' AND ISNULL(D.RefInfoInd,0) <> 0 AND R.ReferenceInfoId IS NULL THEN 1 ELSE 0 END) AS DrBadRef
                 FROM {ACC}.dbo.tblDebitCreditVoucherMaster VM2 WITH (NOLOCK)
                 LEFT JOIN {ACC}.dbo.tblDebitCreditVoucherDetail D WITH (NOLOCK) ON D.VoucherMasterId = VM2.VoucherMasterId
                 LEFT JOIN {ACC}.dbo.tblChartOfAccounts COA WITH (NOLOCK) ON COA.AccountId = D.AccountId
                      AND ((E.BankAccId <> 0 AND COA.BankAccountId = E.BankAccId) OR (E.BankAccId = 0 AND COA.CashAccountId = E.CashAccId))
                 LEFT JOIN {ACC}.dbo.tblReferenceInfo R WITH (NOLOCK) ON R.ReferenceInfoId = D.RefInfoInd AND R.ReferenceTypeId = 1016 AND R.R_Info_Id = E.InvoiceId
                 WHERE VM2.TableID = 2017 AND VM2.CommonPK_ID = E.CustPayId) VL
    LEFT JOIN {ACC}.dbo.tblSubsidiaryInfo SI WITH (NOLOCK) ON SI.SubsidiaryInfoId = VL.ArSub
    WHERE (@From IS NULL OR P.PaymentDate >= @From) AND (@To IS NULL OR P.PaymentDate < DATEADD(DAY,1,@To)))
SELECT P.Problem, F.CustPayId AS DocId, CASE WHEN F.DetailRows = 1 THEN F.InvoiceId ELSE 0 END AS DocId2,
       CONCAT('PAY-', F.CustPayId, CASE WHEN F.DetailRows = 1 THEN CONCAT(' / ', F.InvoiceNo) ELSE CONCAT(' / ', F.DetailRows, ' invoices') END) AS DocNo,
       CAST(F.PaymentDate AS DATE) AS DocDate, CASE WHEN F.DetailRows = 1 THEN F.InvCustomer ELSE F.PayCustomer END AS CustomerId,
       CAST(F.Pay + F.Disc + F.AIT AS DECIMAL(18,2)) AS Expected, CAST(ISNULL(F.Dr,0) AS DECIMAL(18,2)) AS Actual, P.Detail
FROM F
CROSS APPLY (SELECT V.Problem, V.Detail FROM (VALUES
    ('NO_VOUCHER',            CASE WHEN F.Vouchers = 0 THEN 1 ELSE 0 END, CONCAT('No receipt voucher (TableID 2017) for ', F.Pay + F.Disc + F.AIT)),
    ('NO_DETAIL_LINES',       CASE WHEN F.Vouchers > 0 AND F.Lines = 0 THEN 1 ELSE 0 END, 'Voucher header has no lines'),
    ('UNBALANCED',            CASE WHEN F.Vouchers > 0 AND F.Lines > 0 AND F.Dr <> F.Cr THEN 1 ELSE 0 END, CONCAT('Voucher Dr ', F.Dr, ' <> Cr ', F.Cr)),
    ('VOUCHER_COUNT_DIFFERS', CASE WHEN F.Vouchers > 0 AND F.Vouchers <> F.DetailRows THEN 1 ELSE 0 END, CONCAT(F.Vouchers, ' voucher(s) for ', F.DetailRows, ' payment row(s)')),
    ('AMOUNT_MISMATCH',       CASE WHEN F.Vouchers > 0 AND F.Lines > 0 AND (ABS(F.Pay + F.Disc + F.AIT - F.Dr) > 1 OR ABS(F.Pay + F.Disc + F.AIT - F.VMaster) > 1) THEN 1 ELSE 0 END, CONCAT('Payment ', F.Pay + F.Disc + F.AIT, ' / voucher Dr ', F.Dr, ' / voucher header ', F.VMaster)),
    ('BANKCASH_LINE',         CASE WHEN F.Single = 1 AND F.Pay > 0 AND ISNULL(F.BankCashAmt,0) <> F.Pay THEN 1 ELSE 0 END, CONCAT('Cash/Bank GL debit ', ISNULL(F.BankCashAmt,0), ' <> payment ', F.Pay, ' (wrong account or amount)')),
    ('DISCOUNT_LINE',         CASE WHEN F.Single = 1 AND F.Disc > 0 AND ISNULL(F.DiscAmt,0) <> F.Disc THEN 1 ELSE 0 END, CONCAT('Discount GL debit ', ISNULL(F.DiscAmt,0), ' <> discount ', F.Disc)),
    ('AIT_LINE',              CASE WHEN F.Single = 1 AND F.AIT > 0 AND ISNULL(F.AitAmt,0) <> F.AIT THEN 1 ELSE 0 END, CONCAT('AIT GL debit ', ISNULL(F.AitAmt,0), ' <> AIT ', F.AIT)),
    ('AR_AMOUNT',             CASE WHEN F.Single = 1 AND ISNULL(F.ArCr,0) <> F.Pay + F.Disc + F.AIT THEN 1 ELSE 0 END, CONCAT('AR credit ', ISNULL(F.ArCr,0), ' <> ', F.Pay + F.Disc + F.AIT)),
    ('NO_SUBSIDIARY',         CASE WHEN F.Single = 1 AND F.ArCr > 0 AND ISNULL(F.ArSub,0) = 0 THEN 1 ELSE 0 END, 'AR credit line has no customer subsidiary ledger'),
    ('WRONG_CUSTOMER_LEDGER', CASE WHEN F.Single = 1 AND ISNULL(F.ArSub,0) <> 0 AND (ISNULL(F.SubType,0) <> 1 OR F.LedgerCust <> F.InvCustomer) THEN 1 ELSE 0 END, CONCAT('AR ledger belongs to customer ', F.LedgerCust, ', invoice customer is ', F.InvCustomer)),
    ('REF_MISSING',           CASE WHEN F.Single = 1 AND F.RefExists = 1 AND F.DrNoRef > 0 THEN 1 ELSE 0 END, 'Debit lines carry no invoice reference'),
    ('REF_WRONG',             CASE WHEN F.Single = 1 AND F.DrBadRef > 0 THEN 1 ELSE 0 END, 'A debit line points to another invoice''s reference')
) V(Problem, Hit, Detail) WHERE V.Hit = 1) P
ORDER BY F.PaymentDate DESC, F.CustPayId DESC;
