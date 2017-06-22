USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Get_Checkin_Room_Data')
BEGIN
	DECLARE @IDs TABLE (ID INT)

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Get_Checkin_Room_Data')

	IF NOT EXISTS(select * from [dbo].[dp_Role_API_Procedures] WHERE API_Procedure_ID = (SELECT TOP(1) * FROM @IDs) AND Role_ID = 62)
	BEGIN
		INSERT INTO [dbo].[dp_Role_API_Procedures]
		([Role_ID],
		[API_Procedure_ID],
		[Domain_ID])
		VALUES
		(62,
		(SELECT TOP(1) * FROM @IDs),
		1)
	END
END