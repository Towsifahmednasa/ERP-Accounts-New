-- Park feature (Journal voucher draft): adds @ActionStatus to the two voucher-master SPs.
-- Default 'Posted' (insert) / NULL->'Posted' (update) keeps every existing caller's behaviour
-- (BankPayments, CreditVoucher, DebitVoucher pass no @ActionStatus). Only JournalDAL passes 'Parked'.
-- Target DB: CSTL_ACCDB_Unicorn. Applied 2026-10-09.
-- The approval queue (AllApprovalController.BankAccountList) filters ActionStatus='Posted',
-- so parked vouchers never reach approval until re-submitted.

ALTER PROCEDURE [dbo].[sp_I_VoucherMaster]
	(
		@VoucherMasterId int OUT,
		@VoucherCode NVARCHAR(MAX)=NULL,
		          @CompanyInfoId INT=NULL,
		          @FinancialYearId INT=NULL,
		          @PostingDate DATETIME=NULL,
		          @VoucherNo NVARCHAR(MAX)=NULL,
		          @VoucherCategoryID INT =NULL,
		          @CurrencyId INT=NULL,
		          @CurrencyCovRate DECIMAL(18,4)=NULL,
		          @Narration NVARCHAR(MAX)=NULL,
		          @IsCostCenter BIT=NULL,
		          @IsBuyerCostCenter BIT=NULL,
		          @IsOtherCostCenter BIT=NULL,
		          @ReceiveFromIsBuyer BIT=NULL,
		          @ByerId INT=NULL,
		          @PaytoAndReceiveFromIsOther BIT=NULL,
		          @Other NVARCHAR(MAX)=NULL,
		          @OtherAdress NVARCHAR(MAX)=NULL,
		          @PaytoIsSupplier BIT=NULL,
		          @SupplierId INT=NULL,
		          @IsInvoice BIT=NULL,
		          @TotalAmountBDT DECIMAL(18,4)=NULL,
		          @TotalAmount DECIMAL(18,4)=NULL,
		          @Remarks NVARCHAR(MAX)=NULL,
		          @EntryBy NVARCHAR(MAX)=NULL,
		          @EntryDate DATETIME=NULL,
		          @UpdateBy NVARCHAR(MAX)=NULL,
		          @UpdateDate DATETIME=NULL,
		          @VoucherType NVARCHAR(MAX)=NULL,
		          @BankCheckNo NVARCHAR(MAX)=NULL,
		          @BankCheckDate DATETIME=NULL,
		          @CashRefNo NVARCHAR(MAX)=NULL,
		          @CashRefDate DATETIME=NULL,
		          @IsExportInvoice BIT=NULL,
		          @IsImportInvoice BIT=NULL,
				    @isManualVCRNo BIT=NULL,

					@BankAccountId  int=NULL
           ,@ChequeNo   NVARCHAR(MAX)=NULL
           ,@ChequeDate date
           ,@CashAccountId  int=NULL
           ,@RefNo NVARCHAR(MAX)=NULL
           ,@RefDate date
           ,@ActionStatus NVARCHAR(50)='Posted'   -- Park feature 2026-10-09
	)
AS
BEGIN
DECLARE @CheckCount int = 0

	-- SELECT @CheckCount=COUNT(*) from tblDebitCreditVoucherMaster where VoucherNo=@VoucherNo  AND VoucherType = @VoucherType  AND CompanyInfoId = @CompanyInfoId


	  if	(@CheckCount=0  )
	  BEGIN

    DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@EntryDate)), 3, 3)
	DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@EntryDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@EntryDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@EntryDate)) END)
	DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@EntryDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@EntryDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@EntryDate)) END)
    SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM tblDebitCreditVoucherMaster
	WHERE @EntryDate=@EntryDate

		IF(@VoucherType='DebitVoucherBank')
		BEGIN
			SET @VoucherCode='DVB-'+@VoucherCode
		END
		IF(@VoucherType='DebitVoucherCash')
		BEGIN
			SET @VoucherCode='DVC-'+@VoucherCode
		END
		IF(@VoucherType='CreditVoucherBank')
		BEGIN
			SET @VoucherCode='CVB-'+@VoucherCode
		END
		IF(@VoucherType='CreditVoucherCash')
		BEGIN
			SET @VoucherCode='CVC-'+@VoucherCode
		END
		IF(@VoucherType='FundTransfer')
		BEGIN
			SET @VoucherCode='FUT-'+@VoucherCode
		END

		IF(@VoucherType='Journal')
		BEGIN
			SET @VoucherCode='JVP-'+@VoucherCode
		END
		INSERT INTO dbo.tblDebitCreditVoucherMaster
		        ( VoucherCode ,
		          CompanyInfoId ,
		          FinancialYearId ,
		          PostingDate ,
		          VoucherNo ,
		          VoucherCategoryID ,
		          CurrencyId ,
		          CurrencyCovRate ,
		          Narration ,
		          IsCostCenter ,
		          IsBuyerCostCenter ,
		          IsOtherCostCenter ,
		          ReceiveFromIsBuyer ,
		          ByerId ,
		          PaytoAndReceiveFromIsOther ,
		          Other ,
		          OtherAdress ,
		          PaytoIsSupplier ,
		          SupplierId ,
		          IsInvoice ,
		          TotalAmountBDT ,
		          TotalAmount ,
		          Remarks ,
		          EntryBy ,
		          EntryDate ,
		          UpdateBy ,
		          UpdateDate ,
		          VoucherType ,
		          BankCheckNo ,
		          BankCheckDate ,
		          CashRefNo ,
		          CashRefDate ,
		          IsExportInvoice,
		          IsImportInvoice,
		          IsReconciliation,ActionStatus,[BankAccountId]
           ,[ChequeNo]
           ,[ChequeDate]
           ,[CashAccountId]
           ,[RefNo]
           ,[RefDate]
		        )
		VALUES  ( @VoucherCode ,
		          @CompanyInfoId ,
		          @FinancialYearId ,
		          @PostingDate ,
		          @VoucherNo ,
		          @VoucherCategoryID ,
		          @CurrencyId ,
		          @CurrencyCovRate ,
		          @Narration ,
		          @IsCostCenter ,
		          @IsBuyerCostCenter ,
		          @IsOtherCostCenter ,
		          @ReceiveFromIsBuyer ,
		          @ByerId ,
		          @PaytoAndReceiveFromIsOther ,
		          @Other ,
		          @OtherAdress ,
		          @PaytoIsSupplier ,
		          @SupplierId ,
		          @IsInvoice ,
		          @TotalAmount ,
		          @TotalAmount ,
		          @Remarks ,
		          @EntryBy ,
		          @EntryDate ,
		          @UpdateBy ,
		          @UpdateDate ,
		          @VoucherType ,
		          @BankCheckNo ,
		          @BankCheckDate ,
		          @CashRefNo ,
		          @CashRefDate,
		          @IsExportInvoice,
		          @IsImportInvoice
		          ,'False',ISNULL(@ActionStatus,'Posted') ,@BankAccountId   -- Park feature 2026-10-09 (was hard-coded 'Posted')
           ,@ChequeNo
           ,@ChequeDate
           ,@CashAccountId
           ,@RefNo
           ,@RefDate
		        )



		      declare @pk int=0


		  set @pk= SCOPE_IDENTITY()
		  	  declare @tpeId int=0
		  if(@VoucherType='DebitVoucherBank' or @VoucherType='DebitVoucherCash')
		  begin

		  if(@VoucherCategoryID=1)
		  begin
		set  @tpeId=@BankAccountId
		  end
		  else
		  begin
		set  @tpeId=@CashAccountId
		  end



		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @pk ,
		          @tpeId ,
		          'Credit' ,
		          0 ,
		          @TotalAmount ,
		          0 ,
		          0

           ,'from System'
		        )
				end


				 if(@VoucherType='CreditVoucherBank' or @VoucherType='CreditVoucherCash')
		  begin

		  if(@VoucherCategoryID=1)
		  begin
		set  @tpeId=@BankAccountId
		  end
		  else
		  begin
		set  @tpeId=@CashAccountId
		  end



		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @pk ,
		          @tpeId ,
		          'Debit' ,
		          @TotalAmount ,0
		           ,
		          0 ,
		          0

           ,'from System'
		        )
				end


				  if(@VoucherType='FundTransfer')
		  begin

		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @pk ,
		          @BankAccountId ,
		          'Credit' ,
		          0 ,
		          @TotalAmount ,
		          0 ,
		          0

           ,''
		        )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @pk ,
		          @CashAccountId ,
		          'Debit' ,
		          @TotalAmount ,
		          0 ,
		          0 ,
		          0

           ,''
		        )
		  end
				select  @pk
END
END
GO

ALTER PROCEDURE [dbo].[sp_UD_VoucherMaster_New]
		@VoucherMasterId int OUT,
		@VoucherCode NVARCHAR(MAX)=NULL,
		          @CompanyInfoId INT=NULL,
		          @FinancialYearId INT=NULL,
		          @PostingDate DATETIME=NULL,
		          @VoucherNo NVARCHAR(MAX)=NULL,
		          @VoucherCategoryID INT =NULL,
		          @CurrencyId INT=NULL,
		          @CurrencyCovRate DECIMAL(18,2)=NULL,
		          @Narration NVARCHAR(MAX)=NULL,
		          @IsCostCenter BIT=NULL,
		          @IsBuyerCostCenter BIT=NULL,
		          @IsOtherCostCenter BIT=NULL,
		          @ReceiveFromIsBuyer BIT=NULL,
		          @ByerId INT=NULL,
		          @PaytoAndReceiveFromIsOther BIT=NULL,
		          @Other NVARCHAR(MAX)=NULL,
		          @OtherAdress NVARCHAR(MAX)=NULL,
		          @PaytoIsSupplier BIT=NULL,
		          @SupplierId INT=NULL,
		          @IsInvoice BIT=NULL,
		          @TotalAmountBDT DECIMAL(18,4)=NULL,
		          @TotalAmount DECIMAL(18,4)=NULL,
		          @Remarks NVARCHAR(MAX)=NULL,
		          @EntryBy NVARCHAR(MAX)=NULL,
		          @EntryDate DATETIME=NULL,
		          @UpdateBy NVARCHAR(MAX)=NULL,
		          @UpdateDate DATETIME=NULL,
		          @VoucherType NVARCHAR(MAX)=NULL,
		          @BankCheckNo NVARCHAR(MAX)=NULL,
		          @BankCheckDate DATETIME=NULL,
		          @CashRefNo NVARCHAR(MAX)=NULL,
		          @CashRefDate DATETIME=NULL,
		          @IsExportInvoice BIT=NULL,
		          @IsImportInvoice BIT=NULL,
				    @isManualVCRNo BIT=NULL,

					@BankAccountId  int=NULL
           ,@ChequeNo   NVARCHAR(MAX)=NULL
           ,@ChequeDate date
           ,@CashAccountId  int=NULL
           ,@RefNo NVARCHAR(MAX)=NULL
           ,@RefDate date
           ,@ActionStatus NVARCHAR(50)=NULL   -- Park feature 2026-10-09; NULL keeps legacy behaviour ('Posted')
AS
BEGIN
	UPDATE [dbo].[tblDebitCreditVoucherMaster]
   SET  [CompanyInfoId] = @CompanyInfoId
      ,[FinancialYearId] = @FinancialYearId
      ,[PostingDate] = @PostingDate

      ,[VoucherCategoryID] = @VoucherCategoryID
      ,[CurrencyId] = @CurrencyId
      ,[CurrencyCovRate] = @CurrencyCovRate
      ,[Narration] = @Narration
      ,[IsCostCenter] = @IsCostCenter
      ,[IsBuyerCostCenter] = @IsBuyerCostCenter
      ,[IsOtherCostCenter] = @IsOtherCostCenter
      ,[ReceiveFromIsBuyer] = @ReceiveFromIsBuyer
      ,[ByerId] = @ByerId
      ,[PaytoAndReceiveFromIsOther] = @PaytoAndReceiveFromIsOther
      ,[Other] = @Other
      ,[OtherAdress] = @OtherAdress
      ,[PaytoIsSupplier] = @PaytoIsSupplier
      ,[SupplierId] = @SupplierId
      ,[IsInvoice] = @IsInvoice
      ,[TotalAmount] = @TotalAmount
      ,[TotalAmountBDT] = @TotalAmount
      ,[Remarks] = @Remarks

      ,[UpdateBy] = @EntryBy
      ,[UpdateDate] =GETDATE()
      ,[VoucherType] = @VoucherType
      ,[BankCheckNo] = @ChequeNo
      ,[BankCheckDate] = @ChequeDate
      ,[CashRefNo] = @RefNo
      ,[CashRefDate] = @RefDate ,
     VoucherNo=@VoucherNo
      ,[IsExportInvoice] = @IsExportInvoice
      ,[IsImportInvoice] = @IsImportInvoice ,  BankAccountId=@BankAccountId,  CashAccountId=@CashAccountId, ActionStatus=ISNULL(@ActionStatus,'Posted')   -- Park feature 2026-10-09 (was hard-coded 'Posted')
       WHERE VoucherMasterId=@VoucherMasterId

	     declare @tpeId int=0


		  delete from tblDebitCreditVoucherDetail  WHERE VoucherMasterId=@VoucherMasterId

		  if(@VoucherType='DebitVoucherBank' or @VoucherType='DebitVoucherCash')
		  begin

		  if(@VoucherCategoryID=1)
		  begin
		set  @tpeId=@BankAccountId
		  end
		  else
		  begin
		set  @tpeId=@CashAccountId
		  end



		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @VoucherMasterId ,
		          @tpeId ,
		          'Credit' ,
		          0 ,
		          @TotalAmount ,
		          0 ,
		          0

           ,'from System'
		        )
				end


				 if(@VoucherType='CreditVoucherBank' or @VoucherType='CreditVoucherCash')
		  begin

		  if(@VoucherCategoryID=1)
		  begin
		set  @tpeId=@BankAccountId
		  end
		  else
		  begin
		set  @tpeId=@CashAccountId
		  end



		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @VoucherMasterId ,
		          @tpeId ,
		          'Debit' ,
		          @TotalAmount ,0
		           ,
		          0 ,
		          0

           ,'from System'
		        )
				end


				  if(@VoucherType='FundTransfer')
		  begin

		INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @VoucherMasterId ,
		          @BankAccountId ,
		          'Credit' ,
		          0 ,
		          @TotalAmount ,
		          0 ,
		          0

           ,''
		        )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId ,
		          AccountId ,
		          DebitorCredit,
		          DebitAmountBDT ,
		          CreditAmountBDT ,
		          DebitAmount ,
		          CreditAmount
           ,[Remarks]
		        )
		VALUES  ( @VoucherMasterId ,
		          @CashAccountId ,
		          'Debit' ,
		          @TotalAmount ,
		          0 ,
		          0 ,
		          0

           ,''
		        )
		  end

END
GO
