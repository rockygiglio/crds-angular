USE MinistryPlatform
GO

UPDATE dp_Pages
SET Selected_Record_Expression = N'Request_Status'
WHERE Display_Name = N'Childcare Request Statuses';

GO
