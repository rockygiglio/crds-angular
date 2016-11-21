USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Printer_Maps]    Script Date: 11/16/2016 9:10:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 11/04/2016
-- Description:	Printer Maps for Cloud Printing
-- ===============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE table_name='cr_Printer_Maps')
BEGIN
CREATE TABLE [dbo].cr_Printer_Maps(
	[Printer_Map_ID] [int] IDENTITY(1,1) NOT NULL,
	[Printer_ID] int NOT NULL,
	[Printer_Name] [nvarchar](100) NOT NULL,
	[Computer_ID] [int] NOT NULL,
	[Computer_Name] [nvarchar](100) NOT NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Printer_Maps_Domain_ID]  DEFAULT ((1)),
 CONSTRAINT [PK_cr_Printer_Maps] PRIMARY KEY CLUSTERED 
(
	[Printer_Map_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

