USE MinistryPlatform
GO

DECLARE @FormId INT = 28
DECLARE @FieldId INT = 1517

IF NOT EXISTS (SELECT * FROM dbo.Form_Fields WHERE Form_Field_ID = @FieldId AND Form_ID = @FormId)
BEGIN
SET IDENTITY_INSERT dbo.Form_Fields ON
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1517, 1 , N'Crossroads Site' , 1 , N'' , 1 , @FormId , 1 , 0)
SET IDENTITY_INSERT dbo.Form_Fields OFF
END
GO	