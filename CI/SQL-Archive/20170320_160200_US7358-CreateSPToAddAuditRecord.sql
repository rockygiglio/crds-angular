USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[crds_Add_Audit]    Script Date: 3/16/2017 4:05:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('crds_Add_Audit', 'P') IS NOT NULL
DROP PROC crds_Add_Audit
GO

CREATE PROCEDURE [dbo].[crds_Add_Audit]
	 @TableName varchar(50)
	,@Record_ID int
	,@Audit_Description varchar(50)
	,@UserName nvarchar(254)
	,@UserID  int
	,@FieldName nvarchar(50)
	,@FieldLabel nvarchar(50)
	,@PreviousValue nvarchar(max)
	,@NewValue nvarchar(max)


AS
BEGIN

DECLARE @AuditItemID INT   

			INSERT INTO [dp_Audit_Log]
							   ([Table_Name]
							   ,[Record_ID]
							   ,[Audit_Description]
							   ,[User_Name]
							   ,[User_ID]
							   ,[Date_Time])

			VALUES
			                   (@TableName
							   ,@Record_ID
							   ,@Audit_Description
							   ,@UserName
							   ,@UserID
							   ,GetUtcDate() )
		

			SET @AuditItemID = SCOPE_IDENTITY()
			
			INSERT INTO dp_Audit_Detail (Audit_Item_ID
										,Field_Name
										,Field_Label
										,Previous_Value
										,New_Value)
			VALUES( @AuditItemID
			, @FieldName
			, @FieldLabel
			, @PreviousValue
			, @NewValue
			)
	
END