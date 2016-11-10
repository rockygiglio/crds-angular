USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 11/04/2016
-- Description:	Bumping Rule Types for Bumping Rules 
-- ===============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE table_name='cr_Bumping_Rule_Types')
BEGIN
	CREATE TABLE [dbo].[cr_Bumping_Rule_Types](
	[Bumping_Rule_Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Bumping_Rule_Type] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Bumping_Rule_Types_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_Bumping_Rule_Types] PRIMARY KEY CLUSTERED 
	(
		[Bumping_Rule_Type_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[cr_Bumping_Rule_Types]  WITH CHECK ADD  CONSTRAINT [FK_Bumping_Rule_Types_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Bumping_Rule_Types] CHECK CONSTRAINT [FK_Bumping_Rule_Types_Domains]
GO


