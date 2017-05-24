-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 4/19/2017
-- Description:	Add guest column to Event Participants for Guest Checkins
-- ===============================================================

USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Guest_Sign_In' AND Object_ID = Object_ID(N'dbo.Event_Participants'))
BEGIN
	ALTER TABLE dbo.Event_Participants ADD
		Guest_Sign_In BIT NULL
END
