USE MinistryPlatform
GO

SET IDENTITY_INSERT dp_Sub_Page_Views ON

IF NOT EXISTS(SELECT * FROM dp_Sub_Page_Views WHERE Sub_Page_View_ID=204 AND View_Title='Current with ContactId')
BEGIN
INSERT INTO dp_Sub_Page_Views(Sub_Page_View_ID, View_Title, Sub_Page_ID, Field_List, View_Clause)
	VALUES(204,
		 'Current with ContactId',
		   298,
		 'Participant_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact_ID] , Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First_Name] , Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last_Name] , Participant_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname] , Group_Role_ID_Table.[Group_Role_ID] AS [Group_Role_ID] , Group_Role_ID_Table.[Role_Title] AS [Role_Title] , Participant_ID_Table.[Participant_ID] AS [Participant_ID],Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [Email]',
		 'GetDate() BETWEEN CONVERT(DATE,Group_Participants.Start_Date) AND ISNULL(Group_Participants.End_Date,GetDate())'
		 )
END

SET IDENTITY_INSERT dp_Sub_Page_Views OFF
GO

