USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_GroupConnectors]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_GroupConnectors](
	[GroupConnector_ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	[Project_ID] [int] NULL,
	[Location_ID] [int] NOT NULL,
	[Organization_ID] [int] NOT NULL,
	[Initiative_ID] [int] NOT NULL,
	[Primary_Contact] INT NOT NULL, 
	[Domain_ID] [int] NOT NULL,
	CONSTRAINT [FK_GroupConnector_Project] FOREIGN KEY ([Project_ID]) REFERENCES [cr_Projects]([Project_ID]), 
	CONSTRAINT [FK_GroupConnector_Initiative] FOREIGN KEY ([Initiative_ID]) REFERENCES [cr_Initiatives]([Initiative_ID]), 
	CONSTRAINT [FK_GroupConnector_Location] FOREIGN KEY (Location_ID) REFERENCES Locations(Location_ID), 
	CONSTRAINT [FK_GroupConnector_Organization] FOREIGN KEY (Organization_ID) REFERENCES cr_Organizations(Organization_ID), 
	CONSTRAINT [FK_GroupConnector_Contacts] FOREIGN KEY ([Primary_Contact]) REFERENCES [Contacts]([Contact_ID]),
	CONSTRAINT [PK_GroupConnector] PRIMARY KEY CLUSTERED([GroupConnector_ID] ASC)
)
END