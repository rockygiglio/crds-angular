USE [MinistryPlatform]
GO

DECLARE @Domain_ID AS INT = 1
DECLARE @Attribute_ID_Base AS INT = 7082

-- Add / Update the Attribute Type
DECLARE @Attribute_Type_ID INT
DECLARE @Attribute_Type_Name VARCHAR(75)
DECLARE @Attribute_Type_Description VARCHAR(255)
DECLARE @Prevent_Multiple_Selection BIT
DECLARE @Available_Online BIT

SELECT
	@Attribute_Type_ID = 85,
	@Attribute_Type_Name = 'Undivided Facilitator Training',
	@Attribute_Type_Description = 'Answer for ''Which REQUIRED Facilitator Training doyou want to attend?''',
	@Prevent_Multiple_Selection = 1,
	@Available_Online = 1

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON
IF NOT EXISTS (SELECT * FROM Attribute_Types WHERE Attribute_Type_ID = @Attribute_Type_ID)
BEGIN
	INSERT INTO [dbo].[Attribute_Types]
		(
			[Attribute_Type_ID],
			[Attribute_Type],
			[Description],
			[Domain_ID],
			[Prevent_Multiple_Selection],
			[Available_Online]
		)
		VALUES
		(
			@Attribute_Type_ID,
			@Attribute_Type_Name,
			@Attribute_Type_Description,
			@Domain_ID,
			@Prevent_Multiple_Selection,
			@Available_Online
		)
END
ELSE
BEGIN
	UPDATE [dbo].[Attribute_Types]
		SET
			Attribute_Type = @Attribute_Type_Name,
			[Description] = @Attribute_Type_Description,
			Domain_ID = @Domain_ID,
			Prevent_Multiple_Selection = @Prevent_Multiple_Selection,
			Available_Online = @Available_Online
		WHERE
			Attribute_Type_ID = @Attribute_Type_ID
END

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF

-- Add / Update the Attributes
SET IDENTITY_INSERT [dbo].[Attributes] ON

DECLARE @Attribute_Names AS TABLE (Attribute_ID INT, Attribute_Name VARCHAR(75), [Description] VARCHAR(255), Sort_Order INT)

INSERT INTO @Attribute_Names
	(Attribute_ID, Attribute_Name, [Description], Sort_Order)
	VALUES
	(@Attribute_ID_Base, 'Saturday, July 16  9:00 - 3:00pm - Oakley', '(childcare is NOT available)', 1),
	(@Attribute_ID_Base+1, 'Saturday, July 30  9:00 - 3:00pm - Mason','(childcare is NOT available)', 2)

MERGE [dbo].[Attributes] AS a
USING @Attribute_Names AS tmp
	ON a.Attribute_ID = tmp.Attribute_ID
WHEN MATCHED THEN
	UPDATE
	SET
		Attribute_Name = tmp.Attribute_Name,
		[Description] = tmp.[Description],
		Attribute_Type_ID = @Attribute_Type_ID,
		Domain_ID = @Domain_ID,
		Sort_Order = tmp.Sort_Order
WHEN NOT MATCHED THEN
	INSERT
		(Attribute_ID, Attribute_Name, [Description], Attribute_Type_ID, Domain_ID, Sort_Order)
		VALUES
		(tmp.Attribute_ID, tmp.Attribute_Name, tmp.[Description], @Attribute_Type_ID, @Domain_ID, tmp.Sort_Order);

SET IDENTITY_INSERT [dbo].[Attributes] OFF
