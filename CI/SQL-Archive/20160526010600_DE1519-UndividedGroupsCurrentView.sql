USE [MinistryPlatform]	
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dp_page_views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(92143,'Undivided Groups - Current',322,'Access current sessions open for registration'
	,'Groups.[Group_Name]
       , Groups.[Description]
       , Congregation_ID_Table.[Congregation_Name]
       , Ministry_ID_Table.[Ministry_Name]
       , Group_Type_ID_Table.[Group_Type]
       , Groups.[Start_Date] AS [Start_Date]
       , Groups.[End_Date] AS [End_Date]
       , Congregation_ID_Table.[Congregation_ID]'    
	,'(GetDate() BETWEEN Groups.Start_Date AND ISNULL(Groups.End_Date, GetDate())) AND Groups.Group_Type_ID = 26 AND Parent_Group_Table.[Group_ID] IS NULL'
	,'Groups.Group_Name'     
	,NULL
    ,NULL )
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
