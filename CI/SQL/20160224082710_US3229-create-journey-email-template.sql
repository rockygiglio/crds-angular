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
	(17462, @Author_User_ID, 'An Invitation to Join a Journey Group', '[PreferredName] is hosting a Journey Group and would like you to join!<div><br /></div><div>Please follow this link to accept the invitation: https://[BaseUrl]/group/join/[GroupId]<div><br /></div></div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17546, @Author_User_ID, 'Confirmation of Your New Journey Group', 'Hello [PrimaryContactName],<div><br /><div>The Journey Group you are hosting is set up and ready to go.<div><br /><div>Your group will meet at [MeetingAddress] on [MeetingDay] at [MeetingTime].</div><div><br /></div></div></div></div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17547, @Author_User_ID, 'Confirmation of Your New Private Journey Group', '<div>Hello [PrimaryContactName],</div><div><br /></div><div>The Private Journey Group you are hosting is set up and ready to go.</div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17548, @Author_User_ID, 'Confirmation - Added to Journey Group', '<div>Hello [PreferredName],</div><div><br /></div><div>The Journey Group you joined will meet at [MeetingAddress] on [MeetingDay] at [MeetingTime].</div><div><br /></div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17549, @Author_User_ID, 'Confirmation - Added to Private Journey Group', '<div>Hello [PreferredName],</div><div><br /></div><div>You have joined a private Journey Group.<br /></div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17550, @Author_User_ID, 'New Member Added to Anywhere Journey Group', 'A new member has been added to the Anywhere Journey Group.<div><br /></div><div>[PreferredName]</div><div>[LastName]</div><div>[Email]</div><div>[Address]<br /></div><div><br /></div><div>[Gender]</div><div>[MaritalStatus]</div><div><br /></div><div>[PastExperience]</div><div>[Goal]</div><div><br /></div><div>[DayPreferences]</div><div>[TimePreferences]</div><div><br /></div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17551, @Author_User_ID, 'No Journey Group Match for Potential Participant', '<div>The following user is trying to join a Journey Group, but has not been able to find a matching group.</div><div><br /></div><div>[PreferredName]</div><div>[LastName]</div><div>[Email]</div><div>[Address]</div><div><br /></div><div>[Gender]</div><div>[MaritalStatus]</div><div><br /></div><div>[PastExperience]</div><div>[Goal]</div><div><br /></div><div>[DayPreferences]</div><div>[TimePreferences]</div>', @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active)

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

