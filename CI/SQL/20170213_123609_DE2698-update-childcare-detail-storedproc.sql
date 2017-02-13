
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
		--GradeGroup nvarchar(200),
		GroupParticipantStartDate datetime,
		Checkin nvarchar(6), 
		GradeGroup nvarchar(255)		
	)


	INSERT INTO @ChildcareDetail SELECT  
			g.Group_Name,     
			e.Event_Start_Date  EventDate, 
			e.Event_Start_Date  StartTime, 
			e.Event_End_Date EndTime, 
			p.Participant_ID,
			parentscontact.Display_Name as 'GroupMemberName',
			childcontact.Display_name as 'ChildName',
			childcontact.__Age Age, 
			childcontact.Date_of_Birth,
			childgp.Start_Date GroupParticipantStartDate,
			IIF(ep.Event_Participant_ID IS NOT NULL, 'Yes', 'No') AS 'Checkin',
			NULL
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
		AND childgp.End_Date is null
		AND parentscontact.Last_Name = 'Silbernagel'
		)

	ORDER BY e.Event_Start_Date, g.Group_name, childcontact.__Age

	DECLARE @participantid int
	DECLARE @eventstart datetime
	DECLARE @parentgroup varchar(255)
	DECLARE cur CURSOR FOR SELECT ParticipantID, StartTime, Group_Name FROM @ChildcareDetail
	OPEN cur
	FETCH NEXT FROM cur INTO @participantid, @eventstart, @parentgroup
	WHILE @@FETCH_STATUS = 0 BEGIN
		-- get age group for participant and update current row
		PRINT 'Looking for' + CONVERT(varchar, @eventstart, 121)
		DECLARE @groupname nvarchar(200) = null;
		SELECT TOP 1 @groupname = g.Group_Name FROM Groups g
		JOIN Group_Participants gp on g.Group_ID = gp.Group_ID AND gp.Participant_ID = @participantid
		WHERE g.Group_Type_ID = @AGE_GROUP_TYPE_ID
		AND ( @eventstart between gp.[Start_Date] and gp.[End_Date]
		   OR(@eventstart >= gp.Start_Date AND gp.End_Date is Null) 
		   )
		ORDER BY gp.[Start_Date] DESC

		UPDATE @ChildcareDetail SET [GradeGroup] = @groupname where ParticipantID = @participantid AND Group_Name = @parentgroup

		FETCH NEXT FROM cur INTO @participantid, @eventstart,@parentgroup
	END

	CLOSE cur    
	DEALLOCATE cur

	SELECT * FROM @ChildcareDetail
END