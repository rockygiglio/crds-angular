USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_filter_camp_participants_crossroads]    Script Date: 5/12/2017 10:19:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_filter_camp_participants_crossroads]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_filter_camp_participants_crossroads] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_filter_camp_participants_crossroads]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID int
	,@EventID int

AS
BEGIN

	DECLARE @Domain_ID int = (SELECT Domain_ID FROM dp_Domains WHERE CAST(Domain_GUID as varchar(40)) = @DomainID)

	SELECT P.Participant_ID, C.Display_Name
	FROM cr_Event_Participant_Waivers W
		JOIN Event_Participants EP ON W.Event_Participant_ID = EP.Event_Participant_ID
		JOIN Participants P ON EP.Participant_ID = P.Participant_ID
		JOIN Contacts C ON P.Contact_ID = C.Contact_ID
	WHERE P.Domain_ID = @Domain_ID
		AND W.Accepted = 1 
		AND EP.Event_ID = @EventID
		AND EP.Participation_Status_ID = 2 -- Registered

	UNION 

	SELECT NULL AS Participant_ID, '*All Participants' AS Display_Name
		WHERE EXISTS(SELECT 1 FROM Events WHERE Events.Event_ID = @EventID) 

	ORDER BY Display_Name

END


GO


