USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Summary]    Script Date: 9/13/2016 3:26:19 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Childcare_Summary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[report_CRDS_Childcare_Summary]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Summary]    Script Date: 9/13/2016 3:26:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Childcare_Summary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Childcare_Summary] AS' 
END
GO



ALTER PROCEDURE [dbo].[report_CRDS_Childcare_Summary]

      @StartDate DATETIME,
	  @EndDate DATETIME,
	  @CongregationId INT

AS
    BEGIN
        SET NOCOUNT ON;
		SET @StartDate =  DATEADD(day, DATEDIFF(day, 0, @StartDate), '00:00:00');
		SET @EndDate =  DATEADD(day, DATEDIFF(day, 0, @EndDate), '23:59:00');
		
		DECLARE @ChildcareSummary TABLE
		(
			Group_Name varchar(255),
			EventDate datetime,
			StartTime datetime, 
			EndTime datetime,
			Age int,
			RSVP int,
			Checkin int
		)

		INSERT INTO @ChildcareSummary
		SELECT  g.Group_Name Group_Name,     
						e.Event_Start_Date  EventDate, 
						e.Event_Start_Date  StartTime, 
						e.Event_End_Date EndTime, 
						c.__Age Age, 
						1 AS 'RSVP',
						IIF(er.ID IS NOT NULL, 1, 0) AS 'Checkin'
				FROM dbo.Events e
				JOIN Event_Groups eg ON e.Event_ID = eg.Event_ID


				JOIN Group_Participants childgp ON childgp.Group_ID = eg.Group_ID 

				JOIN Group_Participants parentgp ON parentgp.Group_Participant_ID = childgp.Enrolled_By
				JOIN Groups g ON g.Group_ID = parentgp.Group_ID
				JOIN Participants p ON p.Participant_ID = childgp.Participant_ID
				JOIN Contacts c on c.Contact_ID = p.Contact_ID
				LEFT JOIN cr_Echeck_Registrations er ON er.Child_ID = c.Contact_ID AND CONVERT(date, er.Checkin_Date) = CONVERT(date, e.Event_Start_Date) AND CONVERT(time, e.Event_Start_Date) = CONVERT(time, er.Service_Time)
				WHERE e.Event_Type_ID = 243
					AND e.Event_Start_Date BETWEEN @StartDate AND @EndDate
					AND e.Congregation_ID = @CongregationId
					AND childgp.End_Date is null

		INSERT INTO @ChildcareSummary
				SELECT '' AS Group_Name, 
					e.Event_Start_Date AS EventDate, 
					e.Event_Start_Date AS StartTime, 
					e.Event_End_Date AS EndTime, 
					c.__Age AS AGE, 
					0 AS 'RSVP', 
					1 AS 'Checkin' 
				FROM dbo.cr_Echeck_Registrations
				JOIN Contacts c ON c.Contact_ID = Child_ID
				JOIN Events e ON CONVERT(date, Checkin_Date) = CONVERT(date, e.Event_Start_Date) AND CONVERT(time, e.Event_Start_Date) = CONVERT(time, Service_Time) AND e.Cancelled != 1
				JOIN Congregations con ON con.Congregation_ID = e.Congregation_ID 
				WHERE e.Event_Type_ID = 243 
					AND Service_Day != 'Saturday'
					AND Service_Day != 'Sunday'
					AND e.Event_Start_Date BETWEEN @StartDate AND @EndDate
					AND e.Congregation_ID = @CongregationId
					AND con.Congregation_Name = Building_Name
					AND Child_ID NOT IN (SELECT Contact_ID FROM Group_Participants gp
						JOIN Event_Groups eg ON gp.Group_ID = eg.Group_ID
						JOIN Groups g ON g.Group_ID = gp.Group_ID
						JOIN Participants p ON gp.Participant_ID = p.Participant_ID
						WHERE g.Group_Type_ID = 27
						AND gp.End_Date IS NULL
						AND eg.Event_ID = e.Event_ID)

		SELECT Group_Name,
			EventDate,
			StartTime, 
			EndTime,
			Age,
			sum(RSVP) AS 'Total RSVP',
			sum(Checkin) AS 'Total Check In'
		FROM @ChildcareSummary
		GROUP BY Group_Name, EventDate, StartTime, EndTime, Age
		ORDER BY EventDate,Group_name, Age
    END;



GO


