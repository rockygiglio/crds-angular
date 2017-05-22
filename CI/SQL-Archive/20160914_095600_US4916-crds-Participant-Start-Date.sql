USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>, Jim Kriz <jim.kriz@ingagepartners.com>
-- Create date: 9/14/2016
-- Description:	Gets participant joined date data for CRDS Groups Summary Report
-- ===============================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Participant_Start_Date]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Participant_Start_Date] AS' 
END
GO

ALTER PROCEDURE [dbo].[crds_Participant_Start_Date]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@GTID Int  = NULL -- Group type id
	,@RepDate DATE

AS
BEGIN


DECLARE @StartDate DATE; -- This will be calculated
DECLARE @NumWeeksBack INT = -12; -- How many weeks back will we look

-- Calculate @NumWeeksBack weeks from the end date selected to get to a start date
SET @StartDate = DATEADD(ww, @NumWeeksBack, CONVERT(date, DATEADD(dd, @@DATEFIRST - DATEPART(dw, @RepDate) - 6, @RepDate)));
-- Get to the end of whatever week was chosen as the ending date
SET @RepDate = CONVERT(date, DATEADD(dd, @@DATEFIRST - DATEPART(dw, @RepDate) - 6, @RepDate))

	-- @@DATEFIRST defaults to SUNDAY
	SELECT
		DATEPART(ww, p.Start_Date) AS WeekNumber
        , CONVERT(date, DATEADD(dd, @@DATEFIRST - DATEPART(dw, p.Start_Date) - 6, p.Start_Date)) AS WeekEnding
        , COUNT(*) AS ParticipantsJoined
	FROM 
			Groups g
			, Group_Participants p
	WHERE
			p.Group_ID = g.Group_ID
			AND g.Group_Type_ID = @GTID
			AND g.End_Date IS NULL
			AND CONVERT(date, p.Start_Date) BETWEEN @StartDate AND @RepDate
	GROUP BY
	        DATEPART(ww, p.Start_Date)
	        , CONVERT(date, DATEADD(dd, @@DATEFIRST - DATEPART(dw, p.Start_Date) - 6, p.Start_Date))
	ORDER BY
	        1

END

GO


