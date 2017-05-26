USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Congregation_ID' AND object_id = Object_ID(N'Payment_Detail'))
BEGIN
	ALTER TABLE dbo.Payment_Detail
	ALTER COLUMN Congregation_ID int NOT NULL
END