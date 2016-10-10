USE [MinistryPlatform]
GO

DECLARE @EVENT_TYPE_NAME nvarchar(50) = N'Camp';

IF NOT EXISTS( SELECT 1 FROM [dbo].[Event_Types] WHERE [Event_Type] = @EVENT_TYPE_NAME)
BEGIN

	INSERT INTO [dbo].[Event_Types]
			   ([Event_Type]
			   ,[Description]
			   ,[Domain_ID])
		 VALUES
			   (@EVENT_TYPE_NAME
			   ,N'Type used specifically for Camps'
			   ,1)
END