USE MinistryPlatform
GO

DECLARE @SubPageId INT = 604

IF EXISTS (SELECT * FROM dp_Sub_Pages WHERE sub_page_id = @SubPageId) 
BEGIN
	UPDATE dp_Sub_Pages SET Link_To_Page_ID = 607, Link_From_Field_Name = 'MedicalInformation_ID' WHERE sub_page_id = @SubPageId
END

GO
