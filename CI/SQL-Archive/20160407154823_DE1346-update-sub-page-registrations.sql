USE [MinistryPlatform]
GO

DECLARE @subPageId AS INT;
SET @subPageId = (SELECT Sub_Page_ID FROM dp_Sub_Pages WHERE Display_Name = 'Registrations');

UPDATE [MinistryPlatform].[dbo].[dp_Sub_Pages]
SET [Primary_Table] = 'cr_Group_Connector_Registrations',
    [Filter_Key] = 'Group_Connector_ID'
WHERE Sub_Page_ID = @subPageId;

GO