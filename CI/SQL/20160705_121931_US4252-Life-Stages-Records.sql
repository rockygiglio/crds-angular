Use [MinistryPlatform]
GO

DELETE FROM LIFE_STAGES;

SET IDENTITY_INSERT [dbo].[LIFE_STAGES] ON;

INSERT INTO [dbo].[LIFE_STAGES]
(Life_Stage_ID, Life_Stage     , Domain_ID, Description) VALUES
(1            , 'Master/Expert',         1, 'Ready to Reproduce');
GO

INSERT INTO [dbo].[LIFE_STAGES]
(Life_Stage_ID, Life_Stage, Domain_ID, Description) VALUES
(2            , 'Active'  ,         1, 'Regularly Meeting');
GO

INSERT INTO [dbo].[LIFE_STAGES]
(Life_Stage_ID, Life_Stage, Domain_ID, Description) VALUES
(3            , 'Emerging',         1, 'Getting Started');
GO

INSERT INTO [dbo].[LIFE_STAGES]
(Life_Stage_ID, Life_Stage , Domain_ID, Description) VALUES
(4            , 'Potential',         1, 'An Idea, Going to strategy Sessions to learn');
GO

SET IDENTITY_INSERT [dbo].[LIFE_STAGES] OFF;