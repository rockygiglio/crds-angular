USE MinistryPlatform

UPDATE dp_page_views SET Field_List = 'Events.[Event_ID] AS [Event ID] , Events.[Event_Title] AS [Event Title] , Event_Type_ID_Table.[Event_Type_ID] AS [Event Type ID] , Event_Type_ID_Table.[Event_Type] AS [Event Type] , Events.[Event_Start_Date] AS [Event Start Date] , Events.[Event_End_Date] AS [Event End Date], Events.[Congregation_ID]'
  WHERE page_view_id=1028
