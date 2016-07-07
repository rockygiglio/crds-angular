USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Life Stages')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO Attribute_Categories 
(Attribute_Category_ID,
Attribute_Category,
Available_Online,
Domain_ID) 
VALUES
(17,
'Life Stages',
0,
1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Neighborhoods')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO Attribute_Categories 
(Attribute_Category_ID,
Attribute_Category,
Available_Online,
Domain_ID) 
VALUES
(18,
'Neighborhoods',
0,
1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Spiritual Growth')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO Attribute_Categories 
(Attribute_Category_ID,
Attribute_Category,
Available_Online,
Domain_ID) 
VALUES
(19,
'Spiritual Growth',
0,
1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Interest')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO Attribute_Categories 
(Attribute_Category_ID,
Attribute_Category,
Available_Online,
Domain_ID) 
VALUES
(20,
'Interest',
0,
1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Healing')
BEGIN
SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO Attribute_Categories 
(Attribute_Category_ID,
Attribute_Category,
Available_Online,
Domain_ID) 
VALUES
(21,
'Healing',
0,
1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END