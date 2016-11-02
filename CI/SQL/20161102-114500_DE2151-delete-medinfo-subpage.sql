USE [MinistryPlatform]
GO

DELETE FROM [dbo].[dp_Role_Sub_Pages]
	WHERE Sub_Page_ID = 604

DELETE FROM [dbo].[dp_Sub_Pages]
      WHERE Sub_Page_ID = 604
GO