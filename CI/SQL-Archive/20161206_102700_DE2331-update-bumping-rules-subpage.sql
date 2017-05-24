USE MinistryPlatform
GO

BEGIN

	UPDATE [dbo].[dp_Sub_Pages]
	SET [Default_Field_List] = 'Bumping_Rules_ID'
		,[Selected_Record_Expression] = 'Bumping_Rules_ID'
	WHERE Sub_Page_ID = 605
	
END

GO
