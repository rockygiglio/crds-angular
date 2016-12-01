USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 10/27/2016
-- Description:	Kiosk Config for Managing Kiosk Configurations
-- ===============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE table_name='cr_Kiosk_Configs')
BEGIN
	CREATE TABLE [dbo].[cr_Kiosk_Configs](
		[Kiosk_Config_ID] [int] IDENTITY(1,1) NOT NULL,
		[_Kiosk_Identifier] [uniqueidentifier] NOT NULL CONSTRAINT [DF_cr_Kiosk_Configs__Kiosk_Identifier]  DEFAULT (newid()),
		[Kiosk_Name] [nvarchar](100) NOT NULL,
		[Kiosk_Description] [nvarchar](4000) NOT NULL,
		[Kiosk_Type_ID] [int] NOT NULL,
		[Location_ID] [int] NOT NULL,
		[Congregation_ID] [int] NOT NULL,
		[Room_ID] [int] NOT NULL,
		[Start_Date] [date] NOT NULL CONSTRAINT [DF_cr_Kiosk_Configs_Start_Date]  DEFAULT (getdate()),
		[End_Date] [date] NULL,
		[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Kiosk_Configs_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_cr_Kiosk_Configs] PRIMARY KEY CLUSTERED 
	(
		[Kiosk_Config_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END

	ALTER TABLE [dbo].[cr_Kiosk_Configs]  WITH CHECK ADD CONSTRAINT [FK_cr_Kiosk_Configs_cr_Kiosk_Types] FOREIGN KEY([Kiosk_Type_ID])
	REFERENCES [dbo].[cr_Kiosk_Types] ([Kiosk_Type_ID])
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs] CHECK CONSTRAINT [FK_cr_Kiosk_Configs_cr_Kiosk_Types]
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs]  WITH CHECK ADD CONSTRAINT [FK_cr_Kiosk_Configs_Locations] FOREIGN KEY([Location_ID])
	REFERENCES [dbo].[Locations] ([Location_ID])
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs] CHECK CONSTRAINT [FK_cr_Kiosk_Configs_Locations]
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs]  WITH CHECK ADD CONSTRAINT [FK_cr_Kiosk_Configs_Congregations] FOREIGN KEY([Congregation_ID])
	REFERENCES [dbo].[Congregations] ([Congregation_ID])
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs] CHECK CONSTRAINT [FK_cr_Kiosk_Configs_Congregations]
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs]  WITH CHECK ADD CONSTRAINT [FK_cr_Kiosk_Configs_Rooms] FOREIGN KEY([Room_ID])
	REFERENCES [dbo].[Rooms] ([Room_ID])
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs] CHECK CONSTRAINT [FK_cr_Kiosk_Configs_Rooms]
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs]  WITH CHECK ADD CONSTRAINT [FK_cr_Kiosk_Configs_Dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
	GO

	ALTER TABLE [dbo].[cr_Kiosk_Configs] CHECK CONSTRAINT [FK_cr_Kiosk_Configs_Dp_Domains]
	GO

GO

SET ANSI_PADDING OFF
GO



