USE [MinistryPlatform]
GO

DECLARE @ContactID int = 768371;
DECLARE @TemplateID int = 264567;
DECLARE @Body VARCHAR(max) = '<div style="font-family: Verdana; font-size: 12px;"><div>Hi there!<br/><br/></div><div>Welcome to the [Event_Date] [Event_Start_Time] Crossroads service.<br/><br/></div><div><a href="https://[BaseUrl]/live/stream">Click here to join us.</a><br/><br/></div><div>We''re glad you''re here,<br/></div><div>-Crossroads</br></div></div>';
DECLARE @Subject VARCHAR(max) = 'Crossroads Live stream is starting';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Contacts] WHERE [Contact_ID] = @ContactID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Contacts] ON
	INSERT INTO [Contacts]
	(
		[Contact_ID],
		[Company],
		[Display_Name],
		[Contact_Status_ID],
		[Email_Address],
		[Domain_ID]
	)
	VALUES
	(
		@ContactID,
		1,
		'Crossroads Anywhere',
		1,
		'anywhere@crossroads.net',
		1
	)
	SET IDENTITY_INSERT [dbo].[Contacts] OFF
END

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
		,@ContactID
		,@ContactID
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
	  ,[From_Contact] = @ContactID
	  ,[Reply_to_Contact] = @ContactID
	  ,[Template] = 1
	  ,[Active] = 1
   WHERE Communication_ID = @TemplateID
END