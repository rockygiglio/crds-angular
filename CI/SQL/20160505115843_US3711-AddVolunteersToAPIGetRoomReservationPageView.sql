USE [MinistryPlatform];

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 1108 AND [Field_List] LIKE '%Volunteers%')
BEGIN
	UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = CONCAT([Field_List], ', Event_Rooms.[Volunteers]')
	WHERE [Page_View_ID] = 1108;
END
