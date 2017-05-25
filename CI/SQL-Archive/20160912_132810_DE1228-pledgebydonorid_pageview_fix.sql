USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
   SET [View_Clause] = 'Pledge_Status_ID_Table.[Pledge_Status_ID] = 1 AND Pledges.[Pledge_ID] IS NOT NULL'
       WHERE [Page_View_ID] = 92187;
GO


