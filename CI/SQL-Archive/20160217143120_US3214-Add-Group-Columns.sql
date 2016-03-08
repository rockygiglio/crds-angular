use MinistryPlatform;

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Remaining_Capacity' AND Object_ID = Object_ID(N'Groups'))
BEGIN
    ALTER TABLE [dbo].[Groups] 
		ADD 
			Small_Group_Information_2 dp_Separator NULL,
			Remaining_Capacity SMALLINT NULL;
END