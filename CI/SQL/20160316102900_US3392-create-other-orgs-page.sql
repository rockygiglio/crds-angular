USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (
			 N'Other Organizations'
			,N'Other Organization'
			,N'List of organizations that are not in the organization table'
			,1
			,N'cr_Other_Organizations'
			,N'Other_Organization_ID'
			,N'cr_Other_Organizations.Other_Organization'
			,N'cr_Other_Organizations.Other_Organization'
			,0
		   )
GO


