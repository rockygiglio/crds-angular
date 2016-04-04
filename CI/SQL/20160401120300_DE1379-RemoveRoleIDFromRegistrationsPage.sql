USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM dp_Pages WHERE Page_ID = 16)
BEGIN
	UPDATE [dbo].[dp_Pages]
	SET Default_Field_List = 'Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Display Name], Organization_ID_Table.[Name] AS [Organization Name], Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred Launch Site], cr_Registrations.[Spouse_Participation] AS [Spouse Participation], cr_Registrations.[Additional_Information] AS [Additional Information], Initiative_ID_Table.[Initiative_Name] AS [Initiative Name]'
	WHERE Page_ID = 16

END
