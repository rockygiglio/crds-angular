USE MinistryPlatform;

BEGIN
    ALTER TABLE [dbo].[Event_Rooms] 
		ALTER COLUMN Allow_Checkin BIT NULL
END