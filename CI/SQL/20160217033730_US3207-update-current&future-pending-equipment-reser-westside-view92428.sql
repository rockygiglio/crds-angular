USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET [View_Title] = 'Pending Equipment Reservations - West Side'
  ,[Page_ID] = 302
  ,[View_Clause] = '(Event_Equipment._Approved = 0 OR Event_Equipment._Approved IS NULL) 
  AND Event_Equipment.Cancelled = 0 
  AND Event_ID_Table.Event_End_Date >= Getdate() 
  AND Event_ID_Table_Congregation_ID_Table.[Congregation_ID] = 8 
  AND Event_Equipment.Event_Equipment_ID NOT IN (SELECT T._Record_ID FROM dp_Tasks T WHERE T._Page_ID= 302 AND T._Rejected = 1)'WHERE [Page_View_ID] = 92428;
GO
