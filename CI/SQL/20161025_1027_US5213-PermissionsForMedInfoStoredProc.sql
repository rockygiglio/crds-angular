USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Create_Medical_Info_For_Contacts')
BEGIN
	DECLARE @ID INT

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	VALUES('api_crds_Create_Medical_Info_For_Contacts')

	SET @ID = SCOPE_IDENTITY();

	IF NOT EXISTS(select * from [dbo].[dp_Role_API_Procedures] WHERE API_Procedure_ID =@ID AND Role_ID = 62)
	BEGIN
		INSERT INTO [dbo].[dp_Role_API_Procedures]
		([Role_ID],
		[API_Procedure_ID],
		[Domain_ID])
		VALUES
		(62,
		@ID,
		1)
	END
END