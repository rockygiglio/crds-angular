USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM MinistryPlatform.INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo'  AND TABLE_NAME = N'cr_Connect_Communications_Type')
BEGIN
	CREATE TABLE [dbo].[cr_Connect_Communications_Type](
		[Connect_Communications_Type_ID] [int] IDENTITY(1,1) NOT NULL,
		[Communication_Type] [varchar](50) NOT NULL,
		[Start_Date] [datetime] NOT NULL,
		[End_Date] [datetime] NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_cr_Connect_Communications_Type] PRIMARY KEY CLUSTERED 
	(
		[Connect_Communications_Type_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[cr_Connect_Communications_Type] ADD  CONSTRAINT [DF_cr_Connect_Communications_Type_Domain_ID]  DEFAULT ((1)) FOR [Domain_ID]

	ALTER TABLE [dbo].[cr_Connect_Communications_Type] ADD  CONSTRAINT [DF_cr_Connect_Communications_Type_Start_Date]  DEFAULT (getdate()) FOR [Start_Date]
END


IF NOT EXISTS (SELECT * FROM MinistryPlatform.INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo'  AND TABLE_NAME = N'cr_Connect_Communications_Status')
BEGIN
	CREATE TABLE [dbo].[cr_Connect_Communications_Status](
		[Connect_Communications_Status_ID] [int] IDENTITY(1,1) NOT NULL,
		[Communication_Status] [varchar](50) NOT NULL,
		[Start_Date] [datetime] NOT NULL,
		[End_Date] [datetime] NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_cr_Connect_Communications_Status] PRIMARY KEY CLUSTERED 
	(
		[Connect_Communications_Status_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[cr_Connect_Communications_Status] ADD  CONSTRAINT [DF_cr_Connect_Communications_Status_Domain_ID]  DEFAULT ((1)) FOR [Domain_ID]

	ALTER TABLE [dbo].[cr_Connect_Communications_Status] ADD  CONSTRAINT [DF_cr_Connect_Communications_Status_Start_Date]  DEFAULT (getdate()) FOR [Start_Date]
END
GO

/* insert some initial values */
IF NOT EXISTS(SELECT * FROM [cr_Connect_Communications_Type] WHERE Communication_Type = 'Say Hi')
BEGIN
    SET IDENTITY_INSERT cr_Connect_Communications_Type ON;
	INSERT INTO [dbo].[cr_Connect_Communications_Type](Connect_Communications_Type_ID, Communication_Type) VALUES(1, 'Say Hi');
	INSERT INTO [dbo].[cr_Connect_Communications_Type](Connect_Communications_Type_ID, Communication_Type) VALUES(2, 'Invite To Gathering');
	INSERT INTO [dbo].[cr_Connect_Communications_Type](Connect_Communications_Type_ID, Communication_Type) VALUES(3, 'Request To Join');
	SET IDENTITY_INSERT cr_Connect_Communications_Type OFF;
END

IF NOT EXISTS(SELECT * FROM [cr_Connect_Communications_Status] WHERE Communication_Status = 'Say Hi')
BEGIN
    SET IDENTITY_INSERT cr_Connect_Communications_Status ON;
	INSERT INTO [dbo].[cr_Connect_Communications_Status](Connect_Communications_Status_ID, Communication_Status) VALUES(1,'Accepted');
	INSERT INTO [dbo].[cr_Connect_Communications_Status](Connect_Communications_Status_ID, Communication_Status) VALUES(2, 'Declined');
	INSERT INTO [dbo].[cr_Connect_Communications_Status](Connect_Communications_Status_ID, Communication_Status) VALUES(3, 'N/A');
	INSERT INTO [dbo].[cr_Connect_Communications_Status](Connect_Communications_Status_ID, Communication_Status) VALUES(4, 'Unanswered');
	SET IDENTITY_INSERT cr_Connect_Communications_Status OFF;
END
GO
