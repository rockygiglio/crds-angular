USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medications](
	[Medication_ID] [INT] IDENTITY(1,1) NOT NULL,
	[MedicationName] NVARCHAR(256) NOT NULL,
	[MedicationType] NVARCHAR(256) NOT NULL,
	[Domain_ID] [INT] NOT NULL,
 CONSTRAINT [PK_cr_Medication] PRIMARY KEY CLUSTERED 
(
	[Medication_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


-------Domain_ID FK------------------------------------------------------

ALTER TABLE [dbo].[cr_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medications_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Medications] CHECK CONSTRAINT [FK_cr_Medications_dp_Domains]

-------------------------------------------------------------

END

