USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Turned_Away_Summary]   Script Date: 6/23/2017 8:43:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author: John Cleaver	
-- Create date: 6/01/2017
-- Description:	Create [report_CRDS_Turned_Away_Summary] proc to show number
-- of kids by group that were not able to check into a KC event
-- ===============================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Turned_Away_Summary]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Turned_Away_Summary] AS' 
END
GO

ALTER PROCEDURE [dbo].report_CRDS_Turned_Away_Summary

      @StartDate DATETIME,
	  @EndDate DATETIME,
	  @EventCongregations NVARCHAR(1000)
AS
    SELECT
	-- get the count of participants for that group line
	(SELECT COUNT(*) FROM (SELECT DISTINCT s_ep.Participant_ID FROM event_participants s_ep
				WHERE s_ep.participation_status_id IN (6, 7)
				AND s_ep.Group_ID = g.Group_ID
				AND s_ep.Event_ID = e.Event_ID
				AND NOT EXISTS (
					SELECT * FROM Event_Participants s2_ep WHERE s2_ep.Participant_ID = s_ep.Participant_ID
					AND s2_ep.Participation_Status_ID IN (3, 4, 5) 
					AND s2_ep.Event_ID = s_ep.Event_ID) 
				GROUP BY Participant_ID) AS items) AS 'Turned_Away_In_Group',
	e.Event_ID, 
	e.Event_Start_Date, 
	e.Event_Title,
	g.Group_Name,
	g.Group_ID,
	ep.Participation_Status_ID

	-- Get groups from an event where there are any turned away participants
	FROM [Events] e INNER JOIN Event_Participants ep ON e.Event_ID = ep.Event_ID
		INNER JOIN Groups g ON ep.Group_ID = g.Group_ID
	WHERE e.Congregation_ID IN (SELECT * FROM dbo.fnSplitString(@EventCongregations,','))
	AND CONVERT(date, e.Event_Start_Date) >= @StartDate
	AND CONVERT(date, e.Event_Start_Date) <= @EndDate
	AND ep.Participation_Status_ID IN (6, 7)
	AND NOT EXISTS (SELECT * FROM Event_Participants s_ep WHERE s_ep.Participant_ID = ep.Participant_ID
		AND s_ep.Participation_Status_ID IN (3, 4, 5) AND s_ep.Event_ID = e.Event_ID)
	AND e.[Allow_Check-in] = 1
	AND e.Event_Type_ID != 243
	Group by e.Event_Id, e.Event_Start_Date, e.Event_Title, g.Group_Name, ep.Participation_Status_ID, g.Group_ID
	ORDER BY e.Event_Start_Date, g.Group_Name, ep.Participation_Status_ID
GO



