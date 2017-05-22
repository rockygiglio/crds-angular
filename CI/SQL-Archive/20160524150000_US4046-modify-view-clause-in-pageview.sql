USE MinistryPlatform
GO

UPDATE [dbo].[dp_Page_Views]
      SET View_Clause = 'cr_Childcare_Preferred_Times.[Deactivate_Date] > GetDate() OR cr_Childcare_Preferred_Times.[Deactivate_Date] IS NULL',
	      Field_list = 'Congregation_ID_Table.[Congregation_ID] , cr_Childcare_Preferred_Times.[Childcare_Preferred_Time_ID] , cr_Childcare_Preferred_Times.[Childcare_Start_Time] , cr_Childcare_Preferred_Times.[Childcare_End_Time] , Childcare_Day_ID_Table.[Meeting_Day], cr_Childcare_Preferred_Times.[Childcare_Day_ID], cr_Childcare_Preferred_Times.[Deactivate_Date]'
      WHERE Page_View_ID = 992
GO