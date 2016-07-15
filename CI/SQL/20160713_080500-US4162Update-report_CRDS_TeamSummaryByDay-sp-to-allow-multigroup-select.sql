USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TeamSummaryByDay]    Script Date: 7/13/2016 7:53:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay]
-- Add the parameters for the stored procedure here
      @Day DATETIME, @GroupID AS VARCHAR(MAX), @Opportunities AS VARCHAR(MAX)
AS
     BEGIN
         -- SET NOCOUNT ON added to prevent extra result sets from
         -- interfering with SELECT statements.
         SET NOCOUNT ON;
         DECLARE @endTime TIME= '23:59:00';
         DECLARE @endDay DATETIME= @Day + @endTime;
         SELECT op.Opportunity_Title, op.Opportunity_ID, gr.Role_Title, c.Nickname, C.Last_Name, e.Event_Start_Date
	    , CONVERT(VARCHAR(15), op.Shift_Start, 100) AS Shift_Start
	    , CONVERT(VARCHAR(15), op.Shift_End, 100) AS Shift_End
	    , op.Room, sud.Sign_Up_Deadline
		, g.Group_Name
         FROM Responses r
              JOIN Opportunities op ON op.Opportunity_ID = r.Opportunity_ID
                                       AND op.Opportunity_ID IN (SELECT Item FROM dbo.dp_Split(@Opportunities, ','))
              JOIN Group_Roles gr ON op.Group_Role_ID = gr.Group_Role_ID
              JOIN Participants p ON r.Participant_ID = p.Participant_ID
              JOIN Contacts c ON p.Contact_ID = c.Contact_ID
              JOIN Events e ON r.Event_ID = e.Event_ID
              JOIN cr_Sign_Up_Deadline sud ON sud.Sign_Up_Deadline_ID = op.Sign_Up_Deadline_ID
			  JOIN Groups g on g.Group_ID = op.Add_to_Group
         WHERE op.Add_to_Group IN (SELECT Item FROM dbo.dp_Split(@GroupID, ','))
               AND r.Response_Result_ID = 1
               AND e.Event_Start_Date BETWEEN @Day AND @endDay
	     ORDER BY g.Group_Name;
     END;


