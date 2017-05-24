USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TeamSummaryByDay]    Script Date: 7/28/2016 11:20:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay]
-- Add the parameters for the stored procedure here
     @start_Day DATETIME, @end_Day DATETIME, @GroupID AS VARCHAR(MAX), @Opportunities AS VARCHAR(MAX)
AS
     BEGIN
         -- SET NOCOUNT ON added to prevent extra result sets from
         -- interfering with SELECT statements.
         SET NOCOUNT ON;
		 DECLARE @startDay as DATETIME = Convert(DateTime, DATEDIFF(DAY, 0, @start_Day))
		 DECLARE @endDay as DATETIME = Convert(DateTime, DATEDIFF(DAY, 0, DATEADD (DAY, 1, @end_Day) ))

         SELECT op.Opportunity_Title, op.Opportunity_ID, gr.Role_Title, c.Nickname, C.Last_Name, e.Event_Start_Date
	    , CONVERT(VARCHAR(15), op.Shift_Start, 100) AS Shift_Start
	    , CONVERT(VARCHAR(15), op.Shift_End, 100) AS Shift_End
	    , op.Room, sud.Sign_Up_Deadline
		, g.Group_Name
		, pc.Nickname AS PC_Nickname
		, pc.Last_Name AS PC_Lastname
		, CASE WHEN Floor (datediff(DAY,  ISNULL(c.Date_of_Birth, DATEADD(YEAR, -18, GETDATE())) , GETDATE()) / (365.23076923074 )) < 18 
	           THEN 'Y'
	           ELSE 'N' 
	       END 
	       AS Is_Student_Volunteer
		 , CONVERT(DATE, e.Event_Start_Date) AS Date_Portion
		 , CASE WHEN LTRIM(right(CONVERT(varchar(25), e.Event_Start_Date , 100), 7)) = '12:00AM'
		        THEN ''
				ELSE LTRIM(right(CONVERT(varchar(25), e.Event_Start_Date , 100), 7))
		    END		 
		   AS Time_Portion
         FROM Responses r
              JOIN Opportunities op ON op.Opportunity_ID = r.Opportunity_ID
                                       AND op.Opportunity_ID IN (SELECT Item FROM dbo.dp_Split(@Opportunities, ','))
              JOIN Group_Roles gr ON op.Group_Role_ID = gr.Group_Role_ID
              JOIN Participants p ON r.Participant_ID = p.Participant_ID
              JOIN Contacts c ON p.Contact_ID = c.Contact_ID
              JOIN Events e ON r.Event_ID = e.Event_ID
              JOIN cr_Sign_Up_Deadline sud ON sud.Sign_Up_Deadline_ID = op.Sign_Up_Deadline_ID
			  JOIN Groups g on g.Group_ID = op.Add_to_Group
			  LEFT JOIN Contacts pc on pc.Contact_ID = g.Primary_Contact
         WHERE op.Add_to_Group IN (SELECT Item FROM dbo.dp_Split(@GroupID, ','))
               AND r.Response_Result_ID = 1
               AND e.Event_Start_Date >= @startDay 
			   AND e.Event_Start_Date <= @endDay
	     ORDER BY e.Event_Start_Date, g.Group_Name
     END


GO


