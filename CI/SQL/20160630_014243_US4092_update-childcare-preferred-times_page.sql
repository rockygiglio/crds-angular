USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Congregation_ID_Table.[Congregation_Name] AS [Congregation Name] , Childcare_Day_ID_Table.[Meeting_Day] AS [Childcare Day] , [Childcare_Start_Time] AS [Childcare Start Time] , [Childcare_End_Time] AS [Childcare End Time] , cr_Childcare_Preferred_Times.[End_Date] AS [End Date]'
   WHERE [Display_Name] = 'Childcare Preferred Times';
GO


