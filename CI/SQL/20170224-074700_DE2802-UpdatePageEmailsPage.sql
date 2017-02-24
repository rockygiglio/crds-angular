USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Description] = 'The master list of active USER email addresses that are, or have been, involved in some way with your crossroads.'
   ,[Default_Field_List] = 'Contacts.Display_Name, Contacts.Contact_ID, User_Account_Table.User_Email, User_Account_Table.User_ID'
WHERE [Page_ID] = 481;
GO