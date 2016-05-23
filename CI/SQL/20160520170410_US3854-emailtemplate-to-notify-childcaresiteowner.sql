USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Communications ON;

INSERT INTO [dbo].[dp_Communications]
           (
		    [Communication_ID],
			[Author_User_ID],
			[Subject],
			[Body],
			[Domain_ID],
			[Start_Date],
			[Communication_Status_ID],
			[From_Contact],
			[Reply_to_Contact],
			[Template],
			[Active]
		  )
     VALUES
           (
		    5500,
			5,
			'A new Childcare Request Submitted',
			'<span style="font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 13px; background-color: rgb(255, 255, 255);">You have a new request for Childcare from [Requester] for [Group] at [Congregation] from [StartDate] to Enddate] . Please click on the link below and review to either approve or reject this request.</span><div><a href="https://adminint.crossroads.net/ministryplatform#/36/[RequestId]" target="_self">New-Childcare-Request</a><font face="Helvetica Neue, Helvetica, Arial, sans-serif"><br /></font><div><br /></div></div>',
			1,
			Getdate(),
			1,
			7542465,
			7542465,
			1,
			1
		   )
GO

 

