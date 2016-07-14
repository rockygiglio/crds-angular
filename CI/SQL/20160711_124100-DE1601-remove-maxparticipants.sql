USE MinistryPlatform
GO


IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Maximum_Participants'
      AND Object_ID = Object_ID(N'dbo.Groups'))
BEGIN
    -- Column Exists
	ALTER TABLE dbo.Groups DROP COLUMN Maximum_Participants
END
GO