USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Reports]
           ([Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           ('Persons of Interest Report'
           ,'Person of interest report, show partcipant picture instead of contact'
           ,'/MPReports/crossroads/CRDS Persons of Interest Report'
           ,1
           ,0
           ,1)
GO


