
USE [MinistryPlatform]
GO


CREATE FUNCTION [dbo].[crds_GoTCInfo](@GroupConnectorRegistrationId INT, @TcAttribute INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	-- 1 = Name(s)
	-- 2 = Contact Info (email and phone)


	DECLARE @ReturnString AS NVARCHAR(MAX);

	DECLARE @ProjectId AS INT;
	SET @ProjectId = (select p.Project_Id 
			from cr_Group_Connector_Registrations gcr
			join cr_Group_Connectors gc on gc.group_connector_id = gcr.group_connector_id
			join cr_Projects p on p.Project_ID = gc.Project_ID
			where Group_Connector_Registration_ID  = @GroupConnectorRegistrationId);


	IF (@TcAttribute = 1)
		BEGIN
			SELECT @ReturnString = COALESCE (@ReturnString + ' & ','') + RTRIM(First_name)  + ' ' + RTRIM(Last_name) 
						FROM Contacts WHERE  Contact_id in 
						(select c.contact_id
							from cr_projects p
							join [dbo].[cr_Group_Connectors] gc on gc.Project_ID = p.Project_ID
							join [dbo].[cr_Group_Connector_Registrations] gcr on gcr.[Group_Connector_ID] = gc.[Group_Connector_ID]
							join [dbo].[cr_Registrations] r on r.registration_id = gcr.Registration_ID
							join Participants pt on pt.participant_id = r.participant_id
							join contacts c on c.contact_id = pt.contact_id
							where p.Project_id = @ProjectId
							AND gcr.role_id=22 );
		END
	ELSE
		BEGIN
			SELECT @ReturnString = COALESCE (@ReturnString + ' or ','') + COALESCE (RTRIM(email_address),'') + '/' + COALESCE (RTRIM(mobile_phone),'')
						FROM Contacts WHERE  Contact_id in 
						(select c.contact_id
							from cr_projects p
							join [dbo].[cr_Group_Connectors] gc on gc.Project_ID = p.Project_ID
							join [dbo].[cr_Group_Connector_Registrations] gcr on gcr.[Group_Connector_ID] = gc.[Group_Connector_ID]
							join [dbo].[cr_Registrations] r on r.registration_id = gcr.Registration_ID
							join Participants pt on pt.participant_id = r.participant_id
							join contacts c on c.contact_id = pt.contact_id
							where p.Project_id = @ProjectId
							AND gcr.role_id=22 );
		END;

	RETURN @ReturnString;
END
	