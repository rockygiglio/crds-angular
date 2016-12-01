USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Medical_Information_Allergies]    Script Date: 10/7/2016 9:26:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medical_Information_Allergies](
	[Medical_Information_Allergy_ID] [int] IDENTITY(1,1) NOT NULL,
	[Medical_Information_ID] [int] NOT NULL,
	[Allergy_ID] [int] NOT NULL,	
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Medical_Information_Allergies] PRIMARY KEY CLUSTERED 
(
	[Medical_Information_Allergy_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Allergies_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies] CHECK CONSTRAINT [FK_cr_Medical_Information_Allergies_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_Medical_Information]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Allergies_Medical_Information] FOREIGN KEY([Medical_Information_ID])
REFERENCES [dbo].[cr_Medical_Information] ([MedicalInformation_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_Medical_Information]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies] CHECK CONSTRAINT [FK_cr_Medical_Information_Allergies_Medical_Information]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_Allergy]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Allergies_Allergy] FOREIGN KEY([Allergy_ID])
REFERENCES [dbo].[cr_Allergy] ([Allergy_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Allergies_Allergy]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Allergies]'))
ALTER TABLE [dbo].[cr_Medical_Information_Allergies] CHECK CONSTRAINT [FK_cr_Medical_Information_Allergies_Allergy]
GO


