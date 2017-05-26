USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_By_Day]    Script Date: 7/20/2016 12:29:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author: Phil Lachmann & Lakshmi Nair	
-- Create date: 7/20/2016
-- Description:	Used to populated childcare summary SSRS report
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

