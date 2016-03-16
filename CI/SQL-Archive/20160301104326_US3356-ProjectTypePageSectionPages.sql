USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE Page_Section_ID = 21 AND Page_ID = 12)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (12, 21)
END
GO