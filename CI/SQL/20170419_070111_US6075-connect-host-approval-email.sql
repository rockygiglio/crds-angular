USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 2014;

IF NOT EXISTS (SELECT 1 FROM dbo.dp_Communications WHERE Communication_ID = @TemplateID)
BEGIN
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
			   (@TemplateID
			   ,5 --Church Administrator
			   ,'Crossroads Connect Gathering Host Approved'
			   ,'We are excited that you will be on the map as a Gathering Host on Crossroads Connect.<br><br>
                 It may take up to one day for your new Gathering to be visible on the Connect Map.<br><br>
                 Thanks again for joining the team and welcoming others!<br><br>
                 The Crossroads Anywhere Team'
			   ,1 --Domain ID
			   ,GETDATE()
			   ,1 --Draft Status
			   ,768371  -- anywhere@crossroads.net
			   ,768371  -- anywhere@crossroads.net
			   ,1 --Template
			   ,1) --Active

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF

END
GO

