USE [MinistryPlatform]
GO

DECLARE @ReportID int = 312

SET IDENTITY_INSERT [dbo].[dp_Reports] ON
INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
		   ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (@ReportID
		   ,'Effective Permissions Report'
           ,'All the Pages/Subpages/Reports/Tools/Etc a user can access'
           ,'/MPReports/Crossroads/CRDSUserEffectivePermissions'
           ,1
           ,1
           ,0)
SET IDENTITY_INSERT [dbo].[dp_Reports] OFF

INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_ID]
           ,[Page_ID])
     VALUES
           (@ReportID
           ,401)
GO

