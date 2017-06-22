USE MinistryPlatform
GO

UPDATE [dbo].[Participants]
SET Group_Leader_Status_ID = 4
WHERE Approved_Small_Group_Leader = 1

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Participa__Appro__566CF21A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [DF__Participa__Appro__566CF21A]
END

ALTER TABLE [dbo].[Participants]
DROP COLUMN Approved_Small_Group_Leader