USE [MinistryPlatform]
GO

DECLARE @Page_ID int = 608

IF EXISTS (SELECT 1 FROM dp_Pages WHERE Page_ID = @Page_ID)
BEGIN
	UPDATE [dbo].[dp_Pages]
	SET [Default_Field_List] = 'MedicalInformation_ID_Table_Contact_ID_Table.[Display_Name] AS [Contact] , MedicalInformation_ID_Table.[PolicyHolderName] AS [Policy Holder] , MedicalInformation_ID_Table.[InsuranceCompany] AS [Insurance Company] , MedicalInformation_ID_Table.[PhysicianName] AS [Physician Name] , MedicalInformation_ID_Table.[PhysicianPhone] AS [Physician Phone] , cr_Medical_Information_Medications.Medication_Name AS [Medication Name], Medication_Type_ID_Table.MedicationType AS [Medication Type], cr_Medical_Information_Medications.[DosageTime] AS [Dosage Time] , cr_Medical_Information_Medications.[DosageAmount] AS [Dosage Amount]'
	WHERE Page_ID = @Page_ID
END
