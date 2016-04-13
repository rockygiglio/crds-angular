USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Other_Organizations'))
BEGIN
	ALTER TABLE [dbo].[cr_Other_Organizations]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Other_Organizations_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Other_Organizations]'))
ALTER TABLE [dbo].[cr_Other_Organizations]  WITH CHECK ADD  CONSTRAINT [FK_Other_Organizations_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO