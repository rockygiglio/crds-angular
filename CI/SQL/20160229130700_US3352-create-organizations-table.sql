USE MinistryPlatform
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Organizations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Organizations]
(
	[Organization_ID] INT NOT NULL IDENTITY (1, 1), 
    [Domain_ID] INT NOT NULL, 
    [Name] NCHAR(100) NOT NULL, 
    [Open_Signup] BIT NOT NULL DEFAULT 0, 
    [Start_Date] DATE NOT NULL, 
    [End_Date] DATE NULL, 
    [Primary_Contact] INT NOT NULL, 
    CONSTRAINT [FK_Organizations_Domains] FOREIGN KEY ([Domain_ID]) REFERENCES [dp_Domains]([Domain_ID]), 
    CONSTRAINT [FK_Organizations_Contacts] FOREIGN KEY ([Primary_Contact]) REFERENCES [Contacts]([Contact_ID]),
	CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED([Organization_ID] ASC)
)
END