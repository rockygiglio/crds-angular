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
           (281
		   ,'Persons of Interest Report'
           ,'Person of interest report, show partcipant picture instead of contact'
           ,'/MPReports/crossroads/CRDS Persons of Interest Report'
           ,1
           ,0
           ,1)
GO


SET IDENTITY_INSERT dp_Reports OFF 