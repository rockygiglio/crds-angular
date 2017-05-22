USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM dp_Communications WHERE Template = 1 AND Communication_ID = 82)
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
           (82
		   ,1
           ,'Online signup for [Group_Name]'
           ,'[GP_Display_Name] has signed up online for [Group_Name]'
           ,1
           ,'04/20/2016'
           ,1519180
           ,1519180
           ,1
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END

