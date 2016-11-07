USE [MinistryPlatform]
GO

DECLARE @categoryId int = 51;
DECLARE @attributeId int = 9040;

IF NOT EXISTS (SELECT * FROM Attribute_Categories WHERE Attribute_Category_ID = @categoryId)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attribute_Categories] ON;
	INSERT INTO Attribute_Categories
	(
		[Attribute_Category_ID],
		[Attribute_Category],
		[Description],
		[Requires_Active_Attribute]
	) 
	VALUES 
	(
		@categoryId,
		'Journey',
		null,
		1
	)
	SET IDENTITY_INSERT [dbo].[Attribute_Categories] OFF
END


IF NOT EXISTS (SELECT * FROM Attributes WHERE Attribute_ID = @attributeId)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attributes] ON;
	INSERT INTO attributes 
	(
		[Attribute_ID],
		[Attribute_Name],
		[Description],
		[Attribute_Type_ID],
		[Attribute_Category_ID],
		[Domain_ID],
		[__ExternalAttributeID],
		[Sort_Order],
		[Start_Date],
		[End_Date]
		)
	VALUES
	(
		@attributeId,
		'I am ______',
		'2017 Crossroads Journey Groups',
		90,
		@categoryId,
		1,
		null,
		0,
		{ts '2017-01-07 00:00:00'},
		{ts '2017-03-24 00:00:00'}
	)
	SET IDENTITY_INSERT [dbo].[Attributes] OFF;	
END

