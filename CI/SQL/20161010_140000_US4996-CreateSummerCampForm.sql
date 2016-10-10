USE MinistryPlatform
GO

DECLARE @FormId INT = 28

IF NOT EXISTS(SELECT * FROM dbo.Forms WHERE Form_ID = @FormId)
BEGIN
	--Create a form
	SET IDENTITY_INSERT dbo.Forms ON
	INSERT INTO Forms(Form_ID,Form_Title,Get_Contact_Info,Get_Address_Info,Domain_ID) VALUES(@FormId,'Summer Camp Form', 1,1,1)
	SET IDENTITY_INSERT dbo.Forms OFF

	--Create form fields
	SET IDENTITY_INSERT dbo.Form_Fields ON
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1501, 1 , N'Current Grade' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1502, 2 , N'School Currently Attending' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1503, 3 , N'Anticipated Grade During Camp' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1504, 4 , N'School Attending Next School Year' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1505, 5 , N'Preferred Roommate First and Last Name' , 1 , N'' , 1 , @FormId , 1 , 0)
	SET IDENTITY_INSERT dbo.Form_Fields OFF
END

GO
--SELECT * FROM  Forms WHERE Form_ID = 28
--SELECT * FROM  Form_Fields WHERE Form_ID = 28

--DELETE  Forms WHERE Form_ID = 28
--DELETE  Form_Fields WHERE Form_ID = 28