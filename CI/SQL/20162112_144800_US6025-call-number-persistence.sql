-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 12/21/2016
-- Description:	Add call number to track participants across multiple events
-- ===============================================================

USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Call_Number' AND Object_ID = Object_ID(N'dbo.Event_Participants'))
BEGIN
	ALTER TABLE dbo.Event_Participants ADD
		Call_Number INT NULL
	
	DECLARE @v1 sql_variant 
	SET @v1 = N'Persistent call number to notify guardians.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v1, N'SCHEMA', N'dbo', N'TABLE', N'Event_Participants', N'COLUMN', N'Call_Number'
END
