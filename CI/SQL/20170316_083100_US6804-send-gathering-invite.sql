USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2011
DECLARE @Body VARCHAR(max) = '[Leader_Name]' + 
							 '<br />Bacon ipsum dolor amet if Trump Ipsum weren’t my own words, perhaps I’d be dating it. He’s not a word hero. He’s a word hero because he was captured. I like text that wasn’t captured. He’s not a word hero. He’s a word hero because he was captured. I like text that wasn’t captured.'
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

IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_Invitation_Types] WHERE Invitation_Type_ID = 3)
BEGIN
	SET IDENTITY_INSERT [dbo].[cr_Invitation_Types] ON
	INSERT INTO [cr_Invitation_Types]
	(
		 [Invitation_Type_ID],
		 [Invitation_Type],
		 [Description],
		 [Domain_ID]
	)
	VALUES
	(
		 3,
		 'Anywhere Gathering',
		 'Invitations for anywhere gathering connect tool',
		 1
	)

	SET IDENTITY_INSERT [dbo].[cr_Invitation_Types] OFF
END
ELSE
BEGIN
   UPDATE [dbo].[cr_Invitation_Types]
   SET	 [Invitation_Type] = 'Anywhere Gathering',
		 [Description] = 'Invitations for anywhere gathering connect tool',
		 [Domain_ID] = 1
   WHERE Invitation_Type_ID = 3
END