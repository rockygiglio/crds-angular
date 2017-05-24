USE [MinistryPlatform]	
GO

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Participants with ContactId'
DECLARE @PAGE_ID int = 355
DECLARE @DESCRIPTION nvarchar(255) = N'Participants with contact ID'

DECLARE @FIELD_LIST nvarchar(2000) = 'Contact_ID_Table.[Contact_ID] AS [Contact ID] 
    , Contact_ID_Table.[Display_Name] AS [Display Name]
    , Contact_ID_Table.[Nickname] AS [Nickname]
    , Contact_ID_Table.[First_Name] AS [First Name]
    , Contact_ID_Table.[Email_Address] AS [Email Address]
    , Participants.[Participant_ID] AS [Participant ID]
    , Contact_ID_Table.[__Age] AS [Age]
    , Participants.[Approved_Small_Group_Leader]'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Participants.[Participant_ID] <> 0'
DECLARE @ORDER_BY nvarchar(1000) = NULL
DECLARE @USER_ID INT = 1577873
DECLARE @USER_GROUP_ID INT = 6
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 1029;

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
	    1029
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