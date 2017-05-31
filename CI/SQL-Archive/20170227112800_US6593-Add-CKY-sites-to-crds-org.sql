USE MinistryPlatform
GO

-- =====================================
-- Author		Jon Horner
-- Date			2/27/2017
-- Description	Adds CKY sites to the Crossroads Organization
-- =====================================

DECLARE @crossroadsOrg INT = 2;
DECLARE @andoverLoc INT = 19;
DECLARE @georgetownLoc INT = 16;
DECLARE @richmondLoc INT = 17;

INSERT INTO cr_Organization_Locations
	SELECT l.Domain_ID
		, @crossroadsOrg AS Organization_ID
		, l.Location_ID
	FROM Locations l
	WHERE (
			l.Location_ID = @andoverLoc
			OR l.Location_ID = @georgetownLoc
			OR l.Location_ID = @richmondLoc
		) AND NOT EXISTS (
			SELECT ol.Domain_ID
				, ol.Organization_ID
				, ol.Location_ID
			FROM cr_Organization_Locations ol
			WHERE l.Domain_ID = ol.Domain_ID
				AND @crossroadsOrg = ol.Organization_ID
				AND l.Location_ID = ol.Location_ID
		);