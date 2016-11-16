USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_RuleSet]    Script Date: 11/16/2016 10:08:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Ruleset](
	[Ruleset_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ruleset_Name] [NVARCHAR] (100) NOT NULL,
	[Description] [NVARCHAR] (100) NULL,
	[Ruleset_Start_Date] [DateTime] NULL,
	[Ruleset_End_Date] [DateTime] NULL,
	[Domain_ID] [int] NOT NULL
 CONSTRAINT [PK_cr_Ruleset] PRIMARY KEY CLUSTERED 
(
	[Ruleset_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Ruleset]  WITH CHECK ADD  CONSTRAINT [FK_cr_Ruleset_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Ruleset] CHECK CONSTRAINT [FK_cr_Ruleset_dp_Domains]
GO