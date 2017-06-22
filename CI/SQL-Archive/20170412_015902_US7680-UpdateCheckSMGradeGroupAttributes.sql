Use [MinistryPlatform];

DECLARE @Contact_ID int = 7494108; -- Deanne Crooms

DECLARE @Grade_6th_Attribute_ID int = 9038;
DECLARE @Grade_7th_Attribute_ID int = 9042;
DECLARE @Grade_8th_Attribute_ID int = 9043;
DECLARE @Grade_9th_Attribute_ID int = 9044;
DECLARE @Grade_10th_Attribute_ID int = 9045;
DECLARE @Grade_11th_Attribute_ID int = 9046;
DECLARE @Grade_12th_Attribute_ID int = 9047;

DECLARE @Domain_ID int = 1;
DECLARE @Attribute_Date DATE = GETDATE();

BEGIN TRANSACTION;

BEGIN TRY
  -- Update Contact on existing groups
  UPDATE Groups SET Primary_Contact = @Contact_ID WHERE Group_ID IN (
    173927, 173928, 173929, 173930, 173931, 173932, 173933
  );

  -- Remove any existing grade attributes
  DELETE FROM Group_Attributes WHERE Group_ID IN (
    173927, 173928, 173929, 173930, 173931, 173932, 173933
  )
  AND Attribute_ID IN (
    @Grade_6th_Attribute_ID, @Grade_7th_Attribute_ID, @Grade_8th_Attribute_ID, @Grade_9th_Attribute_ID, @Grade_10th_Attribute_ID, @Grade_11th_Attribute_ID, @Grade_12th_Attribute_ID
  );

  -- Insert Group grade attributes
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_6th_Attribute_ID, 173933, @Domain_ID, @Attribute_Date); -- SM 6th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_7th_Attribute_ID, 173932, @Domain_ID, @Attribute_Date); -- SM 7th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_8th_Attribute_ID, 173931, @Domain_ID, @Attribute_Date); -- SM 8th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_9th_Attribute_ID, 173930, @Domain_ID, @Attribute_Date); -- SM 9th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_10th_Attribute_ID, 173929, @Domain_ID, @Attribute_Date); -- SM 10th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_11th_Attribute_ID, 173928, @Domain_ID, @Attribute_Date); -- SM 11th
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Grade_12th_Attribute_ID, 173927, @Domain_ID, @Attribute_Date); -- SM 12th
END TRY
BEGIN CATCH
  PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

