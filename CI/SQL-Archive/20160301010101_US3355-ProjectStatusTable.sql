USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_ProjectStatuses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Project_Statuses](
	[Project_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] nvarchar(100) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Project_Statuses] PRIMARY KEY CLUSTERED 
(
	[Project_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--Domains
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Project_Statuses_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Project_Statuses]'))
ALTER TABLE [dbo].[cr_Project_Statuses]  WITH CHECK ADD  CONSTRAINT [FK_Project_Statuses_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Project_Statuses_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Project_Statuses]'))
ALTER TABLE [dbo].[cr_Project_Statuses] CHECK CONSTRAINT [FK_Project_Statuses_Domains]
GO

