USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'Participants')
   AND NOT EXISTS(select top 1 * from sys.columns  WHERE Name = N'Approved_Host' AND Object_ID = Object_ID(N'Participants'))
BEGIN	
    ALTER TABLE [dbo].[Participants]
    ADD Approved_Host BIT NOT NULL DEFAULT(0)
END
GO