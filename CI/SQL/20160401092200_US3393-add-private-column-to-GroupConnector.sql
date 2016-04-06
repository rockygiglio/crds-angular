use MinistryPlatform



IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Private' AND Object_ID = Object_ID(N'cr_GroupConnectors'))
BEGIN
    -- Add the column
	ALTER TABLE [dbo].[cr_GroupConnectors] 
		ADD Private BIT NOT NULL default 0
END



GO
