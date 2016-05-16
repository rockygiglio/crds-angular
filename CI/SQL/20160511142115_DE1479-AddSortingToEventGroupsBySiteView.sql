USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

UPDATE [dbo].[dp_Page_Views]
SET Order_By = 'Events.[Event_Start_Date], [Event_Title]'
WHERE Page_View_ID = '2222'

GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
GO
