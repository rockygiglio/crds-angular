USE [MinistryPlatform]
GO

BEGIN
	DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID BETWEEN 2207 AND 2211
END