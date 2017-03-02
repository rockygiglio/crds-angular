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
GO

DECLARE @CRorg int = 2
DECLARE @ArchOrg int = 1
DECLARE @OtherOrg int = 3

UPDATE cr_Organizations
SET Image_URL = 'https://crds-cms-uploads.imgix.net/content/images/gc-crossroads.jpg'
WHERE Organization_ID = @CRorg

UPDATE cr_Organizations
SET Image_URL = 'https://crds-cms-uploads.imgix.net/content/images/gc-archdiocese.jpg'
WHERE Organization_ID = @ArchOrg

UPDATE cr_Organizations
SET Image_URL = 'https://crds-cms-uploads.imgix.net/content/images/gc-other.jpg'
WHERE Organization_ID = @OtherOrg