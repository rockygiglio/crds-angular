USE [MinistryPlatform]
GO

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Active'
      AND Object_ID = Object_ID(N'cr_Event_Waivers'))
BEGIN
	UPDATE [dbo].[cr_Event_Waivers] set Active = 1;

	ALTER TABLE [dbo].[cr_Event_Waivers] 
		ADD [Active] [bit] Default 1 not null;
END