USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2220)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
			   ([Page_View_ID]
			   ,[View_Title]
			   ,[Page_ID]
			   ,[Description]
			   ,[Field_List]
			   ,[View_Clause])
		 VALUES
			   (2220
			   ,'Locations by Organization'
			   ,337
			   ,'Location details by Organization'
			   ,'Organization_ID_Table.[Organization_ID] AS [Organization ID]
	, Organization_ID_Table.[Name] AS [Name]
	, Locations.[Location_ID] AS [Location ID]
	, Locations.[Location_Name] AS [Location Name]
	, Location_Type_ID_Table.[Location_Type_ID] AS [Location Type ID]
	, Location_Type_ID_Table.[Location_Type] AS [Location Type]
	, Address_ID_Table.[Address_Line_1] AS [Address Line 1]
	, Address_ID_Table.[City] AS [City]
	, Address_ID_Table.[State/Region] AS [State/Region]
	, Address_ID_Table.[Postal_Code] AS [Postal Code]'
			   ,'1=1')
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END