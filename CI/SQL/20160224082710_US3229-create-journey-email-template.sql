USE [MinistryPlatform]
GO

DECLARE @Domain_ID AS INT = 1 
DECLARE @Author_User_ID AS INT = 5
DECLARE @Start_Date as DATETIME = '2016-02-01 00:00:00.000'
DECLARE @Expire_Date as DATETIME = NULL
DECLARE @Communication_Status_ID AS INT =  1
DECLARE @From_Contact AS INT = 1519180
DECLARE @Reply_to_Contact AS INT = 1519180
DECLARE @Template AS INT = 1
DECLARE @Active AS INT = 1


-- Add / Update the Email Templates for Journey Groups
SET IDENTITY_INSERT [dbo].[dp_Communications] ON 

DECLARE @Email_Templates AS TABLE (Communication_ID INT, Author_User_ID INT, [Subject] VARCHAR(256), Body NVARCHAR(max), Domain_ID INT, [Start_Date] DATETIME, Communication_Status_ID INT, From_Contact INT, Reply_to_Contact INT, Template BIT, Active BIT)

INSERT INTO @Email_Templates
	(Communication_ID, Author_User_ID, [Subject], Body, Domain_ID, [Start_Date], Communication_Status_ID, From_Contact, Reply_to_Contact, Template, Active)
	VALUES
	(17462
	     , @Author_User_ID
	     , 'You’re invited to a BRAVE group hosted by [PreferredName]'
		 , '<div><span style="font-family: Arial; font-size: 13px;"><b>Are you ready for a six-week spiritual journey? </b></span><br /></div><div><br /></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">BRAVE is about taking a new step and walk in confidence of who God made us to be. Through weekend teaching, individual work and meeting with a small group, we’ll be challenged to lean into God and to do new things. Step one is to join a group. </span><br /></div><div><br /></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">Here’s the link to the group. </span><br /></div><div><span style="font-family: Arial; font-size: 13px;"><a href="https://[BaseUrl]/bravegrouptool/group/join/[GroupId]">Click here to join</a></span></div><div></div><div><div><br /></div></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17546
	     , @Author_User_ID
	     , 'You’re hosting a BRAVE group!'
		 , '<span style="font-family: Arial; font-size: 13px;"><b>So what happens next? </b></span><div><br /></div><div><span style="font-family: Arial; font-size: 13px; font-weight: bold;">First the Review:  
			</span><span style="font-family: Arial; font-size: 13px;">[PreferredName] is hosting a BRAVE group @ [Address_Line_1] On [Meeting_Day] @ [Meeting_Time]
			If any of that is wrong, contact us. </span><br /></div><div><span style="font-family: Arial; font-size: 13px;"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; font-weight: bold;">Next
			</span><span style="font-family: Arial; font-size: 13px;">You kick back, wait and watch your group fill grow. We’ll email you updates as new members join. </span></div><div><span style="font-family: Arial; font-size: 13px; font-weight: bold;"><br /></span></div><div><span style="font-family: Arial; font-size: 13px;"></span><span style="font-family: Arial; font-size: 13px; font-weight: bold;">Then, it’s on to your Dashboard
			</span><span style="font-family: Arial; font-size: 13px;">Your dashboard is your host command center. 
			There you can:</span></div><div><span style="font-family: Arial; font-size: 13px;"> * Invite specific people to your group </span></div><div><span style="font-family: Arial; font-size: 13px;"> * Send notes to your group. </span></div><div><span style="font-family: Arial; font-size: 13px;"> * Access all of your BRAVE materials. 
			</span></div><div><span style="font-family: Arial; font-size: 13px;"><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">That sounds awesome, right? Why not go check out </span><span style="font-family: Arial; font-size: 13px; text-decoration: underline;"><a href="https://[BaseUrl]/group/dashboard">Your Dashboard</a></span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"> now. </span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"><br /></span></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17547
	     , @Author_User_ID
	     , 'You’re hosting a private BRAVE group'
		 , '<span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"><b>So what happens next? </b>
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; font-style: italic;">
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; font-weight: bold;">You need to invite people to your group 
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;">As the host of a private group, it’s up to you to fill out your roster. But we made it super easy. 

</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; font-weight: bold;">Just go to your dashboard
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;">From here you can send out invites to your people. And they’ll get a direct link to your specific group. 

</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; text-decoration: underline;"><a href="[BaseUrl]/Dashboard">Your dashboard</a>
</span><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; text-decoration: underline;"><br /></span></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17548
	     , @Author_User_ID
	     , 'You’ve joined a BRAVE group'
		 , '<div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"><b>Welcome to [HostName]’s group. </b>
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; font-style: italic;">
</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;">You will be meeting @ [AddressLine1] On [MeetingDay] @ [MeetingTIme]
If any of this information is incorrect, please contact us. 

In the meantime, just kick back and relax. Your host will reach out to you as the group takes shape with some details around gathering together. 

Thanks for being BRAVE!</span><br /></div><div><br /></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17549
	     , @Author_User_ID
	     , 'You’ve joined a BRAVE group'
		 , '<div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"><b>Welcome to [HostPreferredName]’s group. </b></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;">			</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;">Your host will reach out to you with details around where and when your group will be meeting. So hang tight. 

           Thanks for being BRAVE!</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap;"><br /></span></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17550
	     , @Author_User_ID
	     , 'I’m doing BRAVE Anywhere - Hook me up with a group'
		 , '<div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b>About me</b></span><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b><br /></b></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[PreferredName]  [LastName]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[email]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[PastExperience]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[goal]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[gender]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[MaritalStatus]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[DayPreference]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[TimePreference]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[Address_Line_1]</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b><br /></b></span></div><div>[City],[State]  [PostalCode]</div></div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17551
	     , @Author_User_ID
	     , 'Help! [PreferredName] couldn’t find a BRAVE group that fit'
		 , '<span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b>About me</b></span><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b><br /></b></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[PreferredName]  [LastName]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[email]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[PastExperience]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[goal]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[gender]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[MaritalStatus]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[DayPreference]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[TimePreference]</span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><br /></span></div><div><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[Address_Line_1]</span><span style="font-family: Arial; font-size: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"><b><br /></b></span></div><div>[City],[State]  [PostalCode]</div>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active)

MERGE [dbo].[dp_Communications] AS a
USING @Email_Templates AS tmp
	ON a.Communication_ID = tmp.Communication_ID
WHEN MATCHED THEN
	UPDATE
	SET
		Author_User_ID = @Author_User_ID,
		[Subject] = tmp.[Subject],
		Body = tmp.Body,
		Domain_ID = @Domain_ID,
		[Start_Date] = @Start_Date,
		Communication_Status_ID = @Communication_Status_ID,
		From_Contact = @From_Contact,
		Reply_to_Contact = @Reply_to_Contact,
		Template = @Template,
		Active = @Active
WHEN NOT MATCHED THEN
	INSERT
		(Communication_ID, Author_User_ID, [Subject], Body, Domain_ID, [Start_Date], Communication_Status_ID, From_Contact, Reply_to_Contact, Template, Active)
		VALUES
		(tmp.Communication_ID, @Author_User_ID, tmp.[Subject], tmp.Body, @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active);

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF

