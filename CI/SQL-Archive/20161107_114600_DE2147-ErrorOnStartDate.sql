USE [MinistryPlatform]
GO

UPDATE dp_page_views 
SET Field_List = 'Contact_ID_Table.[Contact_ID] 
    , Contact_ID_Table.[Display_Name] 
    , Contact_ID_Table.[Nickname] 
    , Contact_ID_Table.[First_Name] 
    , Contact_ID_Table.[Email_Address] 
    , Participants.[Participant_ID] 
    , Contact_ID_Table.[__Age] 
    , Participants.[Approved_Small_Group_Leader]
	, Participants.[Attendance_Start_Date]'
WHERE page_view_id = 1029