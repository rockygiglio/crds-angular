use MinistryPlatform
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_Add_As_TripParticipant')
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures] (
		[Procedure_Name],
		[Description]
	) VALUES (
		N'api_crds_Add_As_TripParticipant',
		N'Add participant to a Trip'
	)
END