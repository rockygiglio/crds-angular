USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Donation_Communications]    Script Date: 2/19/2016 9:23:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Donation_Communications](
	[Donation_Communications_ID] [int] IDENTITY(1,1) NOT NULL,
	[Donation_ID] [int] NOT NULL,
	[Communication_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Donation_Communications] PRIMARY KEY CLUSTERED 
(
	[Donation_Communications_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donation_Communications_Donations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Communications_Donations] FOREIGN KEY([Donation_ID])
REFERENCES [dbo].[Donations] ([Donation_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donation_Communications_Donations]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications] CHECK CONSTRAINT [FK_Donation_Communications_Donations]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donation_Communications_Communications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Communications_Communications] FOREIGN KEY([Communication_ID])
REFERENCES [dbo].[dp_Communications] ([Communication_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donation_Communications_Communications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications] CHECK CONSTRAINT [FK_Donation_Communications_Communications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Communications_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Donation_Communications]'))
ALTER TABLE [dbo].[cr_Donation_Communications] CHECK CONSTRAINT [FK_Donation_Communications_dp_Domains]
GO