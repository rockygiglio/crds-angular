USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Organization_Locations](
	[Organization_Location_ID] [int] IDENTITY(1,1) NOT NULL,	
	[Domain_ID] [int] NOT NULL,
	[Organization_ID] [int] NOT NULL,
	[Location_ID] [int] NOT NULL
 CONSTRAINT [PK_Organization_Locations] PRIMARY KEY CLUSTERED 
(
	[Organization_Location_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Organization_Locations]  WITH CHECK ADD  CONSTRAINT [FK_Organization_Location_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Organization_Locations] CHECK CONSTRAINT [FK_Organization_Location_Domains]
GO

ALTER TABLE [dbo].[cr_Organization_Locations]  WITH CHECK ADD  CONSTRAINT [FK_Organization_Locations_Organizations] FOREIGN KEY([Organization_ID])
REFERENCES [dbo].[cr_Organizations] ([Organization_ID])
GO

ALTER TABLE [dbo].[cr_Organization_Locations] CHECK CONSTRAINT [FK_Organization_Locations_Organizations]
GO

ALTER TABLE [dbo].[cr_Organization_Locations]  WITH CHECK ADD  CONSTRAINT [FK_Organization_Locations_Locations] FOREIGN KEY([Location_ID])
REFERENCES [dbo].[Locations] ([Location_ID])
GO

ALTER TABLE [dbo].[cr_Organization_Locations] CHECK CONSTRAINT [FK_Organization_Locations_Locations]
GO