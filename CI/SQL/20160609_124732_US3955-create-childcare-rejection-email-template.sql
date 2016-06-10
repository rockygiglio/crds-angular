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
		    5601,
			5,
			'Your Childcare Request has been rejected',
			'<span style="font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 13px; background-color: rgb(255, 255, 255);">Your request for Childcare for [Group] at [Congregation] on [ChildcareSession], [Frequency] has been denied for the following dates [Dates]. Please click on the link below to review.</span><div><a href="https://[Base_Url]/ministryplatform#/36/[RequestId]" target="_self">Rejected-Childcare-Request</a><font face="Helvetica Neue, Helvetica, Arial, sans-serif"><br /></font><div><br /></div></div>',
			1,
			Getdate(),
			1,
			7542465,
			7542465,
			1,
			1
		   )
GO