USE [MinistryPlatform]
GO

DECLARE @Page_ID int = 604

IF EXISTS (SELECT 1 FROM dp_Pages WHERE Page_ID = @Page_ID)
BEGIN
	UPDATE [dbo].[dp_Pages]
	   SET [Display_Name] = 'Medication Types'
		  ,[Singular_Name] = 'Medication Type'
		  ,[Description] = 'Medication Types'
		  ,[Table_Name] = 'cr_Medication_Types'
		  ,[Primary_Key] = 'Medication_Type_ID'
		  ,[Default_Field_List] = 'Medication_Type'
		  ,[Selected_Record_Expression] = 'Medication_Type'
	 WHERE Page_ID = @Page_ID
END
