USE [MinistryPlatform];

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON;

INSERT INTO [dbo].[dp_Sub_Page_Views](
	 [Sub_Page_View_ID]
	,[View_Title]
	,[Sub_Page_ID]
	,[Description]
	,[Field_List]
	,[View_Clause]
	,[Order_By]
	,[User_ID]
) VALUES (
	 185
	,'AssignedToRoomApi'
	,281
	,'Get event participants with room assignment'
	,'Event_Participants.[Event_Participant_ID]
, Participant_ID_Table.[Participant_ID]
, Participation_Status_ID_Table.[Participation_Status_ID]
, Room_ID_Table.[Room_ID]
'
	,'Room_ID_Table.[Room_ID] IS NOT NULL AND
(Participation_Status_ID_Table.[Participation_Status_ID] = 3 OR
Participation_Status_ID_Table.[Participation_Status_ID] = 4)'
	,NULL
	,NULL);
	
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF;
