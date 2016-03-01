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
		(14,
           'Projects',
           'Project',
           'A volunteer project.',
           100,
           'cr_Projects',
           'Project_ID',
           'Project_Name, Location_ID_Table.Location_Name, ProjectType_ID_Table.Description, Organization_ID_Table.Name, Initiative_ID_Table.Initiative_Name, Minimum_Volunteers, Maximum_Volunteers',
           'Project_Name',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO