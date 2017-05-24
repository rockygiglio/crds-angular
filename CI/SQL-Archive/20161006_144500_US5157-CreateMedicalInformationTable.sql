USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medical_Information](
	[MedicalInformation_ID] [INT] IDENTITY(1,1) NOT NULL,
	[InsuranceCompany] NVARCHAR(256) NOT NULL,
	[PolicyHolderName] NVARCHAR(256) NOT NULL,
	[PhysicianName] NVARCHAR(256) NOT NULL,
	[PhysicianPhone] NVARCHAR(256) NOT NULL,
	[Waiver_ID] [INT] NOT NULL,
	[Domain_ID] [INT] NOT NULL,
 CONSTRAINT [PK_cr_Medical_Information] PRIMARY KEY CLUSTERED 
(
	[MedicalInformation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

-------Waiver_ID FK------------------------------------------------------

ALTER TABLE [dbo].[cr_Medical_Information]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Waivers] FOREIGN KEY([Waiver_ID])
REFERENCES [dbo].[cr_Waivers] ([Waiver_ID])


ALTER TABLE [dbo].[cr_Medical_Information] CHECK CONSTRAINT [FK_cr_Medical_Information_Waivers]

-------------------------------------------------------------

-------Domain_ID FK------------------------------------------------------

ALTER TABLE [dbo].[cr_Medical_Information]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Medical_Information] CHECK CONSTRAINT [FK_cr_Medical_Information_dp_Domains]

-------------------------------------------------------------

END

