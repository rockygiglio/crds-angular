USE MinistryPlatform
GO

-- Update columns in Medical_Information_Medications Table

IF EXISTS(
	SELECT *
    FROM sys.columns 
    WHERE Name      = N'DosageTime'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information_Medications'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information_Medications.DosageTime', 'Dosage_Time', 'COLUMN';
END

IF EXISTS(
	SELECT *
    FROM sys.columns 
    WHERE Name      = N'DosageAmount'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information_Medications'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information_Medications.DosageAmount', 'Dosage_Amount', 'COLUMN';
END


-- Update Medical Information Medications Page
DECLARE @Page_ID int = 608
IF EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE [Page_ID] = @Page_ID) 
BEGIN 
	UPDATE [dbo].[dp_Pages]
	SET [Default_Field_List] = N'MedicalInformation_ID_Table_Contact_ID_Table.[Display_Name] AS [Contact] , MedicalInformation_ID_Table.[Policy_Holder_Name] AS [Policy Holder] , MedicalInformation_ID_Table.[Insurance_Company] AS [Insurance Company] , MedicalInformation_ID_Table.[Physician_Name] AS [Physician Name] , MedicalInformation_ID_Table.[Physician_Phone] AS [Physician Phone] , cr_Medical_Information_Medications.Medication_Name AS [Medication Name], Medication_Type_ID_Table.Medication_Type AS [Medication Type], cr_Medical_Information_Medications.[Dosage_Time] AS [Dosage Time] , cr_Medical_Information_Medications.[Dosage_Amount] AS [Dosage Amount]'
	WHERE [Page_ID] = @Page_ID
END

DECLARE @Sub_Page_ID int = 608
IF EXISTS(SELECT 1 FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = @Sub_Page_ID)
BEGIN
	UPDATE [dbo].[dp_Sub_Pages]
	SET [Default_Field_List] = N'[Medication_Name], Medication_Type_ID_Table.[Medication_Type], [Dosage_Time], [Dosage_Amount]'
	WHERE [Sub_Page_ID] = @Sub_Page_ID
END