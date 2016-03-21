USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Registration_PrepWork_Attributes]    Script Date: 3/4/2016 4:50:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Registration_PrepWork_Attributes](
	[Registration_PrepWork_Attribute_ID] [int] IDENTITY(1,1) NOT NULL,
	[Registration_ID] [int] NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Spouse] [bit] NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Registration_PrepWork_Attributes] PRIMARY KEY CLUSTERED 
(
	[Registration_PrepWork_Attribute_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_PrepWork_Attributes_Attributes] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes] CHECK CONSTRAINT [FK_Registration_PrepWork_Attributes_Attributes]
GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_PrepWork_Attributes_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes] CHECK CONSTRAINT [FK_Registration_PrepWork_Attributes_dp_Domains]
GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Registration_PrepWork_Attributes_Registration] FOREIGN KEY([Registration_ID])
REFERENCES [dbo].[cr_Registrations] ([Registration_ID])
GO

ALTER TABLE [dbo].[cr_Registration_PrepWork_Attributes] CHECK CONSTRAINT [FK_Registration_PrepWork_Attributes_Registration]
GO