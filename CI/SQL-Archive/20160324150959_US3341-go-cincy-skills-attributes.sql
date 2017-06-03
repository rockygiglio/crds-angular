USE [MinistryPlatform]
GO

DECLARE @MURAL_SKILL nvarchar(200) = N'Mural Design and Painting';
DECLARE @HANDYMAN_SKILL nvarchar(200) = N'Handyman (Not Professional)';
DECLARE @KIDS nvarchar(200) = N'Entertain Kids';
DECLARE @CARPENTER nvarchar(200) = N'Carpenter';
DECLARE @CONTRACTOR nvarchar(200) = N'General Contractor';

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Attribute_Category_ID]
           ,[Domain_ID]
           )
     VALUES
           (@MURAL_SKILL
           ,24
           ,6
           ,1
           ),
		   (@HANDYMAN_SKILL
		   ,24
		   ,7
		   ,1
		   ),
		   (@KIDS
		   ,24
		   ,11
		   ,1
		   ),
		   (@CARPENTER
		   ,24
		   ,7
		   ,1
		   ),
		   (@CONTRACTOR
		   ,24
		   ,7
		   ,1
		   )


INSERT INTO [dbo].[cr_Go_Volunteer_Skills]
           ([Domain_ID]
           ,[Attribute_ID]
           ,[Label])
     VALUES
           (1
           ,6939
           ,N'I''m a professional painter - I do this every day for a living'
		   ),
		   (1
		   ,6929
		   ,N'I''m a professional carpet installer - I do this every day for a living'
		   ),
		   (1
		   ,6935
		   ,N'I''m a professional electrician - I do this every day for a living'
		   ),
		   (1
		   ,6937
		   ,N'I''m a professional plumber - I do this every day for a living'
		   ),
		   (1
		   ,6931
		   ,N'I''m a professional landscaper - I do this every day for a living'
		   ),
		   (1
		   ,(Select Attribute_ID from Attributes WHERE Attribute_Name = @CONTRACTOR)
		   ,N'I''m a professional contractor - I do this every day for a living'
		   ),
		   (1
		   ,(Select Attribute_ID from Attributes WHERE Attribute_Name = @HANDYMAN_SKILL)
		   ,N'I''m a jack of all trades but not a professional'
		   ),
		   (1
		   ,(Select Attribute_ID from Attributes WHERE Attribute_Name = @KIDS)
		   ,N'I like to entertain kids'
		   ),
		   (1
		   ,(Select Attribute_ID from Attributes WHERE Attribute_Name = @CARPENTER)
		   ,N'I''m a professional carpenter - I do this every day for a living'
		   ),
		   (1
		   ,(Select Attribute_ID from Attributes WHERE Attribute_Name = @MURAL_SKILL)
		   ,N'I can design and paint a mural'
		   )
GO


