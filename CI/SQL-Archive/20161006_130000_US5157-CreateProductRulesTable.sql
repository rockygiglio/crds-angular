USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Product_Rules]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Product_Rules](
	[ProductRule_ID] [INT] IDENTITY(1,1) NOT NULL,
	[Product_Rule_Name] NVARCHAR(64) NOT NULL,
	[Product_ID] [INT] NOT NULL,
	[Gender_ID] [INT] NULL,
	[Congregation_ID] [INT] NOT NULL,
	[Maximum_Quantity] [INT] NOT NULL,
	[Wait_List_Quantity] [INT] NULL,
	[Domain_ID] [INT] NOT NULL,
 CONSTRAINT [PK_cr_Product_Rules] PRIMARY KEY CLUSTERED 
(
	[ProductRule_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[cr_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Rules_Congregations] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])


ALTER TABLE [dbo].[cr_Product_Rules] CHECK CONSTRAINT [FK_cr_Product_Rules_Congregations]


ALTER TABLE [dbo].[cr_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Rules_Genders] FOREIGN KEY([Gender_ID])
REFERENCES [dbo].[Genders] ([Gender_ID])


ALTER TABLE [dbo].[cr_Product_Rules] CHECK CONSTRAINT [FK_cr_Product_Rules_Genders]


ALTER TABLE [dbo].[cr_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Rules_Products] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])


ALTER TABLE [dbo].[cr_Product_Rules] CHECK CONSTRAINT [FK_cr_Product_Rules_Products]

ALTER TABLE [dbo].[cr_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Product_Rules_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Product_Rules] CHECK CONSTRAINT [FK_cr_Product_Rules_dp_Domains]

END

