USE [MinistryPlatform]
GO

BEGIN
	DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 92400 OR Page_View_ID BETWEEN 977 AND 980
	DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2161 OR Page_View_ID = 966 OR Page_View_ID BETWEEN 968 AND 970

	UPDATE [dbo].[dp_Page_Views] 
		SET Field_List='Equipment.[Equipment_ID], 
Equipment.[Equipment_Name], 
Room_ID_Table.[Room_ID], 
Room_ID_Table_Building_ID_Table.[Building_ID], 
Room_ID_Table_Building_ID_Table_Location_ID_Table.[Location_ID],
Room_ID_Table_Building_ID_Table_Location_ID_Table.[Location_Name] AS [Location Name],
Equipment.[Quantity_On_Hand]'
		WHERE Page_View_ID=1105

END
