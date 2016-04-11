USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Cancelled' AND Object_ID = Object_ID(N'cr_Registrations'))
BEGIN
	ALTER TABLE [dbo].[cr_Registrations]
	ADD Cancelled bit 
END