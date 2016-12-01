USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Client_Api_Keys]    Script Date: 10/4/2016 21:06:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cr_Client_Api_Keys](
	[Client_Api_Key_ID] [int] IDENTITY(1,1) NOT NULL,
	[Client_Name] [nvarchar](100) NOT NULL,
	[Client_Description] [nvarchar](4000) NOT NULL,
	[Client_Contact_Name] [nvarchar](100) NULL,
	[Client_Contact_Email] [dbo].[dp_Email] NULL,
	[_Api_Key] [uniqueidentifier] NOT NULL,
	[Allowed_Domains] [nvarchar](2000) NULL,
	[Start_Date] [date] NOT NULL,
	[End_Date] [date] NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Client_Api_Keys_Domain_ID] DEFAULT ((1)),
 CONSTRAINT [PK_cr_Client_Api_Keys] PRIMARY KEY CLUSTERED 
(
	[Client_Api_Key_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[cr_Client_Api_Keys] ADD  CONSTRAINT [DF_cr_Client_Api_Keys__Api_Key]  DEFAULT (newid()) FOR [_Api_Key]
GO

ALTER TABLE [dbo].[cr_Client_Api_Keys] ADD  CONSTRAINT [DF_cr_Client_Api_Keys_Start_Date]  DEFAULT (getdate()) FOR [Start_Date]
GO

ALTER TABLE [dbo].[cr_Client_Api_Keys]  WITH CHECK ADD  CONSTRAINT [FK_cr_Client_Api_Keys_Dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the client using this key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Client_Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A description of the client (who is it, why do they need it, etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Client_Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the primary contact at the client' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Client_Contact_Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The email address of the primary contact of the client' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Client_Contact_Email'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The auto-generated guid assigned to a client' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'_Api_Key'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A comma-separated list of domains that the key is allowed from (wildcards, like *.example.com, are allowed)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Allowed_Domains'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The first date this API key should be valid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'Start_Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date when this key becomes invalid - null means it does not expire.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Client_Api_Keys', @level2type=N'COLUMN',@level2name=N'End_Date'
GO


