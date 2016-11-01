USE MinistryPlatform;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Delete_Table_Rows]') AND type in (N'P', N'PC'))
BEGIN
  EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Delete_Table_Rows] AS'
END
GO

-- =============================================
-- Author:      Jim Kriz
-- Create date: 2016-10-28
-- Description:  Delete rows by primary key from a table.
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_Delete_Table_Rows]
  @TableName nvarchar(255),
  @PrimaryKeyColumnName nvarchar(255),
  @IdentifiersToDelete nvarchar(max)
AS
BEGIN
  DECLARE @ErrorVar INT;  
  DECLARE @RowCountVar INT;
  DECLARE @ActualDeletes INT = 0;

  BEGIN TRANSACTION
  BEGIN TRY
    EXEC('DELETE FROM [' + @TableName + '] WHERE [' + @PrimaryKeyColumnName + '] IN (' + @IdentifiersToDelete + ')');
    SET @ActualDeletes = @@ROWCOUNT;
    IF @ActualDeletes > 0
    BEGIN
      INSERT INTO dp_Audit_Log (Table_Name, Record_ID, Audit_Description, [User_Name], [User_ID], [Date_Time]) 
        SELECT @TableName, CAST(Item AS INT), 'Deleted from REST API procedure', 'register_api', 1577873, GETDATE() FROM dp_Split(@IdentifiersToDelete, ',');
    END
  END TRY
  BEGIN CATCH
    PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
      ROLLBACK TRANSACTION;
  END CATCH;
  
  IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
END
GO