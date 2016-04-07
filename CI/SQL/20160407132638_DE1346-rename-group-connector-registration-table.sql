USE MinistryPlatform; 
GO
EXEC sp_rename 'dbo.cr_GroupConnectorRegistrations', 'cr_Group_Connector_Registrations';

EXEC sp_rename 'dbo.cr_Group_Connector_Registrations.GroupConnectorRegistration_ID', 'Group_Connector_Registration_ID', 'COLUMN';

EXEC sp_rename 'dbo.cr_Group_Connector_Registrations.GroupConnector_ID', 'Group_Connector_ID', 'COLUMN';