USE [MinistryPlatform]
GO

DECLARE @ConfirmationTemplateID int = 2007;

IF NOT EXISTS (SELECT 1 FROM dbo.dp_Communications WHERE Communication_ID = @ConfirmationTemplateID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON

	INSERT INTO [dbo].[dp_Communications]
			   ([Communication_ID]
			   ,[Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[Start_Date]
			   ,[Communication_Status_ID]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[Template]
			   ,[Active])
		 VALUES
			   (@ConfirmationTemplateID
			   ,5 --Church Administrator
			   ,'GO Local Signup Confirmation - Crossroads Anywhere'
			   ,'Hi [Nickname] [LastName],
You have successfully signed up to volunteer on May 20 with GO Local!

Below is what you told us when signing up for GO Local. Please double-check the information and make sure everything is correct so we can plan for a super awesome day of serving your city. If you need to make changes to any of this information, please email [Project Leader email].

Name: [Nickname] [LastName]
Email: [Participant Email address]
Birthdate: [Date of Birth]
Mobile Phone: [Mobile Phone]
Number of Children Ages 0-17: [Number of Children]
Project Leader: [Group Connector]
Project Leader Email: [Project Leader Email]


Thanks again for joining the team and serving others in your city!
The GO Local Team'
			   ,1 --Domain ID
			   ,GETDATE()
			   ,1 --Draft Status
			   ,50
			   ,50
			   ,1 --Template
			   ,1) --Active

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF

END
