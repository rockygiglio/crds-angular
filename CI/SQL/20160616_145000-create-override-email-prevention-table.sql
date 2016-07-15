USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Override_Email_Prevention]    Script Date: 6/16/2016 3:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Override_Email_Prevention](
    [Override_Email_Prevention_ID] [int] IDENTITY(1,1) NOT NULL,
	[Contact_ID] int NOT NULL,
	[Domain_ID] int NOT NULL,
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]') AND name = N'PK_OEP')
ALTER TABLE [dbo].[cr_Override_Email_Prevention] ADD  CONSTRAINT [PK_OEP] PRIMARY KEY CLUSTERED 
(
	[Override_Email_Prevention_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


--contact
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OEP_ContactId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]'))
ALTER TABLE [dbo].[cr_Override_Email_Prevention]  WITH CHECK ADD  CONSTRAINT [FK_OEP_ContactId] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OEP_ContactId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]'))
ALTER TABLE [dbo].[cr_Override_Email_Prevention] CHECK CONSTRAINT [FK_OEP_ContactId]
GO

--domain

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OEP_DomainId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]'))
ALTER TABLE [dbo].[cr_Override_Email_Prevention]  WITH CHECK ADD  CONSTRAINT [FK_OEP_DomainId] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OEP_DomainId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Override_Email_Prevention]'))
ALTER TABLE [dbo].[cr_Override_Email_Prevention] CHECK CONSTRAINT [FK_OEP_DomainId]
GO
