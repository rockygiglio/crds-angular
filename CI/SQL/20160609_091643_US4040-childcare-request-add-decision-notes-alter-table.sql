USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Decision_Notes' AND Object_ID = Object_ID(N'cr_Childcare_Requests'))
BEGIN

	ALTER TABLE dbo.cr_Childcare_Requests ADD
		Decision_Notes VARCHAR(2000)
END
GO
