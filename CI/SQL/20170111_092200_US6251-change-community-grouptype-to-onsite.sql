USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Group_Types] WHERE [Group_Type] = 'Onsite Group') 
Begin
update Group_Types
set Group_Type = 'Onsite Group'
where Group_Type_ID = 8
End
IF NOT EXISTS (SELECT 1 FROM [dbo].[Event_Types] WHERE [Event_Type] = '*Onsite Groups') 
Begin
update Event_Types
set Event_Type = '*Onsite Groups'
where Event_Type_ID = 80
End

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2126 AND [View_Title] like '%onsite group%')
Begin 
Update dp_Page_Views set View_Title = 'Onsite Groups - ' + 'All', View_Clause = 'Group_Type_ID_Table.[Group_Type_ID] = 8 OR (Group_Type_ID_Table.[Group_Type] = ''Wait List'' AND Parent_Group_Table_Group_Type_ID_Table.[Group_Type_ID] = 8)' where Page_View_ID = 2126
End
IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2135 AND [View_Title] like '%onsite group%')
Begin
Update dp_Page_Views set View_Title = 'Onsite Group' + ' Participants - All', View_Clause = 'Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID] = 8 OR (Group_ID_Table_Group_Type_ID_Table.[Group_Type] = ''Wait List'' AND Group_ID_Table_Parent_Group_Table_Group_Type_ID_Table.[Group_Type_ID] = 8)' where Page_View_ID = 2135
End
IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2136 AND [View_Title] like '%onsite group%')
Begin
Update dp_Page_Views set View_Title = 'Onsite Group' + ' Event Participants - All', View_Clause = 'Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 80' where Page_View_ID = 2136
End
IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2137 AND [View_Title] like '%onsite group%')
Begin
Update dp_Page_Views set View_Title = 'Onsite Group' + ' Events - All', View_Clause = 'Event_Type_ID_Table.[Event_Type_ID] = 80' where Page_View_ID = 2137
End
IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 92157 AND [View_Title] like '%onsite group%')
Begin
Update dp_Page_Views set View_Title = 'Onsite Group' + ' Event Groups - All', View_Clause = 'Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID] = 8 AND Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 80' where Page_View_ID = 92157
End