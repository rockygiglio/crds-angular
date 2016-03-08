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
		(15,
           'Project Statuses',
           'Project Status',
           'The status of a volunteer project.',
           100,
           'cr_Project_Statuses',
           'Project_Status_ID',
           'Description',
           'Description',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO