USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Other_Organization_Name' AND Object_ID = Object_ID(N'cr_Registrations'))
BEGIN
	ALTER TABLE [dbo].[cr_Registrations]
	ADD Other_Organization_Name nvarchar(100)
END