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
			   ,N'Summer Camp Application and Payment Received'
			   ,N'<div>Hey there!</div><div><br /></div><div>We’re so excited that your student will be going to [Event_Title] with us!  Thanks for submitting your payment of [Payment_Amount]. Just a reminder, payments for camp are <b>NOT tax deductible</b>.</div><div><br /></div><div>Camp must be paid in full by May 1st. If you need to make additional payments, you can do so at any time by going to <a href="https://[Base_URL]/mycamps" target="_self">crossroads.net/mycamps</a> and clicking on the Make Payment button. Please note that these payments are also <b>NOT tax deductible</b>.<br /></div><div><br /></div><div>Camp will be from [Event_Start_Date] through [Event_End_Date]. You’ll be receiving lots more details as we get closer to camp. You can check out some commonly asked camp questions <a campfaq2016.pdf"="" content="" crds-cms-uploads="" documents="" href="https://[Base_URL]/mycamps&gt; https://crossroads.net/mycamps &lt;/a&gt; and clicking on the Make Payment button. Please note that these payments are also &lt;b&gt;NOT tax deductible&lt;/b&gt;. &lt;/div&gt;&lt;div&gt;&lt;br /&gt;&lt;/div&gt;&lt;div&gt;Camp will be from [Event_Start_Date] through [Event_End_Date]. You’ll be receiving lots more details as we get closer to camp. You can check out some commonly asked camp questions &lt;a href=" https:="" s3.amazonaws.com="" target="_self">HERE</a>. We''re looking forward to an amazing time at camp with your students. As we continue to prepare, please let us know if you have any questions. If your student has friends who would like to attend, they can register at <a href="https://[Base_URL]/camp" target="_self">https://crossroads.net/camp</a>.</div><div><br /></div><div>If you have any questions along the way, email studentministry@crossroads.net.</div><div><br /></div><div>Thanks,</div><div>Crossroads Student Ministry Team</div>'
			   ,1
			   ,@FROM
			   ,@FROM
			   ,1
			   ,1)

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END
