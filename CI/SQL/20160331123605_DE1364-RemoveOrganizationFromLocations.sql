USE [MinistryPlatform]
GO 
IF EXISTS(SELECT 1 FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Locations_Organizations')
   AND parent_object_id = OBJECT_ID(N'dbo.Locations'))
BEGIN
	ALTER TABLE dbo.Locations
	DROP CONSTRAINT FK_Locations_Organizations
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Organization_ID' AND Object_ID = Object_ID(N'dbo.Locations'))
BEGIN
    ALTER TABLE dbo.Locations
	DROP COLUMN Organization_ID
END