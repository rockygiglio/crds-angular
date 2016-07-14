USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_GroupsForCongregation]    Script Date: 7/13/2016 8:25:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].report_CRDS_GroupsForCongregation
	-- Add the parameters for the stored procedure here
	@CongregationID AS int
AS
BEGIN

	SELECT Group_ID, Group_Name FROM Groups
	WHERE Group_Type_ID = 9 
	AND Group_ID IN (select Add_To_Group from Opportunities)
	AND (End_Date IS NULL OR End_Date > GETDATE())
	And (Congregation_ID = @CongregationID OR @CongregationID = 15)

	Order by group_name

END



