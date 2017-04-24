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
		[FromUser_Contact_ID] [int] NOT NULL,
		[ToUser_Contact_ID] [int] NOT NULL,
		[Communication_ID] [int] NOT NULL,
		[Status] [varchar](50) NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_cr_Connect_Communications] PRIMARY KEY CLUSTERED 
	(
		[Connect_Communications_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	/* FromUser */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Contacts_From] FOREIGN KEY([FromUser_Contact_ID])
	REFERENCES [dbo].[Contacts] ([Contact_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Contacts_From]
	

	/* ToUser */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Contacts_To] FOREIGN KEY([ToUser_Contact_ID])
	REFERENCES [dbo].[Contacts] ([Contact_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Contacts_To]
	

	/* Message */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_Communication_ID] FOREIGN KEY([Communication_ID])
	REFERENCES [dbo].[dp_Communications] ([Communication_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_Communication_ID]
	

	/* Domain */
	ALTER TABLE [dbo].[cr_Connect_Communications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Connect_Communications_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
	
	ALTER TABLE [dbo].[cr_Connect_Communications] CHECK CONSTRAINT [FK_cr_Connect_Communications_dp_Domains]
	
END

