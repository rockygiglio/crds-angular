USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Display_Name] = 'Events with Reminder'
  ,[Singular_Name] = 'Event with Reminder'
  ,[Description] = 'The calendared activities.'
  ,[View_Order] = 25
  ,[Table_Name] = 'Events'
  ,[Primary_Key] = 'Event_ID'
  ,[Display_Search] = 1
  ,[Default_Field_List] = 'Events.[Event_Start_Date] 
  ,Events.[Event_Title] 
  ,Event_Type_ID_Table.[Event_Type] 
  ,Congregation_ID_Table.[Congregation_Name] 
  ,Program_ID_Table.[Program_Name] 
  ,Events.[Event_End_Date] 
  ,Visibility_Level_ID_Table.[Visibility_Level]
  , Events.[Send_Reminder]
  , Events.[Event_ID]
  , Events.[Reminder_Sent]
  , Primary_Contact_Table.[Contact_ID] as [Primary_Contact_ID]
  , Primary_Contact_Table.[Email_Address] as [Primary_Contact_Email_Address] '
  ,[Selected_Record_Expression] = 'Events.Event_Title'
  ,[Filter_Clause] = 'Events.Cancelled = 0 and 
  Events.Send_Reminder = 1 and 
  Events.Reminder_Sent = 0 and 
  DATEDIFF( d, GETDATE(), Events.Event_Start_Date) <= Reminder_Days_Prior_ID_Table.Reminder_Days_Prior'
  ,[Start_Date_Field] = 'Event_Start_Date'
  ,[End_Date_Field] = 'Event_End_Date'
  ,[Contact_ID_Field] = 'Events.Primary_Contact'
  ,[Direct_Delete_Only] = 1
  ,[Display_Copy] = 1
WHERE [Page_ID] = 492;
GO
