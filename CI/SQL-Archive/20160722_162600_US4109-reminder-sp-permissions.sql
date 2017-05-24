USE [MinistryPlatform]
GO

DECLARE @PROCEDURE_NAME nvarchar(128) = N'api_crds_ChildcareReminderEmails';
DECLARE @DESCRIPTION nvarchar(500)  = N'Get emails of Users who are bringing kiddos to childcare a certain number of days out'

DECLARE @PROCEDURE_ID int;
DECLARE @API_ROLE_ID int = 62;

IF EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE Procedure_Name = @Procedure_Name)
BEGIN
	UPDATE [dbo].[dp_API_Procedures]
	   SET [Procedure_Name] = @PROCEDURE_NAME
		  ,[Description] = @DESCRIPTION
	 WHERE Procedure_Name = @PROCEDURE_NAME
	 SELECT @PROCEDURE_ID = [API_Procedure_ID] FROM [dbo].[dp_API_Procedures] WHERE Procedure_Name = @Procedure_Name;
END
ELSE 
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures] (
		 [Procedure_Name]
		,[Description]
	) VALUES (
		 @PROCEDURE_NAME
		,@DESCRIPTION
	)
	SELECT @PROCEDURE_ID = [API_Procedure_ID] FROM [dbo].[dp_API_Procedures] WHERE Procedure_Name = @Procedure_Name;
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_API_Procedures] WHERE API_Procedure_ID = @PROCEDURE_ID AND Role_ID = @API_ROLE_ID)
BEGIN
	INSERT INTO [dbo].[dp_Role_API_Procedures] (
		 [Role_ID]
		,[API_Procedure_ID]
		,[Domain_ID]
	) VALUES (
		 @API_ROLE_ID
		,@PROCEDURE_ID
		,1
	)
END