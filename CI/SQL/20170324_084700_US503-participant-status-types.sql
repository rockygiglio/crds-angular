USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Participation_Statuses] ON

IF NOT EXISTS( SELECT 1 FROM [dbo].[Participation_Statuses] WHERE [Participation_Status] = '06 Capacity')
BEGIN

	INSERT INTO [dbo].[Participation_Statuses]
			   ([Participation_Status_ID]
			   ,[Participation_Status]
			   ,[Description])
		 VALUES
			   (6,
			    '06 Capacity'
			   ,N'Type used for Sign In')
END

IF NOT EXISTS( SELECT 1 FROM [dbo].[Participation_Statuses] WHERE [Participation_Status] = '07 Error')
BEGIN

	INSERT INTO [dbo].[Participation_Statuses]
			   ([Participation_Status_ID]
			   ,[Participation_Status]
			   ,[Description])
		 VALUES
			   (7,
			    '07 Error'
			   ,N'Type used for Sign In')
END

SET IDENTITY_INSERT [dbo].[Participation_Statuses] OFF
 