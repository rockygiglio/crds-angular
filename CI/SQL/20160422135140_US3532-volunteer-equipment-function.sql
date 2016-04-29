USE [MinistryPlatform]
GO

CREATE FUNCTION [dbo].[crds_GoVolunteerEquipment](@GroupConnectorRegistrationId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Equipment AS NVARCHAR(MAX);
	DECLARE @RegistrationId AS INT;
	SET @RegistrationId = (SELECT Registration_ID FROM cr_Group_Connector_Registrations WHERE Group_Connector_Registration_ID  = @GroupConnectorRegistrationId);
		
	SELECT @Equipment  = COALESCE (@Equipment + ',','') + RTRIM(Attribute_Name) 
                  FROM Attributes WHERE  Attribute_ID in (SELECT Attribute_ID FROM cr_Registration_Equipment_Attributes WHERE Registration_ID =  @RegistrationId );
	
	RETURN @Equipment;
END
    

