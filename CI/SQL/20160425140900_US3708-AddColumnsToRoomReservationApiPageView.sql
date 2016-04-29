USE [MinistryPlatform];

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 1108 AND [Field_List] LIKE '%Allow_Checkin%')
BEGIN
	UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = CONCAT([Field_List], ', Room_ID_Table.[Room_Name], Event_Rooms.[Label], Event_Rooms.[Capacity], Event_Rooms.[Allow_Checkin]')
	WHERE [Page_View_ID] = 1108;
END