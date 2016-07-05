Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Categories] ON;

IF NOT EXISTS(Select * from attribute_categories where Attribute_Category = 'Life Stages')
BEGIN
INSERT INTO [dbo].[Attribute_Categories]
(Attribute_Category_ID,Attribute_Category,Description,Available_Online,Domain_ID,__ExternalSkillAreaID) VALUES
(17                   ,'Life Stages'     ,null       ,0               ,1        ,null                   );
END
GO

IF NOT EXISTS(Select * from attribute_categories where Attribute_Category = 'Neighborhoods')
BEGIN
INSERT INTO [dbo].[Attribute_Categories] 
(Attribute_Category_ID,Attribute_Category,Description,Available_Online,Domain_ID,__ExternalSkillAreaID) VALUES
(18                   ,'Neighborhoods'   ,null       ,0               ,1        ,null                   );
END
GO

IF NOT EXISTS(Select * from attribute_categories where Attribute_Category = 'Spritual Growth')
BEGIN
INSERT INTO [dbo].[Attribute_Categories] 
(Attribute_Category_ID,Attribute_Category,Description,Available_Online,Domain_ID,__ExternalSkillAreaID) VALUES
(19                   ,'Spiritual Growth',null       ,0               ,1        ,null                   );
END
GO

IF NOT EXISTS(Select * from attribute_categories where Attribute_Category = 'Interest')
BEGIN
INSERT INTO [dbo].[Attribute_Categories] 
(Attribute_Category_ID,Attribute_Category,Description,Available_Online,Domain_ID,__ExternalSkillAreaID) VALUES
(20                   ,'Interest'        ,null       ,0               ,1        ,null                   );
END
GO

IF NOT EXISTS(Select * from attribute_categories where Attribute_Category = 'Healing')
BEGIN
INSERT INTO [dbo].[Attribute_Categories] 
(Attribute_Category_ID,Attribute_Category,Description,Available_Online,Domain_ID,__ExternalSkillAreaID) VALUES
(21                   ,'Healing'         ,null       ,0               ,1        ,null                   );
END
GO

SET IDENTITY_INSERT [dbo].[Attribute_Categories] OFF;