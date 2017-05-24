USE MinistryPlatform
GO

IF NOT EXISTS(
   SELECT *
   FROM sys.columns 
   WHERE Name      = N'Kids_Welcome'
     AND Object_ID = Object_ID(N'Groups'))
BEGIN
ALTER TABLE dbo.Groups 
ADD Kids_Welcome bit NOT NULL
CONSTRAINT DF_Groups_Kids_Welcome DEFAULT 0
END