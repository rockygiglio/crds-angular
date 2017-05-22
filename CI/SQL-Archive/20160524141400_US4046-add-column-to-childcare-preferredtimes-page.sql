USE [MinistryPlatform]

UPDATE [dbo].[dp_Pages] 
	SET   Default_Field_List = 'Congregation_ID_Table.[Congregation_Name] AS [Congregation Name]
				, [Childcare_Start_Time] AS [Childcare Start Time] 
				, [Childcare_End_Time] AS [Childcare End Time]  
				, Childcare_Day_ID_Table.[Meeting_Day] AS [Childcare Day]
				, [Deactivate_Date] AS [Deactivate Date] '
	WHERE Page_ID = 37;

GO