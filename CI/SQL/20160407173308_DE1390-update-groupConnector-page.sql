USE [MinistryPlatform]
GO

DECLARE @pageId AS INT;
SET @pageId = (SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Group Connectors');

UPDATE [MinistryPlatform].[dbo].[dp_Pages]
SET [Table_Name] = 'cr_Group_Connectors',
    [Primary_Key] = 'Group_Connector_ID'
WHERE PAGE_ID = @pageId

GO