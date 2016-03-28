USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Initiatives]    Script Date: 3/24/2016 1:58:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Go_Volunteer_Skills](
	[Go_Volunteer_Skills_ID] [int] IDENTITY(1,1) NOT NULL,	
	[Domain_ID] [int] NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Label] [nvarchar] (255) NULL
 CONSTRAINT [PK_Go_Volunteer_Skills] PRIMARY KEY CLUSTERED 
(
	[Go_Volunteer_Skills_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Go_Volunteer_Skills]  WITH CHECK ADD  CONSTRAINT [FK_Go_Volunteer_Skills_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Go_Volunteer_Skills] CHECK CONSTRAINT [FK_Go_Volunteer_Skills_Domains]
GO

ALTER TABLE [dbo].[cr_Go_Volunteer_Skills]  WITH CHECK ADD  CONSTRAINT [FK_Go_Volunteer_Skills_Attribute] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[cr_Go_Volunteer_Skills] CHECK CONSTRAINT [FK_Go_Volunteer_Skills_Attribute]
GO


