USE [MINISTRYPLATFORM]
GO


UPDATE [dbo].[Attributes]
SET [Attribute_Name] = 'Anyone welcome',
[Description] = '(men and women together)',
[Sort_Order] = 1
WHERE [Attribute_Id] = 7007;

UPDATE [dbo].[Attributes]
SET [Attribute_Name] = 'Men only',
[Description] = '(no girls allowed)',
[Sort_Order] = 2
WHERE [Attribute_Id] = 7008;

UPDATE [dbo].[Attributes]
SET [Attribute_Name] = 'Women only',
[Description] = '(don''t be a creeper, dude)',
[Sort_Order] = 3
WHERE [Attribute_Id] = 7009;

UPDATE [dbo].[Attributes]
SET [Attribute_Name] = 'Couples',
[Description] = '(married, engaged, etc)',
[Sort_Order] = 4
WHERE [Attribute_Id] = 7010;
GO