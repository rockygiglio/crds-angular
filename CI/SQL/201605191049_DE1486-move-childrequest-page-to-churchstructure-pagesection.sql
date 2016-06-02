USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           ((SELECT Page_ID FROM dp_Pages WHERE Display_Name = N'Childcare Requests')
           ,(Select Page_Section_ID FROM dp_Page_Sections WHERE Page_Section = N'Church Structure'))
GO
