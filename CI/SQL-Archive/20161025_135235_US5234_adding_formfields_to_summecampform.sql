USE MinistryPlatform
GO

DECLARE @FormId INT = 28

SET IDENTITY_INSERT dbo.Form_Fields ON
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1506, 1 , N'Emergency Contact First Name' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1507, 3 , N'Emergency Contact Last Name' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1508, 4 , N'Emergency Contact Mobile Phone' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1509, 3 , N'Emergency Contact Email' , 1 , N'' , 1 , @FormId , 1 , 0)
	INSERT INTO dbo.Form_Fields(Form_Field_ID, Field_Order ,Field_Label ,Field_Type_ID ,Field_Values ,Required ,Form_ID ,Domain_ID ,Placement_Required) VALUES  (1510, 4 , N'Emergency Contact Relationship' , 1 , N'' , 1 , @FormId , 1 , 0)	
SET IDENTITY_INSERT dbo.Form_Fields OFF

GO
