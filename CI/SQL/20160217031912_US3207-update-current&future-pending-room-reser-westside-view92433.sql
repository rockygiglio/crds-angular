USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET [View_Title] = 'Pending Room Reservations - West Side'
  ,[Page_ID] = 384
  ,[View_Clause] = 'Event_Rooms.Cancelled = 0 AND Event_ID_Table.Event_End_Date >= Getdate() AND Event_Rooms.[_Approved] IS NULL AND Event_ID_Table_Congregation_ID_Table.[Congregation_ID] = 8'
WHERE [Page_View_ID] = 92433;
GO
