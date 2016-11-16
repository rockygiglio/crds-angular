USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'cr_Allergy_Types.Allergy_Type'
      ,[Selected_Record_Expression] = 'Allergy_Type'      
 WHERE [Page_ID] = 603
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Allergy_Type_ID_Table.[Allergy_Type] as [Allergy_Type],cr_Allergy.[Description]'
      ,[Selected_Record_Expression] = 'Allergy_Type_ID_Table.[Allergy_Type]'      
 WHERE [Page_ID] = 606
GO


