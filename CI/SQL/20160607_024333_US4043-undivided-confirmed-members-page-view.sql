USE [MinistryPlatform]	
GO

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Confirmed Participants'
DECLARE @PAGE_ID int = 556
DECLARE @DESCRIPTION nvarchar(255) = N'Staff members can view the users that have been confirmed to a group as a member.'

DECLARE @FIELD_LIST nvarchar(2000) = 'Group_ID_Table.[Group_Name]
	,Participant_ID_Table_Contact_ID_Table.[First_Name]
	,Participant_ID_Table_Contact_ID_Table.[Last_Name]'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Group_ID_Table_Group_Type_ID_Table.Group_Type_ID = 26 AND  Group_Role_ID_Table.Group_Role_ID = 16 AND (Group_Participants.End_Date > GetDate() OR Group_Participants.End_Date IS NULL) AND (Group_ID_Table.End_Date > GetDate() OR Group_ID_Table.End_Date IS NULL) AND Group_ID_Table_Parent_Group_Table.Group_ID IS NOT NULL'
DECLARE @ORDER_BY nvarchar(1000) = N'Group_ID_Table.[Group_Name]'
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2302;

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
		,[Order_By])	
	VALUES (
	    2302
		,@VIEW_TITLE
		,@PAGE_ID
		,@DESCRIPTION
		,@FIELD_LIST
		,@VIEW_CLAUSE
		,@ORDER_BY
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