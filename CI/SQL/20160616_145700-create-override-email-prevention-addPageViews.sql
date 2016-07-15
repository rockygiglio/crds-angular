USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID]
           ,[User_Group_ID])
     VALUES
           (2306,
		   'With Name and Email',
		   558,
		   NULL,
		   'Contact_ID_Table.[Display_Name] AS [Display Name],
           Contact_ID_Table.[Email_Address] AS [Email Address]',
           'Contact_ID_Table.[Display_Name] IS NOT NULL',
		   NULL,
		   NULL,
		   NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

UPDATE [dbo].[dp_Pages]
set [Default_View] = 2306
where Page_ID = 558
		   
GO


