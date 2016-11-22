USE [MinistryPlatform]
GO

DECLARE @COMMUNICATION_ID int = 2005;
DECLARE @AUTHOR int;
DECLARE @FROM int;

SELECT @AUTHOR = [User_ID] FROM [dbo].[dp_Users] WHERE [User_Name] = N'updates@crossroads.net';
SELECT TOP 1 @FROM = [Contact_ID] FROM [dbo].[Contacts] WHERE [Email_Address] = N'studentministry@crossroads.net';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @COMMUNICATION_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON;

	INSERT INTO [dbo].[dp_Communications]
			   ([Communication_ID]
			   ,[Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[Template]
			   ,[Active])
		 VALUES
			   (@COMMUNICATION_ID
			   ,@AUTHOR
			   ,N'[EVENT_TITLE] Application and Payment Received'
			   ,N'<div>Hey there!</div><div><br /></div><div>Thanks for signing up for [EVENT_TITLE]! We have your payment of [PAYMENT_AMOUNT]. Just a reminder, payments for camp are <b>NOT tax deductible</b>.</div><div><br /></div><div>If you need to make additional payments, you can do so at any time by going to <a href="https://[BASE_URL]/mycamps" target="_self">https://[BASE_URL]/mycamps</a> and clicking on the Make Payment button. Please note that these payments are also <b>NOT tax deductible</b>.<br /></div><div><br /></div><div>[EVENT_TITLE] will be from [EVENT_START_DATE] through [EVENT_END_DATE]. You’ll be receiving lots more details as we get closer to camp. We''re looking forward to an amazing time at camp! As we continue to prepare, please let us know if you have any questions. If you know of anyone else that would like to attend, they can register at <a href="[EVENT_URL]">[EVENT_URL]</a>.</div><div><br /></div><div>If you have any questions along the way, email [EMAIL_ADDRESS].</div><div><br /></div><div>Thanks,</div><div>[DISPLAY_NAME]</div>'
			   ,1
			   ,@FROM
			   ,@FROM
			   ,1
			   ,1)

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END
ELSE
BEGIN
	UPDATE [dbo].[dp_Communications]
	SET Body = N'<div>Hey there!</div><div><br /></div><div>Thanks for signing up for [EVENT_TITLE]! We have your payment of [PAYMENT_AMOUNT]. Just a reminder, payments for camp are <b>NOT tax deductible</b>.</div><div><br /></div><div>If you need to make additional payments, you can do so at any time by going to <a href="https://[BASE_URL]/mycamps" target="_self">https://[BASE_URL]/mycamps</a> and clicking on the Make Payment button. Please note that these payments are also <b>NOT tax deductible</b>.<br /></div><div><br /></div><div>[EVENT_TITLE] will be from [EVENT_START_DATE] through [EVENT_END_DATE]. You’ll be receiving lots more details as we get closer to camp. We''re looking forward to an amazing time at camp! As we continue to prepare, please let us know if you have any questions. If you know of anyone else that would like to attend, they can register at <a href="[EVENT_URL]">[EVENT_URL]</a>.</div><div><br /></div><div>If you have any questions along the way, email [EMAIL_ADDRESS].</div><div><br /></div><div>Thanks,</div><div>[DISPLAY_NAME]</div>',
	    Subject = N'[EVENT_TITLE] Application and Payment Received'
	WHERE Communication_ID = @COMMUNICATION_ID
END
