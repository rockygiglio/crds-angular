USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Event_Types] ON

DECLARE @EVENT_TYPE_NAME nvarchar(50) = N'Adventure Club';

IF NOT EXISTS( SELECT 1 FROM [dbo].[Event_Types] WHERE [Event_Type] = @EVENT_TYPE_NAME)
BEGIN

	INSERT INTO [dbo].[Event_Types]
			   ([Event_Type_ID]
			   ,[Event_Type]
			   ,[Description]
			   ,[Domain_ID])
		 VALUES
			   (20,
			    @EVENT_TYPE_NAME
			   ,N'Kids Club Adventure Club'
			   ,1)
END

SET IDENTITY_INSERT [dbo].[Event_Types] OFF
