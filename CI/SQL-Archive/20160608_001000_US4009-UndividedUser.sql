USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Users] ON

INSERT INTO [dbo].[dp_Users]
           ([User_ID]
		   ,[User_Name]
		   ,[User_Email]
           ,[Display_Name]
		   ,[Contact_ID]
		   ,[DOmain_ID]
		   )
     VALUES
		   (1344416
		   ,'undivided@crossroads.net'
		   ,'undivided@crossroads.net'
           ,'Undivided'
		   ,7672342
		   ,1
		   )

SET IDENTITY_INSERT [dbo].[dp_Users] OFF
GO