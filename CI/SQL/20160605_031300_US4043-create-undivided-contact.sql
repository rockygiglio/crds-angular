USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Contacts] WHERE Contact_ID = 7672342)
BEGIN
	SET IDENTITY_INSERT [dbo].[Contacts] ON;
	INSERT INTO [dbo].[Contacts]
           ([Contact_ID]
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
           (7672342
		   ,N'Undivided'
		   ,N'Undivided'
		   ,N'Communications'
		   ,N'Undivided'
		   ,1
		   ,N'undivided@crossroads.net'
		   ,0
		   ,0
		   ,0
		   ,NEWID()
		   ,1
		   )
	SET IDENTITY_INSERT [dbo].[Contacts] OFF;
END