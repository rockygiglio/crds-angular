USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medications_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medications]'))
ALTER TABLE [dbo].[cr_Medications] DROP CONSTRAINT [FK_cr_Medications_dp_Domains]
GO

/****** Object:  Table [dbo].[cr_Medications]    Script Date: 12/28/2016 10:46:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medications]') AND type in (N'U'))
DROP TABLE [dbo].[cr_Medications]
GO

/****** Object:  Table [dbo].[cr_Medications]    Script Date: 12/28/2016 10:46:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Medication_Types]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Medication_Types](
	[Medication_Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Medication_Type] [nvarchar](256) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Medication_Types] PRIMARY KEY CLUSTERED 
(
	[Medication_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medication_Types_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medication_Types]'))
ALTER TABLE [dbo].[cr_Medication_Types]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medication_Types_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medication_Types_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medication_Types]'))
ALTER TABLE [dbo].[cr_Medication_Types] CHECK CONSTRAINT [FK_cr_Medication_Types_dp_Domains]
GO

SET IDENTITY_INSERT cr_Medication_Types ON
	INSERT INTO [dbo].[cr_Medication_Types]
			   ([Medication_Type_ID]
			   ,[Medication_Type]
			   ,[Domain_ID])
		 VALUES
			   (1, 'Prescription', 1),
			   (2, 'Over The Counter', 1)
SET IDENTITY_INSERT cr_Medication_Types OFF
GO


