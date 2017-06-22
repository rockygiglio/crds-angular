USE MinistryPlatform
GO

ALTER TABLE dbo.cr_Registrations DROP COLUMN _Family_Count;

ALTER TABLE dbo.cr_Registrations
    ADD [_Family_Count]  AS ([dbo].[crds_GoVolunteerFamilyCount](Registration_ID));

GO

