USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Categories]    Script Date: 7/6/2016 08:48:47 AM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE Object_ID = Object_ID(N'dbo.cr_Categories') AND Type = N'U')
BEGIN
CREATE TABLE dbo.cr_Categories
	(
	Category_ID int NOT NULL IDENTITY (1, 1),
	Category nvarchar(50) NOT NULL,
	Description nvarchar(100) NULL,
	Domain_ID int NOT NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.cr_Categories ADD CONSTRAINT
	DF_cr_Categories_Domain_ID DEFAULT (1) FOR Domain_ID

ALTER TABLE dbo.cr_Categories ADD CONSTRAINT
	PK_cr_Categories PRIMARY KEY CLUSTERED 
	(
	Category_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



ALTER TABLE dbo.cr_Categories SET (LOCK_ESCALATION = TABLE)


ALTER TABLE [dbo].[cr_Categories]  WITH CHECK ADD  CONSTRAINT [FK_cr_Categories_Dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Categories] CHECK CONSTRAINT [FK_cr_Categories_Dp_Domains]
END

