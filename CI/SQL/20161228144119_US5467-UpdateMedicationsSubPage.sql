USE [MinistryPlatform]
GO

DECLARE @SubPageId int = 608

IF EXISTS(SELECT 1 FROM dp_Sub_Pages WHERE Sub_Page_ID = @SubPageId)
BEGIN
	UPDATE [dbo].[dp_Sub_Pages]
	SET 
	   [Default_Field_List] = '[Medication_Name], Medication_Type_ID_Table.[Medication_Type], [DosageTime], [DosageAmount]'
      ,[Selected_Record_Expression] = 'Medication_Type_ID_Table.[Medication_Type]'
	WHERE Sub_Page_ID = @SubPageId
END