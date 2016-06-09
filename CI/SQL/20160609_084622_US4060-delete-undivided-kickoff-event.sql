USE MinistryPlatform
GO

DECLARE @Attribute_Type_ID INT = 86
DECLARE @Attributes_To_Delete TABLE (Attribute_ID INT)

INSERT INTO @Attributes_To_Delete (Attribute_ID) SELECT Attribute_ID FROM dbo.Attributes WHERE Attribute_Type_ID = @Attribute_Type_ID 

-- Delete all instances of the attribute
DELETE FROM dbo.Group_Participant_Attributes WHERE Attribute_ID IN (SELECT a.Attribute_ID FROM @Attributes_To_Delete a)
DELETE FROM dbo.Contact_Attributes WHERE Attribute_ID IN (SELECT a.Attribute_ID FROM @Attributes_To_Delete a)
DELETE FROM dbo.Group_Attributes WHERE Attribute_ID IN (SELECT a.Attribute_ID FROM @Attributes_To_Delete a)

-- Delete attribute and coresponding type
DELETE FROM dbo.Attributes WHERE Attribute_ID IN (SELECT a.Attribute_ID FROM @Attributes_To_Delete a)
DELETE FROM dbo.Attribute_Types WHERE Attribute_Type_ID = @Attribute_Type_ID 