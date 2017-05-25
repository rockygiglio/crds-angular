use MinistryPlatform

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'cr_Childcare_Preferred_Times')
   AND NOT EXISTS(select top 10 * from sys.columns  WHERE Name = N'Deactivate_Date' AND Object_ID = Object_ID(N'cr_Childcare_Preferred_Times'))
BEGIN
	ALTER TABLE dbo.cr_Childcare_Preferred_Times ADD Deactivate_Date DATETIME NULL;
END
GO

