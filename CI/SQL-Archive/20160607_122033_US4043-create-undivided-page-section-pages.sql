USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID]
           ,[User_ID])
     VALUES
           (556 
           ,(SELECT Page_Section_ID FROM dp_Page_Sections WHERE Page_Section = 'People Lists')
           ,null)
GO