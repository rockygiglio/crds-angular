USE [MinistryPlatform]
GO
SET IDENTITY_INSERT dbo.dp_Pages ON
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
           (617
		   ,'Medical Information Allergies'
           ,'Medical Information Allergy'
           ,'Medical Information Allergy'
           ,11
           ,'cr_Medical_Information_Allergies'
           ,'Medical_Information_Allergy_ID'
           ,'cr_Medical_Information_Allergies.Medical_Information_ID
		     ,cr_Medical_Information_Allergies.Allergy_ID'
           ,'cr_Medical_Information_Allergies.Allergy_ID'
		   ,1)
SET IDENTITY_INSERT dbo.dp_Pages OFF
           
GO
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (617
           ,22)
GO



