USE [MinistryPlatform]
GO

DECLARE @Domain_ID AS INT = 1 
DECLARE @Author_User_ID AS INT = 1579976
DECLARE @Start_Date as DATETIME = '2016-06-01 00:00:00.000'
DECLARE @Expire_Date as DATETIME = NULL
DECLARE @Communication_Status_ID AS INT =  1
DECLARE @From_Contact AS INT = 7672342
DECLARE @Reply_to_Contact AS INT = 7672342
DECLARE @Template AS INT = 1
DECLARE @Active AS INT = 1

SET IDENTITY_INSERT [dbo].[dp_Communications] ON 

DECLARE @Email_Templates AS TABLE (Communication_ID INT, Author_User_ID INT, [Subject] VARCHAR(256), Body NVARCHAR(max), Domain_ID INT, [Start_Date] DATETIME, Communication_Status_ID INT, From_Contact INT, Reply_to_Contact INT, Template BIT, Active BIT)

INSERT INTO @Email_Templates
	(Communication_ID, Author_User_ID, [Subject], Body, Domain_ID, [Start_Date], Communication_Status_ID, From_Contact, Reply_to_Contact, Template, Active)
	VALUES
	(17552
	     , @Author_User_ID
	     , 'Crossroads Undivided: Request to Participate Received'
		 , '<span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We have received your REQUEST to PARTICIPATE in Undivided at Crossroads.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We will send you an official registration (or wait list) confirmation by email no later than Wednesday, July 20. Before then, we''re working hard to configure groups to ensure a positive experience for all participants.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">Here''s what we take into consideration as we put together groups:</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- We are building groups with 40/60 racial mix across 8-9 people, including two Facilitators of two different races.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- We will operate on a first-come, first-served basis.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- We will do our best to maximize participation so that few as possible are on the wait list.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- Anyone who makes the wait list will be notified as soon as possible.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- Anyone on the wait list may be asked last minute to participate, as spots open up. If not, they will be invited to attend the next round of Undivided.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We appreciate your heart to explore the racial divide in our city and your patience while we arrange these groups. God is working in BIG ways here and we''re honored to be a part of it!</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">Uniting His Kingdom,</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- The Undivided Team</span>'
		 , @Domain_ID, @Start_Date, @Communication_Status_ID, @From_Contact, @Reply_to_Contact, @Template, @Active),
	(17553
	     , @Author_User_ID
	     , 'Crossroads Undivided: Request to Facilitate Received'
		 , '<span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We have received your REQUEST to FACILITATE an Undivided Group at Crossroads.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We will send you an official registration (or wait list) confirmation by email before training dates (July 16 and 30) no later than Wednesday, July 20. Before then, we''re working hard to configure groups to ensure a positive experience for all participants.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">Here''s what we take into consideration as we put together groups:</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- Groups will be 8-9 people with a 40/60 racial mix, including two facilitators of two different races.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- We''ll do our best to honor all requests for facilitators and participants. Anyone who is not placed in a group will be added to a wait list, on a first-come, first-served basis.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- As spots open up, anyone on the wait list may be asked last minute to participate.  If not, they will be invited to attend the next round of Undivided.</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">We appreciate your heart to explore the racial divide in our city and your patience while we arrange these groups. God is working in BIG ways here and we''re honored to be a part of it!</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);"> </span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">United for His Kingdom,</span><br style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap;" /><span style="font-family: arial, sans, sans-serif; font-size: 13px; line-height: 13px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">- The Undivided Team</span>'
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
