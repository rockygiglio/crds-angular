USE [MinistryPlatform]
GO

DECLARE @PageId int = 627

IF NOT EXISTS(SELECT 1 FROM dp_Pages WHERE Page_ID = @PageId)
BEGIN 

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (@PageId
		   ,'Group Connector Registrations'
           ,'Group Connector Registration'
           ,'Everybody signed up under a group connector'
           ,100
           ,'cr_Group_Connector_Registrations'
           ,'cr_Group_Connector_Registration_ID'
           ,'Group_Connector_Registration_ID, Group_Connector_ID, Registration_ID'
           ,'cr_Group_Connector_Registration_ID'
           ,0)
SET IDENTITY_INSERT [dbo].[dp_Pages] OFF

END


