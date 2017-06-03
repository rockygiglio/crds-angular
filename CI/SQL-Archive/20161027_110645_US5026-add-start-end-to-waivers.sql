USE [MinistryPlatform]
GO

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Waiver_Start_Date'
      AND Object_ID = Object_ID(N'cr_Waivers'))
BEGIN
	ALTER TABLE [dbo].[cr_Waivers] 
	ADD [Waiver_Start_Date] [DateTime] Default GetDate();
END

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Waiver_End_Date'
      AND Object_ID = Object_ID(N'cr_Waivers'))
BEGIN
	ALTER TABLE [dbo].[cr_Waivers] 
	ADD [Waiver_End_Date] [DateTime];
END
