USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'_Minimum_Age' AND Object_ID = Object_ID(N'cr_GroupConnectors'))
BEGIN
	ALTER TABLE [dbo].[cr_GroupConnectors]
	ADD [_Minimum_Age] AS ([dbo].[crds_GoVolunteerGroupConnectMinAge](Primary_Registration, Project_ID))
END