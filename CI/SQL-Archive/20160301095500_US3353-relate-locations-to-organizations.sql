USE MinistryPlatform;
GO

IF NOT EXISTS (SELECT 1
         FROM   INFORMATION_SCHEMA.COLUMNS
         WHERE  TABLE_NAME = 'Locations'
                AND COLUMN_NAME = 'Organization_ID')
BEGIN
	ALTER TABLE [dbo].[Locations] ADD Organization_ID int NULL
END

IF NOT EXISTS ( SELECT  name
               FROM    sys.foreign_keys
               WHERE   name = 'FK_Locations_Organizations' )
BEGIN
   ALTER TABLE Locations ADD CONSTRAINT FK_Locations_Organizations FOREIGN KEY (Organization_ID) 
                          REFERENCES cr_Organizations(Organization_ID)
END