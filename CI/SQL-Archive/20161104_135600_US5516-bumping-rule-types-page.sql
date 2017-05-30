USE [MinistryPlatform]
GO

DECLARE @EXISTING_PAGE_ID int = 0
DECLARE @PAGE_ID int = 616 -- Assigned by MPIdentityMaintenance.dbo.Get_Next_Available_ID
DECLARE @PAGE_SECTION_ID int = 4 -- "Lookups" page
DECLARE @DISPLAY_NAME nvarchar(50) = N'Bumping Rule Types'
DECLARE @SINGULAR_NAME nvarchar(50) = N'Bumping Rule Type'
DECLARE @DESCRIPTION nvarchar(255) = N'Types used for determining Bumping Rule type.'
DECLARE @VIEW_ORDER smallint = 43
DECLARE @TABLE_NAME nvarchar(50) = 'cr_Bumping_Rule_Types'
DECLARE @PRIMARY_KEY nvarchar(50) = 'Bumping_Rule_Type_ID'
DECLARE @DEFAULT_FIELD_LIST nvarchar(2000) = 'Bumping_Rule_Type_ID,
Bumping_Rule_Type,
Description'
DECLARE @SELECTED_RECORD_EXPRESSION nvarchar(255) = 'CONCAT(Description, ''; '', Bumping_Rule_Type)'
DECLARE @DISPLAY_COPY bit = 0

SELECT @EXISTING_PAGE_ID = [Page_ID] FROM [dbo].[dp_Pages] WHERE [Page_ID] = @PAGE_ID;

IF @EXISTING_PAGE_ID = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Pages] ON
    PRINT 'Inserting new page ' + Convert(varchar, @PAGE_ID)
    INSERT INTO [dbo].[dp_Pages] (
		[Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Description]
		,[View_Order]
		,[Table_Name]
		,[Primary_Key]
		,[Display_Search]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Filter_Clause]
		,[Start_Date_Field]
		,[End_Date_Field]
		,[Contact_ID_Field]
		,[Default_View]
		,[Pick_List_View]
		,[Image_Name]
		,[Direct_Delete_Only]
		,[System_Name]
		,[Date_Pivot_Field]
		,[Custom_Form_Name]
		,[Display_Copy])
	VALUES (
		@PAGE_ID
		,@DISPLAY_NAME
		,@SINGULAR_NAME
		,@DESCRIPTION
		,@VIEW_ORDER
		,@TABLE_NAME
		,@PRIMARY_KEY
		,NULL -- Display_Search
		,@DEFAULT_FIELD_LIST
		,@SELECTED_RECORD_EXPRESSION
		,NULL -- Filter_Clause
		,NULL -- Start_Date_Field
		,NULL -- End_Date_Field
		,NULL -- Contact_ID_Field
		,NULL -- Default_View
		,NULL -- Pick_List_View
		,NULL -- Image_Name
		,NULL -- Direct_Delete_Only
		,NULL -- System_Name
		,NULL -- Date_Pivot_Field
		,NULL -- Custom_Form_Name
		,@DISPLAY_COPY
	);
    SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END
ELSE
BEGIN
	PRINT 'Update existing page ' + Convert(varchar, @PAGE_ID)
	UPDATE [dbo].[dp_Pages] SET
		[Display_Name] = @DISPLAY_NAME
		,[Singular_Name] = @SINGULAR_NAME
		,[Description] = @DESCRIPTION
		,[View_Order] = @VIEW_ORDER
		,[Table_Name] = @TABLE_NAME
		,[Primary_Key] = @PRIMARY_KEY
		,[Default_Field_List] = @DEFAULT_FIELD_LIST
		,[Selected_Record_Expression] = @SELECTED_RECORD_EXPRESSION
		,[Display_Copy] = @DISPLAY_COPY
	WHERE [Page_ID] = @PAGE_ID
END

-- Add this to the "Administration" Page
SELECT @EXISTING_PAGE_ID = [Page_ID] FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_ID] = @PAGE_ID AND [Page_Section_ID] = @PAGE_SECTION_ID;
IF @EXISTING_PAGE_ID = 0
BEGIN
  PRINT 'Adding page ' + Convert(varchar, @PAGE_ID) + ' to page section ' + Convert(varchar, @PAGE_SECTION_ID);
  INSERT INTO [dbo].[dp_Page_Section_Pages] (Page_ID, Page_Section_ID) VALUES (@PAGE_ID, @PAGE_SECTION_ID);
END
ELSE
BEGIN
  PRINT 'Page ' + Convert(varchar, @PAGE_ID) + ' is already in page section ' + Convert(varchar, @PAGE_SECTION_ID);
END;