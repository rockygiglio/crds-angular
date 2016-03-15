USE [MinistryPlatform]
GO

DECLARE @attr_type int;

SELECT @attr_type = Attribute_Type_ID from Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration Children';

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
     VALUES
           ('Ages 2-7'
           ,@attr_type
           ,1
           ,1)

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
     VALUES
           ('Ages 8-12'
           ,@attr_type
           ,1
           ,2)

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
     VALUES
           ('Ages 13-17'
           ,@attr_type
           ,1
           ,3)

GO