use MinistryPlatform;
GO

DECLARE @PROCEDURE_NAME nvarchar(100) = N'api_crds_ChildRsvpd';

IF NOT EXISTS (SELECT 1 FROM dbo.dp_API_Procedures WHERE Procedure_Name = @PROCEDURE_NAME)
BEGIN
	INSERT INTO dbo.dp_API_Procedures (
		 Procedure_Name
		,Description
	) VALUES (
		 @PROCEDURE_NAME
		,N'Based on a contact_id and a group_id, returns a column if the contact is rsvpd in childcare'
	)
END 

DECLARE @PROCEDURE_ID int;
DECLARE @API_ROLE_ID int = 62;

SELECT @PROCEDURE_ID = API_Procedure_ID FROM dbo.dp_API_Procedures WHERE Procedure_Name = @PROCEDURE_NAME;

IF NOT EXISTS (SELECT 1 FROM dbo.dp_Role_API_Procedures WHERE Role_ID = @API_ROLE_ID AND API_Procedure_ID = @PROCEDURE_ID)
BEGIN
	INSERT INTO dbo.dp_Role_API_Procedures (
		 Role_ID
		,API_Procedure_ID
		,Domain_ID
	) VALUES (
		 @API_ROLE_ID
		,@PROCEDURE_ID
		,1
	)
END