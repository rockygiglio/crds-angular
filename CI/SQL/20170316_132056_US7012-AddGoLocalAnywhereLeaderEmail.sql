USE [MinistryPlatform]
GO

DECLARE @ConfirmationTemplateID int = 2012;

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
			   ,'New GO Local Signup - Crossroads Anywhere Project'
			   ,'You have a new sign up for your GO Local Project!<br><br>
			    Below is what they told us when signing up:<br>
				Name: [Nickname] [LastName]<br>
				Email: [Participant_Email_Address]<br>
				Mobile Phone: [Mobile_Phone]<br>
				Total number of adults: [Adults_Participating]<br>
				Number of children Ages 0-17: [Number_Of_Children]<br>
				Total Number of Volunteers on this sign-up form:<br><br>
				If you need to make any changes to this information, please contact [Anywhere_GO_Contact].<br><br>
				Thanks for stepping up to lead. Your initiative is changing your city.<br>
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
