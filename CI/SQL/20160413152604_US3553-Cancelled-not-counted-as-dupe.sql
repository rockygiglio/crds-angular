USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET View_Clause = 'ISNULL(cr_Registrations.Cancelled, 0) != 1 AND Participant_ID_Table_Contact_ID_Table.Email_Address IN (SELECT Email_Address FROM dbo.cr_Registrations AS R JOIN dbo.Participants AS P ON R.Participant_ID = P.Participant_ID JOIN dbo.Contacts AS C ON C.Contact_ID = P.Contact_ID WHERE ISNULL(R.Cancelled, 0) != 1 GROUP BY Email_Address HAVING Count(1) > 1)'
WHERE Page_View_ID = 2220