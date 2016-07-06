USE [MinistryPlatform]
GO

IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 2
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (2,287,1 )
 END

 IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 4
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (4,287,1 )
 END
 IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 38
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (38,287,1 )
 END
 IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 107
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (107,287,1 )
 END
 IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 89
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (89,287,1 )
 END
 IF NOT EXISTS ( SELECT * FROM dp_Role_Reports 
                WHERE  Role_ID = 90
                   AND Report_ID = 287
                   AND Domain_ID = 1 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Role_Reports]([Role_ID],[Report_ID],[Domain_ID])
	VALUES (90,287,1 )
 END

 GO