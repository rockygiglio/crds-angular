USE MinistryPlatform
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Registration_Project_Type]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Registration_Project_Type]
(
	[Registration_Project_Type_ID] INT NOT NULL IDENTITY (1, 1), 
    [Domain_ID] INT NOT NULL, 
	[Registration_ID] INT NOT NULL,
	[Project_Type_ID] INT NOT NULL,
	[Order] Int NOT NULL,

    CONSTRAINT [FK_Registration_Project_Type_Domains] FOREIGN KEY ([Domain_ID]) REFERENCES [dp_Domains]([Domain_ID]), 
    CONSTRAINT [FK_Registration_Project_Type] FOREIGN KEY ([Registration_ID]) REFERENCES [cr_Registrations]([Registration_ID]),
	CONSTRAINT [FK_Registration_Project_Type] FOREIGN KEY ([Registration_ID]) REFERENCES [cr_Project_Types]([Project_Type_ID]),
	CONSTRAINT [PK_Registration_Project_Type] PRIMARY KEY CLUSTERED([Registration_Project_Type_ID] ASC)
)
END