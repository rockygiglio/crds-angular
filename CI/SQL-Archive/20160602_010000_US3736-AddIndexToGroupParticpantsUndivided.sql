USE [MinistryPlatform]
GO

/****** Object:  Index [IX_GroupParticipants_GroupID_RoleID]    Script Date: 6/2/2016 5:30:00 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Group_Participants]') AND name = N'IX_GroupParticipants_GroupID_RoleID')
DROP INDEX [IX_GroupParticipants_GroupID_RoleID] ON [dbo].[Group_Participants]
GO

/****** Object:  Index [IX_GroupParticipants_GroupID_RoleID]    Script Date: 6/2/2016 5:30:00 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Group_Participants]') AND name = N'IX_GroupParticipants_GroupID_RoleID')
CREATE NONCLUSTERED INDEX [IX_GroupParticipants_GroupID_RoleID] ON [dbo].[Group_Participants]
(
    [Group_ID] ASC,
    [Group_Role_ID] ASC
)
INCLUDE (     [Participant_ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO