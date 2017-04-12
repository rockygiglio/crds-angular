USE [MinistryPlatform];

BEGIN TRANSACTION;

BEGIN TRY
  DECLARE @Existing_Row int;
  DECLARE @Rows_To_Insert int;
  DECLARE @Domain_ID int = 1;

  -- ====================================================================
  -- Attribute categories
  DECLARE @Attribute_Category_ID int = 50;
  DECLARE @AttributeCategories TABLE (
    Attribute_Category_ID int,
    Attribute_Category nvarchar(50),
    [Description] nvarchar(500)
  );

  -- ====================================================================
  -- Attribute types
  DECLARE @Grade_Attribute_Type_ID int = 107;

  DECLARE @AttributeTypes TABLE (
    Attribute_Type_ID int,
    Attribute_Type nvarchar(50),
    [Description] nvarchar(500)
  );
  INSERT INTO @AttributeTypes VALUES
    (@Grade_Attribute_Type_ID, 'SM Checkin Grade', 'Grade for SM Checkin group')

  DECLARE @Attribute_Type_ID int, @Attribute_Type nvarchar(50), @Attribute_Type_Description nvarchar(500);
  DECLARE Attribute_Type_Cursor CURSOR FOR
    SELECT * FROM @AttributeTypes;
  OPEN Attribute_Type_Cursor;
  FETCH NEXT FROM Attribute_Type_Cursor INTO
    @Attribute_Type_ID, @Attribute_Type, @Attribute_Type_Description;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    SELECT @Existing_Row=COUNT(*) FROM Attribute_Types WHERE Attribute_Type_ID = @Attribute_Type_ID;

    IF @Existing_Row <= 0
    BEGIN
      PRINT 'Inserting new attribute type ' + CONVERT(varchar, @Attribute_Type_ID);
      SET IDENTITY_INSERT Attribute_Types ON;
      INSERT INTO Attribute_Types
        (Attribute_Type_ID, Attribute_Type, [Description], Available_Online, Domain_ID, Prevent_Multiple_Selection)
      VALUES
        (@Attribute_Type_ID, @Attribute_Type, @Attribute_Type_Description, 1, @Domain_ID, 1);
      SET IDENTITY_INSERT Attribute_Types OFF;
    END
    ELSE
    BEGIN
      PRINT 'Updating existing attribute type ' + CONVERT(varchar, @Attribute_Type_ID);
      UPDATE Attribute_Types
      SET Attribute_Type = @Attribute_Type,
          [Description] = @Attribute_Type_Description,
          Available_Online = 1,
          Domain_ID = @Domain_ID,
          Prevent_Multiple_Selection = 1
      WHERE Attribute_Type_ID = @Attribute_Type_ID;
    END

    FETCH NEXT FROM Attribute_Type_Cursor INTO
      @Attribute_Type_ID, @Attribute_Type, @Attribute_Type_Description;
  END

  CLOSE Attribute_Type_Cursor;

  -- ====================================================================
  -- Attributes
  DECLARE @Attributes TABLE (
    Attribute_ID int,
    Attribute_Name nvarchar(100),
    [Description] nvarchar(255),
    Attribute_Type_ID int,
    Sort_Order int
  );

  -- Group Grades
  INSERT INTO @Attributes VALUES
    (9038, 'Sixth Grade', 'Sixth Grade', @Grade_Attribute_Type_ID, 6),
    (9042, 'Seventh Grade', 'Seventh Grade', @Grade_Attribute_Type_ID, 8),
    (9043, 'Eighth Grade', 'Eighth Grade', @Grade_Attribute_Type_ID, 9),
    (9044, 'Ninth Grade', 'Ninth Grade', @Grade_Attribute_Type_ID, 10),
    (9045, 'Tenth Grade', 'Tenth Grade', @Grade_Attribute_Type_ID, 11),
    (9046, 'Eleventh Grade', 'Eleventh Grade', @Grade_Attribute_Type_ID, 12),
    (9047, 'Twelfth Grade', 'Twelfth Grade', @Grade_Attribute_Type_ID, 13);

  DECLARE @Attribute_ID int, @Attribute_Name nvarchar(100), @Attribute_Description nvarchar(255), @Attribute_Sort_Order int;
  DECLARE Attribute_Cursor CURSOR FOR
    SELECT * FROM @Attributes;
  OPEN Attribute_Cursor;
  FETCH NEXT FROM Attribute_Cursor INTO
    @Attribute_ID, @Attribute_Name, @Attribute_Description, @Attribute_Type_ID, @Attribute_Sort_Order;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    SELECT @Existing_Row=COUNT(*) FROM Attributes WHERE Attribute_ID = @Attribute_ID;

    IF @Existing_Row <= 0
    BEGIN
      PRINT 'Inserting new attribute ' + CONVERT(varchar, @Attribute_ID);
      SET IDENTITY_INSERT Attributes ON;
      INSERT INTO Attributes
        (Attribute_ID, Attribute_Name, [Description], Attribute_Type_ID, Attribute_Category_ID, Domain_ID, Sort_Order)
      VALUES
        (@Attribute_ID, @Attribute_Name, @Attribute_Description, @Attribute_Type_ID, @Attribute_Category_ID, @Domain_ID, @Attribute_Sort_Order);
      SET IDENTITY_INSERT Attributes OFF;
    END
    ELSE
    BEGIN
      PRINT 'Updating existing attribute ' + CONVERT(varchar, @Attribute_ID);
      UPDATE Attributes
      SET Attribute_Name = @Attribute_Name,
          [Description] = @Attribute_Description,
          Attribute_Type_ID = @Attribute_Type_ID,
          Attribute_Category_ID = @Attribute_Category_ID,
          Domain_ID = @Domain_ID,
          Sort_Order = @Attribute_Sort_Order
      WHERE Attribute_ID = @Attribute_ID;
    END

  FETCH NEXT FROM Attribute_Cursor INTO
    @Attribute_ID, @Attribute_Name, @Attribute_Description, @Attribute_Type_ID, @Attribute_Sort_Order;
  END

  CLOSE Attribute_Cursor;
END TRY
BEGIN CATCH
  PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

