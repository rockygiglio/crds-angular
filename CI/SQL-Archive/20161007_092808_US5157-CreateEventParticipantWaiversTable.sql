USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Event_Participant_Waivers]    Script Date: 10/7/2016 9:28:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Event_Participant_Waivers](
	[Event_Participant_Waiver_ID] [int] IDENTITY(1,1) NOT NULL,
	[Waiver_ID] [int] NOT NULL,
	[Event_Participant_ID] [int] NOT NULL,
	[Accepted] [bit] NOT NULL DEFAULT 0,
	[Signee] [int] NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Event_Participant_Waivers] PRIMARY KEY CLUSTERED 
(
	[Event_Participant_Waiver_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Waivers]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers]  WITH CHECK ADD  CONSTRAINT [FK_Event_Participant_Waivers_Waivers] FOREIGN KEY([Waiver_ID])
REFERENCES [dbo].[cr_Waivers] ([Waiver_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Waivers]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers] CHECK CONSTRAINT [FK_Event_Participant_Waivers_Waivers]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Event_Participants]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers]  WITH CHECK ADD  CONSTRAINT [FK_Event_Participant_Waivers_Event_Participants] FOREIGN KEY([Event_Participant_ID])
REFERENCES [dbo].[Event_Participants] ([Event_Participant_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Event_Participants]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers] CHECK CONSTRAINT [FK_Event_Participant_Waivers_Event_Participants]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers]  WITH CHECK ADD  CONSTRAINT [FK_Event_Participant_Waivers_Contacts] FOREIGN KEY([Signee])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers] CHECK CONSTRAINT [FK_Event_Participant_Waivers_Contacts]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers]  WITH CHECK ADD  CONSTRAINT [FK_Event_Participant_Waivers_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Participant_Waivers_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Event_Participant_Waivers]'))
ALTER TABLE [dbo].[cr_Event_Participant_Waivers] CHECK CONSTRAINT [FK_Event_Participant_Waivers_Domains]
GO
