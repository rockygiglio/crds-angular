USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 16)
BEGIN
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
		(16,
           'Registrations',
           'Registration',
           'A volunteer registration.',
           100,
           'cr_Registrations',
           'Registration_ID',
           'Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Display Name],
			 Role_ID_Table.[Role_Title] AS [Role Title],
			 Organization_ID_Table.[Name] AS [Organization Name],
			 Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred Launch Site],
			 cr_Registrations.[Spouse_Participation] AS [Spouse Participation],
			 cr_Registrations.[Additional_Information] AS [Additional Information],
			 Initiative_ID_Table.[Initiative_Name] AS [Initiative Name]',
           'Participant_ID_Table_Contact_ID_Table.[Display_Name]',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID],[Page_Section_ID])
     VALUES (16,21)
END
