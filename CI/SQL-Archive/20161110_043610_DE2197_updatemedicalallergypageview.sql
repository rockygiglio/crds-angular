USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Medical_Information_ID_Table_Contact_ID_Table.[Display_Name] AS [Contact]
                              , Medical_Information_ID_Table.[PolicyHolderName] AS [Policy Holder] 
							  , Medical_Information_ID_Table.[InsuranceCompany] AS [Insurance Company] 
							  , Medical_Information_ID_Table.[PhysicianName] AS [Physician Name] 
							  , Medical_Information_ID_Table.[PhysicianPhone] AS [Physician Phone]
							  , Allergy_ID_Table_Allergy_Type_ID_Table.[Allergy_Type] AS [Allergy Type] 
							  , Allergy_ID_Table.[Description] AS [Allergy Description]'
      ,[Selected_Record_Expression] = 'Allergy_ID_Table_Allergy_Type_ID_Table.Allergy_Type'
 WHERE Page_ID = 617;
GO


