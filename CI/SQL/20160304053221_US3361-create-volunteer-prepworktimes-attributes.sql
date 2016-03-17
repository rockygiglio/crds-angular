USE [MinistryPlatform]
GO

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
           
     VALUES
           ('Preparation Work 12PM - 3PM'
           ,(SELECT Attribute_Type_ID FROM Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration PrepWork')
           ,1
           ,1);
GO

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
           
     VALUES
           ('Preparation Work 12PM - 6PM'
           ,(SELECT Attribute_Type_ID FROM Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration PrepWork')
           ,1
           ,1);
GO

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
           
     VALUES
           ('Preparation Work 3PM - 6PM'
           ,(SELECT Attribute_Type_ID FROM Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration PrepWork')
           ,1
           ,1);
GO

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
           
     VALUES
           ('Preparation Work 6PM - 9PM'
           ,(SELECT Attribute_Type_ID FROM Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration PrepWork')
           ,1
           ,1);
GO

INSERT INTO [dbo].[Attributes]
           ([Attribute_Name]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
           
     VALUES
           ('Preparation Work 3PM - 9PM'
           ,(SELECT Attribute_Type_ID FROM Attribute_Types WHERE Attribute_Type = 'GO Cincinnati - Registration PrepWork')
           ,1
           ,1);
GO


