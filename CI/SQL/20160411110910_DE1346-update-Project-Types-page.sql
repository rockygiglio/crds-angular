USE [MinistryPlatform]
GO

DECLARE @pageId AS INT;
SET @pageId = (SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Project Types');

UPDATE [dbo].[dp_Pages]
   SET [Primary_Key] = 'Project_Type_ID',
       [Default_Field_List] = 'Description, Minimum_Age, Inactive, Sort_Order, Image_URL'
 WHERE Page_ID = @pageId;
GO

