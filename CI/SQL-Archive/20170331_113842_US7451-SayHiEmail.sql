USE [MinistryPlatform]
GO

DECLARE @templateId as int;
Set @templateId = 2013;

IF NOT EXISTS(SELECT * FROM [dbo].dp_Communications WHERE communication_id = @templateId)
	BEGIN

	SET IDENTITY_INSERT [dbo].[dp_Communications] ON

	INSERT INTO [dbo].dp_Communications 
	(Communication_ID,Author_User_ID,Subject                               ,Body  ,Domain_ID,Start_Date                ,Communication_Status_ID,From_Contact,Reply_to_Contact,Template,Active) VALUES
	(@templateId     ,5             ,'Connect with [Community_Member_Name]','temp',1        ,{ts '2017-03-10 00:00:00'},1                      ,768371     ,768371           ,1       ,1     )

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END

UPDATE [dbo].dp_Communications 
SET [Body] = N'<font face="arial, sans, sans-serif"><span style="font-size: 13px; white-space: pre-wrap;">Hi [Pin_First_Name],<br /><p>[Community_Member_Name] ([Community_Member_Email]) from [Community_Member_City], [Community_Member_State] says hi! That means they''re interested in connecting.   Be a hero and respond to this email right now. Your new Crossroads best friend is waiting. Don''t know what to say? Don''t sweat it. Reply with an answer to these questions and you''re guaranteed to break the ice.   </p><ol><li>What''s your name? </li><li>How did you you get connected to Crossroads? </li><li>Would you rather punch a shark or slap a bull?  </li></ol>For bonus points, include your favorite, non-email way to connect with people (text, Facebook Messenger, Facetime, etc.). ''Cause let''s face it, no one likes email.  </span></font><div><font face="arial, sans, sans-serif"><span style="font-size: 13px; white-space: pre-wrap;"><br /></span></font></div><div><font face="arial, sans, sans-serif"><span style="font-size: 13px; white-space: pre-wrap;">Get to know your new Crossroads pal!<br /><p></p></span></font></div>'
WHERE Communication_ID = @templateId