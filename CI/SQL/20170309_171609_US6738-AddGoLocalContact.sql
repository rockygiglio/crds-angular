USE [MinistryPlatform]
GO

DECLARE @GOLocalContact int = 50;

SET IDENTITY_INSERT dbo.Contacts ON
INSERT INTO [dbo].[Contacts]
           ([Contact_ID]
		   ,[Company]
           ,[Display_Name]
           ,[First_Name]
           ,[Last_Name]
           ,[Nickname]
           ,[Contact_Status_ID]
           ,[Email_Address]
           ,[Domain_ID])
     VALUES
           (@GOLocalContact
		   ,0
           ,'GO Local'
           ,'GO'
           ,'Local'
           ,'GO'
           ,1 --Active Contact Status
           ,'golocal@crossroads.net'
           ,1 --Domain ID
		   )
GO

SET IDENTITY_INSERT dbo.Contacts OFF