Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attributes] ON;

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = 'Middle School Students' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name          ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7089        ,'Middle School Students',NULL       ,89               ,NULL                 ,1        ,NULL                 ,0         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = 'High School Students' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name        ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7090        ,'High School Students',NULL       ,89               ,NULL                 ,1        ,NULL                 ,1         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = 'College Students' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name    ,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7091        ,'College Students',NULL       ,89               ,NULL                 ,1        ,NULL                 ,2         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = '20s' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7092        ,'20s'         ,NULL       ,89               ,NULL                   ,1        ,NULL                 ,3         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = '30s' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7093        ,'30s'         ,NULL       ,89               ,NULL                 ,1        ,NULL                 ,4         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = '40s' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7094        ,'40s'         ,NULL       ,89               ,NULL                 ,1        ,NULL                 ,5         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = '50s' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7095        ,'50s'         ,NULL       ,89               ,NULL                 ,1        ,NULL                 ,6         ,NULL                ,NULL       ,NULL          );
END
GO

IF NOT EXISTS(SELECT * FROM attributes WHERE attribute_name = '60s+' AND attribute_type_id = 89)
BEGIN
INSERT INTO [dbo].[Attributes] 
(Attribute_ID,Attribute_Name,Description,Attribute_Type_ID,Attribute_Category_ID,Domain_ID,__ExternalAttributeID,Sort_Order,__SpiritualJourneyID,__AirlineID,__ProfessionID) VALUES
(7096        ,'60s+'         ,NULL       ,89              ,NULL                 ,1        ,NULL                 ,7         ,NULL                ,NULL       ,NULL          );
END
GO

SET IDENTITY_INSERT [dbo].[Attributes] OFF;