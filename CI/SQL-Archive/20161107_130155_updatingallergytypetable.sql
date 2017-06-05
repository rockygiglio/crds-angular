USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[cr_Allergy_Types] DROP CONSTRAINT [FK_Allergy_Types_Domains]
ALTER TABLE [dbo].[cr_Allergy] DROP CONSTRAINT [FK_cr_Allergy_Types] 
GO

/****** Object:  Table [dbo].[cr_Allergy_Types]    Script Date: 11/7/2016 12:53:51 PM ******/
DROP TABLE [dbo].[cr_Allergy_Types]
GO

/****** Object:  Table [dbo].[cr_Allergy_Types]    Script Date: 11/7/2016 12:53:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Allergy_Types](
	[Allergy_Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Allergy_Type] [nvarchar](100) NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_AllergyTypes] PRIMARY KEY CLUSTERED 
(
	[Allergy_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Allergy_Types]  WITH CHECK ADD  CONSTRAINT [FK_Allergy_Types_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Allergy_Types] CHECK CONSTRAINT [FK_Allergy_Types_Domains]
GO

ALTER TABLE [dbo].[cr_Allergy]  WITH CHECK ADD  CONSTRAINT [FK_cr_Allergy_cr_Allergy_Types] FOREIGN KEY([Allergy_Type_ID])
REFERENCES [dbo].[cr_Allergy_Types] ([Allergy_Type_ID])
GO

ALTER TABLE [dbo].[cr_Allergy] CHECK CONSTRAINT [FK_cr_Allergy_cr_Allergy_Types]
GO


IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Allergy_Type] = N'Medicine')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Allergy_Type]
		,[Domain_ID]
		) VALUES (
		 N'Medicine'
		,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Allergy_Type] = N'Food')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Allergy_Type]
		,[Domain_ID]
		) VALUES (
		 N'Food'
		,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Allergy_Type] = N'Environmental')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Allergy_Type]
		,[Domain_ID]
		) VALUES (
		 N'Environmental'
        ,1
		)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Allergy_Types] WHERE [Allergy_Type] = N'Other')
BEGIN
	INSERT INTO [dbo].[cr_Allergy_Types] (
		 [Allergy_Type]
		,[Domain_ID]
		) VALUES (
		 N'Other'
		,1
		)
END


