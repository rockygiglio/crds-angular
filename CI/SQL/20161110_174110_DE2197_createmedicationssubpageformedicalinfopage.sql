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
           (608
		   ,'Medications'
           ,'Medication'
           ,607
           ,2
		   ,617
		   ,'MedicalInformationMedication_ID'
           ,'cr_Medical_Information_Medications'
           ,'MedicalInformationMedication_ID'
           ,'Medication_ID_Table.[MedicationName]
		    , Medication_ID_Table.[MedicationType]
			, [DosageTime]
			,[DosageAmount]'
           ,'Medication_ID_Table.[MedicationType]'
           ,'MedicalInformation_ID'
           ,2
		   ,1)
GO
