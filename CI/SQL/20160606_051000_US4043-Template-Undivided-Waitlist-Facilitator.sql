USE [MinistryPlatform]
GO

	INSERT INTO [dbo].[dp_Communications]
		       ([Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[Start_Date]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[Template]
			   ,[Active])
	VALUES 
           (5
           ,'Crossroads Undivided:  Request to Facilitate on Wait List'
           ,'<div><span style="font-family: Arial; font-size: 13px;">Dear Reconciler,<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">Thanks again for requesting to be an Undivided Facilitator. This is to confirm you are on a WAIT LIST for this round of conversations.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">If a spot opens up, we will reach out to you as soon as possible. Otherwise, we hope to see you at future sessions.<br /><br /><div><span style="font-family: Arial; font-size: 13px;">Uniting God''s Kingdom one conversation at a time,<br /></span></div><div><span style="font-family: Arial; font-size: 13px;">-The Undivided Team</span></div>'
           ,1
           ,'06/06/2016'
           ,7672342 --undivided@crossroads.net
           ,7672342 --undivided@crossroads.net
           ,1
           ,1)