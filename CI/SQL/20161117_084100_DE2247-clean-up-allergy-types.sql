use MinistryPlatform
GO

DECLARE @MED_ALLEGY_TYPE int = 1;
DECLARE @FOOD_ALLERGY_TYPE int = 2;
DECLARE @ENV_ALLERY_TYPE int = 3;
DECLARE @OTHER_ALLERY_TYPE int = 4;

-- Clean up Medical Allergies
Update [dbo].[cr_Allergy] 
SET [Allergy_Type_ID] = @MED_ALLEGY_TYPE Where [Allergy_ID] in (SELECT a.Allergy_ID from [dbo].[cr_Allergy] a
JOIN [dbo].[cr_Allergy_Types] at on a.Allergy_Type_ID = at.Allergy_Type_ID
WHERE at.Allergy_Type = 'Medicine')

-- Clean up Food Allergies
Update [dbo].[cr_Allergy] 
SET [Allergy_Type_ID] = @FOOD_ALLERGY_TYPE Where [Allergy_ID] in (SELECT a.Allergy_ID from [dbo].[cr_Allergy] a
JOIN [dbo].[cr_Allergy_Types] at on a.Allergy_Type_ID = at.Allergy_Type_ID
WHERE at.Allergy_Type = 'Food')

-- Clean up Environmental Allergies
Update [dbo].[cr_Allergy] 
SET [Allergy_Type_ID] = @ENV_ALLERY_TYPE Where [Allergy_ID] in (SELECT a.Allergy_ID from [dbo].[cr_Allergy] a
JOIN [dbo].[cr_Allergy_Types] at on a.Allergy_Type_ID = at.Allergy_Type_ID
WHERE at.Allergy_Type = 'Environmental')

-- Clean up Other Allergies
Update [dbo].[cr_Allergy] 
SET [Allergy_Type_ID] = @OTHER_ALLERY_TYPE Where [Allergy_ID] in (SELECT a.Allergy_ID from [dbo].[cr_Allergy] a
JOIN [dbo].[cr_Allergy_Types] at on a.Allergy_Type_ID = at.Allergy_Type_ID
WHERE at.Allergy_Type = 'Other')

-- Remove all duplicated allergy types
DELETE FROM [dbo].[cr_Allergy_Types] WHERE [Allergy_Type_ID] NOT IN (@MED_ALLEGY_TYPE, @FOOD_ALLERGY_TYPE, @ENV_ALLERY_TYPE, @OTHER_ALLERY_TYPE);