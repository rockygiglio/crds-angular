USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'MedicalInformation_ID_Table_Contact_ID_Table.[Display_Name] AS [Contact] 
                              , MedicalInformation_ID_Table.[PolicyHolderName] AS [Policy Holder] 
							  , MedicalInformation_ID_Table.[InsuranceCompany] AS [Insurance Company] 
							  , MedicalInformation_ID_Table.[PhysicianName] AS [Physician Name] 
							  , MedicalInformation_ID_Table.[PhysicianPhone] AS [Physician Phone] 
							  , Medication_ID_Table.[MedicationName] AS [Medication Name] 
							  , Medication_ID_Table.[MedicationType] AS [Medication Type] 
							  , cr_Medical_Information_Medications.[DosageTime] AS [Dosage Time] 
							  , cr_Medical_Information_Medications.[DosageAmount] AS [Dosage Amount]'
 WHERE [Page_ID] = 608
GO


