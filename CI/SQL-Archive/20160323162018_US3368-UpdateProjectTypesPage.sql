USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 12)
BEGIN
	UPDATE [dbo].[dp_Pages]
		SET Default_Field_List = 'Description, Minimum_Age, Inactive, SortOrder, Image_URL'
	WHERE Page_ID = 12
END