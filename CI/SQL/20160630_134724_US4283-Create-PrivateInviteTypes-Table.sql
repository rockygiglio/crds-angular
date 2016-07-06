USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Invitation_Types]    Script Date: 6/30/2016 2:48:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (select * from sys.objects where Object_ID = Object_ID(N'dbo.cr_Invitation_Types') and Type = N'U')
BEGIN
CREATE TABLE [dbo].[cr_Invitation_Types](
	[Invitation_Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Invitation_Type] [nvarchar] (50) NOT NULL,
	[Description] [nvarchar] (255) NULL,
	[Domain_ID] [int] NOT NULL DEFAULT ((1)),
	CONSTRAINT [PK_Private_Invitation_Types] PRIMARY KEY CLUSTERED 
(
	[Invitation_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[cr_Invitation_Types]  WITH CHECK ADD  CONSTRAINT [FK_Invitation_types_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Invitation_Types] CHECK CONSTRAINT [FK_Invitation_types_dp_Domains]
END