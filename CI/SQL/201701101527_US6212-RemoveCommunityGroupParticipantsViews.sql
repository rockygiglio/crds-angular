USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE View_Title LIKE 'Community Group Participants - %' AND View_Title != 'Community Group Participants - All')
BEGIN
	DELETE FROM [dbo].[dp_Page_Views]
	WHERE View_Title LIKE 'Community Group Participants - %' AND View_Title != 'Community Group Participants - All'
END
