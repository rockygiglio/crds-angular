USE MinistryPlatform; 
GO
EXEC sp_rename 'dbo.cr_GroupConnectors', 'cr_Group_Connectors';

EXEC sp_rename 'dbo.cr_Group_Connectors.GroupConnector_ID', 'Group_Connector_ID', 'COLUMN';