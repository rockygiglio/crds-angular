USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GoVolunteerFamilyCount]    Script Date: 3/10/2016 1:18:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[crds_GoVolunteerFamilyCount](@Key INT)
RETURNS INT
AS
BEGIN
    RETURN(
	   SELECT (self + Spouse_Participation + ISNULL(children,0)) from (
		  SELECT 1 as self
		  , (SELECT sum(count) FROM cr_registration_children_attributes WHERE registration_id = r.registration_id) as children
		  , r.Spouse_Participation
		  FROM cr_registrations r
		  WHERE r.registration_id = @Key) x
    )
END
GO