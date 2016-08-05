USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].dp_Communications WHERE communication_id = 280854)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Communications] ON;

INSERT INTO [dbo].dp_Communications 
(Communication_ID,Author_User_ID,Subject                                                  ,Body  ,Domain_ID,Start_Date                ,Communication_Status_ID,From_Contact,Reply_to_Contact,Template,Active) VALUES
(280854          ,5             ,'Someone is posting a Student group:  Please investigate','temp',1        ,{ts '2016-08-02 00:00:00'},1                      ,7           ,7               ,1       ,1     );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END

UPDATE [dbo].dp_Communications 
SET [Body] = N'The following group leaders are posting a group for students Grades (either 6-8, or 9-12) in the group tool.  <br />Please check to ensure this person is known and approved by SM.  <br />If not, please proceed with your normal process for approval/background check with this person.  <br />If you do not know them, ask them to remove their group until you have approved them.<br /><br />[Leaders]'
WHERE Communication_ID = 280854;