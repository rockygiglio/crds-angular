USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Checkin_Guest_Families]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Checkin_Guest_Families] AS' 
END
GO
-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-03-28
-- Description:	Creates report_CRDS_Checkin_Guest_Families to show new family
-- registrations for checkins
-- =============================================
ALTER PROCEDURE [dbo].report_CRDS_Checkin_Guest_Families 
	@StartDate DATETIME
  , @EndDate DATETIME
  , @EventCongregations NVARCHAR(1000)
  , @EventType INT
AS
BEGIN
	SELECT 
		E.Event_Title AS "Event Title",
		E.Event_Start_Date,
		H.Household_Name AS "Household Last Name"
		, H.Home_Phone AS "Household Phone"
		, (SELECT COUNT(s_c.Contact_Id) FROM contacts s_c WHERE s_c.household_position_id=2 and
			s_c.Household_Id = H.Household_ID) AS "Total Household Children"
		, COUNT (EP.Participant_ID) AS "Total Children"
		, CONVERT(time, MIN(P.Participant_Start_Date)) AS "Time Created"
		, COUNT(DISTINCT H.Household_Name) AS "Total Households"
	FROM
			Events E INNER JOIN Event_Participants EP ON E.Event_ID = EP.Event_ID
			INNER JOIN Participants P ON EP.Participant_ID = P.Participant_ID
			INNER JOIN Contacts C ON C.Contact_ID = P.Contact_ID
			-- C.Household_ID would show "Guest Household" for every household in the report
			INNER JOIN Households H ON H.Household_ID = EP.Checkin_Household_ID
	WHERE
			--C.Household_Id = 5771805 -- Guest checkin household
			EP.Checkin_Household_ID != C.Household_ID
			AND EXISTS (SELECT * FROM Event_Participants s_EP WHERE EP.Guest_Sign_In = 1)
			AND E.Congregation_ID IN (SELECT * FROM dbo.fnSplitString(@EventCongregations,','))
			AND CONVERT(date, E.Event_Start_Date) >= @StartDate
			AND CONVERT(date, E.Event_Start_Date) <= @EndDate
			-- This is a little brittle, but this proc is backing a report for only two
			-- kinds of events
			AND ((@EventType = 1 AND E.Event_Type_ID NOT IN (369, 402, 403)) OR
					(@EventType = 2 AND E.Event_Type_ID IN (369, 402, 403))) 
	GROUP BY
			E.Event_ID,
			E.Event_Title,
			E.Event_Start_Date,
			H.Household_Name
			, H.Home_Phone
			, H.Household_ID
END