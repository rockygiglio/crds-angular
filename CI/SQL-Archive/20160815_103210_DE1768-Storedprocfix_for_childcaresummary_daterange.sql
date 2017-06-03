USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Summary]    Script Date: 8/15/2016 9:10:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
		
		SELECT  g.Group_Name,     
		        e.Event_Start_Date  EventDate, 
				e.Event_Start_Date  StartTime, 
				e.Event_End_Date EndTime, 
				c.__Age Age, 
				count(*) AS 'Total RSVP'
		FROM dbo.Events e
		JOIN Event_Groups eg ON e.Event_ID = eg.Event_ID


		JOIN Group_Participants childgp ON childgp.Group_ID = eg.Group_ID 

		JOIN Group_Participants parentgp ON parentgp.Group_Participant_ID = childgp.Enrolled_By
		JOIN Groups g ON g.Group_ID = parentgp.Group_ID
		JOIN Participants p ON p.Participant_ID = childgp.Participant_ID
		JOIN Contacts c on c.Contact_ID = p.Contact_ID
		WHERE e.Event_Type_ID = 243
			AND e.Event_Start_Date BETWEEN @StartDate AND @EndDate
			AND e.Congregation_ID = @CongregationId
			AND childgp.End_Date is null
		GROUP BY g.Group_Name, e.Event_Start_Date, e.Event_End_Date, c.__Age
		ORDER BY e.Event_Start_Date, g.Group_name, c.__Age

    END;


GO


