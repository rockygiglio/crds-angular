USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].dp_Communications WHERE communication_id = 2002)
	BEGIN

	SET IDENTITY_INSERT [dbo].[dp_Communications] ON

	INSERT INTO [dbo].dp_Communications 
	(Communication_ID,Author_User_ID,Subject                                    ,Body  ,Domain_ID,Start_Date                ,Communication_Status_ID,From_Contact,Reply_to_Contact,Template,Active) VALUES
	(2002            ,5             ,'Someone has requested to join your group!','temp',1        ,{ts '2016-08-11 00:00:00'},1                      ,7675411     ,7675411         ,1       ,1     )

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END

UPDATE [dbo].dp_Communications 
SET [Body] = N'Hello [Name],<p>[Requestor] has requested to join your group. Visit your My Groups dashboard in <a href="http://www.crossroads.net/groups/mygroups">crossroads.net</a>, and select "approve" to add this person to your group.</p><p>If your group is full, kindly let the person know you are at capacity, and switch your group from public to private.</p>'
WHERE Communication_ID = 2002