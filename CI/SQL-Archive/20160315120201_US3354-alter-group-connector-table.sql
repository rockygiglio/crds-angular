
IF EXISTS ( SELECT  name
               FROM    sys.foreign_keys
               WHERE   name = 'FK_GroupConnector_Organization' )
BEGIN
   ALTER TABLE dbo.cr_GroupConnectors
	DROP CONSTRAINT FK_GroupConnector_Organization
END

IF EXISTS ( SELECT  name
               FROM    sys.foreign_keys
               WHERE   name = 'FK_GroupConnector_Initiative' )
BEGIN
   ALTER TABLE dbo.cr_GroupConnectors
	DROP CONSTRAINT FK_GroupConnector_Initiative
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'cr_GroupConnectors'
            AND COLUMN_NAME = 'Name')
	BEGIN
		ALTER TABLE [dbo].[cr_GroupConnectors] DROP COLUMN Name;
	END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'cr_GroupConnectors'
            AND COLUMN_NAME = 'Organization_ID')
	BEGIN
		ALTER TABLE [dbo].[cr_GroupConnectors] DROP COLUMN Organization_ID;
	END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'cr_GroupConnectors'
            AND COLUMN_NAME = 'Initiative_ID')
	BEGIN
		ALTER TABLE [dbo].[cr_GroupConnectors] DROP COLUMN Initiative_ID;
	END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'cr_GroupConnectors'
            AND COLUMN_NAME = 'Primary_Contact')
	BEGIN
		EXECUTE sp_rename N'dbo.cr_GroupConnectors.Primary_Contact', N'Primary_Registration', 'COLUMN' 
	END




