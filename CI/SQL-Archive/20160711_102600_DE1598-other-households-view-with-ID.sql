use MinistryPlatform;
GO

DECLARE @SUB_PAGE_VIEW_ID int = 191;
DECLARE @SUB_PAGE_ID int = 270;
DECLARE @VIEW_TITLE nvarchar(250) = N'Other Household IDs';
DECLARE @DESCRIPTION nvarchar(500) = N'A view of Other Households that includes the Other Households ID';
DECLARE @FIELD_LIST nvarchar(500) = N'Household_ID_Table.[Household_ID], Household_ID_Table.[Household_Name]';
DECLARE @VIEW_CLAUSE nvarchar(500) = N'Household_ID_Table.[Household_ID] IS NOT NULL';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Sub_Page_Views] WHERE Sub_Page_View_ID = @SUB_PAGE_VIEW_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
	INSERT INTO [dbo].[dp_Sub_Page_Views] (
		 [Sub_Page_View_ID]
		,[View_Title]
		,[Sub_Page_ID]
		,[Description]
		,[Field_List]
		,[View_Clause]		
	) VALUES (
		 @SUB_PAGE_VIEW_ID
		,@VIEW_TITLE
		,@SUB_PAGE_ID
		,@DESCRIPTION
		,@FIELD_LIST
		,@VIEW_CLAUSE
	)
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE
BEGIN
	UPDATE [dbo].[dp_Sub_Page_Views] SET 
		 [View_Title] = @VIEW_TITLE
		,[Sub_Page_ID] = @SUB_PAGE_ID
		,[Description] = @DESCRIPTION
		,[Field_List] = @FIELD_LIST
		,[View_Clause] = @VIEW_CLAUSE
	WHERE [Sub_Page_View_ID] = @SUB_PAGE_VIEW_ID
END