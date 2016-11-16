USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Poduct_RuleSet]    Script Date: 11/16/2016 10:08:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Product_Ruleset](
	[Product_Ruleset_ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Ruleset_ID] [int] NOT NULL,
	[Start_Date] [DateTime] NULL,
	[End_Date] [DateTime] NULL,
	[Domain_ID] [int] NOT NULL
 CONSTRAINT [PK_cr_Product_Ruleset] PRIMARY KEY CLUSTERED 
(
	[Product_Ruleset_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Product_Ruleset]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Ruleset_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Product_Ruleset] CHECK CONSTRAINT [FK_cr_Product_Ruleset_dp_Domains]
GO

ALTER TABLE [dbo].[cr_Product_Ruleset]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Ruleset_Products] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
GO

ALTER TABLE [dbo].[cr_Product_Ruleset] CHECK CONSTRAINT [FK_cr_Product_Ruleset_Products]
GO

ALTER TABLE [dbo].[cr_Product_Ruleset]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Ruleset_cr_Ruleset] FOREIGN KEY([Ruleset_ID])
REFERENCES [dbo].[cr_Ruleset] ([Ruleset_ID])
GO

ALTER TABLE [dbo].[cr_Product_Ruleset] CHECK CONSTRAINT [FK_cr_Product_Ruleset_cr_Ruleset]
GO