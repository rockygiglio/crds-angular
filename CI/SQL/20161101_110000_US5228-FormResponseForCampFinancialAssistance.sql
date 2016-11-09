USE MinistryPlatform
GO

DECLARE @FormId INT = 28

IF EXISTS(SELECT * FROM dbo.Forms WHERE Form_ID = @FormId)
BEGIN
	
	--Create form fields
	SET IDENTITY_INSERT dbo.Form_Fields ON
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1511, 8 , N'Financial Assistance' , 1 , N'' , 1 , @FormId , 1 , 0)
	SET IDENTITY_INSERT dbo.Form_Fields OFF
END

GO


