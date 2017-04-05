USE [MinistryPlatform]
GO

DECLARE @GOEquipment int = 82

UPDATE [dbo].[Attribute_Types]
   SET [Attribute_Type] = 'GO Local - Equipment'
 WHERE Attribute_Type_ID = @GOEquipment
GO

DECLARE @GOOtherEquipment int = 3

UPDATE [dbo].[Attribute_Types]
   SET [Attribute_Type] = 'GO Local - Other Equipment'
 WHERE Attribute_Type_ID = @GOOtherEquipment
GO

DECLARE @GORegistrationChildren int = 81

UPDATE [dbo].[Attribute_Types]
   SET [Attribute_Type] = 'GO Local - Registration Children'
 WHERE Attribute_Type_ID = @GORegistrationChildren
GO

DECLARE @GORegistrationPrepWork int = 80

UPDATE [dbo].[Attribute_Types]
   SET [Attribute_Type] = 'GO Local - Registration PrepWork'
 WHERE Attribute_Type_ID = @GORegistrationPrepWork
GO

DECLARE @GORegistrationChildrenAnywhere int = 106

UPDATE [dbo].[Attribute_Types]
   SET [Attribute_Type] = 'GO Local - Registration Children - Anywhere'
 WHERE Attribute_Type_ID = @GORegistrationChildrenAnywhere
GO
