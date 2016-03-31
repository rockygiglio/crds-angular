USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Sub_Pages] WHERE Sub_Page_ID = 5 )
BEGIN
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
           (5
		   ,'Locations'
           ,'Location'
           ,10
           ,5
           ,'cr_Organization_Locations'
           ,'Organization_Location_ID'
           ,'Location_ID_Table.[Location_Name]'
           ,'Location_ID_Table.[Location_Name]'
           ,'Organization_ID'
           ,2
           ,1)

	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
END