Use [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_pages] WHERE Page_ID = 563)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_pages] ON;

INSERT INTO [dbo].[dp_pages] 
(Page_ID
,Display_Name            
,Singular_Name           
,Description                       
,View_Order,Table_Name          
,Primary_Key           
,Display_Search
,Default_Field_List                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
,Selected_Record_Expression
,Filter_Clause                                                                                                                                                                                 
,Display_Copy) 
VALUES
(563    
,'My Group Participation'
,'My Group Participation'
,'My group participation records.'
,10        
,'Group_Participants'
,'Group_Participant_ID'
,true          
,'Group_Participants.[Group_Participant_ID]
, Participant_ID_Table.[Participant_ID]
, Group_ID_Table.[Group_Name]
, Group_ID_Table.[Group_ID]
, Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID]
, Group_Role_ID
, Group_ID_Table_Congregation_ID_Table.[Congregation_ID]
, Group_ID_Table_Ministry_ID_Table.[Ministry_ID]
, Group_ID_Table_Primary_Contact_Table.[Contact_ID] AS [Primary_Contact]
, Group_ID_Table_Primary_Contact_Table.[Display_Name] AS [Primary_Contact_Name]
, Group_ID_Table_Primary_Contact_Table.[Email_Address] AS [Primary_Contact_Email]
, Group_ID_Table.[Description]
, Group_ID_Table.[Start_Date]
, Group_ID_Table.[End_Date]
, Group_ID_Table_Meeting_Day_ID_Table.[Meeting_Day_ID]
, Group_ID_Table_Meeting_Day_ID_Table.[Meeting_Day]
, Group_ID_Table_Meeting_Frequency_ID_Table.[Meeting_Frequency]
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
,'Group_Participant_ID'    
,'Group_Participants.[Participant_ID] = (select participant_record from contacts where User_account = dp_UserID) AND
(GROUP_ID_TABLE.End_Date IS NULL OR GROUP_ID_TABLE.End_Date >= GetDate()) AND (Group_Participants.End_Date is NULL OR Group_Participants.End_Date >= GetDate())'                                                 
,true)

SET IDENTITY_INSERT [dbo].[dp_pages] OFF;
END

IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE page_id = 563 AND page_section_id = 17)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) 
VALUES
(563    , 17)
END

