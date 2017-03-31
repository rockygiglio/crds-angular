USE [MinistryPlatform]
GO

DECLARE @CommunicationID int = 2012

IF EXISTS(SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @CommunicationID)
BEGIN
UPDATE [dbo].[dp_Communications]
   SET [Body] = 'You have a new sign up for your GO Local Project!<br><br>
			    Below is what they told us when signing up:<br>
				Name: [Nickname] [LastName]<br>
				Email: [Participant_Email_Address]<br>
				Mobile Phone: [Mobile_Phone]<br>
				Total number of adults: [Adults_Participating]<br>
				Number of children Ages 0-17: [Number_Of_Children]<br>
				Total Number of Volunteers on this sign-up form: [Total_Volunteers]<br><br>
				<a href="[Base_URL]/go-volunteer/dashboard/[Project_ID]">View your whole team here.</a><br><br>
				If you need to make any changes to this information, please contact [Anywhere_GO_Contact].<br><br>
				Thanks for stepping up to lead. Your initiative is changing your city.<br>
				The GO Local Team'
 WHERE Communication_ID = @CommunicationID
END