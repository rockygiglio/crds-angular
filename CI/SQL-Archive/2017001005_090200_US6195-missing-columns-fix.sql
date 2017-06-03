-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 1/5/2017
-- Description:	Add checkin data to Event Participants for Guest Checkins
-- ===============================================================

USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Checkin_Phone' AND Object_ID = Object_ID(N'dbo.Event_Participants'))
BEGIN
	ALTER TABLE dbo.Event_Participants ADD
		Checkin_Phone VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Checkin_Household_ID' AND Object_ID = Object_ID(N'dbo.Event_Participants'))
BEGIN
	ALTER TABLE dbo.Event_Participants ADD
		Checkin_Household_ID VARCHAR(50) NULL
END