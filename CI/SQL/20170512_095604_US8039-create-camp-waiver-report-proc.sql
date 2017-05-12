USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_summer_camp_waiver_crossroads]    Script Date: 5/12/2017 9:51:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_summer_camp_waiver_crossroads]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_summer_camp_waiver_crossroads] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_summer_camp_waiver_crossroads]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID int
	,@EventID int
	,@ParticipantID int

AS
BEGIN

	DECLARE @Domain_ID int = (SELECT Domain_ID FROM dp_Domains WHERE CAST(Domain_GUID as varchar(40)) = @DomainID)

	SELECT 
		P.Participant_ID
		,C.Display_Name
		,E.Event_Title
		,W.Waiver_Name
		,W.Waiver_Text
		,CW.Display_Name AS Signee_Name
		,PW.Waiver_Create_Date

	FROM 
		cr_Event_Participant_Waivers PW
		JOIN Event_Participants EP ON PW.Event_Participant_ID = EP.Event_Participant_ID
		JOIN Participants P ON EP.Participant_ID = P.Participant_ID
		JOIN Contacts C ON P.Contact_ID = C.Contact_ID
		JOIN Events E ON EP.Event_ID = E.Event_ID
		JOIN cr_Waivers W ON PW.Waiver_ID = W.Waiver_ID
		LEFT JOIN Contacts CW ON PW.Signee_Contact_ID = CW.Contact_ID
	WHERE P.Domain_ID = @Domain_ID
		AND PW.Accepted = 1 
		AND EP.Event_ID = @EventID
		AND EP.Participation_Status_ID = 2 -- Registered
		AND EP.Participant_ID = ISNULL(@ParticipantID,EP.Participant_ID)

	ORDER BY Display_Name

END


GO


