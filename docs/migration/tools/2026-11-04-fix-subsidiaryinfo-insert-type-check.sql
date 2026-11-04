-- =====================================================================================================
-- Subsidiary Info page: fix silent "save does nothing" bug
-- Run ONCE per accounts DB (live: CSTL_ACCDB_Unicorn; change the USE line for any other tenant DB).
--
-- Problem : sp_I_SubsidiaryInfoDtls checked "already exists" by S_Info_Id only (no SubsidiaryTypeId), so a
--           Supplier/Employee whose id equals an already-saved Customer id (e.g. Supplier 21 vs Customer 21)
--           was skipped silently - page said "Save Successfully" but nothing was inserted.
-- Fix     : "already exists" is now per SubsidiaryTypeId + S_Info_Id.
-- Scope   : this is the ONLY database object changed. No table/data change, no other SP touched.
--           Safe to re-run (CREATE OR ALTER).
-- Rollback: re-create the SP with the old check:  select @Count=count(*) from tblSubsidiaryInfo where S_Info_Id=@S_Info_Id
-- App side: deploy the matching build too (disabled checkbox for saved rows, insert-only save, no delete).
-- =====================================================================================================
USE CSTL_ACCDB_Unicorn;
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_I_SubsidiaryInfoDtls]
(
    @SubsidiaryTypeId INT = NULL,
    @S_Info_Id int = NULL,
    @IsSelect bit = NULL,
    @Description nvarchar(max) = NULL,
    @EntryBy int
)
AS
BEGIN
    IF (@IsSelect = 1)
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM tblSubsidiaryInfo WHERE S_Info_Id = @S_Info_Id AND SubsidiaryTypeId = @SubsidiaryTypeId)
            INSERT INTO dbo.tblSubsidiaryInfo (SubsidiaryTypeId, S_Info_Id, IsSelect, Description, EntryBy, EntryDate)
            VALUES (@SubsidiaryTypeId, @S_Info_Id, @IsSelect, @Description, @EntryBy, getdate());
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM tblSubsidiaryInfo WHERE rtrim(LTRIM(Description)) = rtrim(LTRIM(@Description)))
            INSERT INTO dbo.tblSubsidiaryInfo (SubsidiaryTypeId, S_Info_Id, IsSelect, Description, EntryBy, EntryDate)
            VALUES (@SubsidiaryTypeId, @S_Info_Id, @IsSelect, @Description, @EntryBy, getdate());
    END
END
GO

-- Optional check after running (read-only): Supplier 21 and Customer 21 may now both exist
-- SELECT SubsidiaryInfoId, SubsidiaryTypeId, S_Info_Id FROM tblSubsidiaryInfo WHERE S_Info_Id = 21;
