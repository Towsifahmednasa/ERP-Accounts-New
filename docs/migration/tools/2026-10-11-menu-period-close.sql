-- Adds "Period Close" (/Journal/PeriodClose) to the Accounting Task menu group,
-- next to Journal / Debit Voucher / Credit Voucher / Contra (parent SL 500050).
-- Target DB: CSTL_ERPDB_Unicorn (menu lives in the ControlPanel DB, not CSTL_ACCDB_Unicorn).
-- Idempotent: safe to run twice. SL is not identity, so the next free SL is taken.
-- Role permissions: every role that can see Journal (SL 500049) also gets Period Close,
-- so users on the permission-filtered branch of sp_Select_Menu_Step_WithUserAccessERPAcc see it too.

USE CSTL_ERPDB_Unicorn;
GO

IF NOT EXISTS (SELECT 1 FROM tblMainMenu WHERE ControllerName = 'Journal' AND ActionName = 'PeriodClose' AND ModuleId = 1014)
BEGIN
    DECLARE @sl INT = (SELECT MAX(SL) + 1 FROM tblMainMenu);

    -- same shape as the Journal row (SL 500049): leaf page under Accounting Task (500050)
    INSERT INTO tblMainMenu (SL, ManuName, ControllerName, ActionName, ParantId, ModuleId,
                             MenuStep, IsPage, IsMVC, MenuTypeId, IsApprovalPage, IsPortalMenu, IsActive,
                             CreateBy, CreateDate)
    VALUES (@sl, 'Period Close', 'Journal', 'PeriodClose', '500050', 1014,
            2, 1, 1, 1, 0, 0, 1,
            'park-feature-script', GETDATE());

    -- mirror the Journal menu's role permissions onto the new item
    INSERT INTO Table_RoleWiseMenuPermission (SL, RoleWiseModuleId, [Add], [View], [Delete], [Edit], Permission, ProcessTime)
    SELECT @sl, p.RoleWiseModuleId, p.[Add], p.[View], p.[Delete], p.[Edit], p.Permission, GETDATE()
    FROM Table_RoleWiseMenuPermission p
    WHERE p.SL = 500049
      AND NOT EXISTS (SELECT 1 FROM Table_RoleWiseMenuPermission x
                      WHERE x.RoleWiseModuleId = p.RoleWiseModuleId AND x.SL = @sl);

    PRINT 'Period Close menu added with SL = ' + CONVERT(varchar(20), @sl);
END
ELSE
BEGIN
    PRINT 'Period Close menu already exists - nothing changed.';
END
GO
