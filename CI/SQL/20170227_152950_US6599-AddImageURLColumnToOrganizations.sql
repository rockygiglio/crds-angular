USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Image_URL' AND object_id = Object_ID(N'cr_Organizations'))
BEGIN
	ALTER TABLE cr_Organizations
	ADD Image_URL nvarchar(255)

	DECLARE @OrgPage int = 10;
	UPDATE dp_Pages
	SET Default_Field_List = 'Name,Open_Signup,Start_Date,End_Date,Primary_Contact_Table.Display_Name as [Primary_Contact],Image_URL'
	WHERE Page_ID = @OrgPage
END