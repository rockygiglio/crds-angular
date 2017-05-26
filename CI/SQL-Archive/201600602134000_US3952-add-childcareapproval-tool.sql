USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Tools]
           ([Tool_Name]
           ,[Description]
           ,[Launch_Page]
           ,[Launch_with_Credentials])
     VALUES
           ('Childcare Decision Tool'
           ,'Approve or Reject childcare requests'
           ,'/CRDStools/childcaredecision'
           ,1)
GO

INSERT INTO [dbo].[dp_Tool_Pages]
           ([Tool_ID]
           ,[Page_ID])
     VALUES
           ((SELECT Tool_ID FROM dp_Tools WHERE Tool_Name = 'Childcare Decision Tool')
           ,(SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'Childcare Requests'))
GO


