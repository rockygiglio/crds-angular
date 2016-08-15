USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Add_As_TripParticipant]    Script Date: 8/15/2016 11:07:21 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Add_As_TripParticipant]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_Add_As_TripParticipant]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Add_As_TripParticipant]    Script Date: 8/15/2016 11:07:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Add_As_TripParticipant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Add_As_TripParticipant] AS' 
END
GO

-- =============================================
-- Author:      Matt Silbernagel
-- Create date: 08/15/2016
-- Description: Add the given contact as a participant 
--				on a the given GO Trip (PledgeCampaign)
--
--				This means create a Pledge, Donor (if it doesn't already exist)
--				and adding as a participant in any Groups and Events linked to the Pledge
--				Campaign. Also adding Documents to the event participant record. 
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_Add_As_TripParticipant]
    @PledgeCampaignID int,
	@ContactID int
AS
BEGIN

	DECLARE @GroupID int;
	DECLARE @ParticipantID int;
	DECLARE @DonorID int = 0;
	DECLARE @EventID int;
	DECLARE @DestinationID int;
	DECLARE @FundraisingGoal money;
	DECLARE @GroupRoleID int = 16;
	DECLARE @Current_Event_ID int

	DECLARE @EVENTS TABLE (
		EventID int
	);
	
	-- GET THE PLEDGE CAMPAIGN DETAILS
	SELECT TOP 1 
		   @DestinationID = pg.[Destination_ID], 
		   @FundraisingGoal = pg.[Fundraising_Goal],
		   @GroupID = ev.Group_ID,
		   @EventID = pg.Event_ID
	FROM [dbo].[Pledge_Campaigns] pg
	JOIN  [dbo].[Event_Groups] ev on ev.Event_ID = pg.Event_ID
	WHERE [Pledge_Campaign_ID] = @PledgeCampaignID;

	SELECT @ParticipantID = Participant_Record, @DonorID = Donor_Record FROM Contacts WHERE Contact_ID = @ContactID;
	-- END PLEDGE CAMPAIGN DETAILS

	-- CREATE GROUP PARTICIPANT RECORD IF IT DOESN'T EXIST
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Group_Participants] 
				   WHERE [Participant_ID] = @ParticipantID
				   AND [Group_ID] = @GroupID
				   AND ( [End_Date] is null OR [End_Date] >= GETDATE()))
	BEGIN
		PRINT 'Adding as a group participant'
		INSERT INTO [dbo].[Group_Participants] (
			Group_ID,
			Participant_ID,
			Group_Role_ID,
			Domain_ID,
			Start_Date			
		) VALUES ( 
			@GroupID,
			@ParticipantID,
			@GroupRoleID,
			1,
			GETDATE()
		)
	END
	-- END GROUP PARTICIPANT

	-- CREATE DONOR RECORD IF IT DOESN'T EXIST
	IF NOT EXISTS ( SELECT 1 WHERE @DonorID > 0 )
	BEGIN
		PRINT 'Donor does not exist yet...';
		INSERT INTO [dbo].[Donors] (
			Contact_ID,
			Statement_Frequency_ID,
			Statement_Type_ID,
			Statement_Method_ID,
			Setup_Date,
			Domain_ID
		) VALUES (
			@ContactID,
			1,
			1,
			2,
			GETDATE(),
			1
		)
		SELECT @DonorID = @@IDENTITY; 
	END

	-- CREATE PLEDGE RECORD IF IT DOESN'T EXIST
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Pledges] 
				   WHERE [Pledge_Campaign_ID] = @PledgeCampaignID 
				   AND [Donor_ID] = @DonorID)
	BEGIN
		PRINT 'Pledge does not exist yet';
		INSERT INTO [dbo].[Pledges] (
			[Donor_ID],
			[Pledge_Campaign_ID],
			[Pledge_Status_ID],
			[Total_Pledge],
			[Installments_Planned],
			[Installments_Per_Year],
			[First_Installment_Date],
			[Domain_ID]
		) VALUES (
			@DonorID,
			@PledgeCampaignID,
			1,
			@FundraisingGoal,
			0,
			0,
			GETDATE(),
			1
		)	
	END
	-- END PLEDGE

	-- ADD AS EVENT PARTICIPANT FOR EACH GROUP EVENT
	INSERT INTO @EVENTS SELECT Event_ID FROM [dbo].[Event_Groups] WHERE Group_ID = @GroupID;

	DECLARE MC CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
	SELECT EventID from @EVENTS

	OPEN MC
	FETCH NEXT FROM MC INTO @CURRENT_EVENT_ID
	WHILE @@FETCH_STATUS = 0
	BEGIN	
		IF NOT EXISTS ( SELECT 1 FROM [dbo].[Event_Participants] 
					WHERE [Participant_ID] = @ParticipantID 
					AND [Event_ID] = @CURRENT_EVENT_ID)				
		BEGIN
			PRINT 'Not a participant of the event yet';
			INSERT INTO [dbo].[Event_Participants] (
				[Event_ID],
				[Participant_ID],
				[Participation_Status_ID],
				[Domain_ID]
			) VALUES (
				@CURRENT_EVENT_ID,
				@ParticipantID,
				2,
				1
			)	 
		END
		FETCH NEXT FROM MC INTO @CURRENT_EVENT_ID
	END
	CLOSE MC;
	DEALLOCATE MC;
	-- END EVENT PARTICIPANT


	-- ADD DOCUMENTS TO THE EVENT PARTICIPANT OF THE MAIN EVENT
	DECLARE @EVENT_PARTICPANT int;
	SELECT @EVENT_PARTICPANT = Event_Participant_ID FROM  [dbo].[Event_Participants] ev 
	WHERE ev.Event_ID = @EventID 
	AND ev.Participant_ID = @ParticipantID

	EXECUTE crds_AddDocumentsToParticipant @DestinationID, @EVENT_PARTICPANT;
	-- END DOCUMENTS
END