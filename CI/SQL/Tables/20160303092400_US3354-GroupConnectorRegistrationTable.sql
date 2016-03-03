USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_GroupConnectorRegistrations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].cr_GroupConnectorRegistrations(
	GroupConnectorRegistration_ID [int] IDENTITY(1,1) NOT NULL,
	GroupConnector_ID int NOT NULL,
	Registration_ID int NOT NULL,
	Domain_ID int NOT NULL,
	CONSTRAINT [FK_GroupConnectorRegistration_GroupConnector] FOREIGN KEY (GroupConnector_ID) REFERENCES cr_GroupConnectors(GroupConnector_ID), 
	CONSTRAINT [FK_GroupConnectorRegistration_Registration] FOREIGN KEY (Registration_ID) REFERENCES cr_Registrations(Registration_ID), 
	CONSTRAINT [FK_GroupConnectorRegistration_Domains] FOREIGN KEY ([Domain_ID]) REFERENCES [dp_Domains]([Domain_ID]),
	CONSTRAINT [PK_GroupConnectorRegistration] PRIMARY KEY CLUSTERED(GroupConnectorRegistration_ID ASC)
)
END