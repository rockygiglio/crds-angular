USE [MinistryPlatform]
GO

IF EXISTS (SELECT 1 FROM dp_Pages WHERE Page_ID = 305)
BEGIN
	UPDATE [dbo].[dp_Pages]
	   SET [Default_Field_List] ='Event_ID_Table.Event_Start_Date ,Participant_ID_Table_Contact_ID_Table.Last_Name AS Attendee_Last_Name ,Participant_ID_Table_Contact_ID_Table.Nickname AS Attendee_Nickname ,Participant_ID_Table_Contact_ID_Table.First_Name AS Attendee_First_Name ,Event_ID_Table.Event_Title ,Event_ID_Table_Program_ID_Table.Program_Name ,Participation_Status_ID_Table.Participation_Status ,Event_Participants.Time_In,Event_Participants.End_Date'
	 WHERE Page_ID = 305
END
GO
