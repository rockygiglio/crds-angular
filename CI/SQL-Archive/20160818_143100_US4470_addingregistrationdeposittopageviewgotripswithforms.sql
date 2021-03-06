USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
   SET [Field_List] = 'Pledge_Campaigns.[Pledge_Campaign_ID] 
   , Pledge_Campaigns.[Campaign_Name] 
   , Pledge_Campaign_Type_ID_Table.[Campaign_Type] 
   , Pledge_Campaigns.[Start_Date] 
   , Pledge_Campaigns.[End_Date] 
   , Pledge_Campaigns.[Campaign_Goal] 
   , Registration_Form_Table.[Form_ID] 
   , Registration_Form_Table.[Form_Title] 
   , Pledge_Campaigns.[Registration_Start] 
   , Pledge_Campaigns.[Registration_End] 
   , Pledge_Campaigns.[Registration_Deposit]
   , Pledge_Campaigns.[Youngest_Age_Allowed] 
   , Event_ID_Table.[Event_Start_Date] 
   ,Pledge_Campaigns.[Nickname] 
   , Event_ID_Table.[Event_ID] 
   , Pledge_Campaigns.[Program_ID]'
 WHERE Page_View_ID = 92193;
GO


