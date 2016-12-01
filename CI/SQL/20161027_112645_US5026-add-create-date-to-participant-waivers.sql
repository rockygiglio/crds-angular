USE [MinistryPlatform]
GO

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Signee'
      AND Object_ID = Object_ID(N'cr_Event_Participant_Waivers'))
BEGIN
	EXEC sp_rename 'cr_Event_Participant_Waivers.Signee', 'Signee_Contact_ID', 'COLUMN'; 
END

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Waiver_Create_Date'
      AND Object_ID = Object_ID(N'cr_Event_Participant_Waivers'))
BEGIN

	ALTER TABLE [dbo].[cr_Event_Participant_Waivers] 
		ADD [Waiver_Create_Date] [DateTime] Default GetDate();

END
