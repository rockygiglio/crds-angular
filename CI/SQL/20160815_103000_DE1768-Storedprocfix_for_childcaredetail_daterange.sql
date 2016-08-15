USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_Detail]    Script Date: 8/15/2016 9:53:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
		

		SELECT  g.Group_Name,     
		        e.Event_Start_Date  EventDate, 
				e.Event_Start_Date  StartTime, 
				e.Event_End_Date EndTime, 
				parentscontact.Display_Name as 'GroupMemberName',
				childcontact.Display_name as 'ChildName',
				childcontact.__Age Age, 
				'Yes' AS 'RSVP Status'
		FROM dbo.Events e
		JOIN Event_Groups eg ON e.Event_ID = eg.Event_ID
		JOIN Group_Participants childgp ON childgp.Group_ID = eg.Group_ID 
		JOIN Group_Participants parentgp ON parentgp.Group_Participant_ID = childgp.Enrolled_By
		JOIN Groups g ON g.Group_ID = parentgp.Group_ID
		JOIN Participants p ON p.Participant_ID = childgp.Participant_ID
		JOIN Contacts childcontact ON childcontact.Contact_ID = p.Contact_ID

		JOIN Participants parentsparticipant ON parentsparticipant.Participant_ID = parentgp.Participant_ID
		JOIN Contacts parentscontact ON parentscontact.Contact_ID = parentsparticipant.Contact_ID
		WHERE e.Event_Type_ID = 243
			AND e.Event_Start_Date BETWEEN @StartDate AND @EndDate
			AND e.Congregation_ID = @CongregationId
			AND childgp.End_Date is null
		
		ORDER BY e.Event_Start_Date, g.Group_name, childcontact.__Age
    END;

GO


