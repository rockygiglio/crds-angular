USE [MinistryPlatform]
GO

DECLARE @attribute_type int;

SELECT @attribute_type = at.[Attribute_Type_ID] 
	FROM [dbo].[Attribute_Types] at 
	WHERE at.Attribute_Type = N'GO Cincinnati - Equipment'

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Description]
           ,[Attribute_Type_ID]
           ,[Domain_ID])
     VALUES
           (N'Other Equipment'
           ,N'User defined equipment'
           ,@attribute_type
           ,1)
GO