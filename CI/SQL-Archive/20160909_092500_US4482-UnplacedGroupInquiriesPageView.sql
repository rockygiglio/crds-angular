USE [MinistryPlatform]
GO

DECLARE @EXISTING_PAGE_VIEW_ID int = 0
DECLARE @PAGE_VIEW_ID int = 1111 -- Assigned by MPIdentityMaintenance.dbo.Get_Next_Available_ID
DECLARE @VIEW_TITLE nvarchar(255) = N'Placed: Unknown (API)'
DECLARE @PAGE_ID int = 315 -- Group Inquiries
DECLARE @DESCRIPTION nvarchar(255) = N'Unplaced Group Inquiries, for API use'

DECLARE @FIELD_LIST nvarchar(2000) = 'Group_ID_Table.[Group_ID]
, Group_Inquiries.[Email]
, Group_Inquiries.[Phone]
, Group_Inquiries.[First_Name]
, Group_Inquiries.[Last_Name]
, Group_Inquiries.[Inquiry_Date]
, Group_Inquiries.[Placed]
, Contact_ID_Table.[Contact_ID]
'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Group_Inquiries.Placed IS NULL 
AND CAST(Group_Inquiries.[Inquiry_Date] AS DATE) = CAST(GETDATE() - 3 AS DATE)'

DECLARE @ORDER_BY nvarchar(1000) = N'Group_ID_Table.[Group_ID]'

DECLARE @USER_ID INT = 1577873
DECLARE @USER_GROUP_ID INT = 6

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
            ,[Order_By]
            ,[User_ID]
            ,[User_Group_ID])
        VALUES (
            @PAGE_VIEW_ID
            ,@VIEW_TITLE
            ,@PAGE_ID
            ,@DESCRIPTION
            ,@FIELD_LIST
            ,@VIEW_CLAUSE
            ,@ORDER_BY
            ,@USER_ID
            ,@USER_GROUP_ID)
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
                ,[User_ID] = @USER_ID
                ,[User_Group_ID] = @USER_GROUP_ID
        WHERE Page_View_ID = @PAGE_VIEW_ID
END