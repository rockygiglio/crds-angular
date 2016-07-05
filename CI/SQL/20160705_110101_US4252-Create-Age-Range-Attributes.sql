Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attributes] ON;

IF NOT EXISTS(Select * from attributes where attribute_name = 'Middle School Students' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name          ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7089        ,'Middle School Students',null       ,89               ,null                 ,1        ,null                 ,0         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'High School Students' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name        ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7090        ,'High School Students',null       ,89               ,null                 ,1        ,null                 ,1         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = 'College Students' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name    ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7091        ,'College Students',null       ,89               ,null                 ,1        ,null                 ,2         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = '20s' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7092        ,'20s'         ,null       ,89               ,null                   ,1        ,null                 ,3         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = '30s' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7093        ,'30s'         ,null       ,89               ,null                 ,1        ,null                 ,4         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = '40s' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7094        ,'40s'         ,null       ,89               ,null                 ,1        ,null                 ,5         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = '50s' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7095        ,'50s'         ,null       ,89               ,null                 ,1        ,null                 ,6         ,null                ,null       ,null          );
END
GO

IF NOT EXISTS(Select * from attributes where attribute_name = '60s+' and attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7096        ,'60s+'         ,null       ,89              ,null                 ,1        ,null                 ,7         ,null                ,null       ,null          );
END
GO

SET IDENTITY_INSERT [dbo].[Attributes] OFF;