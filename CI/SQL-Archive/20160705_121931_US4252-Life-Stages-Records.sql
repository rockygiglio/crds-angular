Use [MinistryPlatform]
GO

UPDATE [dbo].[LIFE_STAGES]
SET Life_Stage = 'Master/Expert', Description = 'Ready to Reproduce'
WHERE Life_Stage_ID =1;

UPDATE [dbo].[LIFE_STAGES]
SET Life_Stage = 'Active', Description = 'Regularly Meeting'
WHERE Life_Stage_ID = 2;

UPDATE [dbo].[LIFE_STAGES]
SET Life_Stage = 'Emerging', Description = 'Getting Started'
WHERE Life_Stage_ID = 3;


UPDATE [dbo].[LIFE_STAGES]
SET Life_Stage = 'Potential', Description = 'An idea, Going to strategy Sessions to learn'
WHERE Life_Stage_ID = 4;
GO
