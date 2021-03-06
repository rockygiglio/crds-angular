USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * 
                 FROM [dbo].[Group_Types]
                 WHERE Group_Type = N'Anywhere Gathering'
                 )
BEGIN
  INSERT INTO [dbo].[Group_Types]
		  ( [Group_Type]
		  ,[Description]
		  ,[Domain_ID]
		  ,[Default_Role])
      VALUES
		  (N'Anywhere Gathering'
		  ,N'Groups created, administered, and led by an approved Host in the Anywhere community'
		  ,1
		  ,16);
END



