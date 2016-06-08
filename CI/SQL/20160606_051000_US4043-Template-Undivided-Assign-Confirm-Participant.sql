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
           ,N'Crossroads Undivided:  Official Confirmation to Participate at Oakley'
           ,N'<div><span style="font-family: Arial; font-size: 13px;">Dear Reconciler,<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">You are officially IN as a Participant for Undivided II - August 2016. Here are a few details you need to know to get ready for the upcoming session.<br /><br /></span></div><span style="font-family: Arial; font-size: 13px;">Undivided Site: <b>[Congregation_Name] [Description]<br /></b>Group Name: <b>[Group_Name]</b><br /><br /></span><div><span style="font-family: Arial; font-size: 13px;">If you require childcare, please RSVP <a href="https://docs.google.com/a/ingagepartners.com/forms/d/1I86QewcbRJPiIiRcFhC1w-Mg-5HFMmEese9Jd8KH_p8/viewform">here</a>.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">You will need your group number to find your group at the first session, when you will meet your facilitators and group members.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">Due to space constraints and a need for intentional racial make-up of our groups, we ask that only those who directly receive these emails attend these sessions.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">We look forward to seeing you at Undivided!<br /></span></div><div><span style="font-family: Arial; font-size: 13px;">- The Undivided Team</span></div>'
           ,1
           ,'06/06/2016'
           ,7672342 --undivided@crossroads.net
           ,7672342 --undivided@crossroads.net
           ,1
           ,1)