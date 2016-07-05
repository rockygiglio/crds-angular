USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Invitations]    Script Date: 6/30/2016 2:48:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cr_Invitations](
	[Invitation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Source_ID] [int] NOT NULL,
	[Invitation_Type_ID] [int] NOT NULL,
	[Email_Address] [dbo].[dp_Email] NOT NULL,
	[Recipient_Name] [nvarchar](75) NOT NULL,
	[Group_Role_ID] [int] NULL,
	[Invitation_GUID] [uniqueidentifier] NOT NULL,
	[Invitation_Used] [bit] NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Invitations] PRIMARY KEY CLUSTERED 
(
	[Invitation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[cr_Invitations] ADD  CONSTRAINT [DF_cr_Invitations_Invitation_GUID]  DEFAULT (newid()) FOR [Invitation_GUID]
GO

ALTER TABLE [dbo].[cr_Invitations] ADD  CONSTRAINT [DF_cr_Invitations_Invitation_Used]  DEFAULT ((0)) FOR [Invitation_Used]
GO

ALTER TABLE [dbo].[cr_Invitations] ADD  CONSTRAINT [DF_cr_Invitations_Domain_ID]  DEFAULT ((1)) FOR [Domain_ID]
GO

ALTER TABLE [dbo].[cr_Invitations]  WITH CHECK ADD  CONSTRAINT [FK_cr_Invitations_cr_Invitation_Types] FOREIGN KEY([Invitation_Type_ID])
REFERENCES [dbo].[cr_Invitation_Types] ([Invitation_Type_ID])
GO

ALTER TABLE [dbo].[cr_Invitations] CHECK CONSTRAINT [FK_cr_Invitations_cr_Invitation_Types]
GO

ALTER TABLE [dbo].[cr_Invitations]  WITH CHECK ADD  CONSTRAINT [FK_cr_Invitations_Dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Invitations] CHECK CONSTRAINT [FK_cr_Invitations_Dp_Domains]
GO

ALTER TABLE [dbo].[cr_Invitations] WITH CHECK ADD CONSTRAINT [FK_cr_Invitations_Group_Roles] FOREIGN KEY([Group_Role_ID])
REFERENCES [dbo].[Group_Roles] ([Group_Role_ID])
GO

ALTER TABLE [dbo].[cr_Invitations] CHECK CONSTRAINT [FK_cr_Invitations_Group_Roles]
GO


