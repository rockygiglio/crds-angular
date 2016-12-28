USE MinistryPlatform
GO

IF EXISTS(SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'cr_Rule_Registrations' AND COLUMN_NAME = 'Minimum_Registrants')
BEGIN
	ALTER TABLE [dbo].[cr_Rule_Registrations]
	DROP COLUMN Minimum_Registrants
END