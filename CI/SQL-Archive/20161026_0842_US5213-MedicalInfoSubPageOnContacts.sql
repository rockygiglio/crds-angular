USE MinistryPlatform
GO

DECLARE @SubPageId INT = 604

IF NOT EXISTS(SELECT * FROM dp_sub_pages WHERE Sub_Page_ID = @SubPageId)
BEGIN
SET IDENTITY_INSERT dp_Sub_Pages ON

INSERT INTO dp_Sub_Pages(Sub_Page_ID,
       Display_Name,
	   Singular_Name,
	   Page_ID,
	   View_Order,
	   Primary_Table,
	   Primary_Key,
	   Default_Field_List,
	   Selected_Record_Expression,
	   Filter_Key,
	   Relation_Type_ID,
	   Display_Copy)
VALUES(@SubPageId,
       'Medical Information',
	   'Medical Information',
	   292,
	   300,
	   'cr_Medical_Information',
	   'MedicalInformation_ID',
	   'InsuranceCompany, PolicyHolderName, PhysicianName, PhysicianPhone',
	   'InsuranceCompany',
	   'MedicalInformation_ID',
	   3,
	   1);

SET IDENTITY_INSERT dp_Sub_Pages OFF
END

