USE [MinistryPlatform]
GO

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Addresses API View'
DECLARE @PAGE_ID int = 271
DECLARE @DESCRIPTION nvarchar(255) = N'Addresses view for CRDS API'

DECLARE @FIELD_LIST nvarchar(2000) = 'Addresses.[Address_Line_1]
  , Addresses.[Address_Line_2]
  , Addresses.[City] AS [City]
  , Addresses.[State/Region]
  , Addresses.[Postal_Code]
  , Addresses.[Foreign_Country]
  , Addresses.[Latitude]
  , Addresses.[Longitude]
  , Addresses.[Address_ID]'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'1=1'
DECLARE @ORDER_BY nvarchar(1000) = NULL
DECLARE @USER_ID INT = 1577873
DECLARE @USER_GROUP_ID INT = 6
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 92816;

IF @VIEW_ID = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
        PRINT 'Inserting new page view'
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
            92816
                ,@VIEW_TITLE
                ,@PAGE_ID
                ,@DESCRIPTION
                ,@FIELD_LIST
                ,@VIEW_CLAUSE
                ,@ORDER_BY
        ,@USER_ID
        ,@USER_GROUP_ID
        )
        SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE
BEGIN
        PRINT 'Update viewId ' + Convert(varchar, @VIEW_ID)
        UPDATE [dbo].[dp_Page_Views] SET
                [View_Title] = @VIEW_TITLE
                ,[Page_ID] = @PAGE_ID
                ,[Description] = @DESCRIPTION
                ,[Field_List] = @FIELD_LIST
                ,[View_Clause] = @VIEW_CLAUSE
                ,[Order_By] = @ORDER_BY
        WHERE Page_View_ID = @VIEW_ID
END