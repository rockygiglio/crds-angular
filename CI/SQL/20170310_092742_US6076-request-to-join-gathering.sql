USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].dp_Communications WHERE communication_id = 2008)
	BEGIN

	SET IDENTITY_INSERT [dbo].[dp_Communications] ON

	INSERT INTO [dbo].dp_Communications 
	(Communication_ID,Author_User_ID,Subject                                       ,Body  ,Domain_ID,Start_Date                ,Communication_Status_ID,From_Contact,Reply_to_Contact,Template,Active) VALUES
	(2008            ,5             ,'[RequestorSub] wants to join your gathering!','temp',1        ,{ts '2017-03-10 00:00:00'},1                      ,7675411     ,7675411         ,1       ,1     )

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END

UPDATE [dbo].dp_Communications 
SET [Body] = N'Hello [Name],<p>[RequestorBod] has requested to join your group. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque, mi vitae luctus commodo, neque nisi scelerisque risus, ultrices bibendum dui arcu non leo. Vivamus nisl mi, vestibulum ac metus id, tristique convallis lorem. Quisque bibendum sodales diam nec commodo. Integer nisl turpis, sollicitudin varius turpis eget, imperdiet dignissim tortor. Mauris metus ante, mattis id malesuada nec, porta mollis neque. Maecenas scelerisque quis dolor eget vulputate. Mauris sit amet purus non nunc pharetra pulvinar. In vitae blandit purus, consequat interdum velit.</p>'
WHERE Communication_ID = 2008