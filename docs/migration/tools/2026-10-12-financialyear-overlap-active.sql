-- Financial year integrity, SAP fiscal-year-variant style (final state of this change set).
--
--   Rule changes
--     * A company's years must not overlap (checked on insert AND edit). Several NON-overlapping years may be Active
--       at once (year-end: post in the new year while the old one is still open). The old "only one Active year"
--       rule (-2) is gone. Safe because the posting SPs already pick the year by "Active AND date BETWEEN" and the
--       voucher pages offer every Active+unlocked year (sp_GET_FinancialYearforNotLock).
--     * End Date must be after Start Date (also enforced in the SP, not only the page).
--     * Edit guard: once vouchers are posted in a year its company cannot change and its dates cannot be edited so
--       that a posted voucher falls outside them.
--     * Locking a year (sp_Lock_FinancialYear_New) now also closes every month of the year in tblFiPeriodClose.
--     * Description: calendar years read "2026" instead of "2026-2026".
--   Errors:  sp_I_FinancialYear_New returns -1 (exists) / -3 (overlap); both SPs THROW 50001 (message shown on the page).
--   Audit:   tblFinancialYearLog + trigger record old/new company, dates, status and lock on every insert/update.
--
-- Target DB: CSTL_ACCDB_Unicorn. Idempotent. Run: sqlcmd -S <server> -d CSTL_ACCDB_Unicorn -i <this file>
USE CSTL_ACCDB_Unicorn;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_I_FinancialYear_New]
(
    @FinancialYearId int,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @CompanyId INT = NULL,
    @Status NVARCHAR(50),
    @ActiveDate DATETIME = NULL,
    @InActiveDate DATETIME = NULL,
    @EntryBy NVARCHAR(50)
)
AS
BEGIN
    IF @EndDate <= @StartDate THROW 50001, 'End Date must be after Start Date.', 1;

    DECLARE @FinancialCode NVARCHAR(MAX)
    DECLARE @FinancialYearDesc NVARCHAR(MAX) =
        CASE WHEN YEAR(@StartDate) = YEAR(@EndDate) THEN CONVERT(CHAR(4), YEAR(@StartDate))
             ELSE CONVERT(CHAR(4), YEAR(@StartDate)) + '-' + CONVERT(CHAR(4), YEAR(@EndDate)) END

    IF EXISTS (SELECT 1 FROM tblFinancialYear WHERE FinancialYearDesc = @FinancialYearDesc AND CompanyId = @CompanyId)
    BEGIN
        SELECT -1
        RETURN
    END

    IF EXISTS (SELECT 1 FROM tblFinancialYear
               WHERE CompanyId = @CompanyId AND StartDate <= @EndDate AND EndDate >= @StartDate)
    BEGIN
        SELECT -3
        RETURN
    END

    SELECT @FinancialCode = (CONVERT(NVARCHAR(MAX), (COUNT(FinancialYearId) + 001))) FROM dbo.tblFinancialYear

    INSERT INTO tblFinancialYear (FinancialCode, StartDate, EndDate, CompanyId, Status, ActiveDate, InActiveDate, FinancialYearDesc, EntryBy, EntryDate)
    VALUES (@FinancialCode, @StartDate, @EndDate, @CompanyId, @Status, @ActiveDate, @InActiveDate, @FinancialYearDesc, @EntryBy, GETDATE())

    SELECT SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_UD_FinancialYear_New]
    @FinancialYearId int,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @CompanyId INT = NULL,
    @Status NVARCHAR(max),
    @ActiveDate DATETIME = NULL,
    @InActiveDate DATETIME = NULL,
    @UpdateBy NVARCHAR(max)
AS
BEGIN
    IF @EndDate <= @StartDate THROW 50001, 'End Date must be after Start Date.', 1;

    DECLARE @FinancialYearDesc NVARCHAR(MAX) =
        CASE WHEN YEAR(@StartDate) = YEAR(@EndDate) THEN CONVERT(CHAR(4), YEAR(@StartDate))
             ELSE CONVERT(CHAR(4), YEAR(@StartDate)) + '-' + CONVERT(CHAR(4), YEAR(@EndDate)) END

    IF EXISTS (SELECT 1 FROM tblFinancialYear
               WHERE CompanyId = @CompanyId AND FinancialYearId <> @FinancialYearId
                 AND StartDate <= @EndDate AND EndDate >= @StartDate)
        THROW 50001, 'These dates overlap another financial year of this company.', 1;

    IF EXISTS (SELECT 1 FROM tblDebitCreditVoucherMaster WHERE FinancialYearId = @FinancialYearId)
    BEGIN
        IF @CompanyId <> (SELECT CompanyId FROM tblFinancialYear WHERE FinancialYearId = @FinancialYearId)
            THROW 50001, 'Company cannot be changed: vouchers are already posted in this financial year.', 1;

        IF EXISTS (SELECT 1 FROM tblDebitCreditVoucherMaster
                   WHERE FinancialYearId = @FinancialYearId
                     AND (PostingDate < @StartDate OR PostingDate >= DATEADD(DAY, 1, @EndDate)))
            THROW 50001, 'Dates cannot exclude vouchers already posted in this financial year.', 1;
    END

    UPDATE tblFinancialYear
    SET StartDate = @StartDate,
        EndDate = @EndDate,
        ActiveDate = @ActiveDate,
        InActiveDate = @InActiveDate,
        CompanyId = @CompanyId,
        FinancialYearDesc = @FinancialYearDesc,
        Status = @Status,
        UpdateBy = @UpdateBy,
        UpdateDate = GETDATE()
    WHERE FinancialYearId = @FinancialYearId
END
GO

-- Lock = close every month of the year for posting (tblFiPeriodClose is what the voucher pages check).
CREATE OR ALTER PROCEDURE [dbo].[sp_Lock_FinancialYear_New]
    @FinancialYearId int,
    @CompanyId INT = NULL,
    @UpdateBy NVARCHAR(max)
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRAN;

    UPDATE tblFinancialYear
    SET IsLock = 1, LockBy = @UpdateBy, LockDate = GETDATE()
    WHERE FinancialYearId = @FinancialYearId;

    DECLARE @Co INT, @S DATE, @E DATE;
    SELECT @Co = CompanyId, @S = DATEFROMPARTS(YEAR(StartDate), MONTH(StartDate), 1), @E = EndDate
    FROM tblFinancialYear WHERE FinancialYearId = @FinancialYearId;

    ;WITH m AS (
        SELECT @S AS d
        UNION ALL
        SELECT DATEADD(MONTH, 1, d) FROM m WHERE DATEADD(MONTH, 1, d) <= @E
    )
    SELECT YEAR(d) AS y, MONTH(d) AS mo INTO #months FROM m OPTION (MAXRECURSION 100);

    UPDATE p SET IsClosed = 1, UpdateBy = @UpdateBy, UpdateDate = GETDATE()
    FROM tblFiPeriodClose p JOIN #months t ON p.PeriodYear = t.y AND p.PeriodMonth = t.mo
    WHERE p.CompanyInfoId = @Co;

    INSERT INTO tblFiPeriodClose (CompanyInfoId, PeriodYear, PeriodMonth, IsClosed, UpdateBy)
    SELECT @Co, t.y, t.mo, 1, @UpdateBy FROM #months t
    WHERE NOT EXISTS (SELECT 1 FROM tblFiPeriodClose p WHERE p.CompanyInfoId = @Co AND p.PeriodYear = t.y AND p.PeriodMonth = t.mo);

    COMMIT;
END
GO

-- ---- audit: who changed what on tblFinancialYear ----
IF OBJECT_ID('dbo.tblFinancialYearLog') IS NULL
CREATE TABLE dbo.tblFinancialYearLog (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    FinancialYearId INT NOT NULL,
    Action NVARCHAR(10) NOT NULL,
    ChangedBy NVARCHAR(100) NULL,
    ChangedAt DATETIME NOT NULL DEFAULT GETDATE(),
    OldCompanyId INT NULL, NewCompanyId INT NULL,
    OldStartDate DATETIME NULL, NewStartDate DATETIME NULL,
    OldEndDate DATETIME NULL, NewEndDate DATETIME NULL,
    OldStatus NVARCHAR(50) NULL, NewStatus NVARCHAR(50) NULL,
    OldIsLock BIT NULL, NewIsLock BIT NULL
);
GO

CREATE OR ALTER TRIGGER dbo.trg_tblFinancialYear_Audit ON dbo.tblFinancialYear
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.tblFinancialYearLog
        (FinancialYearId, Action, ChangedBy, OldCompanyId, NewCompanyId, OldStartDate, NewStartDate,
         OldEndDate, NewEndDate, OldStatus, NewStatus, OldIsLock, NewIsLock)
    SELECT i.FinancialYearId,
           CASE WHEN d.FinancialYearId IS NULL THEN 'INSERT' ELSE 'UPDATE' END,
           CASE WHEN d.FinancialYearId IS NULL THEN i.EntryBy
                WHEN ISNULL(d.IsLock, 0) <> ISNULL(i.IsLock, 0) THEN i.LockBy
                ELSE i.UpdateBy END,
           d.CompanyId, i.CompanyId, d.StartDate, i.StartDate, d.EndDate, i.EndDate,
           d.Status, i.Status, d.IsLock, i.IsLock
    FROM inserted i LEFT JOIN deleted d ON d.FinancialYearId = i.FinancialYearId
    WHERE d.FinancialYearId IS NULL
       OR ISNULL(d.CompanyId, 0) <> ISNULL(i.CompanyId, 0)
       OR ISNULL(d.StartDate, 0) <> ISNULL(i.StartDate, 0)
       OR ISNULL(d.EndDate, 0) <> ISNULL(i.EndDate, 0)
       OR ISNULL(d.Status, '') <> ISNULL(i.Status, '')
       OR ISNULL(d.IsLock, 0) <> ISNULL(i.IsLock, 0);
END
GO
