USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dbo.dp_Page_Views ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2307
		   ,'Current Group Participants by Type'
           ,316
           ,''
           ,'Group_Participants.[Group_Participant_ID]
, Participant_ID_Table.[Participant_ID]
, Group_ID_Table.[Group_Name]
, Group_ID_Table.[Group_ID]
, Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID]
, Group_ID_Table_Group_Type_ID_Table_Default_Role_Table.[Group_Role_ID]
, Group_ID_Table_Congregation_ID_Table.[Congregation_ID]
, Group_ID_Table_Ministry_ID_Table.[Ministry_ID]
, Group_ID_Table_Primary_Contact_Table.[Contact_ID] AS [Primary_Contact]
, Group_ID_Table_Primary_Contact_Table.[Display_Name] AS [Primary_Contact_Name]
, Group_ID_Table_Primary_Contact_Table.[Email_Address] AS [Primary_Contact_Email]
, Group_ID_Table.[Description]
, Group_ID_Table.[Start_Date]
, Group_ID_Table.[End_Date]
, Group_ID_Table_Meeting_Day_ID_Table.[Meeting_Day_ID]
, Group_ID_Table.[Meeting_Time]
, Group_ID_Table.[Available_Online]
, Group_ID_Table_Offsite_Meeting_Address_Table.[Address_ID]
, Group_ID_Table_Offsite_Meeting_Address_Table.[Address_Line_1]
, Group_ID_Table_Offsite_Meeting_Address_Table.[Address_Line_2]
, Group_ID_Table_Offsite_Meeting_Address_Table.[City]
, Group_ID_Table_Offsite_Meeting_Address_Table.[State/Region] AS [State]
, Group_ID_Table_Offsite_Meeting_Address_Table.[Postal_Code] AS [Zip_Code]
, Group_ID_Table_Offsite_Meeting_Address_Table.[Foreign_Country]
, Group_ID_Table.[Maximum_Age]
, Group_ID_Table.[Remaining_Capacity]'
           ,'((Group_ID_Table.[End_Date] IS NULL OR Group_ID_Table.[End_Date] >= GetDate()) AND (Group_Participants.[End_Date] IS NULL OR Group_Participants.[End_Date] >= GetDate()))')
GO

SET IDENTITY_INSERT dbo.dp_Page_Views OFF