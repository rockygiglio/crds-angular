Use [MinistryPlatform]
GO

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'Anyone is welcome', SORT_ORDER = 0
Where Attribute_id = 7007;
GO

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'Men only', SORT_ORDER =2
Where Attribute_id = 7008;
GO

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'Women only', SORT_ORDER = 3
Where Attribute_id = 7009;
GO

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'Married couples', SORT_ORDER = 1
Where Attribute_id = 7010;
GO