USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Allergy]    Script Date: 10/6/2016 3:20:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Allergy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Allergy](
	[Allergy_ID] [int] IDENTITY(1,1) NOT NULL,
	[Allergy_Type_ID] [int] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Reaction] [nvarchar](200) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Allergy] PRIMARY KEY CLUSTERED (
	[Allergy_ID] ASC
 )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Allergy_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy]'))
ALTER TABLE [dbo].[cr_Allergy]  WITH CHECK ADD  CONSTRAINT [FK_cr_Allergy_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Allergy_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy]'))
ALTER TABLE [dbo].[cr_Allergy] CHECK CONSTRAINT [FK_cr_Allergy_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Allergy_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy]'))
ALTER TABLE [dbo].[cr_Allergy]  WITH CHECK ADD  CONSTRAINT [FK_cr_Allergy_Types] FOREIGN KEY([Allergy_Type_ID])
REFERENCES [dbo].[cr_Allergy_Types] ([Allergy_Type_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Allergy_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy]'))
ALTER TABLE [dbo].[cr_Allergy] CHECK CONSTRAINT [FK_cr_Allergy_Types]
GO


