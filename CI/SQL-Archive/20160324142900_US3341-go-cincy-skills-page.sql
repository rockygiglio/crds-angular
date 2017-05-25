USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON;

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
           (17
           ,'Go Volunteer Skills'
           ,'Go Volunteer Skill'
           ,'Skills that are only applicable to Go Cincinnati'
           ,12
           ,'cr_Go_Volunteer_Skills'
           ,'Go_Volunteer_Skills_ID'
           ,'Go_Volunteer_Skills_ID,Label,Attribute_ID_Table.Attribute_Name'
           ,'Attribute_ID_Table.Attribute_Name'
           ,0)

GO

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;