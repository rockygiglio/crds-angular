USE MinistryPlatform
GO

-- Update all columns in Medical_Information Table

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'InsuranceCompany'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information.InsuranceCompany', 'Insurance_Company', 'COLUMN';
END

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'PolicyHolderName'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information.PolicyHolderName', 'Policy_Holder_Name', 'COLUMN';
END

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'PhysicianName'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information.PhysicianName', 'Physician_Name', 'COLUMN';
END

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'PhysicianPhone'
      AND Object_ID = Object_ID(N'dbo.cr_Medical_Information'))
BEGIN
	EXECUTE sp_rename 'dbo.cr_Medical_Information.PhysicianPhone', 'Physician_Phone', 'COLUMN';
END

-- Update Medical Information Page
IF EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE [Page_ID] = 607) 
BEGIN 
	UPDATE [dbo].[dp_Pages]
	SET 
	 [Default_Field_List] = N'Contact_ID_Table.Display_Name, cr_Medical_Information.Insurance_Company,cr_Medical_Information.Policy_Holder_Name AS Policy_Holder,cr_Medical_Information.Physician_Name,cr_Medical_Information.Physician_Phone'
	,[Selected_Record_Expression] = N'cr_Medical_Information.Insurance_Company + ''; '' + cr_Medical_Information.Policy_Holder_Name'
	WHERE [Page_ID] = 607
END