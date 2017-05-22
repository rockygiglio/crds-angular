
USE [MinistryPlatform]
GO

CREATE FUNCTION [dbo].[crds_GoLocAddress](@GroupConnectorRegistrationId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN

	DECLARE @FullAddress AS NVARCHAR(MAX);
	DECLARE @AddressId AS INT;
	SET @AddressId = (select l.address_id 
			from cr_Group_Connector_Registrations gcr
			join cr_Group_Connectors gc on gc.group_connector_id = gcr.group_connector_id
			join cr_Projects p on p.project_id = gc.project_id
			join Locations l on l.location_id = p.location_id
			where Group_Connector_Registration_ID  = @GroupConnectorRegistrationId);

	DECLARE @Addr AS NVARCHAR(MAX);
	DECLARE @City AS NVARCHAR(MAX);
	DECLARE @State AS NVARCHAR(MAX);
	DECLARE @Zip AS NVARCHAR(MAX);

	SELECT @Addr = Address_Line_1, @City = City, @State = [State/Region], @Zip=Postal_code FROM Addresses WHERE Address_ID = @AddressId;
		
	SET @FullAddress  = @Addr + ', ' + @City + ', ' + @State + ', ' + @Zip
	
	RETURN @FullAddress;
END
	
	


