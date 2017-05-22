USE [MinistryPlatform]
GO

-- =============================================
-- Author:		Jonathan Horner
-- Create date: 2/10/2017
-- Description:	Corrects the column names so
--   that the page is no longer broken in MP.
-- =============================================
UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Medical_Information_ID_Table_Contact_ID_Table.[Display_Name] AS [Contact]
                              , Medical_Information_ID_Table.[Policy_Holder_Name] AS [Policy Holder] 
							  , Medical_Information_ID_Table.[Insurance_Company] AS [Insurance Company] 
							  , Medical_Information_ID_Table.[Physician_Name] AS [Physician Name] 
							  , Medical_Information_ID_Table.[Physician_Phone] AS [Physician Phone]
							  , Allergy_ID_Table_Allergy_Type_ID_Table.[Allergy_Type] AS [Allergy Type] 
							  , Allergy_ID_Table.[Description] AS [Allergy Description]'
      ,[Selected_Record_Expression] = 'Allergy_ID_Table_Allergy_Type_ID_Table.Allergy_Type'
 WHERE Page_ID = 617;
GO