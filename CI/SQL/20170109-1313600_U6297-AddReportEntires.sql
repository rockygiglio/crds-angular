USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Reports ON 
INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
		   ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (308
		   ,'Contribution Statements'
           ,'Contribution Statements'
           ,'/MPReports/crossroads/CRDS Statement Columns'
           ,1
           ,0
           ,1)
GO


INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
		   ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (309
		   ,'Contribution Statements Selected'
           ,'Contribution Statements Selected'
           ,'/MPReports/crossroads/CRDS Statement Columns'
           ,1
           ,0
           ,1)
GO


SET IDENTITY_INSERT dp_Reports OFF 