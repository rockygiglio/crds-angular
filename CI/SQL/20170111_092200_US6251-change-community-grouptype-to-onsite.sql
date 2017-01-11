USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Group_Types] WHERE [Group_Type] = 'Onsite Group') 
Begin
update Group_Types
set Group_Type = 'Onsite Group'
where Group_Type_ID = 8
End