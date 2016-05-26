USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
          SET View_Clause = 'Groups.[End_Date] IS NULL OR Groups.[End_Date] >= DATEADD(day, DATEDIFF(day, 0, GETDATE()+7),0)'
          WHERE Page_View_Id = 991
    
GO
