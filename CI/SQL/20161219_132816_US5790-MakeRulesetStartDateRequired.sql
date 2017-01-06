USE MinistryPlatform
GO

IF (SELECT IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='cr_Ruleset' AND COLUMN_NAME='Ruleset_Start_Date') = 'YES'
BEGIN
	--Make sure there are no null values
	UPDATE [dbo].[cr_Ruleset]
	SET Ruleset_Start_Date = GETDATE()
	WHERE Ruleset_Start_Date IS NULL

	--Make it non-nullable
	ALTER TABLE [dbo].[cr_Ruleset]
	ALTER COLUMN Ruleset_Start_Date datetime NOT NULL
END