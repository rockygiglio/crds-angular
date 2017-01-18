USE [MinistryPlatform]
GO

BEGIN
	DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID BETWEEN 92424 AND 92428

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

	INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (1116
		   ,'Pending Equipment Reservations'
           ,302
           ,'Event_ID_Table.[Event_Start_Date] AS [Event Start Date]
, Event_ID_Table.[Event_End_Date] AS [Event End Date]
, Event_ID_Table.[Event_Title] AS [Event Title]
, Equipment_ID_Table.[Equipment_Name] AS [Equipment Name]
, Event_Equipment.[Quantity_Requested] AS [Quantity Requested]
, Event_Equipment.[_Approved] AS [Approved]
, Event_ID_Table_Location_ID_Table.[Location_Name] AS [Location Name]'
           ,'(Event_Equipment._Approved = 0 OR Event_Equipment._Approved IS NULL) 
  AND Event_Equipment.Cancelled = 0 
  AND Event_ID_Table.Event_End_Date >= Getdate() 
  AND Event_Equipment.Event_Equipment_ID NOT IN (SELECT T._Record_ID FROM dp_Tasks T WHERE T._Page_ID= 302 AND T._Rejected = 1)');
  
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

END