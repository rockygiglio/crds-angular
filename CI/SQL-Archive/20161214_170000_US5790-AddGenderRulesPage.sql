USE [MinistryPlatform]
GO

DECLARE @PageId int = 621;

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
			   ,'Gender Rules'
			   ,'Gender Rule'
			   ,'Which Genders this ruleset applies to'
			   ,1
			   ,'cr_Rule_Genders'
			   ,'Rule_Gender_ID'
			   ,1
			   ,'Gender_ID_Table.Gender, cr_Rule_Genders.Rule_Start_Date, cr_Rule_Genders.Rule_End_Date'
			   ,'Gender_ID_Table.Gender'
			   ,1)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END