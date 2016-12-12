USE [MinistryPlatform]
GO

DECLARE @EXISTING_PAGE_ID int = 0
DECLARE @PAGE_ID int = 516
DECLARE @PAGE_SECTION_ID int = 17 -- "My Records" page
DECLARE @DISPLAY_NAME nvarchar(50) = N'My Household Donation Distributions'
DECLARE @SINGULAR_NAME nvarchar(50) = N'My Household Donation Distribution'
DECLARE @DESCRIPTION nvarchar(255) = N'Distributions of the donation amount to programs, event projects, or pledges.  This is used by the Giving History page in CR.net'
DECLARE @VIEW_ORDER smallint = 20
DECLARE @TABLE_NAME nvarchar(50) = 'Donation_Distributions'
DECLARE @PRIMARY_KEY nvarchar(50) = 'Donation_Distribution_ID'
DECLARE @DEFAULT_FIELD_LIST nvarchar(2000) = N'Donation_ID_Table.Donation_Date
,CASE WHEN (Donation_Distributions.Soft_Credit_Donor IS NULL) THEN ''False'' ELSE ''True'' END AS [Soft_Credit_Donation]
,ISNULL(Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Last_Name,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Display_Name) AS [Last_Name]
,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.First_Name
,Donation_Distributions.Amount
,Donation_ID_Table_Payment_Type_ID_Table.Payment_Type
,Donation_ID_Table.Item_Number
,Program_ID_Table.Program_Name
,Program_ID_Table.Statement_Title
,Pledge_ID_Table_Pledge_Campaign_ID_Table.Campaign_Name
,Donation_Distributions.Donation_ID
,Donation_ID_Table.Donor_ID
,Donation_ID_Table_Batch_ID_Table.Batch_ID
,Pledge_ID_Table.Pledge_ID
,Target_Event_Table.Event_Title AS [Target_Event]
,Donation_ID_Table.Donation_Status_Date ,Donation_ID_Table.Donation_Status_ID ,Donation_ID_Table.Transaction_Code ,Donation_ID_Table.Payment_Type_ID ,Soft_Credit_Donor_Table.Donor_ID AS [Soft_Credit_Donor_ID] ,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Donor_Display_Name]
,Donation_ID_Table.Is_Recurring_Gift
,Congregation_ID_Table_Accounting_Company_ID_Table.[Company_Name]
,Congregation_ID_Table_Accounting_Company_ID_Table.[Show_Online]'
DECLARE @SELECTED_RECORD_EXPRESSION nvarchar(255) = 'Program_ID_Table.Program_Name'
DECLARE @DISPLAY_COPY bit = 0
DECLARE @DISPLAY_SEARCH bit = 1
DECLARE @FILTER_CLAUSE NVARCHAR(500) = 'Donation_Distributions.Donation_Distribution_ID IN (
SELECT * FROM [dbo].[crds_udfGetDonationDistributionIdsForUser](dp_UserID)
)
'
DECLARE @CONTACT_ID_FIELD NVARCHAR(100) = 'Donation_ID_Table_Donor_ID_Table.Contact_ID'

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
		,@DISPLAY_SEARCH
		,@DEFAULT_FIELD_LIST
		,@SELECTED_RECORD_EXPRESSION
		,@FILTER_CLAUSE
		,NULL -- Start_Date_Field
		,NULL -- End_Date_Field
		,@CONTACT_ID_FIELD
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
    ,[Display_Search] = @DISPLAY_SEARCH
    ,[Filter_Clause] = @FILTER_CLAUSE
    ,[Contact_ID_Field] = @CONTACT_ID_FIELD
	WHERE [Page_ID] = @PAGE_ID
END

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
