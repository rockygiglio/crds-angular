USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2011
DECLARE @Body VARCHAR(max) = 'Hi [Recipient_Name],<br />' + 
							 '<br />' +
							 'Somebody wants you to join them!  [Leader_Name] from [City], [State] would like you to join ' +
							 'their gathering.  Here''s a description of what''s happening:' + 
							 '<br />' +
							 '[Description]' +
							 '<br />' +
							 'Head to crossroads.net/connect to accept and meet up for service.' +
							 '<br />' +
							 '<a href="https://www.crossroads.net/connect/accept-invite/[Group_ID]/[Invitation_GUID]">Click here to accept the invite.</a>' + 
							 '<br />' +
							 '<a href="https://www.crossroads.net/connect/decline-invite/[Group_ID]/[Invitation_GUID]">Click here to decline the invite.</a>'
DECLARE @Subject VARCHAR(max) = 'You''ve been invited'

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
		,768371
		,768371
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
	  ,[From_Contact] = 768371
	  ,[Reply_to_Contact] = 768371
	  ,[Template] = 1
	  ,[Active] = 1
   WHERE Communication_ID = @TemplateID
END