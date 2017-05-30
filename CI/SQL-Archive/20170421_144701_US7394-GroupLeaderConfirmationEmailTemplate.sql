USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2015;

IF NOT EXISTS (SELECT 1 FROM dbo.dp_Communications WHERE Communication_ID = @TemplateID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON

	INSERT INTO [dbo].[dp_Communications]
			   ([Communication_ID]
			   ,[Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[Start_Date]
			   ,[Communication_Status_ID]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[Template]
			   ,[Active])
		 VALUES
			   (@TemplateID
			   ,5 --Church Administrator
			   ,'Group Leader Application Received'
			   ,'Thanks so much for applying to be a group leader.<br><br>We''ve received your application and will be in touch in the next 48 hours with next steps.<br><br>If you have questions in the meantime, email us at [Reply_To_Email].'
			   ,1 --Domain ID
			   ,GETDATE()
			   ,1 --Draft Status
			   ,7675411  -- groups@crossroads.net
			   ,7675411  -- groups@crossroads.net
			   ,1 --Template
			   ,1) --Active

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF

END
