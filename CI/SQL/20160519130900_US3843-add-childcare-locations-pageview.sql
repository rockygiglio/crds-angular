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
           (990
		   ,'Congregations With Childcare'
           ,288
           ,'Congregations.[Congregation_ID] , Congregations.[Congregation_Name] '
           ,'Childcare_Contact_Table.[Contact_ID] IS NOT NULL '
		   ,'Congregations.[Congregation_Name]');


		   SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


