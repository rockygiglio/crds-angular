USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE View_Title LIKE 'Pending Room Reservations - %' AND View_Title != 'Pending Room Reservations - All')
  BEGIN
    DELETE FROM [dbo].[dp_Page_Views]
    WHERE View_Title LIKE 'Pending Room Reservations - %' AND View_Title != 'Pending Room Reservations - All'
  END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE View_Title = 'Pending Room Reservations - All')
  BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
    INSERT INTO [dbo].[dp_Page_Views](Page_View_ID, View_Title, Page_ID, [Description], Field_List, View_Clause, Order_By)
    VALUES (1115,
      'Pending Room Reservations - All',
          384,
          '',
          'Event_ID_Table.[Event_Start_Date] AS [Event Start Date] , Event_ID_Table.[Event_End_Date] AS [Event End Date] , Event_ID_Table.[Event_Title] AS [Event Title] , Room_ID_Table.[Room_Name] AS [Room Name] , Event_Rooms.[Hidden] AS [Hidden] , Event_Rooms.[_Approved] AS [Approved] , Event_ID_Table_Location_ID_Table.[Location_Name] AS [Location Name], Event_ID_Table_Congregation_ID_Table.[Congregation_Name] AS [Congregation Name]',
          '(Event_Rooms._Approved = 0 OR Event_Rooms._Approved IS NULL) AND Event_Rooms.Cancelled = 0 AND Event_ID_Table.Event_End_Date >= Getdate() AND Event_Rooms.Event_Room_ID NOT IN (SELECT T._Record_ID FROM dp_Tasks T WHERE T._Page_ID= 384 AND T._Rejected = 1)',
          '')
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
  END
