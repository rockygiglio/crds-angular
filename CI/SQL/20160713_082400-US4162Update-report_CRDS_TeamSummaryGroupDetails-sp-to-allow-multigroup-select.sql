USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TeamSummaryGroupDetails]    Script Date: 7/13/2016 8:25:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[report_CRDS_TeamSummaryGroupDetails]
	-- Add the parameters for the stored procedure here
	@GroupID AS VARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT g.Group_Name, c.Nickname, c.Last_Name, g.Description  FROM Groups g
	JOIN Contacts c on g.Primary_Contact = c.Contact_ID
	WHERE g.Group_ID IN (SELECT Item FROM dbo.dp_Split(@GroupID, ','))
END



