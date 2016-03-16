USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Primary Registration] , Project_ID_Table.[Project_Name] AS [Project Name] , Primary_Registration_Table_Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred Launch Site] , Primary_Registration_Table_Organization_ID_Table.[Name] AS [Organization] , Primary_Registration_Table_Initiative_ID_Table.[Initiative_Name] AS [Initiative]'
      ,[Selected_Record_Expression] = 'Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name]'
 WHERE Page_ID = 13
GO


