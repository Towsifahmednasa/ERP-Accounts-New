namespace New_Accounts.DAL.AccountsIntegrity
{
    /// <summary>
    /// The SQL of every automatic fix. Each batch takes @DocId / @DocId2 (see the Detect_*.sql files) and returns one
    /// number: rows changed (or 1 for a posting SP). {ACC} = accounts DB, {ERP} = ERP master DB.
    /// Posting fixes call the same stored procedures the pages call; the other fixes only touch the voucher line column
    /// named in the fix (subsidiary ledger or reference) or the invoice delivery totals.
    /// </summary>
    internal static class AccountsIntegrityFixSql
    {
        public static string For(string fixCode, string areaKey)
        {
            switch (fixCode)
            {
                case "DELIVERY_POST":   return "EXEC dbo.sp_Sales_DeliverySalesPosting @InvoiceId = @DocId; SELECT 1;";
                case "PAYMENT_POST":    return "EXEC dbo.sp_Sales_CustomerPaymentPosting @CustPayId = @DocId, @InvoiceId = @DocId2; SELECT 1;";
                case "ADJ_POST":        return "EXEC dbo.sp_Sales_InvoiceAdjustmentPosting @BadDebtsId = @DocId; SELECT 1;";
                case "MARKET_POST":     return "EXEC dbo.sp_SalesReturnPosting @ReturnInvoiceId = @DocId; SELECT 1;";
                case "DRETURN_POST":    return "EXEC dbo.sp_DeliverySalesReturnPosting @ReturnId = @DocId; SELECT 1;";
                case "HEADER_SYNC":     return HeaderSync;
                case "AR_SUBSIDIARY":   return ArSubsidiaryHead(areaKey) + ArSubsidiaryTail;
                case "REF_FIX":         return RefHead(areaKey) + RefTail;
            }
            return null;
        }

        // The delivery header total includes the delivery charge; the voucher posts lines + charge.
        // DeliveryTpTotal is moved by the same difference so the two header fields keep their relation.
        private const string HeaderSync = """
UPDATE INV SET
    DeliveryTpGrandTotal = L.Net + ISNULL(INV.DeliveryCharge,0),
    DeliveryTpTotal = ISNULL(INV.DeliveryTpTotal,0) + (L.Net + ISNULL(INV.DeliveryCharge,0)) - ISNULL(INV.DeliveryTpGrandTotal,0)
FROM dbo.tblInvoice INV
CROSS APPLY (SELECT ISNULL(SUM(ISNULL(DeliveryNetAmount, ISNULL(TotalPrice,0))),0) AS Net FROM dbo.tblInvoiceDetail WHERE InvoiceId = INV.InvoiceId) L
WHERE INV.InvoiceId = @DocId;
SELECT @@ROWCOUNT;
""";

        // ---- AR subsidiary ledger: the head finds the voucher, the customer and the AR line, the tail sets the ledger ----
        private static string ArSubsidiaryHead(string areaKey)
        {
            switch (areaKey)
            {
                case "delivery": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2014 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 ORDER BY VoucherMasterId),
        @cust INT = (SELECT CustomerMasterId FROM dbo.tblInvoice WHERE InvoiceId = @DocId),
        @dc VARCHAR(10) = 'Debit',
        @gl INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 2), 4);
""";
                case "payment": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2017 AND CommonPK_ID = @DocId ORDER BY VoucherMasterId),
        @cust INT = (SELECT CustomerMasterId FROM dbo.tblInvoice WHERE InvoiceId = @DocId2),
        @dc VARCHAR(10) = 'Credit',
        @gl INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2);
""";
                case "adjustment": return """
DECLARE @v INT = (SELECT TOP 1 VM.VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster VM
                  WHERE VM.TableID = (SELECT TOP 1 IntegrationTableId FROM {ERP}.dbo.tbl_AccIntegrationTable WHERE LTRIM(RTRIM(UPPER(Table_Name))) = 'TBLINVOICEBADDEBTSMASTER')
                    AND VM.CommonPK_ID = @DocId AND VM.Narration LIKE '%adjusted against Invoice No.%' ORDER BY VM.VoucherMasterId),
        @cust INT = (SELECT CustomerMasterId FROM dbo.tblInvoice WHERE InvoiceId = @DocId2),
        @dc VARCHAR(10) = 'Credit',
        @gl INT = (SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2);
""";
                case "market": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2018 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Market Sales Return%' ORDER BY VoucherMasterId),
        @cust INT = (SELECT CustomerMasterId FROM dbo.tblReturnInvoice WHERE ReturnInvoiceId = @DocId),
        @dc VARCHAR(10) = 'Credit',
        @gl INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2), 2112);
""";
                case "dreturn": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2018 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Sales Return from customer%' ORDER BY VoucherMasterId),
        @cust INT = (SELECT CustomerMasterId FROM dbo.tblInvoice WHERE InvoiceId = @DocId2),
        @dc VARCHAR(10) = 'Credit',
        @gl INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 2), 2112);
""";
            }
            return "DECLARE @v INT = NULL, @cust INT = NULL, @dc VARCHAR(10) = '', @gl INT = NULL;";
        }

        // Same insert the posting SPs do when a customer has no subsidiary record yet.
        private const string ArSubsidiaryTail = """

IF @v IS NULL OR @cust IS NULL OR @gl IS NULL BEGIN SELECT -1; RETURN; END
DECLARE @sub BIGINT = (SELECT TOP 1 SubsidiaryInfoId FROM {ACC}.dbo.tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1 AND S_Info_Id = @cust ORDER BY SubsidiaryInfoId);
IF @sub IS NULL
BEGIN
    INSERT INTO {ACC}.dbo.tblSubsidiaryInfo (SubsidiaryTypeId, S_Info_Id, IsSelect, Description, EntryBy, EntryDate)
    SELECT 1, CustomerMasterId, 1, ISNULL(CustomerName, 'Customer ' + CAST(CustomerMasterId AS VARCHAR(20))), 1, GETDATE() FROM dbo.tblCustMaster WHERE CustomerMasterId = @cust;
    SET @sub = SCOPE_IDENTITY();
END
IF @sub IS NULL BEGIN SELECT -1; RETURN; END
UPDATE D SET SubsidoryLedgerId = @sub
FROM {ACC}.dbo.tblDebitCreditVoucherDetail D
WHERE D.VoucherMasterId = @v AND D.DebitorCredit = @dc AND D.AccountId = @gl;
SELECT @@ROWCOUNT;
""";

        // ---- reference: the head sets @v, @refType, @refDoc, @refDesc and @mode (which lines carry the reference) ----
        // mode 1 = credit lines, 2 = debit lines, 3 = debit lines and credit lines other than AR, 4 = every line
        private static string RefHead(string areaKey)
        {
            switch (areaKey)
            {
                case "delivery": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2014 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 ORDER BY VoucherMasterId),
        @refType INT = 1016, @refDoc INT = @DocId, @mode INT = 1, @glAR INT = 0,
        @refDesc NVARCHAR(200) = 'Invoice ' + ISNULL((SELECT InvoiceNo FROM dbo.tblInvoice WHERE InvoiceId = @DocId), '');
""";
                case "payment": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2017 AND CommonPK_ID = @DocId ORDER BY VoucherMasterId),
        @refType INT = 1016, @refDoc INT = @DocId2, @mode INT = 2, @glAR INT = 0,
        @refDesc NVARCHAR(200) = 'Invoice ' + ISNULL((SELECT InvoiceNo FROM dbo.tblInvoice WHERE InvoiceId = @DocId2), '');
""";
                case "adjustment": return """
DECLARE @v INT = (SELECT TOP 1 VM.VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster VM
                  WHERE VM.TableID = (SELECT TOP 1 IntegrationTableId FROM {ERP}.dbo.tbl_AccIntegrationTable WHERE LTRIM(RTRIM(UPPER(Table_Name))) = 'TBLINVOICEBADDEBTSMASTER')
                    AND VM.CommonPK_ID = @DocId AND VM.Narration LIKE '%adjusted against Invoice No.%' ORDER BY VM.VoucherMasterId),
        @refType INT = 1016, @refDoc INT = @DocId2, @mode INT = 1, @glAR INT = 0,
        @refDesc NVARCHAR(200) = 'Invoice ' + ISNULL((SELECT InvoiceNo FROM dbo.tblInvoice WHERE InvoiceId = @DocId2), '');
""";
                case "market": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2018 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Market Sales Return%' ORDER BY VoucherMasterId),
        @refType INT = 1018, @refDoc INT = @DocId, @mode INT = 3,
        @glAR INT = ISNULL((SELECT TOP 1 GLId FROM {ERP}.dbo.Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2), 2112),
        @refDesc NVARCHAR(200) = 'Return Invoice ' + ISNULL((SELECT ReturnInvoiceNo FROM dbo.tblReturnInvoice WHERE ReturnInvoiceId = @DocId), '');
""";
                case "dreturn": return """
DECLARE @v INT = (SELECT TOP 1 VoucherMasterId FROM {ACC}.dbo.tblDebitCreditVoucherMaster WHERE TableID = 2018 AND CommonPK_ID = @DocId AND ISNULL(IsInventoryValuation,0) = 0 AND Narration LIKE 'Sales Return from customer%' ORDER BY VoucherMasterId),
        @refType INT = 1016, @refDoc INT = @DocId2, @mode INT = 4, @glAR INT = 0,
        @refDesc NVARCHAR(200) = 'Invoice ' + ISNULL((SELECT InvoiceNo FROM dbo.tblInvoice WHERE InvoiceId = @DocId2), '');
""";
            }
            return "DECLARE @v INT = NULL, @refType INT = 0, @refDoc INT = 0, @mode INT = 0, @glAR INT = 0, @refDesc NVARCHAR(200) = '';";
        }

        private const string RefTail = """

IF @v IS NULL OR @refDoc IS NULL OR @refDoc = 0 BEGIN SELECT -1; RETURN; END
DECLARE @ref BIGINT = (SELECT TOP 1 ReferenceInfoId FROM {ACC}.dbo.tblReferenceInfo WHERE ReferenceTypeId = @refType AND R_Info_Id = @refDoc ORDER BY ReferenceInfoId);
IF @ref IS NULL
BEGIN
    INSERT INTO {ACC}.dbo.tblReferenceInfo (ReferenceTypeId, R_Info_Id, Description, EntryBy, EntryDate) VALUES (@refType, @refDoc, @refDesc, 1, GETDATE());
    SET @ref = SCOPE_IDENTITY();
END
UPDATE D SET RefInfoInd = @ref
FROM {ACC}.dbo.tblDebitCreditVoucherDetail D
WHERE D.VoucherMasterId = @v
  AND (@mode = 4 OR (@mode = 1 AND D.DebitorCredit = 'Credit') OR (@mode = 2 AND D.DebitorCredit = 'Debit')
       OR (@mode = 3 AND (D.DebitorCredit = 'Debit' OR D.AccountId <> @glAR)));
SELECT @@ROWCOUNT;
""";
    }
}
