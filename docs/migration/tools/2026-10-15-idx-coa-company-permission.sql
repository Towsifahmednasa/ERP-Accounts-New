-- Chart Of Account List search is slow on the deployed server (sp_GET_ChartOfAccountParamSearch).
-- The SP builds the CompanyName column with a correlated STUFF(... FOR XML PATH) subquery that
-- scans tblCOACompanyPermission once per COA row; without an index on BankAccountId that is a
-- full-table scan per row. The local dev DB already has this index (fast); the server DB lacks it.
-- Target DB: CSTL_ACCDB_Unicorn. Idempotent: safe to run twice. No SP change (SP contract preserved).

USE CSTL_ACCDB_Unicorn;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID('dbo.tblCOACompanyPermission')
                 AND name = 'IDX_tblCOACompanyPermission_BankAccountId_CompanyId')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_tblCOACompanyPermission_BankAccountId_CompanyId
        ON dbo.tblCOACompanyPermission (BankAccountId, CompanyId);
    PRINT 'Index created.';
END
ELSE
    PRINT 'Index already exists - nothing changed.';
GO
