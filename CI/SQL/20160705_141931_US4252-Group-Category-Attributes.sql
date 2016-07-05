Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attributes] ON;

IF NOT EXISTS(Select * from attributes where attribute_name = 'Life Stages' and attribute_type_id = 90)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7097        ,'Life Stages' ,null       ,90               ,null                 ,1        ,null                 ,0         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'Neighborhoods' and attribute_type_id = 90)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name  ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7098        ,'Neighborhoods' ,null       ,90               ,null                 ,1        ,null                 ,1         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'Spritual Growth' and attribute_type_id = 90)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name    ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7099        ,'Spritual Growth' ,null       ,90               ,null                 ,1        ,null                 ,2         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'Interest' and attribute_type_id = 90)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7100        ,'Interest'    ,null       ,90               ,null                 ,1        ,null                 ,3         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'Healing' and attribute_type_id = 90)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7101        ,'Healing'     ,null       ,90               ,null                 ,1        ,null                 ,4         ,null                ,null       ,null          );
END
GO

SET IDENTITY_INSERT [dbo].[Attributes] OFF;