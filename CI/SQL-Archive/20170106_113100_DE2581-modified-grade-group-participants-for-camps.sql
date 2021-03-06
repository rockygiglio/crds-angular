USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_Grade_Group_Participant_For_Camps]    Script Date: 1/5/2017 2:14:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Matt Silbernagel
-- Create date: 10/20/2016
-- Description: Determine if the passed in contact is in an eligible grade group 
--
-- Edit
-- Author:		Jon Horner
-- Date:		1/6/2017
-- Description:	Added checks to make sure that group participant and group end dates aren't passed.
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_Grade_Group_Participant_For_Camps]
	@ContactID int,
	@EventID int
AS	
BEGIN
		
	DECLARE @GroupTypeID int = 4;
	
	SELECT DISTINCT(1) as 'Status' from [dbo].[Events] e
	JOIN Event_Groups eg on eg.Event_ID = e.Event_ID AND e.[Event_ID] = @EventID
	JOIN Groups g on eg.Group_ID = g.Group_ID AND g.Group_Type_ID = @GroupTypeID
	JOIN Group_Participants gp on gp.[Group_ID] = g.[Group_ID]
	JOIN Participants p on p.[Participant_ID] = gp.[Participant_ID]
	WHERE e.[Event_Type_ID] = 8 	
	AND getDate() between e.[Registration_Start] and e.[Registration_End]
	AND (gp.[End_Date] IS NULL OR gp.[End_Date] > getDate())
	AND (g.[End_Date] IS NULL OR g.[End_Date] > getDate())
	AND e.[Cancelled] = 0
	AND p.Contact_ID = @ContactID

END
