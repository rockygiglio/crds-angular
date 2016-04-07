USE [MinistryPlatform]
GO

DECLARE @pageId AS INT;
SET @pageId = (SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Volunteers');

UPDATE [MinistryPlatform].[dbo].[dp_Pages]
SET [Table_Name] = 'cr_Group_Connector_Registrations',
    [Default_Field_List] = 'cr_Group_Connector_Registrations.[Group_Connector_Registration_ID] AS [Group Connector Registration ID] 
	                      , Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Nickname] AS [Vol Nickname] 
						  , Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Vol Last Name] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table.[Location_Name] AS [Launch Site] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table_Address_ID_Table.[Address_Line_1] AS [Project Address Line 1] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table_Address_ID_Table.[Address_Line_2] AS [Project Address Line 2] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table_Address_ID_Table.[City] AS [Project City] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table_Address_ID_Table.[State/Region] AS [Project State] 
						  , Registration_ID_Table_Preferred_Launch_Site_ID_Table_Address_ID_Table.[Postal_Code] AS [Project Zip] 
						  , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Nickname] AS [TC Nickname] 
						  , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[First_Name] AS [TC First Name] 
						  , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [TC Email Address] 
						  , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Mobile_Phone] AS [TC Mobile Phone] 
						  , Group_Connector_ID_Table_Project_ID_Table.[Project_Name] AS [Project Name] 
						  , Group_Connector_ID_Table_Project_ID_Table_Project_Type_ID_Table.[Description] AS [Project Type]',
    [Selected_Record_Expression] = 'cr_Group_Connector_Registrations.[Group_Connector_Registration_ID]'
WHERE PAGE_ID = @pageId;

GO