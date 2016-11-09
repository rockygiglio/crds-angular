USE [MinistryPlatform]
GO

DECLARE @lifeStagesCat int = 17;
DECLARE @neighborhoodsCat int = 18;
DECLARE @spiritualGrowthCat int = 19;
DECLARE @interestCat int = 20;
DECLARE @healingCat int = 21;
DECLARE @journeyCat int = 51;

UPDATE [dbo].[Attributes]
SET [Description] = 'For people who share a common activity. From cooking to karate, motorcycles to frisbee golf, veterans or entrepreneurs, whatever your interest, we bet there’s a group looking for it.',
[Example_Text] = 'Ex. Boxing, Xbox'
WHERE [Attribute_Category_Id] = @interestCat;

UPDATE [dbo].[Attributes]
SET [Description] = 'Your group is primarily focused on building community with the people who live closest together in your town, zip code or on your street.',
[Example_Text] = 'Ex. Norwood, Gaslight'
WHERE [Attribute_Category_Id] = @neighborhoodsCat;

UPDATE [dbo].[Attributes]
SET [Description] = 'Grow together through Huddle, reading a book or studying the Bible and applying what you learn to your everyday life.',
[Example_Text] = 'Ex. Huddle, James'
WHERE [Attribute_Category_Id] = @spiritualGrowthCat;

UPDATE [dbo].[Attributes]
SET [Description] = 'For people in a similar life stage like empty nesters, singles, foster parents, moms, young married couples, etc.',
[Example_Text] = 'Ex. new family, young married, college, empty nesters'
WHERE [Attribute_Category_Id] = @lifeStagesCat;

UPDATE [dbo].[Attributes]
SET [Description] = 'For people looking for healing and recovery in an area of life.',
[Example_Text] = 'Ex. grief, infertility, addiction, divorce, crisis, etc'
WHERE [Attribute_Category_Id] = @healingCat;

UPDATE [dbo].[Attributes]
SET [Description] = 'Ain''t no journey like a west coast journey but this journey actually does stop'
WHERE [Attribute_Category_Id] = @journeyCat;
GO