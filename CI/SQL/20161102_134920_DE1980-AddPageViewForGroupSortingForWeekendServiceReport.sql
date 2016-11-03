USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By])
     VALUES
           ('KC Group Sort'
           ,322 --Groups
           ,'Set sort order for Weekend Service Report'
           ,'Groups.KC_Sort_Order, Groups.Group_Name'
           ,'Ministry_ID_Table.[Ministry_Name] = ''Kids Club'' AND Groups.[End_Date] IS NULL
			 AND (Groups.End_Date IS NULL OR Groups.End_Date >= GetDate())'
           ,'Groups.KC_Sort_Order, Groups.Group_Name')
GO

