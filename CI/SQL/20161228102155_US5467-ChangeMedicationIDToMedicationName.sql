USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] DROP CONSTRAINT [FK_cr_Medical_Information_Medications_Medications]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medical_Information]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] DROP CONSTRAINT [FK_cr_Medical_Information_Medications_Medical_Information]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] DROP CONSTRAINT [FK_cr_Medical_Information_Medications_dp_Domains]
GO

/****** Object:  Table [dbo].[cr_Medical_Information_Medications]    Script Date: 12/28/2016 10:12:45 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]') AND type in (N'U'))
DROP TABLE [dbo].[cr_Medical_Information_Medications]
GO

/****** Object:  Table [dbo].[cr_Medical_Information_Medications]    Script Date: 12/28/2016 10:12:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medical_Information_Medications](
	[MedicalInformationMedication_ID] [int] IDENTITY(1,1) NOT NULL,
	[MedicalInformation_ID] [int] NOT NULL,
	[Medication_Name] [nvarchar](256) NOT NULL,
	[Medication_Type_ID] [int] NOT NULL,
	[DosageTime] [nvarchar](128) NOT NULL,
	[DosageAmount] [nvarchar](128) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Medical_Information_Medications] PRIMARY KEY CLUSTERED 
(
	[MedicalInformationMedication_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medical_Information]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_Medical_Information] FOREIGN KEY([MedicalInformation_ID])
REFERENCES [dbo].[cr_Medical_Information] ([MedicalInformation_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medical_Information]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_Medical_Information]
GO
