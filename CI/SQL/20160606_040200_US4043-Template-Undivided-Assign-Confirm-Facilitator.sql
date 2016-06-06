USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM dp_Communications WHERE Template = 1 AND Communication_ID = 17552
)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON
	INSERT INTO [dbo].[dp_Communications]
		       ([Communication_ID]
			   ,[Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[Start_Date]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[Template]
			   ,[Active])
	VALUES 
           (17552 --communication_id
		   ,5
           ,'Crossroads Undivided:  Official Confirmation to Facilitate'
           ,'Body [GP_Display_Name] [Group_Name]'
           ,1
           ,'06/06/2016'
           ,7672342 --undivided@crossroads.net
           ,7672342 --undivided@crossroads.net
           ,1
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END