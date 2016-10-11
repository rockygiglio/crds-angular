USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Add_As_CampParticipant]    Script Date: 10/11/2016 10:13:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[api_crds_Add_As_CampParticipant]
    @EventID int,
	@ContactID int
AS
BEGIN
    DECLARE @ParticipantID INT;
    SELECT @ParticipantID = Participant_Record FROM Contacts WHERE Contact_ID = @ContactID;

	IF NOT EXISTS ( SELECT 1 FROM [dbo].[Event_Participants] 
						WHERE [Participant_ID] = @ParticipantID 
						AND [Event_ID] = @EventID)				
			BEGIN
				PRINT 'Not a participant of the event yet';
				INSERT INTO [dbo].[Event_Participants] (
					[Event_ID],
					[Participant_ID],
					[Participation_Status_ID],
					[Domain_ID]
				) VALUES (
					@EventID,
					@ParticipantID,
					2,
					1
				)	 
			END
	SELECT SCOPE_IDENTITY() AS Record_ID
			
END
GO


