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

  SELECT @Existing_Row = COUNT(*) FROM Attribute_Categories WHERE Attribute_Category_ID = @Attribute_Category_ID;
  IF @Existing_Row <= 0
  BEGIN
    PRINT 'Inserting new attribute category ' + CONVERT(varchar, @Attribute_Category_ID);
    SET IDENTITY_INSERT Attribute_Categories ON;
    INSERT INTO Attribute_Categories
      (Attribute_Category_ID, Attribute_Category, [Description], Available_Online, Domain_ID)
    VALUES
      (@Attribute_Category_ID, 'Kids Club eCheck', 'Kids Club eCheck', 1, @Domain_ID);
    SET IDENTITY_INSERT Attribute_Categories OFF;
  END
  ELSE
  BEGIN
    PRINT 'Updating existing attribute category ' + CONVERT(varchar, @Attribute_Category_ID);
    UPDATE Attribute_Categories
    SET Attribute_Category = 'Kids Club eCheck',
        [Description] = 'Kids Club eCheck',
        Available_Online = 1,
        Domain_ID = @Domain_ID
    WHERE Attribute_Category_ID = @Attribute_Category_ID;
  END  

  -- ====================================================================
  -- Attribute types
  DECLARE @Age_Attribute_Type_ID int = 102;
  DECLARE @Birth_Month_Attribute_Type_ID int = 103;
  DECLARE @Grade_Attribute_Type_ID int = 104;
  DECLARE @Nursery_Month_Attribute_Type_ID int = 105;

  DECLARE @AttributeTypes TABLE (
    Attribute_Type_ID int,
    Attribute_Type nvarchar(50),
    [Description] nvarchar(500)
  );
  INSERT INTO @AttributeTypes VALUES
    (@Age_Attribute_Type_ID, 'KC eCheck Age', 'Age for Kids Club eCheck group'),
    (@Birth_Month_Attribute_Type_ID, 'KC eCheck Birth Month', 'Birth Month for Kids Club eCheck group'),
    (@Grade_Attribute_Type_ID, 'KC eCheck Grade', 'Grade for Kids Club eCheck group'),
    (@Nursery_Month_Attribute_Type_ID, 'KC eCheck Nursery Month', 'Nursery Month for Kids Club eCheck group');

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
  -- Group Birth Months
  INSERT INTO @Attributes VALUES
    (9002, 'January', 'January birth month', @Birth_Month_Attribute_Type_ID, 0),
    (9003, 'February', 'February birth month', @Birth_Month_Attribute_Type_ID, 1),
    (9004, 'March', 'March birth month', @Birth_Month_Attribute_Type_ID, 2),
    (9005, 'April', 'April birth month', @Birth_Month_Attribute_Type_ID, 3),
    (9006, 'May', 'May birth month', @Birth_Month_Attribute_Type_ID, 4),
    (9007, 'June', 'June birth month', @Birth_Month_Attribute_Type_ID, 5),
    (9008, 'July', 'July birth month', @Birth_Month_Attribute_Type_ID, 6),
    (9009, 'August', 'August birth month', @Birth_Month_Attribute_Type_ID, 7),
    (9010, 'September', 'September birth month', @Birth_Month_Attribute_Type_ID, 8),
    (9011, 'October', 'October birth month', @Birth_Month_Attribute_Type_ID, 9),
    (9012, 'November', 'November birth month', @Birth_Month_Attribute_Type_ID, 10),
    (9013, 'December', 'DecemberJanuary birth month', @Birth_Month_Attribute_Type_ID, 11);

  -- Group Ages
  INSERT INTO @Attributes VALUES
    (9014, 'Nursery', 'Nursery age', @Age_Attribute_Type_ID, 0),
    (9015, 'First Year', 'First Year age', @Age_Attribute_Type_ID, 1),
    (9016, 'Second Year', 'Second Year age', @Age_Attribute_Type_ID, 2),
    (9017, 'Third Year', 'Third Year age', @Age_Attribute_Type_ID, 3),
    (9018, 'Fourth Year', 'Fourth Year age', @Age_Attribute_Type_ID, 4),
    (9019, 'Fifth Year', 'Fifth Year age', @Age_Attribute_Type_ID, 5);

  -- Group Nursery Months
  INSERT INTO @Attributes VALUES
    (9020, '0-1', '0-1 months old', @Nursery_Month_Attribute_Type_ID, 0),
    (9021, '1-2', '1-2 months old', @Nursery_Month_Attribute_Type_ID, 1),
    (9022, '2-3', '2-3 months old', @Nursery_Month_Attribute_Type_ID, 2),
    (9023, '3-4', '3-4 months old', @Nursery_Month_Attribute_Type_ID, 3),
    (9024, '4-5', '4-5 months old', @Nursery_Month_Attribute_Type_ID, 4),
    (9025, '5-6', '5-6 months old', @Nursery_Month_Attribute_Type_ID, 5),
    (9026, '6-7', '6-7 months old', @Nursery_Month_Attribute_Type_ID, 6),
    (9027, '7-8', '7-8 months old', @Nursery_Month_Attribute_Type_ID, 7),
    (9028, '8-9', '8-9 months old', @Nursery_Month_Attribute_Type_ID, 8),
    (9029, '9-10', '9-10 months old', @Nursery_Month_Attribute_Type_ID, 9),
    (9030, '10-11', '10-11 months old', @Nursery_Month_Attribute_Type_ID, 10),
    (9031, '11-12', '11-12 months old', @Nursery_Month_Attribute_Type_ID, 11);

  -- Group Grades
  INSERT INTO @Attributes VALUES
    (9032, 'Kindergarten', 'Kindergarten', @Grade_Attribute_Type_ID, 0),
    (9033, 'First Grade', 'First Grade', @Grade_Attribute_Type_ID, 1),
    (9034, 'Second Grade', 'Second Grade', @Grade_Attribute_Type_ID, 2),
    (9035, 'Third Grade', 'Third Grade', @Grade_Attribute_Type_ID, 3),
    (9036, 'Fourth Grade', 'Fourth Grade', @Grade_Attribute_Type_ID, 4),
    (9037, 'Fifth Grade', 'Fifth Grade', @Grade_Attribute_Type_ID, 5),
    (9038, 'Sixth Grade', 'Sixth Grade', @Grade_Attribute_Type_ID, 6),
    (9039, 'CSM', 'CSM', @Grade_Attribute_Type_ID, 7);

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
