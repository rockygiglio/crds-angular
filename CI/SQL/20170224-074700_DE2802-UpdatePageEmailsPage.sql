USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Table_Name] = 'dp_Users'
   ,[Description] = 'The master list of active User email addresses that are, or have been, involved in some way with your crossroads account.' 
  ,[Default_Field_List] = 'dp_Users.User_Name, dp_Users.User_Email, dp_Users.User_ID'
  ,[Selected_Record_Expression] = 'User_Name'
  ,[Filter_Clause] = 'dp_Users.User_ID > 0'
  ,[Contact_ID_Field] = '' 
  ,[System_Name] = ''
  ,[Custom_Form_Name] = ''
  ,[Display_Copy] = 0
WHERE [Page_ID] = 481;
GO