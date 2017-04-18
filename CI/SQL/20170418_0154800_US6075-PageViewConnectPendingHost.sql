USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Page_Views ON

IF NOT EXISTS (SELECT * FROM dp_Page_Views WHERE Page_View_ID = 1119)
BEGIN
   INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
		   )
        VALUES
           (1119
		   , 'Connect - Pending Hosts'
           , 355 
           ,'Connect hosts awaiting apporval from staff'
           ,'Contact_ID_Table.[Last_Name] AS [Last Name]
              , Contact_ID_Table.[First_Name] AS [First Name]
              , Contact_ID_Table.[Email_Address] AS [Email Address]
              , Host_Status_ID_Table.[Description] AS [Description]'
           ,'Host_Status_ID_Table.[Host_Status_ID] = ''1''')
END
SET IDENTITY_INSERT dp_Page_Views OFF
GO
