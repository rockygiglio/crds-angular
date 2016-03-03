USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 12)
BEGIN
INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
		(12,
           'Project Types',
           'Project Type',
           'Type of a volunteer project.',
           100,
           'cr_Project_Types',
           'ProjectType_ID',
           'Description, Minimum_Age',
           'Description',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END