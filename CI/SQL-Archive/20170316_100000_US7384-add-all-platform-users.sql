-- DE7384: As part of the story to add the All Platform Users role for all users who are 
-- currently missing that role, we're going to add a new index to dp_User_Roles so that
-- performance is not negatively impacted after we add the 68K new rows that are currently
-- missing.  Originally we wanted to make this a UNQIUE index, but testing revealed that
-- MP's Combine Contacts tool (Core Tools) will break if we do not allow duplicates.
-- Apparently Combine Contacts assigns ContactA's roles to ContactB temporarily, then
-- removes duplicates for ContactB after the fact (which breaks if there is a UNIQUE
-- constraint).  Also, there are roughly ~100 duplicates in PROD from the past, so we'll
-- clean those up as well.

USE [MinistryPlatform]

DECLARE @AllPlatformUsers_RoleID INT = 39;

-- remove duplicate roles from dp_User_Roles
; WITH UserAndRoles (User_ID, Role_ID, Domain_ID, Row_Num)
AS (
	SELECT
		User_ID,
		Role_ID,
		Domain_ID,
		ROW_NUMBER() OVER(PARTITION BY User_ID, Domain_ID, Role_ID ORDER BY User_ID, Domain_ID, Role_ID, User_Role_ID) AS Row_Num
	FROM
		dp_User_Roles
)
DELETE FROM UserAndRoles WHERE row_num > 1;


-- create new index 
IF IndexProperty(Object_Id('dp_User_Roles'), 'IX_dp_User_Roles__UserID_DomainID_RoleID', 'IndexId') IS NULL
	CREATE INDEX IX_dp_User_Roles__UserID_DomainID_RoleID ON dp_User_Roles(User_ID, Domain_ID, Role_ID);


-- add All Platform Users role to all users who do not already have it
INSERT INTO dp_User_Roles (
	User_ID,
	Role_ID,
	Domain_ID
)
SELECT
	u.User_ID,
	@AllPlatformUsers_RoleID,
	1
FROM
	dp_Users u
		LEFT JOIN dp_User_Roles ur ON ur.User_ID = u.User_ID AND ur.Role_ID = @AllPlatformUsers_RoleID AND ur.Domain_ID = 1
WHERE
	ur.User_Role_ID IS NULL
;
