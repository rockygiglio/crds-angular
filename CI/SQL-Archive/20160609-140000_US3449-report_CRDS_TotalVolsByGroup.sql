USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TotalVolsByGroup]    Script Date: 6/9/2016 8:28:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_TotalVolsByGroup]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_TotalVolsByGroup] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_CRDS_TotalVolsByGroup]
    @DomainID VARCHAR(40)
   ,@UserID VARCHAR(40)
   ,@PageID INT
   ,@GroupTypes NVARCHAR(MAX) = '0'
   
AS
BEGIN

CREATE TABLE #TEAMS
(
	OldGroupID			INT,
	NewGroupID			INT,
	GroupName	VARCHAR(100),
	ActiveVolunteers		INT
)

-- total volunteers per serve group
INSERT INTO #TEAMS
SELECT g.__ExternalGroupID AS OldGroupID, g.group_id AS NewGroupID, 
       g.group_name AS GroupName, 
	   (SELECT COUNT(*) FROM group_participants WHERE group_id = g.group_id AND end_date IS NULL) AS ActiveVolunteers	   
FROM groups g
WHERE g.Group_Type_ID IN (SELECT Item FROM dp_Split(@GroupTypes, ','))
GROUP BY g.__ExternalGroupID,g.group_id, g.group_name
ORDER BY group_name


CREATE TABLE #SITES
(
	groupid		INT,
	GroupName	VARCHAR(100),
	con_name	VARCHAR(100),
	con_id		INT,
	total		INT
)

--Total of volunteers by site
INSERT INTO #SITES
	SELECT g.group_id, g.group_name, con.congregation_name, con.Congregation_ID , COUNT(*)
	FROM contacts c
	JOIN participants p ON p.contact_id = c.contact_id
	JOIN group_participants gp ON gp.participant_id = p.participant_id
	JOIN groups g ON g.group_id = gp.group_id
	JOIN households h ON h.household_id = c.household_id
	LEFT JOIN congregations con ON con.congregation_id = h.congregation_id
	JOIN #TEAMS t on t.NewGroupID = g.group_id
	WHERE gp.end_date IS NULL
	GROUP BY g.group_id,g.group_name, con.congregation_name, con.Congregation_ID

SELECT t.newgroupid,groupname,ActiveVolunteers FROM #TEAMS t

IF OBJECT_ID('tempdb..#TEAMS') IS NOT NULL   
	DROP TABLE #TEAMS
IF OBJECT_ID('tempdb..#SITES') IS NOT NULL   
	DROP TABLE #SITES

END
GO