USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Enrolled_By' AND Object_ID = Object_ID(N'dbo.GroupParticipants'))
BEGIN
	ALTER TABLE dbo.Group_Participants ADD
		Enrolled_By int NULL
	
	DECLARE @v sql_variant 
	SET @v = N'The Group Participant who enrolled this participant.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Group_Participants', N'COLUMN', N'Enrolled_By'
	
	IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_Participant_Group_Participant]') AND parent_object_id = OBJECT_ID(N'[dbo].[Group_Participants]'))
	ALTER TABLE [dbo].[Group_Participants]  WITH CHECK ADD  CONSTRAINT [FK_Group_Participant_Group_Participant] FOREIGN KEY([Enrolled_By])
	REFERENCES [dbo].[Group_Participants] ([Group_Participant_ID])
	
	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_Participant_Group_Participant]') AND parent_object_id = OBJECT_ID(N'[dbo].[Group_Participants]'))
	ALTER TABLE [dbo].[Group_Participants] CHECK CONSTRAINT [FK_Group_Participant_Group_Participant]
END