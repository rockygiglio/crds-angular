USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.cr_GroupConnectors', 'U') IS NULL
BEGIN

    CREATE TABLE [dbo].[cr_GroupConnectors](
	    [GroupConnector_ID] [int] IDENTITY(1,1) NOT NULL,
	    [Project_ID] [int] NULL,
	    [Primary_Registration] INT NOT NULL, 
	    [Domain_ID] [int] NOT NULL,
	    CONSTRAINT [FK_GroupConnector_Project] FOREIGN KEY ([Project_ID]) REFERENCES [cr_Projects]([Project_ID]),  
	    CONSTRAINT [FK_GroupConnector_PrimaryRegistration] FOREIGN KEY ([Primary_Registration]) REFERENCES [cr_Registrations]([Registration_ID]),
	    CONSTRAINT [FK_GroupConnector_Domains] FOREIGN KEY ([Domain_ID]) REFERENCES [dp_Domains]([Domain_ID]),
	    CONSTRAINT [PK_GroupConnector] PRIMARY KEY CLUSTERED([GroupConnector_ID] ASC)
    )

END



-- DOWN
/*
USE [MinistryPlatform]
GO

DROP TABLE [dbo].[cr_GroupConnectorRegistrations]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [PK_GroupConnector]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Domains]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_PrimaryRegistration]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Project]
GO

DROP TABLE [dbo].[cr_GroupConnectors]
GO
*/
