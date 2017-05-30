-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 1/4/2017
-- Description:	Add default household for Check In guests
-- ===============================================================

USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Households] ON

DECLARE @GUEST_HOUSEHOLD_ID INT = 5771805;

IF NOT EXISTS( SELECT 1 FROM [dbo].[Households] WHERE [Household_ID] = @GUEST_HOUSEHOLD_ID)
BEGIN
INSERT INTO [Households]([Household_ID],
	[Household_Name],
	[Address_ID],
	[Home_Phone],
	[Domain_ID],
	[Congregation_ID],
	[Care_Person],
	[Household_Source_ID],
	[Family_Call_Number],
	[Household_Preferences],
	[Home_Phone_Unlisted],
	[Home_Address_Unlisted],
	[Bulk_Mail_Opt_Out],
	[_Last_Donation],
	[_Last_Activity],
	[__ExternalHouseholdID],
	[__ExternalBusinessID])
VALUES(@GUEST_HOUSEHOLD_ID,
	N'GuestCheckinHousehold',
	NULL,
	NULL,
	1,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	1,
	NULL,
	NULL,
	NULL,
	NULL)
END

SET IDENTITY_INSERT [dbo].[Households] OFF