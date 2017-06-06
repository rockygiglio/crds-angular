USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Page_Views]
   SET [Field_List] = 'Event_ID_Table.[Event_ID]
                      , Event_ID_Table.[Event_Title]
					  , Event_ID_Table_Congregation_ID_Table.[Congregation_Name]
					  , Event_ID_Table.[Event_Start_Date]
					  , Event_ID_Table.[Event_End_Date]
					  , Event_ID_Table_Event_Type_ID_Table.[Event_Type]'
      
   WHERE [View_Title]= 'Events with Congregation';
GO


