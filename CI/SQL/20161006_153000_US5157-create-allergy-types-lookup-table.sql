USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Allergy_Types]    Script Date: 10/6/2016 3:26:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Allergy_Types]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Allergy_Types](
	[Allergy_Type_ID] [int] IDENTITY(1,1) NOT NULL,	
	[Description] [nvarchar](100) NOT NULL,	
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_AllergyTypes] PRIMARY KEY CLUSTERED 
(
	[Allergy_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Allergy_Types_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy_Types]'))
ALTER TABLE [dbo].[cr_Allergy_Types]  WITH CHECK ADD  CONSTRAINT [FK_Allergy_Types_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Allergy_Types_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Allergy_Types]'))
ALTER TABLE [dbo].[cr_Allergy_Types] CHECK CONSTRAINT [FK_Allergy_Types_Domains]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Description] = N'Medicine')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Description]
		,[Domain_ID]
		) VALUES (
		 N'Medicine'
		,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Description] = N'Food')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Description]
		,[Domain_ID]
		) VALUES (
		 N'Food'
		,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Description] = N'Environmental')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Description]
		,[Domain_ID]
		) VALUES (
		 N'Environmental'
        ,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Description] = N'Other')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Description]
		,[Domain_ID]
		) VALUES (
		 N'Other'
		,1
		)
END
