USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2220)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2220
		   ,'Duplicate Registrations'
           ,16
           ,'Registrations with the same Email Address'
           ,'Participant_ID_Table_Contact_ID_Table.[Display_Name], Participant_ID_Table_Contact_ID_Table.[Email_Address]
, Initiative_ID_Table.[Initiative_Name], [dp_Created].[Date_Time] AS [Registration Date], Organization_ID_Table.[Name] AS [Organization]
, Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred Launch Site], cr_Registrations.[_Family_Count], cr_Registrations.[Cancelled]'
           ,'ISNULL(cr_Registrations.Cancelled, 0) != 1 AND Participant_ID_Table_Contact_ID_Table.Email_Address IN (SELECT Email_Address FROM dbo.cr_Registrations AS R JOIN dbo.Participants AS P ON R.Participant_ID = P.Participant_ID JOIN dbo.Contacts AS C ON C.Contact_ID = P.Contact_ID GROUP BY Email_Address HAVING Count(1) > 1)')
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END