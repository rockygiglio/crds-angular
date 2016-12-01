USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
   SET [View_Clause] = 'Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 6 AND (DATEADD(day, 90, Event_ID_Table.[Event_End_Date]) >= GETDATE()) AND
Participation_Status_ID_Table.[Participation_Status_ID] = 2 AND Event_ID_Table_Program_ID_Table_Pledge_Campaign_ID_Table.[Pledge_Campaign_ID] IS NOT NULL'
       WHERE [Page_View_ID] = 92155;
GO