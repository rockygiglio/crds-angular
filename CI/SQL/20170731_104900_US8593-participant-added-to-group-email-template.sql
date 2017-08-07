USE [MinistryPlatform]
GO
DECLARE @CommunicationID    AS INT
SET     @CommunicationID = 2025
IF EXISTS (SELECT * 
		   FROM [dbo].[dp_Communications]
		   WHERE Communication_ID = @CommunicationID)

BEGIN
DELETE  
FROM [dbo].[dp_Communications]
WHERE Communication_ID = @CommunicationID
END

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
           (@CommunicationID
		   ,5
		   ,'[Leader_Name] has added you to [Group_Name]'
		   ,'<div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">Hi <span font-size:="" medium;"="" new="" roman";="" style="font-family: " times="">[Participant_Name]!</span><br font-size:="" medium;"="" new="" roman";="" style="font-family: " times="" /><br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;"><br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">[Leader_Full_Name] just added you to the Group they lead called [Group_Name].</div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;"><br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">Hopefully, this is not a surprise and you know where and when the Group meets!</div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;"><br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">If you have any questions, please contact [Leader_Name] directly at [Leader_Email]</div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;"><br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">Thanks!<br /></div><div class="gmail_default rally-rte-class-03755033359875" style="font-family: arial, sans-serif; font-size: 12px;">The Groups Team</div>'
		   ,1
		   ,'2017-06-23 00:00:00.000'	   
		   ,1
		   ,7675411
		   ,7675411		   
		   ,0
		   ,0)  

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
