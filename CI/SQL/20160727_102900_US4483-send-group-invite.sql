USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 261888
DECLARE @Body VARCHAR(max) = '<div style="font-family: Verdana; font-size: 12px;"><div>Hi, [Recipient_Name]!<br /><br /></div><div>[Leader_Name] has invited you have been invited to join [Group_Name].  <a href="https://int.crossroads.net/groups/invitation/accept/[Invitation_GUID]">Click here to accept</a></div></div>'
DECLARE @Subject VARCHAR(max) = 'Group Invitation'

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @TemplateID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON
	INSERT INTO [dp_Communications]
	(
		 [Communication_ID]
		,[Author_User_ID]
		,[Subject]
		,[Body]
		,[Domain_ID]
		,[Start_Date]
		,[From_Contact]
		,[Reply_to_Contact]
		,[Template]
		,[Active]
	)
	VALUES
	(
		 @TemplateID
		,5
	    ,@Subject
		,@Body
		,1
		,GetDate()
		,1519180
		,1519180
		,1
		,1
	)

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END
ELSE
BEGIN
   UPDATE [dbo].[dp_Communications]
   SET [Body] = @Body
      ,[Subject] = @Subject
	  ,[From_Contact] = 1519180
	  ,[Reply_to_Contact] = 1519180
	  ,[Template] = 1
	  ,[Active] = 1
   WHERE Communication_ID = @TemplateID
END
