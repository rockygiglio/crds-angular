USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

INSERT INTO [dbo].[Attribute_Types]
           (Attribute_Type_ID
		 ,[Attribute_Type]
           ,[Description]
           ,[Domain_ID]
           ,[Prevent_Multiple_Selection])
     VALUES
           (80,
		 'GO Cincinnati - Registration Children'
           ,0, 1, 0)
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;

