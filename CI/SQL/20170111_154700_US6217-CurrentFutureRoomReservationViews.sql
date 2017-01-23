USE [MinistryPlatform]
GO

BEGIN
	DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID=2163 OR Page_View_ID BETWEEN 985 AND 988
	UPDATE [dbo].[dp_Page_Views] 
		SET View_Title='Current & Future Reserv. - All', 
		Field_List='Event_ID_Table.[Event_Start_Date] AS [Event Start Date], 
Event_ID_Table.[Event_End_Date] AS [Event End Date], 
Event_ID_Table.[Event_Title] AS [Event Title], 
Room_ID_Table.[Room_Name] AS [Room Name], 
Room_ID_Table_Building_ID_Table.[Building_Name] AS [Building Name], 
Room_ID_Table_Building_ID_Table_Location_ID_Table.[Location_Name] AS [Location Name], 
Event_Rooms.[_Approved] AS [Approved]'
		WHERE Page_View_ID=441
END