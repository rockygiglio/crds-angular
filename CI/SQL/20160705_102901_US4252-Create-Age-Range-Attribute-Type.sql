Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

IF NOT EXISTS(SELECT * FROM attribute_types WHERE attribute_type = 'Age Range')
BEGIN
INSERT INTO [dbo].[Attribute_Types] 
(Attribute_Type_ID,Attribute_Type,Description            ,Domain_ID,Available_Online,__ExternalAttributeTypeID,Prevent_Multiple_Selection,Online_Sort_Order) VALUES
(89               ,'Age Range'   ,'Age range selection'  ,1        ,1               ,NULL                     ,0                         ,NULL             )
END
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;