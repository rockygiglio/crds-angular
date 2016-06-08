USE MinistryPlatform;

DECLARE @NewRecID int = 27;

IF NOT EXISTS(SELECT * FROM dbo.group_types WHERE Group_Type_ID = @NewRecID AND Group_Type = 'Childcare')
BEGIN
	SET IDENTITY_INSERT dbo.group_types  ON

	INSERT INTO dbo.group_types(Group_Type_ID,Group_Type,Description,Domain_ID,Default_Role,Show_On_Group_Finder,Show_On_Sign_Up_To_Serve)
		VALUES(@NewRecID,'Childcare','Childcare Group to attach to childcare events.',1,16,0,0)

	SET IDENTITY_INSERT dbo.group_types  OFF
END

