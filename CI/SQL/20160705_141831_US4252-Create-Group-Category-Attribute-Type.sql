Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;

IF NOT EXISTS(Select * from attribute_types where attribute_type = 'Group Focus')
BEGIN
INSERT INTO [dbo].[Attribute_Types] 
(Attribute_Type_ID,Attribute_Type   ,Description                       ,Domain_ID,Available_Online,__ExternalAttributeTypeID,Prevent_Multiple_Selection,Online_Sort_Order) VALUES
(90               ,'Group Category' ,'What is the focus of your group' ,1        ,1               ,null                     ,0                         ,null             )
END
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;