USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dp_Pages] ON

IF NOT EXISTS (SELECT * FROM dp_Pages WHERE Page_ID = 620)
BEGIN

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
           (620
		   ,'Rulesets'
           ,'Ruleset'
           ,'will provide a set of rules'
           ,1
           ,'cr_Ruleset'
           ,'cr_Ruleset_ID'
           ,1
           ,'Ruleset_Name, Description, Ruleset_Start_Date, Ruleset_End_Date'
           ,'Ruleset_Name'
           ,1)

END
GO

INSERT INTO [dbo].[dp_Page_Sections]
           ([Page_Section]
           ,[View_Order])
     VALUES
           ('Rules'
           ,10)
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (620
           ,(SELECT Page_Section_ID FROM dp_Page_Sections WHERE Page_Section = 'Rules'))
GO



