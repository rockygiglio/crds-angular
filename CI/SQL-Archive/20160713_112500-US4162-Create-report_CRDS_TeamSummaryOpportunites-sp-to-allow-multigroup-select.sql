
USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TeamSummaryOpportunities]    Script Date: 7/13/2016 11:25:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[report_CRDS_TeamSummaryOpportunities]
-- Add the parameters for the stored procedure here
      @GroupID AS VARCHAR(MAX)
AS
     BEGIN

	SELECT o.Opportunity_ID, o.Opportunity_Title, o.Add_To_Group, 
	 CONCAT(g.Group_name, ': ',o.Opportunity_Title, ' ', LTRIM(RIGHT(CONVERT(VARCHAR(25), [Shift_Start], 100), 7)), '-', LTRIM(RIGHT(CONVERT(VARCHAR(25), [Shift_End], 100), 7))) OpportunityLabel
	FROM Opportunities o
	JOIN Groups g on o.Add_To_Group = g.Group_ID  
	WHERE GetDate() BETWEEN o.Publish_Date AND ISNULL(o.Opportunity_Date,GetDate())
	 AND o.Add_to_Group is NOT NULL 
	 AND o.Event_Type_ID is NOT NULL 
	 AND o.Add_To_Group IN (SELECT Item FROM dbo.dp_Split(@GroupID, ','))
	ORDER BY g.Group_name, o.Opportunity_Title

END;

GO



