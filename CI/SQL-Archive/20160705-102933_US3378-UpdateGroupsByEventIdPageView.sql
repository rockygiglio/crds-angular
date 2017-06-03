USE [MinistryPlatform]
GO

IF EXISTS(SELECT * FROM dbo.dp_Page_Views WHERE Page_View_ID = 2221 AND View_Title = 'Groups By Event Id')
BEGIN
	UPDATE dbo.dp_Page_Views
	SET Field_List = 'Event_ID_Table.[Event_ID] , Group_ID_Table.[Group_Name] , Event_Groups.[Event_Group_ID] , Group_ID_Table.[Group_ID] , Room_ID_Table.[Room_ID] , Event_Groups.[Closed] , Event_Room_ID_Table.[Event_Room_ID], Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID]'
	WHERE Page_View_ID = 2221
END