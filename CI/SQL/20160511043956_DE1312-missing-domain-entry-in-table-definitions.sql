USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Destinations'))
BEGIN
	ALTER TABLE [dbo].[cr_Destinations]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Destinations_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Onboarding_Statuses]'))

ALTER TABLE [dbo].[cr_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Destinations] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID]);
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Onboarding_Statuses'))
BEGIN
	ALTER TABLE [dbo].[cr_Onboarding_Statuses]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Onboarding_Statuses_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Onboarding_Statuses]'))
ALTER TABLE [dbo].[cr_Onboarding_Statuses]  WITH CHECK ADD  CONSTRAINT [FK_Onboarding_Statuses] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Other_Organizations'))
BEGIN
	ALTER TABLE [dbo].[cr_Other_Organizations]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Other_Organizations_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Other_Organizations]'))
ALTER TABLE [dbo].[cr_Other_Organizations]  WITH CHECK ADD  CONSTRAINT [FK_Other_Organizations] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])

GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Reminder_Days_Prior'))
BEGIN
	ALTER TABLE [dbo].[cr_Reminder_Days_Prior]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reminder_Days_Prior_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Reminder_Days_Prior]'))
ALTER TABLE [dbo].[cr_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_Days_Prior] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Sign_Up_Deadline'))
BEGIN
	ALTER TABLE [dbo].[cr_Sign_Up_Deadline]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sign_Up_Deadline_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Sign_Up_Deadline]'))
ALTER TABLE [dbo].[cr_Sign_Up_Deadline]  WITH CHECK ADD  CONSTRAINT [FK_Sign_Up_Deadline] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Serve_Restrictions_Reasons'))
BEGIN
	ALTER TABLE [dbo].[cr_Serve_Restrictions_Reasons]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Serve_Restrictions_Reasons_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions_Reasons]'))
ALTER TABLE [dbo].[cr_Serve_Restrictions_Reasons]  WITH CHECK ADD  CONSTRAINT [FK_Serve_Restrictions_Reasons] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Serve_Restrictions_Reasons_Domain'))
BEGIN
	ALTER TABLE [dbo].[cr_Serve_Restrictions_Reasons]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Serve_Restriction_Reasons_Domain_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions_Reasons]'))
ALTER TABLE [dbo].[cr_Serve_Restrictions_Reasons]  WITH CHECK ADD  CONSTRAINT [FK_Serve_Restrictions_Reasons_Domain] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Work_Teams'))
BEGIN
	ALTER TABLE [dbo].[cr_Work_Teams]
	ADD Domain_ID [int] NOT NULL DEFAULT(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Work_Teams_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Work_Teams]'))
ALTER TABLE [dbo].[cr_Work_Teams]  WITH CHECK ADD  CONSTRAINT [FK_Work_Teams] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO