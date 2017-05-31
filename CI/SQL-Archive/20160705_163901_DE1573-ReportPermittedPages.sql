USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Report_Pages ON 

IF NOT EXISTS ( SELECT * FROM dp_Report_Pages 
                WHERE Report_Page_ID = 1961
                   AND Report_ID = 287
                   AND Page_ID = 308 ) 
 BEGIN
	INSERT INTO [dbo].[dp_Report_Pages]([Report_Page_ID],[Report_ID],[Page_ID])
	VALUES (1961,287,308 )
 END


 SET IDENTITY_INSERT dp_Report_Pages OFF 

