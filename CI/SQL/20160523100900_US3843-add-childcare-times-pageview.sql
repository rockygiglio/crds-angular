USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By])
     VALUES
           (992
		   ,'Childcare Times By Congregation'
           ,37
           ,'Congregation_ID_Table.[Congregation_ID] AS [Congregation ID] , cr_Childcare_Preferred_Times.[Childcare_Preferred_Time_ID] AS [Childcare Preferred Time ID] , cr_Childcare_Preferred_Times.[Childcare_Start_Time] AS [Childcare Start Time] , cr_Childcare_Preferred_Times.[Childcare_End_Time] AS [Childcare End Time] , Childcare_Day_ID_Table.[Meeting_Day] AS [Meeting Day] '
           ,'Congregation_ID_Table.[End_Date] IS NULL '
		   ,'Childcare_Day_ID_Table.[Meeting_Day_ID] , cr_Childcare_Preferred_Times.[Childcare_Start_Time] ');


		   SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


