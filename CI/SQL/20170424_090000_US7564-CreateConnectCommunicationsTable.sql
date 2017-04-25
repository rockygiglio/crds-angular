USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM MinistryPlatform.INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo'  AND TABLE_NAME = N'cr_Connect_Communications')
BEGIN
	CREATE TABLE [dbo].[cr_Connect_Communications](
		[Connect_Communications_ID] [int] IDENTITY(1,1) NOT NULL,
		[From_Contact_ID] [int] NOT NULL,
		[To_Contact_ID] [int] NOT NULL,
		[Communication_Date] [datetime] NOT NULL,
		[Communication_Type_ID] [int] NOT NULL,
		[Communication_Status_ID] [int] NOT NULL,
		[Group_ID] [int] NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_cr_Connect_Communications] PRIMARY KEY CLUSTERED 
	(
		[Connect_Communications_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[cr_Connect_Communications] ADD  CONSTRAINT [DF_cr_Connect_Communications_Domain_ID]  DEFAULT ((1)) FOR [Domain_ID]
	ALTER TABLE [dbo].[cr_Connect_Communications] ADD  CONSTRAINT [DF_cr_Connect_Communications_Start_Date]  DEFAULT (getdate()) FOR [Communication_Date]

	/* FromUser */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Contacts_From] FOREIGN KEY([From_Contact_ID])
	REFERENCES [dbo].[Contacts] ([Contact_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Contacts_From]
	
	/* ToUser */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Contacts_To] FOREIGN KEY([To_Contact_ID])
	REFERENCES [dbo].[Contacts] ([Contact_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Contacts_To]

	/* Group */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Group] FOREIGN KEY([Group_ID])
	REFERENCES [dbo].[Groups] ([Group_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Group]

	/* Communication Type */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Communication_Type_ID] FOREIGN KEY([Communication_Type_ID])
	REFERENCES [dbo].[cr_Connect_Communications_Type] ([Connect_Communications_Type_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Communication_Type_ID]
	
	/* Communication Status */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Communication_Status_ID] FOREIGN KEY([Communication_Status_ID])
	REFERENCES [dbo].[cr_Connect_Communications_Status] ([Connect_Communications_Status_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Communication_Status_ID]

	/* Domain */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_dp_Domains]
	
END

