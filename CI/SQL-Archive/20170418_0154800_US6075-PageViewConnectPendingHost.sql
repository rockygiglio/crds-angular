USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Page_Views ON

IF NOT EXISTS (SELECT * FROM dp_Page_Views WHERE Page_View_ID = 1119)
BEGIN
   INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
		   )
        VALUES
           (1119
		   , 'Connect - Pending Hosts'
           , 322 -- Groups 
           ,'Connect hosts awaiting apporval from staff'
           ,'CASE WHEN Primary_Contact_Table_Participant_Record_Table_Host_Status_ID_Table.[Host_Status_ID] = 1 THEN ''Pending''
                WHEN Primary_Contact_Table_Participant_Record_Table_Host_Status_ID_Table.[Host_Status_ID] = 3 THEN ''Approved''
                WHEN Primary_Contact_Table_Participant_Record_Table_Host_Status_ID_Table.[Host_Status_ID] = 2 THEN ''Denied''
             END AS [Host_Status]
           , CASE WHEN Groups.[Available_Online] = 1 THEN ''Public''
               WHEN Groups.[Available_Online] = 0 THEN ''Private''
             END AS [Available Online]
           , Groups.[Group_Name] AS [Group Name]
           , Primary_Contact_Table.[First_Name] AS [First Name]
           , Primary_Contact_Table.[Last_Name] AS [Last Name]'
         ,'Primary_Contact_Table_Participant_Record_Table_Host_Status_ID_Table.[Host_Status_ID] ! = 0
           AND Group_Type_ID_Table.[Group_Type_ID] = ''30''')
END
SET IDENTITY_INSERT dp_Page_Views OFF
GO
