USE [MinistryPlatform]
GO

EXEC sp_rename 'cr_GroupConnectors.Primary_Contact', 'Primary_Registration', 'COLUMN';

ALTER TABLE [dbo].[cr_GroupConnectors] DROP CONSTRAINT [FK_GroupConnector_Contacts]
GO

ALTER TABLE [dbo].[cr_GroupConnectors]  WITH CHECK ADD  CONSTRAINT [FK_GroupConnector_Registration] FOREIGN KEY([Primary_Registration])
REFERENCES [dbo].[cr_Registrations] ([Registration_ID])
GO

ALTER TABLE [dbo].[cr_GroupConnectors] CHECK CONSTRAINT [FK_GroupConnector_Registration]
GO
