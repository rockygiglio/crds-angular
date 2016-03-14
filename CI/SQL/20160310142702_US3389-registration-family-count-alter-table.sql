

USE [MinistryPlatform]
GO
ALTER TABLE dbo.cr_Registrations ADD
	_FamilyCount AS dbo.crds_GoVolunteerFamilyCount(Registration_ID)
GO
