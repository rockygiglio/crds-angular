USE [MinistryPlatform]
GO

UPDATE dp_page_views 
SET Field_List = 'Contact_ID_Table.[Contact_ID] AS [Contact ID] 
    , Contact_ID_Table.[Display_Name] AS [Display Name]
    , Contact_ID_Table.[Nickname] AS [Nickname]
    , Contact_ID_Table.[First_Name] AS [First Name]
    , Contact_ID_Table.[Email_Address] AS [Email Address]
    , Participants.[Participant_ID] AS [Participant ID]
    , Contact_ID_Table.[__Age] AS [Age]
    , Participants.[Approved_Small_Group_Leader]
	, Participants.[Attendance_Start_Date]'
WHERE page_view_id = 1029