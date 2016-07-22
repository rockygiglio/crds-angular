USE [MinistryPlatform]
GO

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'Middle School Students (Grades 6-8)'
WHERE Attribute_ID = 7089;

UPDATE [dbo].[Attributes]
SET Attribute_Name = 'High School Students (Grades 9-12)'
WHERE Attribute_ID = 7090;
