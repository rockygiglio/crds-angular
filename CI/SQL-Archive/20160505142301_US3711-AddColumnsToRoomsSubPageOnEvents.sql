USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Pages]
  SET [Default_Field_List] = '
  Room_ID_Table_Building_ID_Table.Building_Name
,Room_ID_Table.Room_Name
,Room_Layout_ID_Table.Layout_Name
,Event_Rooms._Approved
,Event_Rooms.Notes
,Event_Rooms.Label
,Event_Rooms.Allow_Checkin
,Event_Rooms.Capacity
,Event_Rooms.Volunteers
'
WHERE [Sub_Page_ID] = 285
GO
