USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 11/17/2016 12:57:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects  WHERE Name= N'Deposit_Email' AND Object_ID = Object_ID(N'[dbo].[Products]'))
BEGIN
	ALTER TABLE [dbo].[Products] 
		ADD Deposit_Email int null
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Products_Communications]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Communications] FOREIGN KEY([Deposit_Email])
REFERENCES [dbo].[dp_Communications] ([Communication_ID])
GO