USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2016
DECLARE @Body VARCHAR(max) = 'Hi [Host],<br />' + 
							 '<br />' +
							 '[Community_Member] has accepted your invitation. They''ve been added to your list of participants so you can keep them updated on what''s happening in your community.<br />' +
							 '<br />' +
							 'Your group is growing. Way to go!'
DECLARE @Subject VARCHAR(max) = '[Community Member] has accepted your invite'

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