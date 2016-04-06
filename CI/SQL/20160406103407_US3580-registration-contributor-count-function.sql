USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[crds_GoVolunteerContributorCount](@Key INT)
RETURNS INT
AS
BEGIN
DECLARE @attributeId AS int;
SET @attributeId = (SELECT Attribute_ID FROM Attributes WHERE Attribute_Name = '2-7');

    RETURN(
	   SELECT (self + Spouse_Participation + ISNULL(children,0)) from (
		  SELECT 1 as self
		  , (SELECT sum(count) FROM cr_registration_children_attributes 
		                       WHERE registration_id = r.registration_id AND attribute_id != @attributeId) as children
		  , r.Spouse_Participation FROM cr_registrations r 
		                           WHERE r.registration_id = @Key) x
    )
END

GO


