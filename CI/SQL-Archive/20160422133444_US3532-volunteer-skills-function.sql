
USE [MinistryPlatform]
GO

CREATE FUNCTION [dbo].[crds_GoVolunteerSkills](@GroupConnectorRegistrationId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Skills AS NVARCHAR(MAX);
	DECLARE @ContactId AS INT;
	SET @ContactId = (SELECT Contact_ID FROM Contacts  c WHERE c.Contact_ID =
	                 (SELECT Contact_ID FROM Participants p WHERE p.Participant_ID  =
					(SELECT Participant_ID FROM cr_Registrations r  WHERE r.Registration_ID =
					(SELECT Registration_ID FROM cr_Group_Connector_Registrations WHERE Group_Connector_Registration_ID  = @GroupConnectorRegistrationId))));
		
	SELECT @Skills  = COALESCE (@Skills + ',','') + RTRIM(Label) 
                  FROM cr_Go_Volunteer_Skills WHERE  Attribute_ID in (SELECT Attribute_ID FROM Contact_Attributes ca WHERE Contact_ID =  @ContactId );
	
	RETURN @Skills;
END
    

