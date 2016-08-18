USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 264567
DECLARE @Body VARCHAR(max) = "<div style='font-family: Verdana; font-size: 12px;'><div>Hi there!<br/><br/></div><div>Welcome to the [Event_Date] [Event_Start_Time] Crossroads service.<br/><br/></div><div><a href='https://[BaseUrl]/live/stream'>Click here to join us.</a><br/><br/></div><div>We're glad you're here,<br/></div><div>-Crossroads</br></div></div>"
DECLARE @Subject VARCHAR(max) = 'Crossroads Live stream is starting'

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