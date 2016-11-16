USE [MinistryPlatform]
GO

DECLARE @PageSectionId AS INT = (SELECT Page_Section_ID from dp_Page_Sections WHERE Page_Section = 'Products & Payments');
DECLARE @ProductPageId AS INT = (SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Product Rules');
DECLARE @EventProductPageId AS INT = (SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Event Product Rules');
DECLARE @ProductRoleId AS INT = (SELECT Role_ID FROM dp_Role_Pages WHERE Page_ID = @ProductPageId);
DECLARE @EventProductRoleId AS INT = (SELECT Role_ID FROM dp_Role_Pages WHERE Page_ID = @EventProductPageId);



DELETE FROM [dbo].[dp_Page_Section_Pages]
      WHERE Page_Section_ID = @PageSectionId AND Page_ID = @ProductPageId ;

DELETE FROM [dbo].[dp_Page_Section_Pages]
      WHERE Page_Section_ID = @PageSectionId AND Page_ID = @EventProductPageId ;

DELETE FROM [dbo].[dp_Role_Pages]
      WHERE Role_ID = @ProductRoleId AND Page_ID = @ProductPageId;

DELETE FROM [dbo].[dp_Role_Pages]
      WHERE Role_ID = @EventProductRoleId AND Page_ID = @EventProductPageId;

DELETE FROM [dbo].[dp_Pages]
      WHERE Page_ID = @ProductPageId;

DELETE FROM [dbo].[dp_Pages]
      WHERE Page_ID = @EventProductPageId;
GO


