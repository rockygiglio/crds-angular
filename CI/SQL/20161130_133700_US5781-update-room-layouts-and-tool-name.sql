USE [MinistryPlatform]
GO

DECLARE @tablesAndChairs int = 1;
DECLARE @chairsOnly int = 2;
DECLARE @createEventToolId int = 376;

UPDATE [dbo].[Room_Layouts]
 SET [Layout_Name] = 'Chairs with tables'
 WHERE Room_Layout_ID = @tablesAndChairs

UPDATE [dbo].[Room_Layouts]
 SET [Layout_Name] = 'Chairs only'
 WHERE Room_Layout_ID = @chairsOnly

UPDATE [dbo].[dp_Tools]
 SET [Tool_Name] = 'Create Event'
 WHERE Tool_ID = @createEventToolId

GO


