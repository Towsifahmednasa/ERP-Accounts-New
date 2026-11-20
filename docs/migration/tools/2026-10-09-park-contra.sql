-- Park feature for Contra (fund transfer) vouchers: @ActionStatus on the two Contra SPs
-- (default keeps legacy behaviour 'Posted'), and ActionStatus added to the Contra get-by-id SP
-- so the entry page can show the parked state. Target DB: CSTL_ACCDB_Unicorn. Applied 2026-10-09.

ALTER PROCEDURE [dbo].[sp_I_VoucherMaster_Contra]
(
    @VoucherMasterId INT OUT,

    -- Optional / Unused in current C# code
    @VoucherCode NVARCHAR(MAX) = NULL,
    @UpdateBy NVARCHAR(MAX) = NULL,
    @UpdateDate DATETIME = NULL,
    @RefNo NVARCHAR(MAX) = NULL,
    @RefDate DATE = NULL,

    -- From C# code
    @CompanyInfoId INT = NULL,
    @FinancialYearId INT = NULL,
    @PostingDate DATETIME = NULL,
    @VoucherNo NVARCHAR(MAX) = NULL,
    @CurrencyId INT = NULL,
    @CurrencyCovRate DECIMAL(18, 4) = NULL,
    @VoucherType NVARCHAR(MAX) = NULL,

    @CashAccountId INT = NULL,
    @BankAccountId INT = NULL,

    @ChequeNo NVARCHAR(MAX) = NULL,
    @ChequeDate DATETIME = NULL,
    @Narration NVARCHAR(MAX) = NULL,
    @CashRefNo NVARCHAR(MAX) = NULL,
    @CashRefDate DATETIME = NULL,
    @TotalAmount DECIMAL(18, 2) = NULL,
	@VoucherCategoryID int,

    @EntryBy NVARCHAR(MAX) = NULL ,
	 @EntryDate DATETIME = NULL
   ,@ActionStatus NVARCHAR(50) = 'Posted'   -- Park feature 2026-10-09
) as
BEGIN
DECLARE @CheckCount int = 0

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

		          TotalAmountBDT ,
		          TotalAmount ,
		          Remarks ,
		          EntryBy ,
		          EntryDate ,

		          VoucherType ,
		          BankCheckNo ,
		          BankCheckDate ,
		          CashRefNo ,
		          CashRefDate ,
		         ActionStatus,
            [ChequeNo]
           ,[ChequeDate]

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

		          @TotalAmount ,
		          @TotalAmount ,
		          '' ,
		          @EntryBy ,
		          @EntryDate ,

		          @VoucherType ,
		          @ChequeNo ,
		          @ChequeDate ,
		          @CashRefNo ,
		          @CashRefDate,
		            ISNULL(@ActionStatus,'Posted')   -- Park feature 2026-10-09 (was hard-coded 'Posted')
           ,@ChequeNo
           ,@ChequeDate

           ,@RefNo
           ,@RefDate
		        )



		      declare @pk int=0


		  set @pk= SCOPE_IDENTITY()



				 		  if(@VoucherType='FundTransfer')
		  begin

		  if(@VoucherCategoryID=1)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @BankAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @CashAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


	  if(@VoucherCategoryID=2)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @CashAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @BankAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


		    if(@VoucherCategoryID=3)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @CashAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @BankAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


	  if(@VoucherCategoryID=4)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @BankAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @pk , @CashAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end

		  end
				select  @pk
END
END
GO

ALTER PROCEDURE [dbo].[sp_UD_VoucherMaster_New_Contra]
		 @VoucherMasterId INT OUT,

    -- Optional / Unused in current C# code
    @VoucherCode NVARCHAR(MAX) = NULL,
    @UpdateBy NVARCHAR(MAX) = NULL,
    @UpdateDate DATETIME = NULL,
    @RefNo NVARCHAR(MAX) = NULL,
    @RefDate DATE = NULL,

    -- From C# code
    @CompanyInfoId INT = NULL,
    @FinancialYearId INT = NULL,
    @PostingDate DATETIME = NULL,
    @VoucherNo NVARCHAR(MAX) = NULL,
    @CurrencyId INT = NULL,
    @CurrencyCovRate DECIMAL(18, 4) = NULL,
    @VoucherType NVARCHAR(MAX) = NULL,

    @CashAccountId INT = NULL,
    @BankAccountId INT = NULL,

    @ChequeNo NVARCHAR(MAX) = NULL,
    @ChequeDate DATETIME = NULL,
    @Narration NVARCHAR(MAX) = NULL,
    @CashRefNo NVARCHAR(MAX) = NULL,
    @CashRefDate DATETIME = NULL,
    @TotalAmount DECIMAL(18, 2) = NULL,
	@VoucherCategoryID int,

    @EntryBy NVARCHAR(MAX) = NULL ,
	 @EntryDate DATETIME = NULL
   ,@ActionStatus NVARCHAR(50) = NULL   -- Park feature 2026-10-09; NULL keeps legacy behaviour ('Posted')
AS
BEGIN
	UPDATE [dbo].[tblDebitCreditVoucherMaster]
   SET  [CompanyInfoId] = @CompanyInfoId
      ,[FinancialYearId] = @FinancialYearId
      ,[PostingDate] = @PostingDate ,
       VoucherNo=@VoucherNo
      ,[VoucherCategoryID] = @VoucherCategoryID
      ,[CurrencyId] = @CurrencyId
      ,[CurrencyCovRate] = @CurrencyCovRate
      ,[Narration] = @Narration


      ,[TotalAmount] = @TotalAmount
      ,[TotalAmountBDT] = @TotalAmount


      ,[UpdateBy] = @EntryBy
      ,[UpdateDate] =GETDATE()
      ,[VoucherType] = @VoucherType
      ,[BankCheckNo] = @ChequeNo
      ,[BankCheckDate] = @ChequeDate
      ,[CashRefNo] = @RefNo
      ,[CashRefDate] = @RefDate ,

      BankAccountId=@BankAccountId,  CashAccountId=@CashAccountId, ActionStatus=ISNULL(@ActionStatus,'Posted')   -- Park feature 2026-10-09 (was hard-coded 'Posted')
       WHERE VoucherMasterId=@VoucherMasterId



		  delete from tblDebitCreditVoucherDetail  WHERE VoucherMasterId=@VoucherMasterId

				 		  if(@VoucherType='FundTransfer')
		  begin

		  if(@VoucherCategoryID=1)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @BankAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @CashAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


	  if(@VoucherCategoryID=2)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @CashAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @BankAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


		    if(@VoucherCategoryID=3)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @CashAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @BankAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end


	  if(@VoucherCategoryID=4)
		  begin

		  	INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @BankAccountId , 'Credit' , 0 , @TotalAmount , 0 , 0 ,'' )

				INSERT INTO dbo.tblDebitCreditVoucherDetail
		        ( VoucherMasterId , AccountId , DebitorCredit, DebitAmountBDT , CreditAmountBDT , DebitAmount , CreditAmount ,[Remarks] )
		VALUES  ( @VoucherMasterId , @CashAccountId , 'Debit' , @TotalAmount , 0 , 0 , 0 ,'' )

		  end

		  end

END
GO

-- ActionStatus added so the Contra entry page can show the parked state on edit.
ALTER PROCEDURE [dbo].[sp_GET_BankPaymentMasterByIdContra]
	(
		@id int

	)
AS
BEGIN

	 SELECT
        mas.VoucherMasterId,
        mas.CompanyInfoId,
        mas.FinancialYearId,
        mas.PostingDate,
        mas.VoucherNo,
        mas.CurrencyId,
        mas.CurrencyCovRate,
        mas.VoucherType,
		mas.VoucherCategoryID,
        mas.ActionStatus,   -- Park feature 2026-10-09

        dtl.AccountId AS CashAccountId,
        dtl2.AccountId AS BankAccountId,

        mas.ChequeNo,
        mas.BankCheckDate AS ChequeDate,
        mas.Narration,
        mas.CashRefNo,
        mas.CashRefDate,
        mas.TotalAmount
    FROM tblDebitCreditVoucherMaster mas
    INNER JOIN tblDebitCreditVoucherDetail dtl
        ON mas.VoucherMasterId = dtl.VoucherMasterId
        AND dtl.DebitorCredit = 'Debit'
        AND dtl.Remarks = ''
    INNER JOIN tblDebitCreditVoucherDetail dtl2
        ON mas.VoucherMasterId = dtl2.VoucherMasterId
        AND dtl2.DebitorCredit = 'Credit'
        AND dtl2.Remarks = ''
    WHERE mas.VoucherMasterId =@id
END
GO
