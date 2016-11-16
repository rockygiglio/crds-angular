USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Rule_Genders]    Script Date: 11/16/2016 10:08:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Rule_Genders](
	[Rule_Gender_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ruleset_ID] [int] NOT NULL,
	[Gender_ID] [int] NOT NULL,
	[Rule_Start_Date] [DateTime] NULL,
	[Rule_End_Date] [DateTime] NULL,
	[Domain_ID] [int] NOT NULL
 CONSTRAINT [PK_cr_Rule_Genders] PRIMARY KEY CLUSTERED 
(
	[Rule_Gender_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Rule_Genders]  WITH CHECK ADD  CONSTRAINT [FK_cr_Rule_Genders_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Rule_Genders] CHECK CONSTRAINT [FK_cr_Rule_Genders_dp_Domains]
GO

ALTER TABLE [dbo].[cr_Rule_Genders]  WITH CHECK ADD  CONSTRAINT [FK_cr_Rule_Genders_Genders] FOREIGN KEY([Gender_ID])
REFERENCES [dbo].[Genders] ([Gender_ID])
GO

ALTER TABLE [dbo].[cr_Rule_Genders] CHECK CONSTRAINT [FK_cr_Rule_Genders_Genders]
GO

ALTER TABLE [dbo].[cr_Rule_Genders]  WITH CHECK ADD  CONSTRAINT [FK_cr_Rule_Genders_cr_Ruleset] FOREIGN KEY([Ruleset_ID])
REFERENCES [dbo].[cr_Ruleset] ([Ruleset_ID])
GO

ALTER TABLE [dbo].[cr_Rule_Genders] CHECK CONSTRAINT [FK_cr_Rule_Genders_cr_Ruleset]
GO

