USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Detail]    Script Date: 5/25/2017 8:43:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author: John Cleaver	
-- Create date: 5/26/2017
-- Description:	Refactor of report_CRDS_Childcare_Detail proc
-- ===============================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Childcare_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Childcare_Detail] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_CRDS_Childcare_Detail]

      @StartDate DATETIME,
	  @EndDate DATETIME,
	  @CongregationId INT
AS
    BEGIN

	SET NOCOUNT ON;

	SET @StartDate =  DATEADD(day, DATEDIFF(day, 0, @StartDate), '00:00:00');
	SET @EndDate =  DATEADD(day, DATEDIFF(day, 0, @EndDate), '23:59:00');

	DECLARE @AGE_GROUP_TYPE_ID int = 4;

	DECLARE @ChildcareDetail TABLE
	(
		Group_Name varchar(255),
		EventDate datetime,
		StartTime datetime, 
		EndTime datetime,
		ParticipantId int,
		GroupMemberName varchar(255),
		ChildName varchar(255),
		Age nvarchar(200),
		Date_Of_Birth datetime,
		GroupParticipantStartDate datetime,
		Checkin nvarchar(6), 
		GradeGroup nvarchar(255),
		RSVPOnline int,
		RSVPOverride int				
	)

	INSERT INTO @ChildcareDetail SELECT DISTINCT
			g.Group_Name,     
			e.Event_Start_Date as EventDate, 
			e.Event_Start_Date as StartTime, 
			e.Event_End_Date as EndTime, 
			p.Participant_ID,
			parentscontact.Display_Name as 'GroupMemberName',
			childcontact.Display_name as 'ChildName',
			childcontact.__Age as Age, 
			childcontact.Date_of_Birth,
			childgp.Start_Date as GroupParticipantStartDate,
			IIF(ep.Event_Participant_ID IS NOT NULL AND ep.Participation_Status_ID = 4, 'Yes', 'No') AS 'Checkin',
			(SELECT TOP(1) Group_Name from Groups s_g inner join Group_Participants s_gp on s_g.Group_ID = s_gp.Group_ID 
				WHERE s_gp.Participant_ID = p.Participant_ID and s_gp.End_Date IS NULL and s_g.Group_Type_ID=4),
				-- look to see if the participant has a childcare group participant record -
				-- this means they were already RSVP'ed for the event
				CASE WHEN EXISTS (SELECT * FROM Groups s_g INNER JOIN Event_Groups s_eg ON s_g.Group_ID = s_eg.Group_ID LEFT OUTER JOIN 
				Group_Participants s_gp on s_eg.Group_ID = s_gp.Group_ID
				WHERE s_eg.Event_ID = e.Event_ID and s_g.Group_Type_ID=27 and s_gp.Participant_ID = p.Participant_ID) 
				THEN 1 ELSE 0 END AS RSVPOnline,
				-- look to see if the participant does NOT have a group participant record for the childcare group - 
				-- this assumes they were overridden into childcare for that event
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
	JOIN Contacts childcontact ON childcontact.Contact_ID = p.Contact_ID	
	JOIN Participants parentsparticipant ON parentsparticipant.Participant_ID = parentgp.Participant_ID
	JOIN Contacts parentscontact ON parentscontact.Contact_ID = parentsparticipant.Contact_ID
	LEFT JOIN Event_Participants ep on ep.Event_ID = e.Event_ID AND ep.Participant_ID = p.Participant_ID AND ep.Participation_Status_ID in (3, 4)
	WHERE (e.Event_Type_ID = 243
		AND e.Event_Start_Date BETWEEN @StartDate AND @EndDate
		AND e.Congregation_ID = @CongregationId		
		AND childgp.End_Date is null)

	ORDER BY e.Event_Start_Date, g.Group_name, childcontact.__Age

	SELECT * FROM @ChildcareDetail
END

GO


