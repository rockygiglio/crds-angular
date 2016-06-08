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
           ,N'Crossroads Undivided:  Official Confirmation to Facilitate at Oakley'
           ,N'<div><span style="font-family: Arial; font-size: 13px;">Dear Reconciler,<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">You are officially IN as a facilitator for Undivided II - August 2016.  Here are the details you need to know to get ready for our first sessions.<br /><br /></span></div>Undivided Site:  <b>[Congregation_Name] [Description]</b><br />Group Name:  <b>[Group_Name]</b><br /><br /><div><span style="font-family: Arial; font-size: 13px;">If you require childcare, please RSVP <a href="https://docs.google.com/a/ingagepartners.com/forms/d/1I86QewcbRJPiIiRcFhC1w-Mg-5HFMmEese9Jd8KH_p8/viewform">here</a>.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;"><b>Required</b> Facilitator Training:  <b>[Facilitator_Training]</b>
             <br /><b>Please arrive at 7:30am to get registered.</b> Childcare will not be provided. If you were previously trained as a facilitator in Undivided I, please plan to attend the full training day as there have been some changes that we wish to communicate.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">Group co-facilitators are: <b>[Facilitators]</b><br /><br /></span></div>
             <div><span style="font-family: Arial; font-size: 13px;">You can meet your co-facilitator at the Facilitator Training or make arrangements on your own. The experience your group has and the depth of the conversation can be directly impacted by the relationship you both build prior to the first session. Make it a point to meet beforehand. You will meet the people in your group at the first session.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">Please note that with space constraints and a need for intentional racial make-up of our groups, we ask that only those who directly receive these emails attend the training dates.<br /><br /></span></div><div><span style="font-family: Arial; font-size: 13px;">We look forward to seeing you at training!<br /></span></div>
             <div><span style="font-family: Arial; font-size: 13px;">- The Undivided Team&quot;<br /><br /></span></div>'
           ,1
           ,'06/06/2016'
           ,7672342 --undivided@crossroads.net
           ,7672342 --undivided@crossroads.net
           ,1
           ,1)