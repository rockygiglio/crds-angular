USE [MinistryPlatform]
GO

DECLARE @EXISTING_PAGE_VIEW_ID int = 0
DECLARE @PAGE_VIEW_ID int = 1110 -- Assigned by MPIdentityMaintenance.dbo.Get_Next_Available_ID
DECLARE @VIEW_TITLE nvarchar(255) = N'Small Groups - Category Detail'
DECLARE @PAGE_ID int = 322 -- Groups
DECLARE @DESCRIPTION nvarchar(255) = N'Small groups with category and leader details'

DECLARE @FIELD_LIST nvarchar(2000) = 'Groups.[Group_Name] AS [Group Name]
, [dbo].[crds_GetGroupCategories](Groups.[Group_ID]) AS [Categories]
, Primary_Contact_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_Name] AS [Leader Site]
, Groups.[Start_Date] AS [Start Date]
, Groups.[End_Date] AS [End Date]
, [dbo].[crds_GetGroupLeaderEmails](Groups.[Group_ID]) AS [All Leaders Emails]
'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Group_Type_ID = ''1'''
DECLARE @ORDER_BY nvarchar(1000) = NULL
SELECT @EXISTING_PAGE_VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = @PAGE_VIEW_ID;

IF @EXISTING_PAGE_VIEW_ID = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
        PRINT 'Inserting new page view ' + Convert(varchar, @PAGE_VIEW_ID)
        INSERT INTO [dbo].[dp_Page_Views] (
            [Page_View_ID]
            ,[View_Title]
            ,[Page_ID]
            ,[Description]
            ,[Field_List]
            ,[View_Clause]
            ,[Order_By])
        VALUES (
            @PAGE_VIEW_ID
            ,@VIEW_TITLE
            ,@PAGE_ID
            ,@DESCRIPTION
            ,@FIELD_LIST
            ,@VIEW_CLAUSE
            ,@ORDER_BY)
        SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE
BEGIN
        PRINT 'Update existing page view ' + Convert(varchar, @PAGE_VIEW_ID)
        UPDATE [dbo].[dp_Page_Views] SET
                [View_Title] = @VIEW_TITLE
                ,[Page_ID] = @PAGE_ID
                ,[Description] = @DESCRIPTION
                ,[Field_List] = @FIELD_LIST
                ,[View_Clause] = @VIEW_CLAUSE
                ,[Order_By] = @ORDER_BY
        WHERE Page_View_ID = @PAGE_VIEW_ID
END