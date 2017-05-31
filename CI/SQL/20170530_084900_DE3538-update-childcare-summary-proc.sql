USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Summary]   Script Date: 5/30/2017 8:43:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author: John Cleaver	
-- Create date: 5/26/2017
-- Description:	Refactor of [report_CRDS_Childcare_Summary] proc
-- ===============================================================
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
		
		DECLARE @AGE_GROUP_TYPE_ID int = 4;
		
		DECLARE @ChildcareSummary TABLE
		(
			Group_Name varchar(255),
			EventDate datetime,
			StartTime datetime, 
			EndTime datetime,
			ParticipantID int,
			Age nvarchar(200),
			RSVP int,
			Checkin int,
			RSVPOnline int,
			RSVPOverride int
		)

		BEGIN		
			INSERT INTO @ChildcareSummary
				SELECT  g.Group_Name Group_Name,  
					e.Event_Start_Date  EventDate, 
					e.Event_Start_Date  StartTime, 
					e.Event_End_Date EndTime, 
					p.Participant_ID ParticipantID,
					'' as Age,
					1 AS 'RSVP',
					IIF(ep.Event_Participant_ID IS NOT NULL AND ep.Participation_Status_ID = 4, 1, 0) AS 'Checkin',
					CASE WHEN EXISTS (SELECT * FROM Groups s_g INNER JOIN Event_Groups s_eg ON s_g.Group_ID = s_eg.Group_ID LEFT OUTER JOIN 
					Group_Participants s_gp on s_eg.Group_ID = s_gp.Group_ID
					WHERE s_eg.Event_ID = e.Event_ID and s_g.Group_Type_ID=27 and s_gp.Participant_ID = p.Participant_ID) 
					THEN 1 ELSE 0 END AS RSVPOnline,
					CASE WHEN NOT EXISTS (SELECT * FROM Groups s_g INNER JOIN Event_Groups s_eg ON s_g.Group_ID = s_eg.Group_ID LEFT OUTER JOIN 
					Group_Participants s_gp on s_eg.Group_ID = s_gp.Group_ID
					WHERE s_eg.Event_ID = e.Event_ID and s_g.Group_Type_ID=27 and s_gp.Participant_ID = p.Participant_ID) 
					THEN 1 ELSE 0 END AS RSVPOverride
				FROM dbo.Events e
				JOIN Event_Groups eg ON e.Event_ID = eg.Event_ID
				JOIN Group_Participants childgp ON childgp.Group_ID = eg.Group_ID 
				JOIN Group_Participants parentgp ON parentgp.Group_Participant_ID = childgp.Enrolled_By
				JOIN Groups g ON g.Group_ID = parentgp.Group_ID
				JOIN Participants p ON p.Participant_ID = childgp.Participant_ID
				JOIN Contacts c on c.Contact_ID = p.Contact_ID		
				LEFT JOIN Event_Participants ep on ep.Event_ID = e.Event_ID AND ep.Participant_ID = p.Participant_ID AND ep.Participation_Status_ID in (3, 4) 
				WHERE e.Event_Type_ID = 243
					AND e.Event_Start_Date >= @StartDate
					AND e.Event_Start_Date <= @EndDate 
					AND e.Congregation_ID = @CongregationId
					AND childgp.End_Date is null

		END

		DECLARE @participantid int
		DECLARE cur CURSOR FOR SELECT ParticipantID FROM @ChildcareSummary
		OPEN cur
		FETCH NEXT FROM cur INTO @participantid
		WHILE @@FETCH_STATUS = 0 BEGIN
		   -- get age group for participant and update current row
		   DECLARE @groupname nvarchar(200) = null;
		   SELECT TOP 1 @groupname = g.Group_Name FROM Groups g
		   JOIN Group_Participants gp on g.Group_ID = gp.Group_ID AND gp.Participant_ID = @participantid
		   WHERE g.Group_Type_ID = @AGE_GROUP_TYPE_ID
		   AND ( @StartDate between gp.Start_Date and gp.End_Date
					OR 
				 @StartDate >= gp.Start_Date )
			ORDER BY gp.Start_Date ASC

		   UPDATE @ChildcareSummary SET [Age] = @groupname where ParticipantID = @participantid

		   FETCH NEXT FROM cur INTO @participantid
		END

		CLOSE cur    
		DEALLOCATE cur

		SELECT Group_Name,
			EventDate,
			StartTime, 
			EndTime,
			Age,
			sum(RSVP) AS 'Total RSVP',
			sum(Checkin) AS 'Total Check In',
			sum(RSVPOnline) AS 'Rsvp Online',
			sum(RSVPOverride) AS 'Rsvp Override'
		FROM @ChildcareSummary
		GROUP BY Group_Name, EventDate, StartTime, EndTime, Age
		ORDER BY EventDate,Group_name, Age
    END;

GO


