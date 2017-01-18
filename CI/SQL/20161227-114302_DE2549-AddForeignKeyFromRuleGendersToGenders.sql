USE MinistryPlatform
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Rule_Genders_Genders]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Rule_Genders]'))
ALTER TABLE [dbo].[cr_Rule_Genders]  WITH CHECK ADD  CONSTRAINT [FK_cr_Rule_Genders_Genders] FOREIGN KEY([Gender_ID])
REFERENCES [dbo].[Genders] ([Gender_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Rule_Genders_Genders]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Rule_Genders]'))
ALTER TABLE [dbo].[cr_Rule_Genders] CHECK CONSTRAINT [FK_cr_Rule_Genders_Genders]
GO