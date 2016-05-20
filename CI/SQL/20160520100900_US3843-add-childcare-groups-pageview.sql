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
           ,'Congregation_ID_Table.[Congregation_ID] AS [Congregation ID] , Ministry_ID_Table.[Ministry_ID] AS [Ministry ID] , Groups.[Group_ID] AS [Group ID] , Groups.[Group_Name] AS [Group Name] '
           ,'Groups.[End_Date] IS NOT NULL '
		   ,'Groups.[Group_Name]');


		   SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


