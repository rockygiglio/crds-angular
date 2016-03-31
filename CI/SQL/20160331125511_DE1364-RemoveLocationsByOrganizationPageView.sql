USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2220 AND View_Title = 'Locations by Organization')
BEGIN
	DELETE FROM [dbo].[dp_Page_Views] 
	WHERE Page_View_ID = 2220
END