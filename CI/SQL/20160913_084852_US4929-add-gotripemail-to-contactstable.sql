USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Contacts] WHERE Contact_ID = 7677101)
BEGIN
	SET IDENTITY_INSERT [dbo].[Contacts] ON;
	INSERT INTO [dbo].[Contacts]
           ([Contact_ID]
		   ,[Company]
           ,[Display_Name]
		   ,[First_Name]
		   ,[Last_Name]
		   ,[Nickname]
		   ,[Contact_Status_ID]
		   ,[Email_Address]
		   ,[Email_Unlisted]
		   ,[Bulk_Email_Opt_Out]
		   ,[Bulk_SMS_Opt_Out]
		   ,[Contact_GUID]
		   ,[Domain_ID]
		   )
     VALUES
           (7677101
		   ,0
		   ,N'Go Trips'
		   ,N'Go Trips'
		   ,N'Communications'
		   ,N'Go Trips'
		   ,1
		   ,N'gotrips@crossroads.net'
		   ,0
		   ,0
		   ,0
		   ,NEWID()
		   ,1
		   )
	SET IDENTITY_INSERT [dbo].[Contacts] OFF;
END