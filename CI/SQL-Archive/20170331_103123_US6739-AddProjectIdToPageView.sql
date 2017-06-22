USE [MinistryPlatform]
GO

DECLARE @PageViewID int = 2219

IF EXISTS(Select 1 from [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewID)
BEGIN
	UPDATE [dbo].[dp_Page_Views]
	   SET [Field_List] = 'CASE
WHEN Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Company] = 1
THEN Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Company_Name]
ELSE Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Nickname] + '' '' + Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name] END AS [Primary_Registration] 
                  , Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Contact_ID] AS [Primary_Registration_Contact_ID] 
				  , Project_ID_Table.[Project_Name] 
				  , Primary_Registration_Table_Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred_Launch_Site] 
				  , Primary_Registration_Table_Organization_ID_Table.[Organization_ID] 
				  , Primary_Registration_Table_Organization_ID_Table.[Open_Signup] AS [Organization_Open_Signup] 
				  , Primary_Registration_Table_Initiative_ID_Table.[Initiative_ID] 
				  , Project_ID_Table_Project_Type_ID_Table.[Description] AS [Project_Type] 
				  , Project_ID_Table_Project_Type_ID_Table.[Minimum_Age] AS [Project_Minimum_Age] 
				  , Project_ID_Table.[_Volunteer_Count] AS [Volunteer_Count] 
				  , Project_ID_Table.[Maximum_Volunteers] AS [Project_Maximum_Volunteers] 
				  ,cr_Group_Connectors.[Group_Connector_ID], cr_Group_Connectors.[Private]
				  , Primary_Registration_Table_Preferred_Launch_Site_ID_Table.[Location_ID] AS [Preferred_Launch_Site_ID]
				  , Project_ID_Table.[Absolute_Maximum_Volunteers], _Minimum_Age, Project_ID_Table.[Project_ID]'
	WHERE Page_View_ID = @PageViewID
END