USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medical_Information_Medications](
	[MedicalInformationMedication_ID] [INT] IDENTITY(1,1) NOT NULL,
	[MedicalInformation_ID] INT NOT NULL,
	[Medication_ID] INT NOT NULL,
	[DosageTime] TIME NOT NULL,
	[DosageAmount] NVARCHAR(128) NOT NULL,
	[Domain_ID] [INT] NOT NULL,
 CONSTRAINT [PK_cr_Medical_Information_Medications] PRIMARY KEY CLUSTERED 
(
	[MedicalInformationMedication_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


-------Domain_ID FK------------------------------------------------------

ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_dp_Domains]

-------------------------------------------------------------

ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_Medications] FOREIGN KEY([Medication_ID])
REFERENCES [dbo].[cr_Medications] ([Medication_ID])



ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_Medications]

-------------------------------------------------------------

ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_Medical_Information] FOREIGN KEY([MedicalInformation_ID])
REFERENCES [dbo].[cr_Medical_Information] ([MedicalInformation_ID])


ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_Medical_Information]

END

