USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].dp_Sub_Pages ON

IF NOT EXISTS (SELECT * FROM dp_Sub_Pages WHERE Sub_Page_ID = 611)
BEGIN	
   INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
		   ,[Link_To_Page_ID]
		   ,[Link_From_Field_Name]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
       VALUES
           (611,
		   'Rulesets'
           ,'Ruleset'
           ,(SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Products')
           ,5
		   ,620
		   ,'Ruleset_ID_Table.Ruleset_Name'
           ,'cr_Product_Ruleset'
           ,'Product_Ruleset_ID'
           ,'Ruleset_ID_Table.Ruleset_Name, Start_date, End_Date'
           ,'Ruleset_ID_Table.Ruleset_Name'
           ,'Product_ID'
           ,2
           ,1)
END
GO
SET IDENTITY_INSERT [dbo].dp_Sub_Pages OFF
GO