USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 261888
DECLARE @Body VARCHAR(max) = '<div>' +
							 	'<div style="font-family: Verdana; font-size: 12px;">' + 
									'Hi [Recipient_Name]!' +
									'<br />' +
									'<br />' +
								 '</div>' +
								 '<div style="font-family: Verdana; font-size: 12px;">' +
								 	'[Leader_Name] has invited you to join [Group_Name]. To accept their request, click the link below. You will be asked to sign into your Crossroads.net account and preview the group before joining. This invite link will only work once.' +
								 '</div>' +
							 	 '<div style="font-family: Verdana; font-size: 12px;">' +
								 	'<br />' +
								 '</div>' +
								 '<div>' +
								 	'<div>' +
										'<font face="Verdana">' +
											'You can also ask any questions directly to the leader by replying to this email. ' +
										'</font>' +
									'</div>' +
								 '</div>' +
								 '<div style="font-family: Verdana; font-size: 12px;">' +
								 	'<br />' +
								 '</div>' +
								 '<div style="font-family: Verdana; font-size: 12px;">' +
								 	'Click here to accept: https://int.crossroads.net/groups/invitation/accept/[Invitation_GUID]' +
								 '</div>' + 
							 '</div>'
DECLARE @Subject VARCHAR(max) = 'You''ve been invited to a Group'

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
