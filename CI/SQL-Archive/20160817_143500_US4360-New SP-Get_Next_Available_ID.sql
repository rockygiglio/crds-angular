USE [MPIdentityMaintenance]
GO
/****** Object:  StoredProcedure [dbo].[Get_Next_Available_ID]    Script Date: 8/17/2016 1:31:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================================================================================
-- Author:		Sandi Ritter
-- Create date: 8/12/2016
-- Description:	This process will be used by MP devs to get the next available ID for any table that has been gapped.
--              This process will take in a table name and then will query the range_tracker table
--              to verify that it is one of the gapped tables.  If it is not, it will return a message to the user.
--              If the input table is a gapped table, it will pull the next ID available in the range, verify
--              that the ID has not yet been used, update the range on the range_tracker table, and return the 
--              ID to the user.  
-- ==============================================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_Next_Available_ID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Get_Next_Available_ID] AS' 
END
GO

ALTER PROCEDURE [dbo].[Get_Next_Available_ID]
       @tableName   NVARCHAR(50),	  
	     @description NVARCHAR(50) 	 
 
AS
BEGIN
	DECLARE @tablePK     NVARCHAR(50),	 					  
		    	@sql         NVARCHAR(4000),
		    	@tableId     INT,
		    	@counts      INT,
		    	@high			   INT,			
		    	@IdCnt       INT = 1			 
	
	WHILE @IdCnt < 99
	BEGIN
		
		SELECT @tableId = low_range, @high = high_range FROM  Range_Tracker 
						WHERE mp_table_name = @tableName;

		IF @tableId IS NULL	
		BEGIN
			RAISERROR('Invalid table name', 16, 1)
			RETURN 	
		END
				
		IF @high <= @tableId
		BEGIN
			RAISERROR('No Values are available.  Contact Prod Support', 16, 1)
			RETURN 	
		END

		IF (@high - @tableId) < 10
		BEGIN
			RAISERROR('Need more values.  Contact Prod Support', 08, 1)
		END		

		SELECT @tablePK = COLUMN_NAME FROM MINISTRYPLATFORM.INFORMATION_SCHEMA.KEY_COLUMN_USAGE
			WHERE TABLE_NAME = '' + @tableName+''
			AND CONSTRAINT_NAME = 'pk_' + @tableName + ''	
		
		SELECT @sql =
			N' SELECT @cnt=COUNT(*) FROM MinistryPlatform.dbo.' + quotename(@tableName) +
			N' WHERE ' + @tablePK + ' = ' +  (CAST(@tableId AS NVARCHAR(50)))
			
		EXEC sp_executesql @sql, N' @cnt int OUTPUT', @cnt=@counts OUTPUT   
	
		UPDATE Range_Tracker
		SET low_range = low_range + 1
		WHERE mp_table_name = @tableName
			  
		IF @counts = 0 
		BEGIN
			SET @IdCnt = 100			
			SELECT @tableId AS Assigned_ID	
		
			INSERT INTO Logging
				(user_name
			    ,mp_table_name
			  	,id_assigned
			    ,usage_description)
			 VALUES
			    ((select SUSER_SNAME())
			    ,@tableName
			   	,@tableId
			    ,@description)
			
			RETURN
		 END
	END

	RAISERROR('All values in the range were already in use. You can attempt to re-run to try to find one in the next block.', 16, 1)
  END