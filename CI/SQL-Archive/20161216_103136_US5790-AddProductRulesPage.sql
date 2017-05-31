USE [MinistryPlatform]
GO

DECLARE @PageId int = 623;

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = @PageId)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Pages] ON
	INSERT INTO [dbo].[dp_Pages]
			   ([Page_ID]
			   ,[Display_Name]
			   ,[Singular_Name]
			   ,[Description]
			   ,[View_Order]
			   ,[Table_Name]
			   ,[Primary_Key]
			   ,[Display_Search]
			   ,[Default_Field_List]
			   ,[Selected_Record_Expression]
			   ,[Display_Copy])
		 VALUES
			   (@PageId
			   ,'Product Rulesets'
			   ,'Product Ruleset'
			   ,'Connects Rulesets to Products'
			   ,1
			   ,'cr_Product_Ruleset'
			   ,'Product_Ruleset_ID'
			   ,1
			   ,'cr_Product_Ruleset.Product_ID, cr_Product_Ruleset.Ruleset_ID, cr_Product_Ruleset.Start_Date, cr_Product_Ruleset.End_Date'
			   ,'Product_Ruleset_ID'
			   ,1)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END