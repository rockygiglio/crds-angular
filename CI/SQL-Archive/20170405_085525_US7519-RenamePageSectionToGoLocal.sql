USE [MinistryPlatform]
GO

DECLARE @GOCincinnatiPageSection int = 21;

UPDATE [dbo].[dp_Page_Sections]
   SET [Page_Section] = 'GO Local'
 WHERE Page_Section_ID = @GOCincinnatiPageSection
GO
