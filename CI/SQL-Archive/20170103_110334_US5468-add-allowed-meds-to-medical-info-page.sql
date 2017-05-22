USE MinistryPlatform
GO

IF NOT EXISTS (
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'Allowed_To_Administer_Medications'
      AND Object_ID = Object_ID(N'cr_Medical_Information'))
BEGIN
	ALTER TABLE [dbo].[cr_Medical_Information] 
	ADD Allowed_To_Administer_Medications nvarchar(400) null
END