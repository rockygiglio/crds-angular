USE [MinistryPlatform];

CREATE TABLE [dbo].[Group_Participant_Attributes](
	[Group_Participant_Attribute_ID] [int] IDENTITY(1,1) NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Group_Participant_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Start_Date] [datetime] NOT NULL,
	[End_Date] [datetime] NULL,
	[Notes] [nvarchar](255) NULL,
	[Order] [int] NULL,
 CONSTRAINT [PK_Group_Participant_Attributes] PRIMARY KEY CLUSTERED
(
	[Group_Participant_Attribute_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [dbo].[Group_Participant_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Participant_Attributes_Attributes] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID]);

ALTER TABLE [dbo].[Group_Participant_Attributes] CHECK CONSTRAINT [FK_Group_Participant_Attributes_Attributes];

ALTER TABLE [dbo].[Group_Participant_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Participant_Attributes_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID]);

ALTER TABLE [dbo].[Group_Participant_Attributes] CHECK CONSTRAINT [FK_Group_Participant_Attributes_dp_Domains];

ALTER TABLE [dbo].[Group_Participant_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Participant_Attributes_Group_Participants] FOREIGN KEY([Group_Participant_ID])
REFERENCES [dbo].[Group_Participants] ([Group_Participant_ID]);

ALTER TABLE [dbo].[Group_Participant_Attributes] CHECK CONSTRAINT [FK_Group_Participant_Attributes_Group_Participants];

--Migrate down
--DROP TABLE [dbo].[Group_Participant_Attributes];