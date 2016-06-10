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
		    5603,
			5,
			'Your Childcare Request has been conditionally approved',
			'<span style="font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 13px; background-color: rgb(255, 255, 255);">Your request for Childcare for [Group] at [Congregation] on [ChildcareSession], [Frequency] has been approved for the following dates [Dates]. <br />Decision Notes: [DecisionNotes]<br /></span><span style="font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 13px; background-color: rgb(255, 255, 255);">Please click on the link below to review.</span><br /><div><a href="https://[Base_Url]/ministryplatform#/36/[RequestId]" target="_self">Conditionally-Approved-Childcare-Request</a><font face="Helvetica Neue, Helvetica, Arial, sans-serif"><br /></font><div><br /></div></div>',
			1,
			Getdate(),
			1,
			7542465,
			7542465,
			1,
			1
		   )
GO