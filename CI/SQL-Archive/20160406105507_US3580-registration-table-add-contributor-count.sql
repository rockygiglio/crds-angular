USE [MinistryPlatform]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Registrations]') AND type in (N'U'))
BEGIN
	ALTER TABLE [dbo].[cr_Registrations]
		ADD [_Contributor_Count]  AS ([dbo].[crds_GoVolunteerContributorCount]([Registration_ID]))
END