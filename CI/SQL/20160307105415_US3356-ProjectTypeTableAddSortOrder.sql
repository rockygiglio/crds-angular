USE [MinistryPlatform]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Project_Types]') AND type in (N'U'))
BEGIN
	ALTER TABLE [dbo].[cr_Project_Types]
		ADD [SortOrder] [INT] NULL
END