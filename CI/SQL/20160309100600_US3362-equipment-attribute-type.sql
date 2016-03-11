USE [MinistryPlatform]
GO

INSERT INTO [dbo].[Attribute_Types]
           ([Attribute_Type]
           ,[Description]
           ,[Domain_ID]
           ,[Available_Online])
     VALUES
           (
		    N'GO Cincinnati - Equipment'
           ,N'Equipment that a volunteer has'
           ,1
           ,1
		   )
GO