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
			   ,'Hi [Nickname] [LastName],<br>You have successfully signed up to volunteer on May 20 with GO Local!<br><br>
			   Below is what you told us when signing up for GO Local. Please double-check the information and make sure everything is correct so we can plan for a super awesome day of serving your city. If you need to make changes to any of this information, please email [Project_Leader_Email_Address].<br><br>
			   Name: [Nickname] [LastName]<br>
			   Email: [Participant_Email_Address]<br>
			   Birthdate: [Date_Of_Birth]<br>
			   Mobile Phone: [Mobile_Phone]<br>
			   Spouse Participating: [Spouse_Participating]<br>
			   Number of Children Ages 0-17: [Number_Of_Children]<br>
			   Project Leader: [Group_Connector]<br>
			   Project Leader Email: [Project_Leader_Email_Address]<br><br>
			   Thanks again for joining the team and serving others in your city!<br>
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
