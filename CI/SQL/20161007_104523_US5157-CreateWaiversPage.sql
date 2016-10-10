USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 605)
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
           (605
		   ,'Waivers'
           ,'Waiver'
           ,'A waiver that needs to be signed by a contact'
           ,100
           ,'cr_Waivers'
           ,'Waiver_ID'
           ,1
           ,'Waiver_Name, Waiver_Text'
           ,'Waiver_Name'
           ,1)
SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE Page_ID = 605)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (605
           ,22)
END

