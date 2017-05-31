USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM dp_Pages WHERE page_id =607)
BEGIN
	UPDATE dp_pages
	SET Default_Field_List='cr_Medical_Information.MedicalInformation_ID AS Medical_Information_ID,cr_Medical_Information.InsuranceCompany AS Insurance_Company,cr_Medical_Information.PolicyHolderName AS Policy_Holder,cr_Medical_Information.PhysicianName as Physician_Name,cr_Medical_Information.PhysicianPhone AS Physician_Phone' 
	WHERE page_id =607
END
GO

