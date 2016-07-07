USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Categories]    Script Date: 7/6/2016 08:48:47 AM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE Object_ID = Object_ID(N'dbo.cr_Group_Category_Details') AND Type = N'U')
BEGIN
CREATE TABLE dbo.cr_Group_Category_Details
	(
	Group_Category_Detail_ID int NOT NULL IDENTITY (1, 1),
	Group_ID int NOT NULL,
	Category_Detail_ID int NOT NULL,
	Domain_ID int NOT NULL,
	Notes nvarchar(50) NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.cr_Group_Category_Details ADD CONSTRAINT
	DF_cr_Group_Category_Details_Domain_ID DEFAULT (1) FOR Domain_ID

ALTER TABLE dbo.cr_Group_Category_Details ADD CONSTRAINT
	PK_cr_Group_Category_Details PRIMARY KEY CLUSTERED 
	(
	Group_Category_Detail_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE dbo.cr_Group_Category_Details ADD CONSTRAINT
	FK_cr_Group_Category_Details_cr_Category_Details FOREIGN KEY
	(
	Category_Detail_ID
	) REFERENCES dbo.cr_Category_Details
	(
	Category_Detail_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.cr_Group_Category_Details ADD CONSTRAINT
	FK_cr_Group_Category_Details_Groups FOREIGN KEY
	(
	Group_ID
	) REFERENCES dbo.Groups
	(
	Group_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.cr_Group_Category_Details ADD CONSTRAINT
	FK_cr_Group_Category_Details_dp_Domains FOREIGN KEY
	(
	Domain_ID
	) REFERENCES dbo.dp_Domains
	(
	Domain_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.cr_Group_Category_Details SET (LOCK_ESCALATION = TABLE)
END
