USE MinistryPlatform;
GO

DECLARE @PAGE_ID int = 13;
DECLARE @PAGE_SECTION_ID INT = 21;
DECLARE @ACCESS_LEVEL_FULL INT = 3;
DECLARE @ROLE_ID INT = 107; --System Administrator - CRDS

DECLARE @PAGE_NAME nvarchar(100) = N'Group Connectors';

DECLARE @DEFAULT_FIELD_LIST NVARCHAR(1000) = N'Created_By_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Created_By]
		, Project_ID_Table.[Project_Name] AS [Project Name]
		, Created_By_Table_Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred Launch Site]
		, Created_By_Table_Organization_ID_Table.[Name] AS [Organization Name]
		, Created_By_Table_Initiative_ID_Table.[Initiative_Name] AS [Initiative Name]';

DECLARE @SELECTED_RECORD_EXPRESSION NVARCHAR(50) = N'Created_By_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name]';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] where [Display_Name] = @PAGE_NAME)
BEGIN

	SET IDENTITY_INSERT [dbo].[dp_Pages] ON

	INSERT INTO [dbo].[dp_Pages] (
		 [Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Primary_Key]
		,[Description]
		,[Table_Name]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Display_Copy]
		,[View_Order]
	) VALUES (
		 @PAGE_ID
		,@PAGE_NAME
		,@PAGE_NAME
		,N'GroupConnector_ID'
		,N'Groups of folks that want to volunteer together'
		,N'cr_GroupConnectors'		
		,@DEFAULT_FIELD_LIST
		,@SELECTED_RECORD_EXPRESSION
		,0
		,100
	)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE Page_Section_ID = 21 AND Page_ID = @PAGE_ID)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (@PAGE_ID, @PAGE_SECTION_ID)
END

INSERT INTO dp_Role_Pages (Role_ID, Page_ID, Access_Level) VALUES (@ROLE_ID, @PAGE_ID, @ACCESS_LEVEL_FULL)