USE [MinistryPlatform]
GO

DECLARE @KcRoomUsageTypeId int = 6

IF NOT EXISTS(SELECT 1 FROM Room_Usage_Types WHERE Room_Usage_Type_ID = @KcRoomUsageTypeId)
BEGIN
	SET IDENTITY_INSERT [dbo].[Room_Usage_Types] ON
	INSERT INTO [dbo].[Room_Usage_Types]
		     ([Room_Usage_Type_ID]
			   ,[Room_Usage_Type]
			   ,[Description])
	VALUES
           (@KcRoomUsageTypeId
           ,'Kids Club Checkin'
           ,'Used for Kids Club')
	SET IDENTITY_INSERT [dbo].[Room_Usage_Types] OFF
END
