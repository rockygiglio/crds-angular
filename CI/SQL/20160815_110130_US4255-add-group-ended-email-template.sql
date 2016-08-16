USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].dp_Communications WHERE communication_id = 290789)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Communications] ON;

INSERT INTO [dbo].dp_Communications 
(Communication_ID,Author_User_ID,Subject                                  ,Body  ,Domain_ID,Start_Date                ,Communication_Status_ID,From_Contact,Reply_to_Contact,Template,Active) VALUES
(290789          ,5             ,'Your existing group is no longer active','temp',1        ,{ts '2016-08-11 00:00:00'},1                      ,7           ,7               ,1       ,1     );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END

UPDATE [dbo].dp_Communications 
SET [Body] = N'Hello [Participant_Name],<br /><br />This group has concluded and is no longer active.  We hope it was a great experience and encourage you to check out the Group Tool for a listing of other group options.<br /><br />[Group_Tool_Url]<br /><br />Crossroads'
WHERE Communication_ID = 290789;