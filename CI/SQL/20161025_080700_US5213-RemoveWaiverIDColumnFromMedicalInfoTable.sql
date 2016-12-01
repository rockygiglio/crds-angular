USE MinistryPlatform
GO

IF EXISTS (SELECT *  FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_cr_Medical_Information_Waivers') AND parent_object_id = OBJECT_ID(N'dbo.cr_Medical_Information'))
BEGIN
  ALTER TABLE [dbo].[cr_Medical_Information] DROP CONSTRAINT [FK_cr_Medical_Information_Waivers]
END


IF EXISTS(SELECT * FROM  INFORMATION_SCHEMA.COLUMNS  WHERE  TABLE_NAME = 'cr_Medical_Information'  AND COLUMN_NAME = 'Waiver_ID') 
BEGIN
	ALTER TABLE dbo.cr_Medical_Information DROP COLUMN Waiver_ID ; 
END

GO
