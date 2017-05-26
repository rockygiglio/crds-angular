USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Types] where Attribute_Type = 'Group Category')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO [dbo].[Attribute_Types] 
(Attribute_Type_ID,
Attribute_Type,
Description,
Domain_ID,
Available_Online,
Prevent_Multiple_Selection) 
VALUES
(90,
'Group Category',
'What is the focus of your group',
1,
1,
0);

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END