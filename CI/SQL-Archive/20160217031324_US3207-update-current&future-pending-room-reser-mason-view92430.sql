USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET [View_Title] = 'Pending Room Reservations - Mason'
  ,[Page_ID] = 384
  ,[View_Clause] = '(Event_Rooms._Approved = 0 OR Event_Rooms._Approved IS NULL)  
  AND Event_Rooms.Cancelled = 0 
  AND Event_ID_Table.Event_End_Date >= Getdate() 
  AND Event_ID_Table_Congregation_ID_Table.[Congregation_ID] = 6
  AND Event_Rooms.Event_Room_ID NOT IN (SELECT T._Record_ID FROM dp_Tasks T WHERE T._Page_ID= 384 AND T._Rejected = 1)'
  WHERE [Page_View_ID] = 92430;
GO
