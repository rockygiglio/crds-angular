USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Waivers]    Script Date: 10/6/2016 2:31:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Waivers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Waivers](
	[Waiver_ID] [int] IDENTITY(1,1) NOT NULL,
	[Waiver_Name] [nvarchar](100) NOT NULL,
	[Waiver_Text] [nvarchar](MAX) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Waivers] PRIMARY KEY CLUSTERED 
(
	[Waiver_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Waivers_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Waivers]'))
ALTER TABLE [dbo].[cr_Waivers]  WITH CHECK ADD  CONSTRAINT [FK_Waivers_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Waivers_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Waivers]'))
ALTER TABLE [dbo].[cr_Waivers] CHECK CONSTRAINT [FK_Waivers_Domains]
GO
