USE MinistryPlatform;
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Registration_Equipment_Attributes] (
	[Registration_Equipment_ID] [int] IDENTITY(1,1) NOT NULL,
	[Registration_ID] [int] NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Notes] nvarchar(255) NULL,
	[Domain_ID] [int] NOT NULL
CONSTRAINT [PK_Registration_Equipment] PRIMARY KEY CLUSTERED 
(
	[Registration_Equipment_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Equipment_Registration] FOREIGN KEY([Registration_ID])
REFERENCES [dbo].[cr_Registrations] ([Registration_ID])
GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes] CHECK CONSTRAINT [FK_Registration_Equipment_Registration]
GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Equipment_Attribute] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes] CHECK CONSTRAINT [FK_Registration_Equipment_Attribute]
GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Equipment_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Registration_Equipment_Attributes] CHECK CONSTRAINT [FK_Registration_Equipment_dp_Domains]
GO