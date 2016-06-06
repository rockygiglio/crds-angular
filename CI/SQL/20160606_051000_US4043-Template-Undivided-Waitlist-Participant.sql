USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM dp_Communications WHERE Template = 1 AND Communication_ID = 17554
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
           (17554
		   ,5
           ,'Crossroads Undivided:  Request to Participate on Wait List'
           ,'Body [GP_Display_Name] [Group_Name]'
           ,1
           ,'06/06/2016'
           ,7672342 --undivided@crossroads.net
           ,7672342 --undivided@crossroads.net
           ,1
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END