USE [MinistryPlatform]
GO

IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 100
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (100,287,1 )
 END

