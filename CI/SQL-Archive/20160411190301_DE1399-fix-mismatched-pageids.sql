USE MinistryPlatform
GO

DECLARE @SubPageId INT;

select @SubPageId = Sub_Page_ID from dbo.dp_Sub_Pages where Display_Name = 'Equipment' AND Primary_Table = 'cr_Registration_Equipment_Attributes';

DELETE FROM dp_Role_Sub_Pages where Sub_Page_ID = @SubPageId;
delete FROM [MinistryPlatform].[dbo].[dp_Sub_Pages] WHERE Display_Name = 'Equipment' AND Primary_Table = 'cr_Registration_Equipment_Attributes';
delete FROM [MinistryPlatform].[dbo].[dp_Sub_Pages] WHERE Sub_Page_ID = @SubPageId;
delete FROM [MinistryPlatform].[dbo].[dp_Sub_Pages] WHERE Sub_Page_ID = 6;

  SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           (6
		   ,'Equipment'
           ,'Equipment'
           ,16
           ,1
           ,'cr_Registration_Equipment_Attributes'
           ,'Registration_Equipment_ID'
           ,'Attribute_ID_Table.[Attribute_Name],Notes'
           ,'Attribute_ID_Table.[Attribute_Name]'
           ,'Registration_ID'
           ,2
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF

INSERT INTO [dbo].[dp_Role_Sub_Pages] ([Role_ID], [Sub_Page_ID],[Access_Level]) VALUES (62, @SubPageId, 3);
INSERT INTO [dbo].[dp_Role_Sub_Pages] ([Role_ID], [Sub_Page_ID],[Access_Level]) VALUES (107, @SubPageId, 3);
