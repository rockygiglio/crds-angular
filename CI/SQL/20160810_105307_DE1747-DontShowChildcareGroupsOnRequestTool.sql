USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
   SET [View_Clause] = '(Groups.[End_Date] IS NULL OR Groups.[End_Date] >= DATEADD(day, DATEDIFF(day, 0, GETDATE()+7),0)) AND Group_Type_ID_Table.[Group_Type_ID] <> 27'
 WHERE Page_View_ID = 991
GO