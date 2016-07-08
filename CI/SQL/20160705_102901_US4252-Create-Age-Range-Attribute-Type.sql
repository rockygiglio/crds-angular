Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

IF NOT EXISTS(SELECT * FROM attribute_types WHERE attribute_type = 'Age Range')
BEGIN
INSERT INTO [dbo].[Attribute_Types] 
(Attribute_Type_ID,Attribute_Type,Description            ,Domain_ID,Available_Online,Prevent_Multiple_Selection) VALUES
(89               ,'Age Range'   ,'Age range selection'  ,1        ,1               ,0                         );
END
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;