USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = N'Allergy_Type_ID_Table.[Description] as [Allergy_Type],cr_Allergy.[Description]'      
 WHERE [Page_ID] = 606
GO


