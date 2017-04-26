USE MinistryPlatform
GO

UPDATE [dbo].[Participants]
SET Group_Leader_Status_ID = 4
WHERE Approved_Small_Group_Leader = 1

ALTER TABLE [dbo].[Participants]
DROP COLUMN Approved_Small_Group_Leader