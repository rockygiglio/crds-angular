USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2017
DECLARE @Body VARCHAR(max) = 'Hi [Host],<br />' + 
							 '<br />' +
							 'Rats. [Community_Member] didn''t accept the request to attend your gathering. No worries, there''s still a ton of ways you can connect with other people. Here are some options to consider:<br />' +
							 '<br />' +
							 '<ul>' +
							 '<li>Invite your friends, family, and neighbors to join your gathering</li>' +
							 '<li>Say "Hi" to other community members on the map</li>' +  
							 '</ul>' + 
							 '<br />' +
							 'If you have any questions, the Anywhere Team is here to help! Email anywhere@crossroads.net and we''ll get back to you in a jiffy.'
DECLARE @Subject VARCHAR(max) = '[Community Member] has declined your invite'

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