USE MinistryPlatform
GO

UPDATE dp_page_views
SET Field_List = 'Events.[Event_ID], Events.[Event_Title], Event_Type_ID_Table.[Event_Type], Event_Type_ID_Table.[Event_Type_ID], Events.[Event_Start_Date], Events.[Event_End_Date], Primary_Contact_Table.[Contact_ID], Primary_Contact_Table.[Email_Address], Parent_Event_ID_Table.[Event_ID] AS [Parent_Event_ID], Congregation_ID_Table.[Congregation_ID], Reminder_Days_Prior_ID_Table.[Reminder_Days_Prior_ID], Events.[Cancelled]'
WHERE View_Title='Event with Details' AND Page_ID=308
GO