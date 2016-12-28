USE MinistryPlatform
GO

IF (SELECT IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='cr_Rule_Registrations' AND COLUMN_NAME='Rule_Start_Date') = 'YES'
BEGIN
	--Make sure there are no null values
	UPDATE [dbo].[cr_Rule_Registrations]
	SET Rule_Start_Date = GETDATE()
	WHERE Rule_Start_Date IS NULL

	--Make it non-nullable
	ALTER TABLE [dbo].[cr_Rule_Registrations]
	ALTER COLUMN Rule_Start_Date datetime NOT NULL
END