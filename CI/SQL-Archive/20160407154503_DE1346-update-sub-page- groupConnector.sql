USE [MinistryPlatform]
GO

DECLARE @subPageId AS INT;
SET @subPageId = (SELECT Sub_Page_ID FROM dp_Sub_Pages WHERE Display_Name = 'Group Connectors');

UPDATE [MinistryPlatform].[dbo].[dp_Sub_Pages]
SET [Primary_Table] = 'cr_Group_Connector_Registrations',
    [Default_Field_List] = 'Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First Name] 
	                      , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last Name] 
						  , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [Email Address]',
    [Selected_Record_Expression] = 'Group_Connector_Registration_ID',
	[Filter_Key] = 'Group_Connector_Registration_ID'
WHERE Sub_Page_ID = @subPageId;

GO