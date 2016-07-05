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
           (287
		   ,'Double Booked Rooms-CRDS'
           ,'Event Room Double Booked by Congregation'
           ,'/MPReports/Crossroads/Event Room Double Booked-CRDS'
           ,0
           ,0
           ,1)
GO


SET IDENTITY_INSERT dp_Reports OFF 