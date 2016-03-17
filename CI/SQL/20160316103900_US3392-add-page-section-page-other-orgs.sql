USE [MinistryPlatform]
GO

DECLARE @OTHER_ORG_PAGE int;
DECLARE @PAGE_SECTION int;

SELECT @OTHER_ORG_PAGE = [Page_ID] FROM 
	[dbo].[dp_Pages] p
	WHERE p.Display_Name = N'Other Organizations'

SELECT @PAGE_SECTION = [Page_Section_ID] FROM
	[dbo].[dp_Page_Sections]
	WHERE [Page_Section] = N'Lookup Values'

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (@OTHER_ORG_PAGE
           ,@PAGE_SECTION)
GO


