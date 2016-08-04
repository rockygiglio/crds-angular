USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Page_Views]
SET FIELD_LIST = 'Participant_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact_ID]
                    , Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First_Name]
                    , Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last_Name]
                    , Participant_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname]
                    , Group_Role_ID_Table.[Group_Role_ID] AS [Group_Role_ID]
                    , Group_Role_ID_Table.[Role_Title] AS [Role_Title]
                    , Participant_ID_Table.[Participant_ID] AS [Participant_ID],
                    Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [Email], 
Group_Participants.Start_Date as [Start_Date]'
WHERE Sub_Page_View_ID = 88;
