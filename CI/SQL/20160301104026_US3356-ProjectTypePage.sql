USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

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
           'An occurance of a volunteer program.',
           100,
           'cr_ProjectTypes',
           'ProjectType_ID',
           'Description, Minimum_Age',
           'Description',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO