USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dbo.dp_sub_Pages ON
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
           (606
		   ,'Allergies'
           ,'Allergy'
           ,607
           ,1
		   ,617
		   ,'Medical_Information_Allergy_ID'
           ,'cr_Medical_Information_Allergies'
           ,'Medical_Information_Allergy_ID'
           ,'Allergy_ID_Table_Allergy_Type_ID_Table.Allergy_Type, Allergy_ID_Table.Description'
           ,'Allergy_ID_Table_Allergy_Type_ID_Table.Allergy_Type'
           ,'Medical_Information_ID'
           ,2
		   ,1)
GO


