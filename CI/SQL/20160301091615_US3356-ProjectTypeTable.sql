USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_ProjectTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_ProjectTypes](
	[ProjectType_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] nvarchar(100) NOT NULL,
	[Minimum_Age] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_ProjectTypes] PRIMARY KEY CLUSTERED 
(
	[ProjectType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectTypes_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_ProjectTypes]'))
ALTER TABLE [dbo].[cr_ProjectTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTypes_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectTypes_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_ProjectTypes]'))
ALTER TABLE [dbo].[cr_ProjectTypes] CHECK CONSTRAINT [FK_ProjectTypes_Domains]
GO