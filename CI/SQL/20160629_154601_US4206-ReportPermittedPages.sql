USE [MinistryPlatform]
GO

IF NOT EXISTS ( SELECT * FROM dp_Report_Pages 
                WHERE Report_Page_ID = 1952
                   AND Report_ID = 281
                   AND Page_ID = 292 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Report_Pages]([Report_Page_ID],[Report_ID],[Page_ID])
	VALUES (1952,281,292 )
 END


 IF NOT EXISTS ( SELECT * FROM dp_Report_Pages 
                WHERE Report_Page_ID = 1955
                   AND Report_ID = 281
                   AND Page_ID = 309 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Report_Pages]([Report_Page_ID],[Report_ID],[Page_ID])
	VALUES (1955,281,309 )
 END



