USE [MinistryPlatform]
GO

DECLARE @PageViewID int = 92720
UPDATE [dbo].[dp_Page_Views]
   SET [View_Title] = 'GO Local'
      ,[Description] = 'Filter attributes to only those used to manage GO Local.'
      ,[View_Clause] = 'Attribute_Type_ID_Table.[Attribute_Type] LIKE ''GO Local -%'''
 WHERE Page_View_ID = @PageViewID
GO
