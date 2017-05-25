USE [MinistryPlatform]
GO

declare @registration_page int;

SELECT @registration_page = p.Page_ID FROM
	[dbo].[dp_Pages] p WHERE p.Display_Name = N'Registrations'

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Display_Name]
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
           (N'Equipment'
           ,N'Equipment'
           ,@registration_page
           ,1
           ,N'cr_Registration_Equipment_Attributes'
           ,N'Registration_Equipment_ID'
           ,N'Attribute_ID_Table.[Attribute_Name],Notes'
           ,N'Attribute_ID_Table.[Attribute_Name]'
           ,N'Registration_ID'
           ,2           
           ,1)
GO


