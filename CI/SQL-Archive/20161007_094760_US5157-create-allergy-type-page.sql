USE [MinistryPlatform]
GO

DECLARE @PAGEID int = 603;
DECLARE @PAGESECTIONID int = 4;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = @PAGEID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Pages] ON;
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
			   (@PAGEID
			   ,N'Allergy Types'
			   ,N'Allergy Type'
			   ,N''
			   ,50
			   ,N'cr_Allergy_Types'
			   ,N'Allergy_Type_ID'
			   ,0
			   ,N'cr_Allergy_Types.Description'
			   ,'Description'
			   ,0)
	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;
END

IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_ID] = @PAGEID )
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages] (
		 [Page_ID]
		,[Page_Section_ID])
		VALUES (
		  @PAGEID
		 ,@PAGESECTIONID)
END