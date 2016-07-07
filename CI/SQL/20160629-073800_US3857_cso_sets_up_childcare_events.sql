USE MinistryPlatform;
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Maximum_Age' AND Object_ID = Object_ID(N'dbo.groups'))
BEGIN
    ALTER TABLE dbo.groups ADD Maximum_Age INT NULL;
END


IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Minimum_Participants' AND Object_ID = Object_ID(N'dbo.groups'))
BEGIN
    ALTER TABLE dbo.groups ADD Minimum_Participants INT NULL;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Maximum_Participants' AND Object_ID = Object_ID(N'dbo.groups'))
BEGIN
    ALTER TABLE dbo.groups ADD Maximum_Participants INT NULL;
END