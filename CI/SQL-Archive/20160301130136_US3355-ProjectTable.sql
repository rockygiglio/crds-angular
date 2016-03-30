USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Projects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Projects](
	[Project_ID] [int] IDENTITY(1,1) NOT NULL,
	[Project_Name] nvarchar(100) NOT NULL,
	[Project_Status_ID] [int] NOT NULL,
	[Location_ID] [int] NOT NULL,
	[Project_Type_ID] [int] NOT NULL,
	[Organization_ID] [int] NOT NULL,
	[Initiative_ID] [int] NOT NULL,
	[Minimum_Volunteers] [int] NOT NULL,
	[Maximum_Volunteers] [int] NOT NULL,
	[Absolute_Maximum_Volunteers] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Project_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--Project Status 
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Project_Statuses]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Project_Statuses] FOREIGN KEY([Project_Status_ID])
REFERENCES [dbo].[cr_Project_Statuses] ([Project_Status_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Project_Statuses]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Project_Statuses]
GO

--Locations 
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Locations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Locations] FOREIGN KEY([Location_ID])
REFERENCES [dbo].[Locations] ([Location_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Locations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Locations]
GO

--ProjectTypes
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Project_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Project_Types] FOREIGN KEY([Project_Type_ID])
REFERENCES [dbo].[cr_Project_Types] ([Project_Type_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Project_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Project_Types]
GO

--Organizations
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Organizations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Organizations] FOREIGN KEY([Organization_ID])
REFERENCES [dbo].[cr_Organizations] ([Organization_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Organizations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Organizations]
GO

--Initiatives
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Initiatives]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Initiatives] FOREIGN KEY([Initiative_ID])
REFERENCES [dbo].[cr_Initiatives] ([Initiative_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Initiatives]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Initiatives]
GO

--Domains
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Projects]'))
ALTER TABLE [dbo].[cr_Projects] CHECK CONSTRAINT [FK_Projects_Domains]
GO

