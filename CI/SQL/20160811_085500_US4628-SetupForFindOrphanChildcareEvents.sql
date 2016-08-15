USE MinistryPlatform
GO

DECLARE @APIProcName NVARCHAR(64) = 'api_crds_GetOrphanChildcareEvents'

IF NOT EXISTS(SELECT * FROM dp_API_Procedures WHERE Procedure_Name = @APIProcName)
BEGIN
	INSERT INTO dp_API_Procedures(Procedure_Name,Description)
       VALUES(@APIProcName,'Find Events with event type of childcare that have no associated group with group type of childcare')
END
GO
