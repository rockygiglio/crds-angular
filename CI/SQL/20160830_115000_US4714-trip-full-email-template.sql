USE [MinistryPlatform]
GO

DECLARE @communicationId AS INT = 2003;

IF NOT EXISTS ( SELECT  1 FROM dp_communications WHERE [Communication_ID] = @communicationId)

BEGIN

SET IDENTITY_INSERT [dbo].dp_communications ON;

INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID]
		   ,[Author_User_ID]
		   ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[Template]
           ,[Active])
     VALUES
          (
           @communicationId
           ,5
           ,'[Pledge_Campaign] is full'
           ,'<p>The [Pledge_Campaign] has reached maximum capacity. Please update <a href="http://crossroadsgo.net" >crossroadsgo.net</a> to reflect that this trip is no longer available</p>'
           ,1
		   ,GetDate()
		   ,1519180
		   ,1519180
		   ,1
		   ,1)

SET IDENTITY_INSERT [dbo].dp_communications OFF;
END 


