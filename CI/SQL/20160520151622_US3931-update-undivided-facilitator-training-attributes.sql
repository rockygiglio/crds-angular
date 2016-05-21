USE [MinistryPlatform]
GO

DECLARE @Domain_ID AS INT = 1
DECLARE @Attribute_ID_Base AS INT = 7080

-- Add / Update the Attribute Type
DECLARE @Attribute_Type_ID INT
DECLARE @Prevent_Multiple_Selection BIT
DECLARE @Available_Online BIT

SELECT
	@Attribute_Type_ID = 85,
	@Prevent_Multiple_Selection = 1,
	@Available_Online = 1

-- Add / Update the Attributes
SET IDENTITY_INSERT [dbo].[Attributes] ON

DECLARE @Attribute_Names AS TABLE (Attribute_ID INT, Attribute_Name VARCHAR(75), [Description] VARCHAR(255), Sort_Order INT)

INSERT INTO @Attribute_Names
	(Attribute_ID, Attribute_Name, [Description], Sort_Order)
	VALUES
	(@Attribute_ID_Base, 'NEW Facilitator - Saturday, July 16  9:00 - 3:00pm - OAKLEY', NULL, 1),
	(@Attribute_ID_Base+1, 'NEW Facilitator - Saturday, July 30  9:00 - 3:00pm - MASON', NULL, 2),
	(@Attribute_ID_Base+2, 'EXPERIENCED Facilitator - Saturday, July 16  9:00 - 11:00am - OAKLEY', NULL, 3),
	(@Attribute_ID_Base+3, 'EXPERIENCED Facilitator - Saturday, July 30  9:00 - 11:00am - MASON', NULL, 4)

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