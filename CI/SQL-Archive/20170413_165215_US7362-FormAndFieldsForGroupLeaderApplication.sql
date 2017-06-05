USE MinistryPlatform
GO

DECLARE @LeaderFormID int = 29;

IF NOT EXISTS (SELECT 1 FROM dbo.Forms WHERE Form_ID = @LeaderFormID)
BEGIN 
	SET IDENTITY_INSERT dbo.Forms ON
	INSERT INTO [dbo].[Forms]
			   ([Form_ID]
			   ,[Form_Title]
			   ,[Get_Contact_Info]
			   ,[Get_Address_Info]
			   ,[Domain_ID])
		 VALUES
			   (@LeaderFormID
			   ,'Group Leader Application'
			   ,0
			   ,0
			   ,1)
	SET IDENTITY_INSERT dbo.Forms OFF

	SET IDENTITY_INSERT dbo.Form_Fields ON
	INSERT INTO [dbo].[Form_Fields]
			([Form_Field_ID]
			,[Field_Order]
			,[Field_Label]
			,[Field_Type_ID]
			,[Required]
			,[Form_ID]
			,[Domain_ID]
			,[Placement_Required])
		VALUES
			(1519
			,10
			,'Staff Reference Contact'
			,1 --Text Box
			,1 --Required
			,@LeaderFormID
			,1
			,0)

	INSERT INTO [dbo].[Form_Fields]
			([Form_Field_ID]
			,[Field_Order]
			,[Field_Label]
			,[Field_Type_ID]
			,[Required]
			,[Form_ID]
			,[Domain_ID]
			,[Placement_Required])
		VALUES
			(1520
			,20
			,'Huddle Experience'
			,1 --Text Box
			,1 --Required
			,@LeaderFormID
			,1
			,0)

	INSERT INTO [dbo].[Form_Fields]
			([Form_Field_ID]
			,[Field_Order]
			,[Field_Label]
			,[Field_Type_ID]
			,[Required]
			,[Form_ID]
			,[Domain_ID]
			,[Placement_Required])
		VALUES
			(1521
			,30
			,'Lead Student Group'
			,1 --Text Box
			,1 --Required
			,@LeaderFormID
			,1
			,0)
	SET IDENTITY_INSERT dbo.Form_Fields OFF
END