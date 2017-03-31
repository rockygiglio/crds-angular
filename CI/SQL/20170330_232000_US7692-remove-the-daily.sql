USE [MinistryPlatform]
GO

-- Hide the opt in/out setting for "The Daily" from the Profile page (Communications tab) on CR.net
UPDATE dp_Publications SET Available_Online = 0 WHERE Publication_ID = 1
