USE [MinistryPlatform]
GO

DECLARE @ReportID int = 313

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
		   ,'Camp Registration Report'
           ,'All Registered and Interested Contacts for a specific Summer Camp'
           ,'/MPReports/Crossroads/CRDS_Camp_Registration'
           ,1
           ,1
           ,0)
SET IDENTITY_INSERT [dbo].[dp_Reports] OFF

INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_ID]
           ,[Page_ID])
     VALUES
           (@ReportID
           ,308) --Events Page
GO

