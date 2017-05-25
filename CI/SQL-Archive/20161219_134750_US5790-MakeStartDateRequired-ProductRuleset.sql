USE MinistryPlatform
GO

IF (SELECT IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='cr_Product_Ruleset' AND COLUMN_NAME='Start_Date') = 'YES'
BEGIN
	--Make sure there are no null values
	UPDATE [dbo].[cr_Product_Ruleset]
	SET Start_Date = GETDATE()
	WHERE Start_Date IS NULL

	--Make it non-nullable
	ALTER TABLE [dbo].[cr_Product_Ruleset]
	ALTER COLUMN Start_Date datetime NOT NULL
END