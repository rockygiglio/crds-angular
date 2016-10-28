USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 10/27/2016
-- Description:	Kiosk Types for Kiosk Config 
-- ===============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE table_name='cr_Kiosk_Types')
BEGIN
	CREATE TABLE [dbo].[cr_Kiosk_Types](
	[Kiosk_Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Kiosk_Type] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Kiosk_Types_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_Kiosk_Types] PRIMARY KEY CLUSTERED 
	(
		[Kiosk_Type_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[cr_Kiosk_Types]  WITH CHECK ADD  CONSTRAINT [FK_Kiosk_Types_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Kiosk_Types] CHECK CONSTRAINT [FK_Kiosk_Types_Domains]
GO


