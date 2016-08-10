USE MinistryPlatform
GO

IF NOT EXISTS(select * from [dbo].[dp_API_Procedures] where procedure_name = 'api_crds_DeleteDatesForChildcareRequest')
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures] ( procedure_name) values('api_crds_DeleteDatesForChildcareRequest')
END
