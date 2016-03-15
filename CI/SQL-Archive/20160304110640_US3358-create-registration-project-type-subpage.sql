USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Pages] WHERE Sub_Page_ID = 248)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           (248
		   ,'Project Preferences'
           ,'Project Preference'
           ,16
           ,10
           ,'cr_Registration_Project_Type'
           ,'Registration_Project_Type_ID'
           ,'Project_Type_ID_Table.Description, Priority'
           ,'Registration_Project_Type_ID'
           ,'Registration_ID'
           ,2
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
END