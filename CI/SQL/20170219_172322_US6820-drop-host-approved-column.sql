USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'Participants')
   AND EXISTS(select top 1 * from sys.columns  WHERE Name = N'Approved_Host' AND Object_ID = Object_ID(N'Participants'))
BEGIN	
	ALTER TABLE [dbo].[Participants] 
	DROP CONSTRAINT [DF__Participa__Appro__3319943C]
	ALTER TABLE [dbo].[Participants] 
	DROP COLUMN [Approved_Host]
END
GO