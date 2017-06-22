USE [MinistryPlatform]
GO

DECLARE @ProcessID int = 41

UPDATE [dbo].[dp_Processes]
   SET [Trigger_Fields] = 'Group_Leader_Status_ID'
      ,[Dependent_Condition] = 'Group_Leader_Status_ID = 4'
 WHERE Process_ID = @ProcessID
GO
