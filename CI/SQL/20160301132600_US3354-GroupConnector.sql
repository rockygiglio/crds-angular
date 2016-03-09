USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_GroupConnectors](
	[GroupConnector_ID] [int] IDENTITY(1,1) NOT NULL,
	[Project_ID] [int] NULL,
	[Created_By] INT NOT NULL, 
	[Domain_ID] [int] NOT NULL,
	CONSTRAINT [FK_GroupConnector_Project] FOREIGN KEY ([Project_ID]) REFERENCES [cr_Projects]([Project_ID]),  
	CONSTRAINT [FK_GroupConnector_CreatedBy] FOREIGN KEY ([Created_By]) REFERENCES [cr_Registrations]([Registration_ID]),
	CONSTRAINT [FK_GroupConnector_Domains] FOREIGN KEY ([Domain_ID]) REFERENCES [dp_Domains]([Domain_ID]),
	CONSTRAINT [PK_GroupConnector] PRIMARY KEY CLUSTERED([GroupConnector_ID] ASC)
)



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

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Initiative]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Organization]
GO

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Project]
GO

DROP TABLE [dbo].[cr_GroupConnectors]
GO
*/
