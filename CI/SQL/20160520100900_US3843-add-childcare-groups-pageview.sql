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
           (991
		   ,'Childcare Group Selection'
           ,322
           ,'Congregation_ID_Table.[Congregation_ID] , Ministry_ID_Table.[Ministry_ID] , Groups.[Group_ID] , Groups.[Group_Name] '
           ,'Groups.[End_Date] IS NOT NULL '
		   ,'Groups.[Group_Name]');


		   SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


