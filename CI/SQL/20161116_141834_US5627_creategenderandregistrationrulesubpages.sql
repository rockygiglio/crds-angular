USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].dp_Sub_Pages ON

IF NOT EXISTS (SELECT * FROM dp_Sub_Pages WHERE Sub_Page_ID = 610)
BEGIN
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
           (610,
		   'Registration Rules'
           ,'Registration Rule'
           ,620
           ,2
           ,'cr_Rule_Registrations'
           ,'Rule_Registration_ID'
           ,'Minimum_Registrants, Maximum_Registrants,Rule_Start_Date, Rule_End_Date'
           ,'Maximum_Registrants'
           ,'Ruleset_ID'
           ,2
           ,1)
END
GO

IF NOT EXISTS (SELECT * FROM dp_Sub_Pages WHERE Sub_Page_ID = 612)
BEGIN
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
           (612,
		   'Gender Rules'
           ,'Gender Rule'
           ,620
           ,2
           ,'cr_Rule_Genders'
           ,'Rule_Gender_ID'
           ,'Gender_ID_Table.Gender, Rule_Start_Date, Rule_End_Date'
           ,'Gender_ID_Table.Gender'
           ,'Ruleset_ID'
           ,2
           ,1)

END
GO

SET IDENTITY_INSERT [dbo].dp_Sub_Pages OFF

GO
