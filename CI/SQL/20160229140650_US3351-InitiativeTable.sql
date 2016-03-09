USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Initiatives]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Initiatives](
	[Initiative_ID] [int] IDENTITY(1,1) NOT NULL,
	[Initiative_Name] nvarchar(100) NOT NULL,
	[Program_ID] [int] NOT NULL,
	[Start_Date] [date] NOT NULL,
	[End_Date] [date] NULL,
	[Leader_Signup_Start_Date] [datetime] NULL,
	[Leader_Signup_End_Date] [datetime] NULL,
	[Volunteer_Signup_Start_Date] [datetime] NULL,
	[Volunteer_Signup_End_Date] [datetime] NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Initiatives] PRIMARY KEY CLUSTERED 
(
	[Initiative_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Initiatives_Program]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Initiatives]'))
ALTER TABLE [dbo].[cr_Initiatives]  WITH CHECK ADD  CONSTRAINT [FK_Initiatives_Program] FOREIGN KEY([Program_ID])
REFERENCES [dbo].[Programs] ([Program_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Initiatives_Program]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Initiatives]'))
ALTER TABLE [dbo].[cr_Initiatives] CHECK CONSTRAINT [FK_Initiatives_Program]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Initiatives_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Initiatives]'))
ALTER TABLE [dbo].[cr_Initiatives]  WITH CHECK ADD  CONSTRAINT [FK_Initiatives_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Initiatives_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Initiatives]'))
ALTER TABLE [dbo].[cr_Initiatives] CHECK CONSTRAINT [FK_Initiatives_Domains]
GO