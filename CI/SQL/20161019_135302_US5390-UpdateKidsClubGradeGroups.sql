Use [MinistryPlatform];

DECLARE @Contact_ID int = 7494108; -- Deanne Crooms

DECLARE @Grade_Kindergarten_Attribute_ID int = 9032;
DECLARE @Grade_First_Attribute_ID int = 9033;
DECLARE @Grade_Second_Attribute_ID int = 9034;
DECLARE @Grade_Third_Attribute_ID int = 9035;
DECLARE @Grade_Fourth_Attribute_ID int = 9036;
DECLARE @Grade_Fifth_Attribute_ID int = 9037;

DECLARE @Domain_ID int = 1;
DECLARE @Attribute_Date DATE = GETDATE();

BEGIN TRANSACTION;

BEGIN TRY
  -- Update Contact on existing groups
  UPDATE Groups SET Primary_Contact = @Contact_ID WHERE Group_ID IN (
    173934, 173935, 173936, 173937, 173938, 173939
  );

  -- Remove any existing grade attributes
  DELETE FROM Group_Attributes WHERE Group_ID IN (
    173934, 173935, 173936, 173937, 173938, 173939
  )
  AND Attribute_ID IN (
    @Grade_Kindergarten_Attribute_ID, @Grade_First_Attribute_ID, @Grade_Second_Attribute_ID, @Grade_Third_Attribute_ID, @Grade_Fourth_Attribute_ID, @Grade_Fifth_Attribute_ID
  );

  -- Insert Group grade attributes
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_Kindergarten_Attribute_ID, 173939, @Domain_ID, @Attribute_Date); -- Kids Club Kindergarten
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_First_Attribute_ID, 173938, @Domain_ID, @Attribute_Date); -- Kids Club Grade 1
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_Second_Attribute_ID, 173937, @Domain_ID, @Attribute_Date); -- Kids Club Grade 2
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_Third_Attribute_ID, 173936, @Domain_ID, @Attribute_Date); -- Kids Club Grade 3
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_Fourth_Attribute_ID, 173935, @Domain_ID, @Attribute_Date); -- Kids Club Grade 4
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_Fifth_Attribute_ID, 173934, @Domain_ID, @Attribute_Date); -- Kids Club Grade 5
END TRY
BEGIN CATCH
  PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;
