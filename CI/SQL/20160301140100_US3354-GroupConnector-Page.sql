USE MinistryPlatform;
GO

DECLARE @PAGE_ID int = 13;
DECLARE @PAGE_NAME nvarchar(100) = N'Group Connectors';

DELETE FROM dp_Page_Section_Pages WHERE Page_Section_ID = 21 AND Page_ID = @PAGE_ID

DELETE FROM dp_Role_Pages WHERE PAGE_ID = @PAGE_ID

DELETE FROM [dbo].[dp_Pages] where Page_ID = @PAGE_ID

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
		,N'cr_GroupConnectors.Name  AS [Group Connector], Project_ID_Table.[Project_Name] AS [Project Name]
		, Organization_ID_Table.Name AS [Organization Name]
		, Initiative_ID_Table.Initiative_Name AS [Initiative Name]
		, Primary_Contact_Table.Display_Name AS [Primary Contact]'
		,N'cr_GroupConnectors.Name'
		,0
		,100
	)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END
