USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM dp_Pages WHERE Page_ID = 606)
BEGIN
   UPDATE [dbo].[dp_Pages]
      SET [Selected_Record_Expression] = 'Allergy_Type_ID_Table.[Allergy_Type] + ''; ''+ cr_Allergy.[Description]'
   WHERE Page_ID = 606
END
GO


