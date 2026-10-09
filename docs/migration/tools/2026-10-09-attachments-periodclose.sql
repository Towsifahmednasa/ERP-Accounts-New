-- Voucher attachments + FI period close (wireframe 81: "Attachments" card, "period closed" hard stop).
-- Target DB: CSTL_ACCDB_Unicorn. Applied 2026-10-09. Idempotent.
-- Files live in the DB (varbinary) so backups carry them and no share/folder rights are needed.
-- ponytail: varbinary storage, move to a file share if attachments outgrow the DB.

IF OBJECT_ID('dbo.tblVoucherAttachment') IS NULL
CREATE TABLE dbo.tblVoucherAttachment (
    AttachmentId    INT IDENTITY(1,1) PRIMARY KEY,
    VoucherMasterId INT            NOT NULL,
    [FileName]      NVARCHAR(255)  NOT NULL,
    ContentType     NVARCHAR(100)  NULL,
    FileSize        BIGINT         NOT NULL,
    FileData        VARBINARY(MAX) NOT NULL,
    EntryBy         NVARCHAR(100)  NULL,
    EntryDate       DATETIME       NOT NULL DEFAULT GETDATE()
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tblVoucherAttachment_Master')
CREATE INDEX IX_tblVoucherAttachment_Master ON dbo.tblVoucherAttachment (VoucherMasterId);
GO

CREATE OR ALTER PROCEDURE dbo.sp_I_VoucherAttachment
    @VoucherMasterId INT, @FileName NVARCHAR(255), @ContentType NVARCHAR(100)=NULL,
    @FileSize BIGINT, @FileData VARBINARY(MAX), @EntryBy NVARCHAR(100)=NULL
AS
BEGIN
    INSERT INTO dbo.tblVoucherAttachment (VoucherMasterId,[FileName],ContentType,FileSize,FileData,EntryBy)
    VALUES (@VoucherMasterId,@FileName,@ContentType,@FileSize,@FileData,@EntryBy);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_GET_VoucherAttachmentsByMasterId
    @VoucherMasterId INT
AS
    SELECT AttachmentId,[FileName],ContentType,FileSize,EntryBy,EntryDate
    FROM dbo.tblVoucherAttachment WHERE VoucherMasterId=@VoucherMasterId ORDER BY AttachmentId;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GET_VoucherAttachmentById
    @AttachmentId INT
AS
    SELECT AttachmentId,VoucherMasterId,[FileName],ContentType,FileSize,FileData
    FROM dbo.tblVoucherAttachment WHERE AttachmentId=@AttachmentId;
GO

CREATE OR ALTER PROCEDURE dbo.sp_D_VoucherAttachment
    @AttachmentId INT
AS
    DELETE FROM dbo.tblVoucherAttachment WHERE AttachmentId=@AttachmentId;
GO

-- ---- FI period close: a row with IsClosed=1 blocks POSTING (not parking) in that company+month ----
IF OBJECT_ID('dbo.tblFiPeriodClose') IS NULL
CREATE TABLE dbo.tblFiPeriodClose (
    PeriodCloseId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyInfoId INT NOT NULL,
    PeriodYear    INT NOT NULL,
    PeriodMonth   INT NOT NULL,
    IsClosed      BIT NOT NULL DEFAULT 0,
    UpdateBy      NVARCHAR(100) NULL,
    UpdateDate    DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_tblFiPeriodClose UNIQUE (CompanyInfoId, PeriodYear, PeriodMonth)
);
GO

CREATE OR ALTER PROCEDURE dbo.sp_SET_FiPeriodClose
    @CompanyInfoId INT, @PeriodYear INT, @PeriodMonth INT, @IsClosed BIT, @UpdateBy NVARCHAR(100)=NULL
AS
BEGIN
    UPDATE dbo.tblFiPeriodClose SET IsClosed=@IsClosed, UpdateBy=@UpdateBy, UpdateDate=GETDATE()
    WHERE CompanyInfoId=@CompanyInfoId AND PeriodYear=@PeriodYear AND PeriodMonth=@PeriodMonth;
    IF @@ROWCOUNT=0
        INSERT INTO dbo.tblFiPeriodClose (CompanyInfoId,PeriodYear,PeriodMonth,IsClosed,UpdateBy)
        VALUES (@CompanyInfoId,@PeriodYear,@PeriodMonth,@IsClosed,@UpdateBy);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_GET_FiPeriodClose
    @CompanyInfoId INT, @PeriodYear INT
AS
    SELECT PeriodMonth, IsClosed, UpdateBy, UpdateDate
    FROM dbo.tblFiPeriodClose
    WHERE CompanyInfoId=@CompanyInfoId AND PeriodYear=@PeriodYear AND IsClosed=1;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GET_FiPeriodCloseByDate
    @CompanyInfoId INT, @Date DATE
AS
    SELECT COUNT(*) FROM dbo.tblFiPeriodClose
    WHERE CompanyInfoId=@CompanyInfoId AND PeriodYear=YEAR(@Date) AND PeriodMonth=MONTH(@Date) AND IsClosed=1;
GO
