USE [MinistryPlatform]
GO

--Removing FK_cr_Medical_Information_Allergies_Allergy from table cr_Medical_Information_Allergies
ALTER TABLE [dbo].[cr_Medical_Information_Allergies] DROP CONSTRAINT [FK_cr_Medical_Information_Allergies_Allergy]
GO

ALTER TABLE [dbo].[cr_Allergy] DROP CONSTRAINT [FK_cr_Allergy_Types]
GO

ALTER TABLE [dbo].[cr_Allergy] DROP CONSTRAINT [FK_cr_Allergy_dp_Domains]
GO

DROP TABLE [dbo].[cr_Allergy]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Allergy](
	[Allergy_ID] [int] IDENTITY(1,1) NOT NULL,
	[Allergy_Type_ID] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Allergy] PRIMARY KEY CLUSTERED 
(
	[Allergy_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Allergy]  WITH CHECK ADD  CONSTRAINT [FK_cr_Allergy_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Allergy] CHECK CONSTRAINT [FK_cr_Allergy_dp_Domains]
GO

ALTER TABLE [dbo].[cr_Allergy]  WITH CHECK ADD  CONSTRAINT [FK_cr_Allergy_Types] FOREIGN KEY([Allergy_Type_ID])
REFERENCES [dbo].[cr_Allergy_Types] ([Allergy_Type_ID])
GO

ALTER TABLE [dbo].[cr_Allergy] CHECK CONSTRAINT [FK_cr_Allergy_Types]
GO

--adding FK_cr_Medical_Information_Allergies_Allergy to table cr_Medical_Information_Allergies
ALTER TABLE [dbo].[cr_Medical_Information_Allergies]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Allergies_Allergy] FOREIGN KEY([Allergy_ID])
REFERENCES [dbo].[cr_Allergy] ([Allergy_ID])
GO

ALTER TABLE [dbo].[cr_Medical_Information_Allergies] CHECK CONSTRAINT [FK_cr_Medical_Information_Allergies_Allergy]
GO

