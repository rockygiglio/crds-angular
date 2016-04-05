use MinistryPlatform



IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'_Registration_Creation_Date' AND Object_ID = Object_ID(N'cr_Registrations'))
BEGIN
    -- Add the column
	ALTER TABLE [dbo].[cr_Registrations] 
		ADD _Registration_Creation_Date smalldatetime NOT NULL default Getdate()
END



GO
