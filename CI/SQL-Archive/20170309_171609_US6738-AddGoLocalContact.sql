USE [MinistryPlatform]
GO

DECLARE @GOLocalContact int = 50;
IF NOT EXISTS (SELECT 1 FROM dbo.Contacts WHERE Contact_ID = @GOLocalContact)
	BEGIN
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
				   ,'GO Local Anywhere'
				   ,'GO Local'
				   ,'Anywhere'
				   ,'GO Local'
				   ,1 --Active Contact Status
				   ,'anywherego@crossroads.net'
				   ,1 --Domain ID
				   )
		SET IDENTITY_INSERT dbo.Contacts OFF
	END
ELSE 
	BEGIN 
		UPDATE [dbo].[Contacts]
		SET 
			Display_Name = 'GO Local Anywhere',
			First_Name = 'GO Local',
			Last_Name = 'Anywhere',
			Nickname = 'GO Local',
			Email_Address = 'anywherego@crossroads.net'
		WHERE Contact_ID = @GOLocalContact
	END