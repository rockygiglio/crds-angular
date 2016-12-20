USE MinistryPlatform
GO

DECLARE @SubPageId int = 610

IF EXISTS(SELECT 1 FROM dp_Sub_Pages WHERE Sub_Page_ID = @SubPageId)
BEGIN
	UPDATE dp_Sub_Pages
	SET Default_Field_List = 'Maximum_Registrants,Rule_Start_Date, Rule_End_Date'
	WHERE Sub_Page_ID = @SubPageId
END