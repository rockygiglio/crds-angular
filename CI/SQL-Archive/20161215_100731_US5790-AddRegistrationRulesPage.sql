USE [MinistryPlatform]
GO

DECLARE @PageId int = 622;

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
			   ,'Registration Rules'
			   ,'Registration Rule'
			   ,'How many people can register'
			   ,1
			   ,'cr_Rule_Registrations'
			   ,'Rule_Registration_ID'
			   ,1
			   ,'cr_Rule_Registrations.Minimum_Registrants, cr_Rule_Registrations.Maximum_Registrants, cr_Rule_Registrations.Rule_Start_Date, cr_Rule_Registrations.Rule_End_Date'
			   ,'Rule_Registration_ID'
			   ,1)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END