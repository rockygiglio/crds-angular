USE [MinistryPlatform]
GO

--delete from dp_Page_Section_Pages WHERE Page_Section_ID = 21 AND Page_ID = 13

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE Page_Section_ID = 21 AND Page_ID = 13)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (13, 21)
END
GO

INSERT INTO dp_Role_Pages (Role_ID, Page_ID, Access_Level) VALUES (107, 13, 3)